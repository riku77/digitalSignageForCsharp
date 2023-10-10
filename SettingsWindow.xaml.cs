using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using WindowsAPICodePack.Dialogs;

// iniファイルの読み書きにいる
using IniParser;
using IniParser.Model;
// ObservableCollectionを使用するため
using System.Collections.ObjectModel;

// 正規表現で使用
using System.Text.RegularExpressions;


namespace pptTester
{




        public partial class SettingsWindow : Window
    {



        public SettingsWindow()
        {
            this.InitializeComponent();
            //WindowState = WindowState.Maximized;


        }


        private void goToVideoSetting(object sender, RoutedEventArgs e)
        {
            // フレームが履歴を持っているとメモリリークするので毎回削除する (あんまり履歴を使わない気がするから無いものとしても問題ないはず)
            while (contentFrame.CanGoBack)
            {
                contentFrame.RemoveBackEntry();
            }

            // videoSettingPageのプロパティに値を代入して処理分岐している
            // これのおかげでvideo設定用とpptx設定用のpage2つを用意する必要がない
            // もしpageを個別に用意する場合、処理がほぼほぼ一緒なので全く同じコードを管理することになってスマートじゃない
            videoSettingPage.fileType = "video";
            contentFrame.Navigate(new videoSettingPage());
        }

        private void goToPptxSetting(object sender, RoutedEventArgs e)
        {
            
            while (contentFrame.CanGoBack)
            {
                contentFrame.RemoveBackEntry();
            }

            videoSettingPage.fileType = "pptx";
            contentFrame.Navigate(new videoSettingPage());
        }


    }
}