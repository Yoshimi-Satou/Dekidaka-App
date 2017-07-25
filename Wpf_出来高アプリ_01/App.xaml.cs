using System;
using System.Collections;
using System.Threading;
using System.Windows;

namespace Wpf_Dekidaka_app
{


    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {

        private const string SettingPath = "\\Sanko\\出来高入力アプリ";

        /// <summary>
        /// アプリケーションが終了する時のイベント。
        /// </summary>
        /// <param name="e">イベント データ。</param>
        protected override void OnExit(ExitEventArgs e)
        {


            if (App._mutex == null) { return; }

            // アプリケーション設定の保存
            Settings.SaveSetting();

            // ミューテックスの解放
            App._mutex.ReleaseMutex();
            App._mutex.Close();
            App._mutex = null;
        }

        /// <summary>
        /// アプリケーションが開始される時のイベント。
        /// </summary>
        /// <param name="e">イベント データ。</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // 多重起動チェック
            App._mutex = new Mutex(false, "{0A8C044C-77AB-422D-A2E2-89F47D9FB2E1}");
            if (!App._mutex.WaitOne(0, false))
            {
                MessageBox.Show("既に起動しています");
                App._mutex.Close();
                App._mutex = null;
                this.Shutdown();
                return;
            }


            //状況表示
            StateWindow StateW = new StateWindow("設定の読み込み中…");
            StateW.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            StateW.Show();

                //設定の読み出し
            Settings.PreferenceFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + SettingPath;
            if(Settings.LoadSetting() == false)
            {
                Settings.InitializeSetting();
            }

            StateW.cx.strDisplayMessage = "パネルの読み込み中…";

            ModuleData.ModuleDataSetup();



            // メイン ウィンドウ表示
            MainWindow window = new MainWindow();
            window.Show();

            //状況表示クローズ
            StateW.Close();
            StateW = null;

        }

        /// <summary>
        /// 多重起動を防止する為のミューテックス。
        /// </summary>
        private static Mutex _mutex;



    }
}
