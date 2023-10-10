using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// iniファイルの読み書きにいる
using IniParser;
using IniParser.Model;
// ObservableCollectionを使用するため
using System.Collections.ObjectModel;

// 正規表現で使用
using System.Text.RegularExpressions;

namespace pptTester
{

    public class videoPlayListDB
    {
        // プレイリストのindex
        public int PlayListIndex { get; set; }
        // 動画の名前
        public string videoName { get; set; }
        // 動画のパス
        public string VideoPath { get; set; }

        // コンストラクタ
        public videoPlayListDB(int index, string name, string path)
        {
            PlayListIndex = index;
            videoName = name;
            VideoPath = path;

        }
    }

        /// <summary>
        /// videoSettingPage.xaml の相互作用ロジック
        /// </summary>
        public partial class videoSettingPage : Page
    {
        // プレイリスト要素を設定画面から追加したり削除するためのリスト
        // ObservableCollectionを使用することで動的にリスト要素をUiから追加することができる
        ObservableCollection<videoPlayListDB> videoPlayListItems = new ObservableCollection<videoPlayListDB>();

        // iniファイル読み込み
        // 相対パスはソリューション名.exeがある場所が基準になっている
        // ソリューション名.slnがある場所ではないので注意
        static String ini_path = "setting.ini";
        static FileIniDataParser parser = new FileIniDataParser();
        static IniData data = parser.ReadFile(ini_path);
        // iniファイルから読み込むsection
        String sectionName = "";
        // ここに代入される値が"video"か"pptx"かで処理を分岐させる。プロパティは設定メイン画面のsettingsWindow.xaml.csから代入されう
        public static String fileType { get; set; }


        public videoSettingPage()
        {
             InitializeComponent();
            //WindowState = WindowState.Maximized;
            playListLoad(fileType: fileType);
        }

        // iniファイルに保存されているプレイリスト情報を初期ロードする。
        private void playListLoad(string fileType)
        {

            int videoCount = 0;
            if (fileType == "video")
            {
                sectionName = "video";
            }
            else if (fileType == "pptx")
            {
                sectionName = "video2";
            }

            while (data[sectionName].ContainsKey($"path{(videoCount + 1)}"))
            {
                string videoPath = data[sectionName][$"path{(videoCount + 1)}"];
                string videoName = System.IO.Path.GetFileNameWithoutExtension(videoPath);
                videoCount += 1;
                videoPlayListItems.Add(new videoPlayListDB(videoCount, videoName, videoPath));
            }

            //DataGridに行を追加
            playListDB.ItemsSource = videoPlayListItems;

        }


        // プレイリストの要素を追加する用のメソッド
        private void playListVideoAdd_Click(object sender, RoutedEventArgs e)
        {
            // プレイリストの要素数を取得して+=1することでindexの追加を正常にできるようにしている
            // プレイリスト要素を削除したときにindexが自動で修正されない問題はある20230820時点
            int listLength = videoPlayListItems.Count();
            int addNextIndex = listLength + 1;


            videoPlayListItems.Add(new videoPlayListDB(addNextIndex, "動画名", "動画パス"));

            data[sectionName][$"path{addNextIndex}"] = "a";
            // 指定したパスのiniファイルにdataを書き込む
            parser.WriteFile(ini_path, data);

            playListDB.ItemsSource = videoPlayListItems;

            // 要素をaddしたらその要素まで移動する(スクロールバー位置を変動させている)
            if (playListDB.Items.Count > 0)
            {
                playListDB.SelectedItem = playListDB.Items[playListDB.Items.Count - 1];

                // 選択されたアイテムを表示
                playListDB.ScrollIntoView(playListDB.SelectedItem);


            }
        }

        // プレイリストの要素を削除する用のメソッド
        private void playListVideoDelete_Click(object sender, RoutedEventArgs e)
        {
            var tag = ((Button)sender).Tag as videoPlayListDB;
            videoPlayListItems.Remove(tag);
        }


    }
}
