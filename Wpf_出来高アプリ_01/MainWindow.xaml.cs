using DekiDaka_Data;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data;
using System.Collections;
using Wpf_Dekidaka_app.Bind;
using System.Threading.Tasks;
using System.Threading;
using System.Text;

namespace Wpf_Dekidaka_app
{





    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// タッチイベントの不具合解消の為のクラスのインスタンス
        /// </summary>
        private TouchEventProcessor tp = new TouchEventProcessor();

        /// <summary>
        /// 出来高データのコレクション、全ての出来高データの本体
        /// </summary>
        private ObservableCollection<Dekidaka_Data> DekiDakaDataCollection;

        /// <summary>
        /// メインウィンドウのデータコンテキスト
        /// </summary>
        public MwContext mwContext = new MwContext();

        /// <summary>
        /// データの個数をなんとなくカウントする
        /// </summary>
        public int iDataCollectionIndex = 0;

        /// <summary>
        /// メインウィンドウから開ける唯一の入力ウィンドウのインスタンス用
        /// </summary>
        private InputWindow cw = null;

        /// <summary>
        /// 一時データのパス
        /// </summary>
        private string tempfilepath = Settings.PreferenceFilePath;

        /// <summary>
        /// 一時データのファイル名
        /// </summary>
        private string tempfilename = "temp.csv";

        /// <summary>
        /// バックアップファイルの世代数を保持する
        /// </summary>
        private int BackupSaveCount = 0;


        //private System.Media.SoundPlayer TouchSound;

        //private Settings Preference  = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {

            //Preference = Settings;
            //Preference.LoadSetting();

            InitializeComponent();

            ////日付を取得してDatePickerにセットする
            //InputDate = System.DateTime.Today;
            //this.TodayDate.SelectedDate = InputDate;

            
            DataContext = mwContext;

            //コンテキストの初期値設定
            mwContext.mwWriteMember = "名前を入力";
            mwContext.mwInputDate = System.DateTime.Today;
            this.TodayDate.SelectedDate = mwContext.mwInputDate;

            
            DekiDakaDataCollection = new ObservableCollection<Dekidaka_Data>();

            //MessageBox.Show(DekiDakaDataCollection.Count.ToString());

            if (System.IO.File.Exists(tempfilepath + "\\" + tempfilename))
            {

                //データをロードする
                Load_Data(tempfilepath + "\\" + tempfilename);


            }
            else
            {

                // 初期データ生成する
                iDataCollectionIndex = 1;

                DekiDakaDataCollection.Add(new Dekidaka_Data(0));

            }


            


            // DataGridに設定する
            this.dataGrid.ItemsSource = DekiDakaDataCollection;



            

        }

        /// <summary>
        /// 出来高データを読み込むする
        /// </summary>
        /// <param name="filepath">データファイルのパス</param>
        public void Load_Data(string filepath)
        {

            
            //ファイルを開いて中身をストリームに読み込む
            string csvstream = "";

            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("Shift_JIS");
            System.IO.StreamReader csvfile;

            try
            {
                csvfile = new System.IO.StreamReader(filepath, enc);
                csvstream = csvfile.ReadToEnd();

                csvfile.Close();

            }
            catch (Exception)
            {
                MessageBox.Show(filepath + " が開けません");

                return;


            }

            
            //ストリームのCsvをパースしてArrayListにする

            ArrayList csvData = new ArrayList();
            csvData = CSVTool.CSVTool.CsvToArrayList2(csvstream);

            int HeaderRow = 0;

            int csvRow = csvData.Count;
            
            ArrayList csvField = new ArrayList();

            csvField = (ArrayList)csvData[0];

            //テンポラリファイルだと、最初の行は日付や記入者のデータなのでそれを読み出してセットする
            if((string)csvField[0] == "得意先")
            {
                HeaderRow = 0;
            }
            else
            {
                HeaderRow = 1;

                mwContext.mwWriteMember = (string)csvField[1];
                mwContext.mwPartGroupNo = int.Parse((string)csvField[2]);
                mwContext.mwPartNumber = int.Parse((string)csvField[3]);

                if (int.TryParse((string)csvField[4], out BackupSaveCount))
                { BackupSaveCount = (BackupSaveCount + 1) % Settings.BackupSaveGeneration; }
                else
                { BackupSaveCount = 0; }


            }

            //最初の初期化中じゃなければコレクション末尾の新規作成用のデータを削除する
            if(DekiDakaDataCollection.Count > 0)
            {
                DekiDakaDataCollection.RemoveAt(DekiDakaDataCollection.Count - 1);

            }


            for (int i = 0; i < csvRow - 1 - HeaderRow; i++)
            {

                //コレクションに新規データを追加する
                DekiDakaDataCollection.Add(new Dekidaka_Data());

                //今のインデックス番号を保持する（末尾に追加されたので末尾のインデックス）
                int DekidakaIndex = DekiDakaDataCollection.Count - 1;

                //IDをインデックスに合わせる
                DekiDakaDataCollection[DekidakaIndex].iUID = DekidakaIndex;

                //フィールドからデータを読み出してセットする
                csvField = (ArrayList)csvData[i + 1 + HeaderRow];
                //string ftext = (string)csvField[0];


                DekiDakaDataCollection[DekidakaIndex].strCustomar = (string)csvField[0];
                DekiDakaDataCollection[DekidakaIndex].strWriteMember = (string)csvField[1];
                DekiDakaDataCollection[DekidakaIndex].iPartGroup = int.Parse((string)csvField[2]);
                DekiDakaDataCollection[DekidakaIndex].iPartNumber = int.Parse((string)csvField[3]);
                DekiDakaDataCollection[DekidakaIndex].strContentsOfWork = (string)csvField[4];
                DekiDakaDataCollection[DekidakaIndex].strCommodity = (string)csvField[5];
                DekiDakaDataCollection[DekidakaIndex].strProductionArea = (string)csvField[6];
                DekiDakaDataCollection[DekidakaIndex].bKind = (( (string)csvField[7]) == "True" ? true : false );
                DekiDakaDataCollection[DekidakaIndex].strSize = (string)csvField[8];
                DekiDakaDataCollection[DekidakaIndex].dtStartTime = DateTime.Parse((string)csvField[9]);
                DekiDakaDataCollection[DekidakaIndex].dtEndTime  = DateTime.Parse((string)csvField[10]);

                DekiDakaDataCollection[DekidakaIndex].iOutputQuantity(int.Parse((string)csvField[11]), 0);
                DekiDakaDataCollection[DekidakaIndex].iOutputQuantity(int.Parse((string)csvField[12]), 1);
                DekiDakaDataCollection[DekidakaIndex].iOutputQuantity(int.Parse((string)csvField[13]), 2);
                DekiDakaDataCollection[DekidakaIndex].iOutputQuantity(int.Parse((string)csvField[14]), 3);
                DekiDakaDataCollection[DekidakaIndex].iOutputQuantity(int.Parse((string)csvField[15]), 4);
                DekiDakaDataCollection[DekidakaIndex].iOutputQuantity(int.Parse((string)csvField[16]), 5);

                DekiDakaDataCollection[DekidakaIndex].iOutputNumber(int.Parse((string)csvField[17]), 0);
                DekiDakaDataCollection[DekidakaIndex].iOutputNumber(int.Parse((string)csvField[18]), 1);
                DekiDakaDataCollection[DekidakaIndex].iOutputNumber(int.Parse((string)csvField[19]), 2);
                DekiDakaDataCollection[DekidakaIndex].iOutputNumber(int.Parse((string)csvField[20]), 3);
                DekiDakaDataCollection[DekidakaIndex].iOutputNumber(int.Parse((string)csvField[21]), 4);
                DekiDakaDataCollection[DekidakaIndex].iOutputNumber(int.Parse((string)csvField[22]), 5);

                DekiDakaDataCollection[DekidakaIndex].strMaterials((string)csvField[23], 0);
                DekiDakaDataCollection[DekidakaIndex].strMaterials((string)csvField[24], 1);
                DekiDakaDataCollection[DekidakaIndex].strMaterials((string)csvField[25], 2);
                DekiDakaDataCollection[DekidakaIndex].strMaterials((string)csvField[26], 3);
                DekiDakaDataCollection[DekidakaIndex].strMaterials((string)csvField[27], 4);
                DekiDakaDataCollection[DekidakaIndex].strMaterials((string)csvField[28], 5);

                DekiDakaDataCollection[DekidakaIndex].iMaterialsNumber(int.Parse((string)csvField[29]), 0);
                DekiDakaDataCollection[DekidakaIndex].iMaterialsNumber(int.Parse((string)csvField[30]), 1);
                DekiDakaDataCollection[DekidakaIndex].iMaterialsNumber(int.Parse((string)csvField[31]), 2);
                DekiDakaDataCollection[DekidakaIndex].iMaterialsNumber(int.Parse((string)csvField[32]), 3);
                DekiDakaDataCollection[DekidakaIndex].iMaterialsNumber(int.Parse((string)csvField[33]), 4);
                DekiDakaDataCollection[DekidakaIndex].iMaterialsNumber(int.Parse((string)csvField[34]), 5);




                //初期のファイルには存在しないフィールド
                if(csvField.Count > 35) { 
                    DekiDakaDataCollection[DekidakaIndex].bMulti = (((string)csvField[35]) == "True" ? true : false);
                }

                if(csvField.Count > 36) { 
                    DekiDakaDataCollection[DekidakaIndex].iArrival = int.Parse((string)csvField[36]);
                }

                if (csvField.Count > 37)
                {
                    DekiDakaDataCollection[DekidakaIndex].straShipment[0] = (string)csvField[37];
                    DekiDakaDataCollection[DekidakaIndex].straShipment[1] = (string)csvField[38];
                    DekiDakaDataCollection[DekidakaIndex].straShipment[2] = (string)csvField[39];
                    DekiDakaDataCollection[DekidakaIndex].straShipment[3] = (string)csvField[40];
                    DekiDakaDataCollection[DekidakaIndex].straShipment[4] = (string)csvField[41];
                    DekiDakaDataCollection[DekidakaIndex].straShipment[5] = (string)csvField[42];


                }


                //編集フラグを立てておく
                DekiDakaDataCollection[DekidakaIndex].bActionable = true;
                //インデックスカウントを増やしておく
                iDataCollectionIndex++;


            }

            //新規データ用の空データを追加する
            DekiDakaDataCollection.Add(new Dekidaka_Data(iDataCollectionIndex));



        }


        private void Edit_TouchUp(object sender, TouchEventArgs e)
        {

            EventProcess Prs = EditDataAsync;

            tp.button_TouchUp(sender, e, Prs);




        }


        //private void Edit_Click(object sender, MouseButtonEventArgs e)
        //{
        //    EditData(sender);
        //}
        //private void Edit_Click(object sender, RoutedEventArgs e)
        //{
        //    EditData(sender);


        //}

        private void Button_Edit_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("Pre");
            EditDataAsync(sender);
        }


        private MessageBoxResult ShowMessageDlg(string message)
        {
            this.Grid_Opa.Visibility = Visibility.Visible;

            OKCancelDlg re = new OKCancelDlg(message);

            re.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            re.ShowDialog();

            this.Grid_Opa.Visibility = Visibility.Collapsed;

            return re.result;

        }


        /// <summary>
        /// 入力用の子ウィンドウを開いて出来高データを入力する。変更されたら一時データとバックアップを保存する
        /// </summary>
        /// <param name="sender">呼び出し元編集ボタンオブジェクト</param>
        private async void EditDataAsync(object sender)
        {

            if(cw != null) { return; } //子ウィンドウのポインタがnullじゃなければ（子ウィンドウが開いていれば）なにもせず戻る

            //呼び出し元ボタンから、tagに紐付けられた行のitemを取得
            Dekidaka_Data Row = ((Button)sender).Tag as Dekidaka_Data;
            

            if(Row == null) { return; }//asキャストに失敗したら戻る

            int id = Row.iUID;

            //元の編集状態（未作成か作成済みか）を保持する
            bool bEdited = Row.bActionable;


            //タッチイベントから呼んだときには選択行がボタンの行にないときがあるので、選択行をデータのインデックスに合わせる
            dataGrid.SelectedItem = Row;

            if (mwContext.mwWriteMember == "" || mwContext.mwWriteMember == "名前を入力" || mwContext.mwPartNumber == 0)
            {

                string message = "記入者名と人数を設定して下さい。";

                ShowMessageDlg(message);

                return ;


            }

            cw = new InputWindow(Row);

            //記入者がnullならメインウィンドウの記入者データを入力
            if (cw.Original.strWriteMember == null)
            {
                cw.ReturnValue.strWriteMember = mwContext.mwWriteMember;
            }

            //班番号が無ければメインウィンドウの班番号を記入
            if (cw.Original.iPartGroup == 0)
            {
                cw.ReturnValue.iPartGroup = mwContext.mwPartGroupNo;
            }

            //人数が無ければメインウィンドウの人数を記入
            if (cw.Original.iPartNumber == 0)
            {
                cw.ReturnValue.iPartNumber = mwContext.mwPartNumber;
            }


            //cw.Closed += new EventHandler(cw_Closed);

            //cw.Topmost = true;
            //cw.Show();




            //this.Hide();

            //入力ダイアログを開く
            cw.ShowDialog();


            //this.Show();


            Row.strOutput = ""; //更新を通知
            Row.iOutputSubTotal = 0; //更新を通知

            //元の編集状態が未編集で、編集済みで返ってきた場合は新しい行を追加する
            if (bEdited == false && Row.bActionable == true)
            {

                DekiDakaDataCollection.Add(new Dekidaka_Data(++iDataCollectionIndex));

            }

            //元の編集状態が編集済みで、未編集で返ってきた場合はアイテムを削除する
            if (bEdited == true && Row.bActionable == false)
            {
                DekiDakaDataCollection.Remove(Row);

            }





            // 一時データを保存する
            if (cw.IsModified)
            {

                //子ウィンドウを解放
                cw = null;


                StateWindow StateW = new StateWindow("データを保存しています");

                StateW.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                StateW.Topmost = true;
                StateW.Show();

                int iState = 0;

                await Task.Run(() =>{ iState = SaveTempData(); } );

                StateW.Close();
                StateW = null;

                if (iState != 1)
                { 
                    string message = string.Format("一時保存に失敗しました({0})", iState == 10 ? "一時パスアクセス不可" : "バックアップパスアクセス不可" );

                    ShowMessageDlg(message);
                }

            }
            else
            {
                //子ウィンドウを解放
                cw = null;
            }




        }



        /// <summary>
        /// 一時データとバックアップデータを保存する
        /// </summary>
        /// <returns>1 = 成功 10=一時ファイルパスが見つからない 11=バックアップフォルダが見つからない</returns>
        private int SaveTempData()
        {

            DataTable TempData;

            TempData = GetDekidakaDataTable();

            //Thread.Sleep(3000);

            //string inputdate = mwContext.mwInputDate.ToShortDateString();
            //inputdate = inputdate.Replace('/', '_');

            string filename = tempfilepath + "\\" + tempfilename;

            if (System.IO.File.Exists(tempfilepath + "\\" + tempfilename) != true)
            {

                if (Settings.CreateDirectory(tempfilepath) != true)
                {
                    //MessageBox.Show(tempfilepath + " の作成に失敗しました");
                    return 10;
                }
                //System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(tempfilepath);
            }



            //CSVファイルに書き込むときに使うEncoding
            System.Text.Encoding enc =
                System.Text.Encoding.GetEncoding("Shift_JIS");

            //書き込むファイルを開く
            System.IO.StreamWriter sr =
                new System.IO.StreamWriter(filename, false, enc);


            //班番号、日付、記入者、人数などのデータを追加する
            StringBuilder sb = new StringBuilder(300);
            sb.Append(mwContext.mwInputDate.ToShortDateString() + "," + mwContext.mwWriteMember + "," + mwContext.mwPartGroupNo.ToString() + "," + mwContext.mwPartNumber.ToString() + "," + BackupSaveCount.ToString() + ",");

            // カンマを追加する　iの初期値は記入済み項目数の次の項目数　（現在5項目記入しているので6)
            for (int i = 6; i < TempData.Columns.Count; i++)
            {
                sb.Append(",");
            }
            //改行を追加する
            sb.Append("\r\n");
            //書き込む
            sr.Write(sb.ToString());



            sr.Close();

            //出来高データをCSVとして書き込む
            CSVTool.CSVTool.ConvertDataTableToCsv(TempData, filename, true, true);


            //バックアップデータを保存する
            if (Settings.BackupFilePath != "")
            {
                string inputdate = mwContext.mwInputDate.ToShortDateString();
                inputdate = inputdate.Replace('/', '_');

                string backfilename = "出来高バックアップ" + inputdate + "_" + mwContext.mwPartGroupNo.ToString() + "_" + BackupSaveCount.ToString() + ".csv";

                filename = Settings.BackupFilePath + "\\" + backfilename;

                if (System.IO.File.Exists(filename) != true)
                {

                    if (Settings.CreateDirectory(Settings.BackupFilePath) != true)
                    {
                        //MessageBox.Show(Settings.BackupFilePath + " の作成に失敗しました");
                        return 11;
                    }
                    //System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(tempfilepath);
                }


                CSVTool.CSVTool.ConvertDataTableToCsv(TempData, filename, true, false);

                BackupSaveCount = (BackupSaveCount + 1) % Settings.BackupSaveGeneration;


            }

            return 1;




        }

        /// <summary>
        /// 出来高データを設定したフォルダに保存する
        /// </summary>
        /// <param name="sender">呼び出し元ボタンオブジェクト</param>
        public void SaveCsvData(object sender)
        {

            string message = "現在の出来高を保存しますか？";


            if(ShowMessageDlg(message) == MessageBoxResult.Cancel)
            { return; }


            StateWindow StateW = new StateWindow("データを保存しています");

            StateW.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            StateW.Topmost = true;

            StateW.Show();

            DataTable TempData;

            TempData = GetDekidakaDataTable();


            string inputdate = mwContext.mwInputDate.ToShortDateString();
            inputdate = inputdate.Replace('/', '_');

            string filename = "出来高_" + inputdate + "_" + mwContext.mwPartGroupNo.ToString() + "班.csv";

            //string filepath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + "\\出来高";

            string filepath = Settings.SaveFilePath;


            //設定の出力先ディレクトリを調べて作成、メッセージを変更
            message = "出来高が設定先に保存されました。";

            if (System.IO.Directory.Exists(filepath) != true)
            {
                filepath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + "\\出来高";
                message = "出来高がパブリックフォルダに保存されました";

                if (System.IO.Directory.Exists(filepath) != true)
                {

                    if(Settings.CreateDirectory(filepath) != true)
                    {
                        message = "書き込み先フォルダが見つかりません";
                        ShowMessageDlg(message);
                        return;
                    }

                }
            }


            filename = filepath + "\\" + filename;


            //CSVとして保存する
            if (CSVTool.CSVTool.ConvertDataTableToCsv(TempData, filename, true,false) == false)
            {
                message = "出来高の保存に失敗しました。";
            }


            StateW.Close();
            StateW = null;



            //結果のメッセージを表示
            ShowMessageDlg(message);



            return;

        }

        /// <summary>
        /// 現在の出来高データをDataTableオブジェクトにして返す
        /// </summary>
        /// <returns>出来高データDataTable</returns>
        private DataTable GetDekidakaDataTable()
        {

            DataTable TempData;

            TempData = new DataTable();

            int DataNumber = DekiDakaDataCollection.Count - 1;


            //for (int i = 0; i < 35; i++)
            //{

            TempData.Columns.Add("得意先");
            TempData.Columns.Add("記入者");
            TempData.Columns.Add("班番号");
            TempData.Columns.Add("パート人数");
            TempData.Columns.Add("作業内容");
            TempData.Columns.Add("品名");
            TempData.Columns.Add("産地");
            TempData.Columns.Add("果実");
            TempData.Columns.Add("サイズ");
            TempData.Columns.Add("開始時間");
            TempData.Columns.Add("終了時間");

            TempData.Columns.Add("出来高入数0");
            TempData.Columns.Add("出来高入数1");
            TempData.Columns.Add("出来高入数2");
            TempData.Columns.Add("出来高入数3");
            TempData.Columns.Add("出来高入数4");
            TempData.Columns.Add("出来高入数5");

            TempData.Columns.Add("出来高個数0");
            TempData.Columns.Add("出来高個数1");
            TempData.Columns.Add("出来高個数2");
            TempData.Columns.Add("出来高個数3");
            TempData.Columns.Add("出来高個数4");
            TempData.Columns.Add("出来高個数5");

            TempData.Columns.Add("資材名0");
            TempData.Columns.Add("資材名1");
            TempData.Columns.Add("資材名2");
            TempData.Columns.Add("資材名3");
            TempData.Columns.Add("資材名4");
            TempData.Columns.Add("資材名5");

            TempData.Columns.Add("資材数0");
            TempData.Columns.Add("資材数1");
            TempData.Columns.Add("資材数2");
            TempData.Columns.Add("資材数3");
            TempData.Columns.Add("資材数4");
            TempData.Columns.Add("資材数5");

            TempData.Columns.Add("複数日");
            TempData.Columns.Add("入荷数");

            TempData.Columns.Add("出荷日0");
            TempData.Columns.Add("出荷日1");
            TempData.Columns.Add("出荷日2");
            TempData.Columns.Add("出荷日3");
            TempData.Columns.Add("出荷日4");
            TempData.Columns.Add("出荷日5");



            //}

            for (int i = 0; i < DataNumber; i++)
            {

                DataRow row = TempData.NewRow();

                row[0] = DekiDakaDataCollection[i].strCustomar;
                row[1] = DekiDakaDataCollection[i].strWriteMember;
                row[2] = DekiDakaDataCollection[i].iPartGroup;
                row[3] = DekiDakaDataCollection[i].iPartNumber;
                row[4] = DekiDakaDataCollection[i].strContentsOfWork;
                row[5] = DekiDakaDataCollection[i].strCommodity;
                row[6] = DekiDakaDataCollection[i].strProductionArea;
                row[7] = DekiDakaDataCollection[i].bKind.ToString();
                row[8] = DekiDakaDataCollection[i].strSize;
                row[9] = DekiDakaDataCollection[i].dtStartTime.ToShortTimeString();
                row[10] = DekiDakaDataCollection[i].dtEndTime.ToShortTimeString();

                row[11] = DekiDakaDataCollection[i].iaOutputQuantity[0];
                row[12] = DekiDakaDataCollection[i].iaOutputQuantity[1];
                row[13] = DekiDakaDataCollection[i].iaOutputQuantity[2];
                row[14] = DekiDakaDataCollection[i].iaOutputQuantity[3];
                row[15] = DekiDakaDataCollection[i].iaOutputQuantity[4];
                row[16] = DekiDakaDataCollection[i].iaOutputQuantity[5];

                row[17] = DekiDakaDataCollection[i].iaOutputNumber[0];
                row[18] = DekiDakaDataCollection[i].iaOutputNumber[1];
                row[19] = DekiDakaDataCollection[i].iaOutputNumber[2];
                row[20] = DekiDakaDataCollection[i].iaOutputNumber[3];
                row[21] = DekiDakaDataCollection[i].iaOutputNumber[4];
                row[22] = DekiDakaDataCollection[i].iaOutputNumber[5];

                row[23] = DekiDakaDataCollection[i].straMaterials[0];
                row[24] = DekiDakaDataCollection[i].straMaterials[1];
                row[25] = DekiDakaDataCollection[i].straMaterials[2];
                row[26] = DekiDakaDataCollection[i].straMaterials[3];
                row[27] = DekiDakaDataCollection[i].straMaterials[4];
                row[28] = DekiDakaDataCollection[i].straMaterials[5];

                row[29] = DekiDakaDataCollection[i].iaMaterialsNumber[0];
                row[30] = DekiDakaDataCollection[i].iaMaterialsNumber[1];
                row[31] = DekiDakaDataCollection[i].iaMaterialsNumber[2];
                row[32] = DekiDakaDataCollection[i].iaMaterialsNumber[3];
                row[33] = DekiDakaDataCollection[i].iaMaterialsNumber[4];
                row[34] = DekiDakaDataCollection[i].iaMaterialsNumber[5];

                row[35] = DekiDakaDataCollection[i].bMulti.ToString();
                row[36] = DekiDakaDataCollection[i].iArrival;

                row[37] = DekiDakaDataCollection[i].straShipment[0];
                row[38] = DekiDakaDataCollection[i].straShipment[1];
                row[39] = DekiDakaDataCollection[i].straShipment[2];
                row[40] = DekiDakaDataCollection[i].straShipment[3];
                row[41] = DekiDakaDataCollection[i].straShipment[4];
                row[42] = DekiDakaDataCollection[i].straShipment[5];


                TempData.Rows.Add(row);



            }


            return TempData;


        }


        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
         /*   switch (e.PropertyName)
            {
                case "bActionable":
                    // bActionableプロパティは表示しない
                    e.Cancel = true;
                    break;

                case "strCustomar":
                    // strCustomarプロパティは表示しない
                    e.Cancel = true;
                    break;


            }
         */

        }

        private void textBox_GroupNo_TouchDown(object sender, TouchEventArgs e)
        {
            
            //MessageBox.Show("ダブルクリック");

            //var txtbx_sender = (TextBox)sender;

            //string sendtext = txtbx_sender.Name;


            TenKeyBord sw = new TenKeyBord(mwContext.mwPartGroupNo);

            this.Grid_Opa.Visibility = Visibility.Visible;

            sw.ShowDialog();


            this.Grid_Opa.Visibility = Visibility.Collapsed;

            if (sw.result != -1) { mwContext.mwPartGroupNo = sw.result; }



        }

        private void textBox_GroupNo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            TenKeyBord sw = new TenKeyBord(mwContext.mwPartGroupNo);


            this.Grid_Opa.Visibility = Visibility.Visible;

            sw.ShowDialog();


            this.Grid_Opa.Visibility = Visibility.Collapsed;


            if (sw.result != -1) { mwContext.mwPartGroupNo = sw.result; }




        }

        private void textBox_PartNumber_TouchDown(object sender, TouchEventArgs e)
        {
            TenKeyBord sw = new TenKeyBord(mwContext.mwPartNumber);



            this.Grid_Opa.Visibility = Visibility.Visible;

            sw.ShowDialog();


            this.Grid_Opa.Visibility = Visibility.Collapsed;


            if (sw.result != -1) { mwContext.mwPartNumber = sw.result; }

        }

        private void textBox_PartNumber_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            TenKeyBord sw = new TenKeyBord(mwContext.mwPartNumber);


            this.Grid_Opa.Visibility = Visibility.Visible;

            sw.ShowDialog();


            this.Grid_Opa.Visibility = Visibility.Collapsed;


            if (sw.result != -1) { mwContext.mwPartNumber = sw.result; }

        }

        private void WriteMember_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            TextBox bx = (TextBox)sender;
            var sw = new SelectWindow(bx.Name, mwContext.mwWriteMember);
            sw.swContext.bIsInputButtonEnable =  true;


            sw.ShowDialog();


            if (sw.ReturnText != null)
            {
                mwContext.mwWriteMember = sw.ReturnText;
            }



            

        }

        private void WriteMember_TouchDown(object sender, TouchEventArgs e)
        {

            TextBox bx = (TextBox)sender;
            var sw = new SelectWindow(bx.Name, mwContext.mwWriteMember);
            sw.swContext.bIsInputButtonEnable = true;


            sw.ShowDialog();


            if (sw.ReturnText != null)
            {
                
                mwContext.mwWriteMember = sw.ReturnText;
            }



           



        }

        private void button_send_Click(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("mouse");


            SaveCsvData(sender);



        }

        private void button_send_TouchUp(object sender, TouchEventArgs e)
        {
            //MessageBox.Show("touch");

            EventProcess Prs = SaveCsvData;

            tp.button_TouchUp(sender, e, Prs);



        }

        private void button_Clear_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = Clear_DataAsync;

            tp.button_TouchUp(sender, e, Prs);


        }

        private void button_Clear_Click(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("mouse");
            Clear_DataAsync(sender);

        }

        /// <summary>
        /// 現在の出来高データを消去して、日付やパネルデータも読み直す
        /// </summary>
        /// <param name="sender">呼び出し元ボタンオブジェクト</param>
        private async void Clear_DataAsync(object sender)
        {

            string message = "現在の出来高を消去しますか？";


            if (ShowMessageDlg(message) == MessageBoxResult.Cancel)
            { return; }

            ConfirmationWindow conf = new ConfirmationWindow("クリア");

            conf.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            this.Grid_Opa.Visibility = Visibility.Visible;

            conf.ShowDialog();

            this.Grid_Opa.Visibility = Visibility.Collapsed;


            if (conf.result == false)
            { return; }


            DekiDakaDataCollection.Clear();
            DekiDakaDataCollection.Add(new Dekidaka_Data(0));
            iDataCollectionIndex = 0;

            //コンテキストの初期値設定
            mwContext.mwWriteMember = "名前を入力";
            mwContext.mwInputDate = System.DateTime.Today;
            this.TodayDate.SelectedDate = mwContext.mwInputDate;
            mwContext.mwPartNumber = 0;

            ModuleData.ModuleDataSetup();

            StateWindow StateW = new StateWindow("データを保存しています");

            StateW.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            StateW.Topmost = true;

            StateW.Show();

            //一時データを保存
            await Task.Run(() => { SaveTempData(); }  );

            StateW.Close();
            StateW = null;

        }

        private void button_Exit_Click(object sender, RoutedEventArgs e)
        {
            buttom_Exit_Press(sender);
        }

        //private void button_Exit_Click(object sender, MouseButtonEventArgs e)
        //{

        //    buttom_Exit_Press(sender);


        //}

        private void button_Exit_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = buttom_Exit_Press;

            tp.button_TouchUp(sender, e, Prs);


        }


        /// <summary>
        /// 確認してアプリを終了する
        /// </summary>
        /// <param name="sender"></param>
        private void buttom_Exit_Press(object sender)
        {

            string message = "アプリを終了しますか？";


            if (ShowMessageDlg(message) == MessageBoxResult.Cancel)
            { return; }

            this.Close();


        }


        private void button_setting_Click(object sender, MouseButtonEventArgs e)
        {

            MainWindow mw = this;

            PreferenceWindow pw = new PreferenceWindow(mw);

            pw.Topmost = true;

            this.Grid_Opa.Visibility = Visibility.Visible;

            pw.ShowDialog();

            this.Grid_Opa.Visibility = Visibility.Collapsed;



        }

        private void button_setting_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = obj =>
            {
                MainWindow mw = this;

                PreferenceWindow pw = new PreferenceWindow(mw);

                pw.Topmost = true;

                this.Grid_Opa.Visibility = Visibility.Visible;

                pw.ShowDialog();

                this.Grid_Opa.Visibility = Visibility.Collapsed;

            };

            tp.button_TouchUp(sender, e, Prs);


        }




        private void TodayDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            DatePicker d = (DatePicker)sender;
            mwContext.mwInputDate = DateTime.Parse(d.Text);



            //MessageBox.Show(mwContext.mwInputDate.ToShortDateString());

        }


        private void button_TouchDown(object sender, TouchEventArgs e)
        {

            tp.button_TouchDown(sender, e);

        }


        private void button_Print_TouchUp(object sender, TouchEventArgs e)
        {
            
            EventProcess Prs = Button_Print_Press;

            tp.button_TouchUp(sender, e, Prs);
        }

        private void Button_Print_Click(object sender, MouseButtonEventArgs e)
        {
            Button_Print_Press(sender);
        }

        /// <summary>
        /// テロップを印刷する
        /// </summary>
        /// <param name="sender">呼び出し元印刷ボタン</param>
        private  void Button_Print_Press(object sender)
        {

            //ボタンからtagに紐付けられたitemを取得する
            Dekidaka_Data Row = ((Button)sender).Tag as Dekidaka_Data;

            if (Row == null) { return; }//asキャストに失敗したら戻る



            //内容を確認するダイアログ
            string message = Row.strCustomar + "の" + Row.strCommodity + Row.strContentsOfWork + "のテロップを印刷しますか？";


            if (ShowMessageDlg(message) == MessageBoxResult.Cancel)
            { return; }



            //出来高が未入力なら確認する
            if (Row.strOutput == null)
            {
                message = "出来高が入力されていませんがよろしいですか？";


                if (ShowMessageDlg(message) == MessageBoxResult.Cancel)
                { return; }
            }


            StateWindow StateW = new StateWindow("印刷データ作成中");

            StateW.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //StateW.Topmost = false;

            StateW.Show();

            //テロップ印刷オブジェクトを作成
            PrintingTelop Pt = new PrintingTelop(Row);

            //現在の出来高データでテロップを設定
            Pt.TelopSetting();

            //StateW.cx.strDisplayMessage = "プリンタに送信中";


            //印刷する
            Pt.Print();


            //印刷ボタンの色を変える
            Row.SetPrintBottonColor(0);

            StateW.Close();
            StateW = null;



        }


    } 
}
