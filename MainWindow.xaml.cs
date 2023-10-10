using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


using System.Net;
using System.Windows.Forms;


// 設定画面表示するのにいる
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight.Command;

// powerpoint関連にいる
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using ppt = Microsoft.Office.Interop.PowerPoint.Application;

// webView2にいる
using Microsoft.Web.WebView2.Core; // WPFアプリケーションでWebView2を使用するためのusingステートメント
using Microsoft.Web.WebView2.Wpf;

// iniファイルの読み書きにいる
using IniParser;
using IniParser.Model;

//デバッグにいる
using System.Diagnostics;


namespace pptTester
{

    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        // iniファイル読み込み
        // 相対パスはソリューション名.exeがある場所が基準になっている
        // ソリューション名.slnがある場所ではないので注意
        static String ini_path = "setting.ini";
        static FileIniDataParser parser = new FileIniDataParser();
        static IniData data = parser.ReadFile(ini_path);

        private bool _exitApp;
        private string _url;

        string status = "loaded";
        bool isLoopableActive = bool.Parse(data["isLoopableActive"]["value"]);
        bool isPptxToVideo = bool.Parse(data["powerPoint"]["pptxToVideo"]);

        // player.sourceにリストのvalueを与えるために「Uri型」で定義
        private List<List<Uri>> videoPlaylists = new List<List<Uri>>();
        // 現在再生中の動画のインデックス
        // index[0]が動画領域1 index[1]が動画領域2
        private int[] currentVideoIndex = new int[] { 0, 0 };


        public MainWindow()
        {

            InitializeComponent();
            InitializeWebView();
            DataContext = this;

            // windowがロードされた時に実行される関数
            this.Loaded += MainWindow_Loaded;


            

            // video




            // 左辺がセッション名 → 【セッション名】
            // 右辺がkey-valueのkey
            // 代入する値がkey-valueのvalue
            //data[ "Dialog" ][ "Width" ] = "0006";

            // pathで指定したiniファイルにdataの内容を書き込む
            //parser.WriteFile( path, data );


        }


        // MainWindowがロードされたときのEvent
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // ウィンドウを最大化している
            this.WindowState = System.Windows.WindowState.Maximized;

            // ウィンドウスタイルを非表示している
            //this.WindowStyle = System.Windows.WindowStyle.None;

            // ウインドウのリサイズを禁止している
            //this.ResizeMode = ResizeMode.NoResize;


            if (isPptxToVideo) {OpenSettings();}

            // なぜか動画が変換できなくなった
            // そんなときは仮の短いpptxを動画に変換してやれば以降、正常に変換できるようになる
            //ppt_to_video();

            // 動画の再生
            //startVideo(1, 1);

            // ウィンドウごとの動画プレイリストをロード
            LoadVideoPlaylists();


            // 動画の再生
            
            
            
            StartNextVideo(videoPlayer, 0); // 最初の動画を再生
            

            StartNextVideo(videoPlayer2, 1); // 最初の動画を再生


        }


        // ウィンドウごとの動画プレイリストをロード
        private void LoadVideoPlaylists()
        {
            string filePath = ini_path; // iniファイルのパスを指定してください

            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile(filePath);

            string[] windowNames = new string[] { "video", "video2" };

            foreach (var windowName in windowNames)
            {   
                // 動画再生領域の数だけ二次元リストにリストをaddする
                List<Uri> playlist = new List<Uri>();

                int videoCount = 1;
                while (data[windowName].ContainsKey($"path{videoCount}"))
                {   // 相対パスを使用したいときは相対パスから絶対パスを作るようにしないと動画が読み込めない
                    Uri videoUri = new Uri(System.IO.Path.GetFullPath(data[windowName][$"path{videoCount}"]));
                    playlist.Add(videoUri);
                    videoCount++;
                }

                videoPlaylists.Add(playlist);


            }
        }



        // 次の動画を再生する
        private void StartNextVideo(MediaElement player, int windowIndex)
        {   
            // 各動画領域で使用するプレイリスト
            List<Uri> playlist = videoPlaylists[windowIndex];
            // 動画を再生する条件
            bool videoPlay_criteria = currentVideoIndex[windowIndex] < playlist.Count;

            if ( videoPlay_criteria )
            {

                int flipX = 1;
                int flipY = 1;
                ScaleTransform flipTrans = new ScaleTransform(flipX, flipY);

                player.RenderTransform = flipTrans;
                player.Source = playlist[currentVideoIndex[windowIndex]];
                player.Play();

                status = "started";


            }

            // 動画再生条件がFalse = プレイリストの終点に到着 かつ ループ再生がTrueならプレイリストの最初に戻って再生
            else if ( (isLoopableActive) && (!videoPlay_criteria) )
            {
                currentVideoIndex[windowIndex] = 0;

                int flipX = 1;
                int flipY = 1;
                ScaleTransform flipTrans = new ScaleTransform(flipX, flipY);

                player.RenderTransform = flipTrans;
                player.Source = playlist[currentVideoIndex[windowIndex]];
                player.Play();
                status = "started";

            }

            else
            {
                player.Visibility = System.Windows.Visibility.Hidden;
                player.Stop();
                status = "stopped";
            }
        }


        // 動画の再生が終了したときに実行される処理
        private void videoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaElement player = sender as MediaElement;

            // ループフラグがTrueなら次の動画を再生
            if (isLoopableActive)
            {


                if (player == videoPlayer)
                {
                    // ウィンドウ1の動画が終了した場合、次の動画を再生
                    currentVideoIndex[0] += 1;
                    player.Position = TimeSpan.FromSeconds(0);
                    StartNextVideo(player, 0);
                }
                else if (player == videoPlayer2)
                {
                    // ウィンドウ2の動画が終了した場合、次の動画を再生
                    currentVideoIndex[1] += 1;
                    player.Position = TimeSpan.FromSeconds(0);
                    StartNextVideo(player, 1);
                }


            }
            // ループフラグがfalseなら動画を停止
            else
            {
                //配列内の全要素を0で初期化してプレイリストの進捗状態をリセット
                Array.Fill(currentVideoIndex, 0);
                // 動画再生ui要素を非表示にする
                // 非表示にすることでスペースの確保やリソースの再利用が容易になる
                videoPlayer.Visibility = System.Windows.Visibility.Hidden;

                videoPlayer2.Visibility = System.Windows.Visibility.Hidden;
                status = "stopped";
            }
        }



        // 動画を停止する
        void stopVideo()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                videoPlayer.Visibility = System.Windows.Visibility.Hidden;
                videoPlayer.Stop();
                videoPlayer2.Visibility = System.Windows.Visibility.Hidden;
                videoPlayer2.Stop();
                status = "stopped";
            }));
        }

        // 動画を開始する
        bool startVideo(int flipX, int flipY)
        {

            // メインスレッドで実行する必要があるため、Dispatcher.BeginInvokeを使用して処理を実行する
            Dispatcher.BeginInvoke(new Action(() =>
            {
                    // ビデオプレーヤーの表示を変換するための処理
                    // flipXとflipYの値に基づいて、ビデオの表示を水平方向と垂直方向に反転させることができます。
                    // &flipX=1 または flipY=1 を追加して、ビデオを X 軸または Y 軸上で反転します。
                ScaleTransform flipTrans = new ScaleTransform(flipX, flipY);
                videoPlayer.RenderTransform = flipTrans;

                videoPlayer.Visibility = System.Windows.Visibility.Visible;
                videoPlayer.Source = new Uri(data["video"]["path1"]);

                videoPlayer2.RenderTransform = flipTrans;

                videoPlayer2.Visibility = System.Windows.Visibility.Visible;
                videoPlayer2.Source = new Uri(data["video"]["path2"]);

                status = "started";
            }));

            return true;
        }



        // ここにプレイリスト処理を追加
        // 動画の再生が終了したときに実行される処理
        private void videoPlayer_MediaEnded2(object sender, RoutedEventArgs e)
        {
            // ループフラグがTrueなら...next
            if (isLoopableActive)
            {   // 動画の再生位置を0にして...next
                videoPlayer.Position = TimeSpan.FromSeconds(0);
                // 動画をStartする...fin
                videoPlayer.Play();

                videoPlayer2.Position = TimeSpan.FromSeconds(0);
                videoPlayer2.Play();
            }
            // ループフラグがfalseなら...next
            else
            {
                // 動画再生ui要素を非表示にする
                // 非表示にすることでスペースの確保やリソースの再利用が容易になる
                videoPlayer.Visibility = System.Windows.Visibility.Hidden;

                videoPlayer2.Visibility = System.Windows.Visibility.Hidden;
                status = "stopped";
            }
        }

        // MainWindowがロードされたときに実行される処理
        private void videoPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            // 動画サイズの変更
            videoPlayer.Stretch = Stretch.Fill;
            videoPlayer2.Stretch = Stretch.Fill;

            videoPlayer.Play();
            videoPlayer2.Play();

        }


        // ここからwebView

        private async void InitializeWebView()
        {
            
            await webView1.EnsureCoreWebView2Async(null);

            webView1.NavigationCompleted += WebView_NavigationCompleted;

            string webUrl = "http://www.jp-weathernews.com/v/wl/33.49007/132.5411/q=%E6%84%9B%E5%AA%9B%E7%9C%8C%E5%A4%A7%E6%B4%B2%E5%B8%82&v=349881b93ee4055ec923e9c1211a1f11d99ee845f281fef0b7b5c6d7ea9fcd49&lang=ja&type=hour";
            webView1.Source = new Uri(webUrl);

            //ｎ秒ごとにブラウザを更新する
            //iniファイルから読み込んだ文字列の秒数をdouble型に変換
            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(double.Parse(data["web"]["reloadTime"])) };
            timer.Tick += (sender, e) =>
            {
                webView1.ExecuteScriptAsync($"location.reload();");
            };
            timer.Start();


            // WebView2コントロールのコアプロパティを取得します
            var webView2 = webView1.CoreWebView2;

            // webView2の設定
            // javaScriptの有効化や開発メニューの無効化やコンテキストの無効化など
            webView2.Settings.IsBuiltInErrorPageEnabled = false;
            webView2.Settings.AreDefaultContextMenusEnabled = false;
            webView2.Settings.IsStatusBarEnabled = false;
            webView2.Settings.IsZoomControlEnabled = false;
            webView2.Settings.AreDevToolsEnabled = false;
            webView2.Settings.AreDefaultScriptDialogsEnabled = false;
            webView2.Settings.AreHostObjectsAllowed = false;
            webView2.Settings.IsScriptEnabled = true;
            webView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
            webView2.Settings.AreDefaultScriptDialogsEnabled = false;

        }



        private async void WebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {

            
            // 特定の要素までスクロール
            await webView1.ExecuteScriptAsync($"document.getElementById('flick_list').scrollIntoView();");
            // トピック欄を削除する(サイトの仕様で二回リムーブしないと消えない)
            await webView1.ExecuteScriptAsync($"document.getElementById('sub').remove();");
            await webView1.ExecuteScriptAsync($"document.getElementById('sub').remove();");
            // 実質的なセンタリング(id要素のstyle要素をjsで強制的に変更)
            await webView1.ExecuteScriptAsync($"document.getElementById('main').style.left = '-150px';");
            await webView1.ExecuteScriptAsync($"document.getElementById('main').style.position = 'relative';");
            // 天気テーブルの横幅を変更して余白を消すための記述
            // 拡大機能も追加
            await webView1.ExecuteScriptAsync(@"
                const styleElement = document.createElement('style');
                styleElement.innerText = '@media screen and (min-width: 770px) { .wTable { width: 1326px !important; } }';
                document.head.appendChild(styleElement);
                //要素拡大部分
                var element = document.getElementById('card_forecast');
                element.style.transform = 'scale(1.110)';

            ");


            // 全国 愛媛県 大洲市 →他の都市を見る この部分の位置調整
            await webView1.ExecuteScriptAsync(@"
                Array.from(document.getElementsByClassName('block-ann')).forEach(function(element) {
                    element.style.position = 'relative';
                    element.style.top = '-15px';
                    element.style.left = '30px';
                });
                

            ");


            // 天気テーブルの邪魔なスクロールバー削除
            await webView1.ExecuteScriptAsync(@"
                let styleElement1 = document.createElement('style');
                const cssCode = `
                    .wTable__body::-webkit-scrollbar {
                  display: none;
                };`
                styleElement1.appendChild(document.createTextNode(cssCode));
                document.head.appendChild(styleElement1);
                ");


            // 疑似的なadBlock
            // Google adsの削除には対応
            await webView1.ExecuteScriptAsync(@"
                    
                var intervalId = setInterval(function() {
                  var elements = document.getElementsByTagName('*');
                  var foundElements = false;

                  for (var i = elements.length - 1; i >= 0; i--) {
                    var element = elements[i];

                    if (
                      (element.nodeType === Node.ELEMENT_NODE && element.textContent.includes('ads')) ||
                      (element.id && element.id.includes('ads'))
                    ) {
                      element.parentNode.removeChild(element);
                      foundElements = true;
                    }
                  }

                  if (!foundElements) {
                    clearInterval(intervalId);
                  }
                }, 10000);
                ");
        }



        public ICommand OpenSettingsCommand => new RelayCommand(OpenSettings);

        public bool ExitApp { get => _exitApp; set => SetProperty(ref _exitApp, value); }
        public string URL { get => _url; set => SetProperty(ref _url, value); }

        private void OpenSettings()
        {
            // ここのnew <> のところを変更すれば開く画面を選択できる。
            // powerToVideoLoadingWindows
            // SettingsWindow
            var settingsWindow = new SettingsWindow { Owner = this, DataContext = this };
            settingsWindow.ShowDialog();

            if (ExitApp)
                System.Windows.Application.Current.Shutdown();
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion



}
}