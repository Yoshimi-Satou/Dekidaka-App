using Microsoft.WindowsAPICodePack.Dialogs;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf_Dekidaka_app.Bind;

namespace Wpf_Dekidaka_app
{
    /// <summary>
    /// PreferenceWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class PreferenceWindow : Window
    {

        public PreferenceContext pwContext = new PreferenceContext();

        private string PrinterName = "";

        private PrintDialog dPrt = null;

        private MainWindow Main = null;

        /// <summary>
        /// タッチイベント処理用オブジェクト
        /// </summary>
        private TouchEventProcessor tp = new TouchEventProcessor();
        

        public PreferenceWindow(MainWindow mw = null)
        {


            InitializeComponent();

            if(mw != null) { Main = mw; }


            pwContext.pwSaveFolderPath = Settings.SaveFilePath;
            pwContext.pwBackFolderPath = Settings.BackupFilePath;
            pwContext.pwPrintLandscape = Settings.PrintLandscape;
            PrinterName = Settings.PrinterName;
            pwContext.iBackUpGen = Settings.BackupSaveGeneration - 1;



            if (PrinterName != "" && Settings.PrintTicketSetting != null)
            {
                dPrt = new PrintDialog();
   
                LocalPrintServer ps = new LocalPrintServer();
                dPrt.PrintQueue = ps.GetPrintQueue(PrinterName);
                dPrt.PrintTicket = Settings.PrintTicketSetting;

            }

            DataContext = pwContext;


        }

        private void button_DekidakaSaveFolder_Click(object sender, RoutedEventArgs e)
        {
            button_DekidakaSaveFolder_Press(sender);


        }

        private void button_DekidakaSaveFolder_TouchUp(object sender, TouchEventArgs e)
        {

            EventProcess Prs = button_DekidakaSaveFolder_Press;

            tp.button_TouchUp(sender, e, Prs);

        }

        private void button_DekidakaSaveFolder_Press(object sender)
        {
            this.Topmost = false;

            var dlg = new CommonOpenFileDialog("出来高データ保存フォルダ選択");
            // フォルダ選択モード。
            dlg.IsFolderPicker = true;

            if (pwContext.pwSaveFolderPath != "")
            {
                dlg.InitialDirectory = pwContext.pwSaveFolderPath;
            }


            var ret = dlg.ShowDialog();
            if (ret == CommonFileDialogResult.Ok)
            {
                pwContext.pwSaveFolderPath = dlg.FileName;

            }

            this.Topmost = true;


        }


        private void button_DekidakaBackFolder_Click(object sender, RoutedEventArgs e)
        {

            button_DekidakaBackFolder_Press(sender);

        }



        private void button_DekidakaBackFolder_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = button_DekidakaBackFolder_Press;

            tp.button_TouchUp(sender, e, Prs);
        }


        private void button_DekidakaBackFolder_Press(object sender)
        {

            this.Topmost = false;

            var dlg = new CommonOpenFileDialog("バックアップデータ保存フォルダ選択");
            // フォルダ選択モード。
            dlg.IsFolderPicker = true;

            if (pwContext.pwBackFolderPath != "")
            {
                dlg.InitialDirectory = pwContext.pwBackFolderPath;
            }


            var ret = dlg.ShowDialog();
            if (ret == CommonFileDialogResult.Ok)
            {
                pwContext.pwBackFolderPath = dlg.FileName;

            }

            this.Topmost = true;

        }



        private void button_DekidakaBackFolder_Clear_Click(object sender, RoutedEventArgs e)
        {
            pwContext.pwBackFolderPath = "";

        }

        private void button_DekidakaBackFolder_Clear_TouchUp(object sender, TouchEventArgs e)
        {

            EventProcess Prs = obj =>
            {
                pwContext.pwBackFolderPath = "";
            };

            tp.button_TouchUp(sender, e, Prs);



        }



        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {

            this.Close();

        }


        private void button_Cancel_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = obj =>
            {
                this.Close();
            };

            tp.button_TouchUp(sender, e, Prs);

        }


        private void button_Submit_Click(object sender, RoutedEventArgs e)
        {

            button_Submit_Press(sender);

        }


        private void button_Submit_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = button_Submit_Press;

            tp.button_TouchUp(sender, e, Prs);
        }

        private void button_Submit_Press(object sender)
        {

            //ファイル保存関連設定
            Settings.SaveFilePath = pwContext.pwSaveFolderPath;
            Settings.BackupFilePath = pwContext.pwBackFolderPath;

            //印刷設定
            Settings.PrintLandscape = pwContext.pwPrintLandscape;
            Settings.PrinterName = PrinterName;

            if (dPrt != null)
            {
                Settings.PrintTicketSetting = dPrt.PrintTicket;
                Settings.PrintDlg = dPrt;
            }

            Settings.BackupSaveGeneration = pwContext.iBackUpGen + 1;

            //設定を保存する
            Settings.SaveSetting();


            this.Close();

        }



        private void button_Printer_Setting_Click(object sender, RoutedEventArgs e)
        {

            button_Printer_Setting_Press(sender);



        }

        private void button_Printer_Setting_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = button_Printer_Setting_Press;

            tp.button_TouchUp(sender, e, Prs);
        }


        private void button_Printer_Setting_Press(object sender)
        {

            if (dPrt == null)
            {
                dPrt = new PrintDialog();
            }


            this.Topmost = false;

            bool response = (bool)dPrt.ShowDialog();

            if (response)
            {
                PrinterName = dPrt.PrintQueue.FullName;
            }

            this.Topmost = true;



        }

        private void checkBox_Print_Landscape_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void checkBox_Print_Landscape_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void button_LoadDekidaka_Click(object sender, RoutedEventArgs e)
        {
            button_LoadDekidaka_Press(sender);

        }

        private void button_LoadDekidaka_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = button_LoadDekidaka_Press;

            tp.button_TouchUp(sender, e, Prs);

        }

        private void button_LoadDekidaka_Press(object sender)
        {
            if (Main == null) { return; }

            this.Topmost = false;

            var dlg = new CommonOpenFileDialog("出来高ファイル選択");
            // フォルダ選択モード。
            dlg.IsFolderPicker = false;

            //if (pwContext.pwBackFolderPath != "")
            //{
            //    dlg.InitialDirectory = pwContext.pwBackFolderPath;
            //}


            var ret = dlg.ShowDialog();
            if (ret == CommonFileDialogResult.Ok)
            {

                //確認ダイアログを表示
                string message = "出来高を読み込んで末尾に追加します。\nよろしいですか？";

                OKCancelDlg re = new OKCancelDlg(message);

                re.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                re.ShowDialog();

                if (re.result == MessageBoxResult.Cancel) { this.Topmost = true; return; }

                Main.Load_Data(dlg.FileName);
                Main.SaveTempData();

            }

            this.Topmost = true;



        }



        private void button_TouchDown(object sender, TouchEventArgs e)
        {

            tp.button_TouchDown(sender, e);


        }



    }
}
