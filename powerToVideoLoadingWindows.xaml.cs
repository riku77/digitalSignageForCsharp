using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using WpfAnimatedGif;


// powerpoint関連にいる
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using ppt = Microsoft.Office.Interop.PowerPoint.Application;

// iniファイルの読み書きにいる
using IniParser;
using IniParser.Model;

//デバッグにいる
using System.Diagnostics;



namespace pptTester
{
    /// <summary>
    /// powerToVideoLoadingWindows.xaml の相互作用ロジック
    /// </summary>

    
    // ロード画面用のクラスみたいなもの。
    // この画面が表示されたら、pptxを動画に変換する処理も実行される
    public partial class powerToVideoLoadingWindows : Window
    {   

        static DispatcherTimer timer;

        // iniファイル読み込み
        // 相対パスはソリューション名.exeがある場所が基準になっている
        // ソリューション名.slnがある場所ではないので注意
        static String path = "setting.ini";
        // 絶対パスに直さないと相対パスのまま使用したらどうあがいても動作しない
        static String loadImgAnimation = System.IO.Path.GetFullPath("animation/icon_loader_d_gw_01_s1.gif");
        static FileIniDataParser parser = new FileIniDataParser();
        static IniData data = parser.ReadFile(path);



        public powerToVideoLoadingWindows()
        {
            InitializeComponent();
            // windowがロードされた時に実行される関数
            this.Loaded += MainWindow_Loaded;

        }

        async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
           

             // ウィンドウを最大化している
            this.WindowState = System.Windows.WindowState.Maximized;

            // ウィンドウスタイルを非表示している
            this.WindowStyle = System.Windows.WindowStyle.None;

            // ウインドウのリサイズを禁止している
            this.ResizeMode = ResizeMode.NoResize;


            //これを実行したときに画像アニメーションがストップするのを何とかしたい。非同期処理実装すればいけるかな
            //非同期処理実装でいけた
            var result = await pptToVideo();


            if (result == PpMediaTaskStatus.ppMediaTaskStatusDone)
            {
                // load画面を閉じる
                window_close();
            }
            else if(result == PpMediaTaskStatus.ppMediaTaskStatusFailed)
            {
                loadText.Text = "変換に失敗しました";
            }





        }

        void setLoadWindow()
        {   
            // GIF画像を読み込む
            BitmapImage gifSource = new BitmapImage(new Uri(loadImgAnimation));
            // Imageコントロールにアニメーションを設定する
            ImageBehavior.SetAnimatedSource(gifImage, gifSource);
            ImageBehavior.SetRepeatBehavior(gifImage, RepeatBehavior.Forever);
        }


        void window_close()
        {

            // 30秒後にウィンドウを閉じるためのタイマーを作成
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10)
            };

            // タイマーのTickイベントをラムダ式で定義
            timer.Tick += (sender, e) =>
                {
                // ウィンドウを閉じる
                    this.Close();
                };

            // タイマーを開始
            timer.Start();


        }

        async Task<PpMediaTaskStatus> pptToVideo()
        {
            // 現在実行している処理を説明する
            loadText.Text = "PowerPointを動画に変換中";
            
            // loadしていることを説明するアニメーションをsetする
            setLoadWindow();

            // powerPointを動画に変換する
            var result =  await ConvertPowerPoint(
                importPath : System.IO.Path.GetFullPath(data["powerPoint"]["import_path"]), 
                exportPath : System.IO.Path.GetFullPath(data["powerPoint"]["export_path"])
                );

            return result;
        }


        async Task<PpMediaTaskStatus> ConvertPowerPoint(string importPath, string exportPath)
        {
            return await Task.Run(async () =>
            {
                // PowerPointスライドを開く
                ppt pptApp = new ppt();
                Presentation presentation = pptApp.Presentations.Open(importPath, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);

                // PowerPointスライドを動画に変換する
                presentation.CreateVideo(exportPath, DefaultSlideDuration: 1);

                // 動画の作成が完了するまで待機する
                while (presentation.CreateVideoStatus != PpMediaTaskStatus.ppMediaTaskStatusDone)
                {
                    await Task.Delay(500);
                }

                return presentation.CreateVideoStatus;

            });
        }


}
}
