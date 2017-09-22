using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Xml;


namespace Wpf_Dekidaka_app
{

    /// <summary>
    /// 設定を管理するクラス
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// 果実のデフォルト設定
        /// </summary>
        private const string strFruits = "有田みかん,愛媛みかん,青島みかん,みかん,ポンカン,デコポン,不知火,金柑,せとか,清美,マーコット,ミネオラ,オレンジ,マンダリン,タンカン,スイートスプリング,れいこう,はるか,はまさき,紅香,津の輝,せとみ,サンクィン,タンゴール,伊予柑,八朔,甘夏,サンフルーツ,グレープフルーツ,文旦,ブラッドオレンジ,セミノール,リンゴ,ふじ,サンふじ,早生ふじ,つがる,王林,ジョナゴールド,シナノゴールド,シナノスイート,紅玉,昂林,千秋,トキ,レッドゴールド,ひめかみ,ハックナイン,梨,幸水,豊水,新高,二十世紀,あきづき,新興,南水,親水,長十郎,愛宕,晩三吉,洋梨,ラ・フランス,ル・レクチェ,バートレット,オーロラ,バラード,プレコース,葡萄,巨峰,デラウェア,ピオーネ,キャンベル,ナイアガラ,スチューベン,シャインマスカット,ナガノパープル,瀬戸ジャイアンツ,甲斐路,ゴルビー,旅路,柿,庄内柿,富士柿,富有柿,イチゴ,アメリカンチェリー,さくらんぼ,プラム,太陽,貴陽,サマービュート,サマーエンジェル,ソルダム,大石早生,桃,黄桃";

        /// <summary>
        /// 日付欄追加項目のデフォルト設定
        /// </summary>
        private const string strOutputShipment = "３Ｓ ２Ｓ Ｓ Ｍ Ｌ ２Ｌ ３Ｌ 外品 先出し分 後出し分 大 小 ２Ｐ ３Ｐ ４Ｐ ５Ｐ Ａ Ｂ";

        /// <summary>
        /// 設定ファイル名
        /// </summary>
        private const string PreferenceFileName = "Preference.csv";

        /// <summary>
        /// プリンター設定ファイル名
        /// </summary>
        private const string PrinterSettingFileName = "PrinterSetting.xml";


        /// <summary>
        /// 設定ファイルのパスを設定する
        /// </summary>
        public static string PreferenceFilePath = "";

        /// <summary>
        /// プリント設定を保持する
        /// </summary>
        public static PrintDialog PrintDlg = null;


        public static int _BackupSaveGeneration = 3;
        /// <summary>
        /// バックアップファイルの世代数を設定する 1から10までの値に制限される
        /// </summary>
        public static int BackupSaveGeneration
        {
            get { return _BackupSaveGeneration; }

            set
            {
                if (value < 1)
                {
                    _BackupSaveGeneration = 1;
                }
                else if(value > 10)
                {
                    _BackupSaveGeneration = 10;
                }
                else
                {
                    _BackupSaveGeneration = value;
                }

            }

        }






        private static bool _IsInitialized = false;
        /// <summary>
        /// 初期化済みフラグ
        /// trueならすべての設定値に何らかの数値が代入されている
        /// </summary>
        public static bool IsInitialized
        {
            get { return _IsInitialized; }

            set { ;}

        }
             

        /// <summary>
        /// 出来高を保存するフォルダのパス
        /// </summary>
        public static string SaveFilePath
        {
            get;
            set;

        }

        /// <summary>
        /// 出来高を保存するバックアップフォルダのパス
        /// </summary>
        public static string BackupFilePath
        {
            get;
            set;

        }

        /// <summary>
        /// 横向き印刷をするかどうか
        /// </summary>
        public static bool PrintLandscape
        {
            get;
            set;

        }

        /// <summary>
        /// 出力プリンターの名前
        /// </summary>
        public static string PrinterName
        {
            get;
            set;

        }

        /// <summary>
        /// 印刷設定
        /// </summary>
        public static PrintTicket PrintTicketSetting
        {
            get;
            set;

        }

        /// <summary>
        /// フルーツ種別の定義の為のCSV文字列
        /// </summary>
        public static string csvStreamFruit
        {
            get;
            set;

        }

        /// <summary>
        /// パネルデータ記録用CSV文字列
        /// </summary>
        public static string csvStreamPanelData
        {
            get;
            set;

        }

        /// <summary>
        /// 日付欄のパネルデータ
        /// </summary>
        public static string OutputShipment
        {
            get;
            set;
        }


        //コンストラクタ
        //public Settings(SettingFilePath)
        //{
        //    SaveFilePath = "";
        //    BackupFilePath = "";
        //    PrintLandscape = false;

        //    PreferenceFilePath = SettingFilePath;
        //}

        /// <summary>
        /// 設定を初期化する
        /// </summary>
        public static void InitializeSetting()
        {
            SaveFilePath = "";
            BackupFilePath = "";
            PrintLandscape = false;
            PrinterName = "";
            PrintTicketSetting = null;
            _BackupSaveGeneration = 3;

            csvStreamFruit = strFruits;
            csvStreamPanelData = "";
            OutputShipment = strOutputShipment;

            _IsInitialized = true;



        }



        /// <summary>
        /// 設定をファイルから読み込み復元する
        /// </summary>
        /// <returns>成功:true 失敗:false</returns>
        public static bool LoadSetting()
        {




            string path = PreferenceFilePath + "\\" + PreferenceFileName;

            string csvstream = "";

            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("Shift_JIS");
            System.IO.StreamReader csvfile;

            try
            {
                csvfile = new System.IO.StreamReader(path, enc);
                csvstream = csvfile.ReadToEnd();

                csvfile.Close();

            }
            catch (Exception)
            {
                //MessageBox.Show(path + " が開けません");

                //設定ファイルが開けなかったらfalseを返して失敗を通知する
                return false;


            }



            //ファイルが開けるならデフォルトの値をセット
            InitializeSetting();


            //CSVから設定データを読み出す
            List<List<string>> csvData = new List<List<string>>();
            csvData = CSVTool.CSVTool.CsvToList(csvstream);


            int csvRow = csvData.Count;


            foreach (List<string> Field in csvData)
            {
                switch(Field[0])
                {

                    case "SaveFilePath" :
                        SaveFilePath = Field[1];
                        break;

                    case "BackupFilePath":
                        BackupFilePath = Field[1];
                        break;

                    case "PrintLandscape":
                        PrintLandscape = bool.Parse(Field[1]);
                        break;

                    case "PrinterName":
                        PrinterName = Field[1];
                        break;

                    case "csvStreamFruit":
                        csvStreamFruit = Field[1];
                        break;

                    case "csvStreamPanelData":
                        csvStreamPanelData = Field[1];
                        break;

                    case "BackupSaveGeneration":
                        _BackupSaveGeneration = int.Parse(Field[1]);
                        break;

                    case "OutputShipment":
                        OutputShipment = Field[1];
                        break;

                }

            }





            // プリンター設定を読み込む

            MemoryStream ms = new MemoryStream();


            //読み込むファイルを開く
            path = PreferenceFilePath + "\\" + PrinterSettingFileName;

            int b;

            try
            {

                //ファイルの内容を読み出してメモリーストリームに書き込む
                System.IO.FileStream sw = new System.IO.FileStream(path, FileMode.Open);

                while ((b = sw.ReadByte()) != -1)
                {
                    ms.WriteByte((byte)b);

                }


                //メモリーストリームでプリントチケットを初期化する

                ms.Position = 0;
                PrintTicketSetting = new PrintTicket(ms);

                //ファイルを閉じる
                sw.Close();
                ms.Close();



            }
            catch (Exception)
            {
                //MessageBox.Show(path + " が開けません");

                ms.Close();

                //PrinterSettingはオプションなのでFalseを返さない

            }


            //ロード済みフラグを立てる
            _IsInitialized = true;

            return true;

        }

        /// <summary>
        /// 現在の設定をファイルに保存する
        /// </summary>
        public static void SaveSetting()
        {

            string path = PreferenceFilePath + "\\" + PreferenceFileName;


            // 設定のデータ定義と実体化
            DataTable TempData;
            TempData = new DataTable();


            // 設定のヘッダ
            TempData.Columns.Add("SettingName");
            TempData.Columns.Add("SettingValue");



            //設定の本体を追加する

            DataRow row = TempData.NewRow();

            //SaveFilePath
            row[0] = "SaveFilePath";
            row[1] = SaveFilePath;

            TempData.Rows.Add(row);

            //BackupFilePath
            row = TempData.NewRow();

            row[0] = "BackupFilePath";
            row[1] = BackupFilePath;

            TempData.Rows.Add(row);

            //PrintLandscape
            row = TempData.NewRow();

            row[0] = "PrintLandscape";
            row[1] = PrintLandscape.ToString();

            TempData.Rows.Add(row);

            //PrinterName
            row = TempData.NewRow();

            row[0] = "PrinterName";
            row[1] = PrinterName;

            TempData.Rows.Add(row);

            //csvStreamFruit
            row = TempData.NewRow();

            row[0] = "csvStreamFruit";
            row[1] = csvStreamFruit;

            TempData.Rows.Add(row);

            //csvStreamPanelData
            row = TempData.NewRow();

            row[0] = "csvStreamPanelData";
            row[1] = csvStreamPanelData;

            TempData.Rows.Add(row);

            //BackupSaveGeneration
            row = TempData.NewRow();

            row[0] = "BackupSaveGeneration";
            row[1] = _BackupSaveGeneration.ToString();

            TempData.Rows.Add(row);

            //OutputShipment
            row = TempData.NewRow();

            row[0] = "OutputShipment";
            row[1] = OutputShipment;

            TempData.Rows.Add(row);



            //設定ファイルを保存するフォルダを調べて、無ければ作成する

            if (System.IO.File.Exists(path) != true)
            {

                if (CreateDirectory(PreferenceFilePath) != true)
                {
                    MessageBox.Show(path + " の作成に失敗しました");
                    return;
                }
            }


            //CSVとして設定ファイルを保存する
            CSVTool.CSVTool.ConvertDataTableToCsv(TempData, path, true, false);




            //プリンター設定をメモリーストリームに取得する

            if (PrintDlg != null)
            {
                MemoryStream ms = PrintDlg.PrintTicket.GetXmlStream();

                ms.Position = 0;

                //書き込むファイルを開く
                path = PreferenceFilePath + "\\" + PrinterSettingFileName;

                int b;

                try
                {

                    //メモリーストリームの内容を読み出してファイルに書き込む
                    System.IO.FileStream sw = new System.IO.FileStream(path, FileMode.Create);

                    while ((b = ms.ReadByte()) != -1)
                    {
                        sw.WriteByte((byte)b);

                    }


                    //ファイルを閉じる
                    sw.Close();
                    ms.Close();

                }
                catch (Exception)
                {
                    //MessageBox.Show(path + " が開けません");

                    ms.Close();

                    return;


                }
            }





        }

        /// <summary>
        /// ディレクトリを作成する
        /// </summary>
        /// <param name="Path">作成したいディレクトリパス</param>
        /// <returns>true:成功 false:失敗</returns>
        public static bool CreateDirectory(string Path)
        {

            try
            {

                System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(Path);

                return true;

            }
            catch (Exception)
            {
                //MessageBox.Show(path + " が開けません");

                return false;


            }



        }



    }
}
