using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DekiDaka_Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Wpf_Dekidaka_app
{


    /// <summary>
    /// InputWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class InputWindow : Window
    {

        /// <summary>
        /// 返す出来高データ(=本体データのコピー)
        /// </summary>
        public Dekidaka_Data ReturnValue;


        /// <summary>
        /// 変更前のデータ(=本体データへの参照)
        /// </summary>
        public Dekidaka_Data Original;

        /// <summary>
        /// 選択ウィンドウオブジェクト
        /// </summary>
        private SelectWindow sw = null;

        /// <summary>
        /// タッチイベント処理用オブジェクト
        /// </summary>
        private TouchEventProcessor tp = new TouchEventProcessor();

        /// <summary>
        /// データが変更された時に立つフラグ
        /// </summary>
        public bool IsModified = false;

        /// <summary>
        /// 品名に対する果実の種別が変更されたときに立つフラグ
        /// </summary>
        private bool IsFruitModified = false;
        private string strModifiedFruit = "";


        private Brush SelectedBg = new SolidColorBrush(Color.FromRgb(0xFF, 0xD8, 0xD8));

        /// <summary>
        /// 入力ウィンドウを初期化する
        /// </summary>
        /// <param name="Dekidaka">出来高データ</param>
        public InputWindow(Dekidaka_Data Dekidaka)
        {

            Original = Dekidaka;
            ReturnValue = Dekidaka.Clone();

            InitializeComponent();

            //Window Main = Application.Current.MainWindow;

            DataContext = ReturnValue;

            //出来高のイベントが変更された時に呼び出されるイベントを登録
            //ReturnValue.PropertyChanged += DekidakaPropertyChanged;

            //日付欄の初期化
            this.OutputShipment0.IsEnabled = ReturnValue.bMulti;
            this.OutputShipment1.IsEnabled = ReturnValue.bMulti;
            this.OutputShipment2.IsEnabled = ReturnValue.bMulti;
            this.OutputShipment3.IsEnabled = ReturnValue.bMulti;
            this.OutputShipment4.IsEnabled = ReturnValue.bMulti;
            this.OutputShipment5.IsEnabled = ReturnValue.bMulti;
            this.textBlock_Shipment.IsEnabled = ReturnValue.bMulti;

            //this.Rect_Customer.Fill = new SolidColorBrush(Color.FromRgb(0xF4, 0xF4, 0xF5));

            //ReturnValue.



        }




        /// <summary>
        /// 出来高のプロパティが変更された時のイベント
        /// </summary>
        /// <param name="sender">Dekidakaオブジェクト</param>
        /// <param name="e">イベントパラメータ</param>
        private void DekidakaPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //資材の変更時
            if (e.PropertyName == "straMaterials")
            {
                //SerchMaterials(sender);


            }


            //発注者・品名・作業内容の変更時
            if (e.PropertyName == "strCustomar" || e.PropertyName == "strCommodity" || e.PropertyName == "strContentsOfWork")
            {


            }





        }


        /// <summary>
        /// 発注先/品名/作業内容が変更された時に呼ばれ、マッチする資材を探す
        /// </summary>
        /// <param name="sender">出来高データ</param>
        private void SerchCCC(Dekidaka_Data sender)
        {

            Dekidaka_Data Data = sender as Dekidaka_Data;

            //nullなら帰る
            if (Data == null) { return; }


            //発注者・品名・作業内容に空白があるなら帰る
            if (Data.strContentsOfWork == "" || Data.strContentsOfWork == null
            || Data.strCommodity == "" || Data.strCommodity == null
            || Data.strCustomar == "" || Data.strCustomar == null)
            { return; }

            
            //処理中はイベントを解除
            //ReturnValue.PropertyChanged -= DekidakaPropertyChanged;

            
            //データが読み込まれていれば
            if (ModuleData.Materials.Array != null)
            {
                //Listの行数と列数をもらう
                List<string> csvField = new List<string>();
                csvField = ModuleData.Materials.Array[0];

                int rows = ModuleData.Materials.Array.Count;
                int cols = csvField.Count;
                
                int MatelialstrCol = 5;
                int MatelialnumCol = 6;



                //項目を見ていく
                for (int i = 1; i < rows;i++)
                {
                    csvField = ModuleData.Materials.Array[i];


                    //作業内容を見る
                    if (!IsMached(CheckRegEx(csvField[2]), Data.strContentsOfWork == null ? "" : Data.strContentsOfWork)) { continue; }

                    //品名を見る
                    if (!IsMached(CheckRegEx(csvField[1]), Data.strCommodity == null ? "" : Data.strCommodity)) { continue; }

                    //得意先がマッチするか見る
                    if ( !IsMached(CheckRegEx(csvField[0]) , Data.strCustomar == null ? "" : Data.strCustomar)) { continue; }
                        
                    //サイズを見る
                    if ( !IsMached(CheckRegEx(csvField[3]), Data.strSize == null ? "" : Data.strSize)) { continue; }

                    //産地を見る
                    if ( !IsMached(CheckRegEx(csvField[4]), Data.strProductionArea == null ? "" : Data.strProductionArea)) { continue; }


                    //全てマッチしたら資材と数を入力する
                    for (int j = 0; j < 3; j++)
                    {
                        string MateName = csvField[MatelialstrCol + j * 2];

                        if (MateName == "") { break; }


                        //既に同じ資材がないかチェックする
                        bool IsExist = false;

                        for (int materials_count = 0; materials_count < 6; materials_count++)
                        {

                            if (Data.straMaterials[materials_count] == MateName)
                            {
                                IsExist = true;
                                break;
                            }

                        }

                        if (IsExist) { continue; }

                        for (int materials_count = 0; materials_count < 6; materials_count++)
                        {
                            if (Data.straMaterials[materials_count] == "" || Data.straMaterials[materials_count] == null)
                            {
                                //資材名
                                Data.strMaterials(MateName, materials_count);

                                //数
                                int mnum;
                                if(int.TryParse(csvField[MatelialnumCol + j * 2], out mnum))
                                { Data.iMaterialsNumber(mnum, materials_count); }
                                else { Data.iMaterialsNumber(1, materials_count); }
                                                

                                break;

                            }

                        }


                    } //for j

                    //ループを終える
                    //break;


                } //for i
                

            }// if array

            
            //終わったらイベントを再登録
            //ReturnValue.PropertyChanged += DekidakaPropertyChanged;


        }


        /// <summary>
        /// 正規表現で文字列がマッチしたかどうかを判定する
        /// </summary>
        /// <param name="RegExString"></param>
        /// <param name="MatchString"></param>
        /// <returns></returns>
        private bool IsMached(string RegExString, string MatchString)
        {


            System.Text.RegularExpressions.Regex r =
            new System.Text.RegularExpressions.Regex(RegExString, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            //正規表現と一致する対象を1つ検索
            System.Text.RegularExpressions.Match m = r.Match(MatchString);

            return m.Success;


        }

        /// <summary>
        /// ^で始まらない文字列に "^(" と ")" を追加する
        /// </summary>
        /// <param name="RegExString"></param>
        /// <returns></returns>
        private string CheckRegEx(string RegExString)
        {
            
            if (!RegExString.StartsWith("^"))
            {
                return "^(" + RegExString + ")";

            }

            return RegExString;

        }





        /// <summary>
        /// 資材の変更時に呼ばれて、袋に対するクリップを自動的に配置する
        /// </summary>
        /// <param name="sender">通知元Dekidaka_Data</param>
        private void SerchMaterials(Dekidaka_Data sender, int index)
        {
            Dekidaka_Data Data = sender as Dekidaka_Data;

            if(Data == null) { return; }


            //処理中はイベントを解除
            //ReturnValue.PropertyChanged -= DekidakaPropertyChanged;

            if (ModuleData.FG.Array != null)
            {


                //Arraylistの行数と列数をもらう
                List<string> csvField = new List<string>();
                csvField = ModuleData.FG.Array[0];

                int rows = ModuleData.FG.Array.Count;
                int cols = csvField.Count;


                int flag = -1;

                
                //文字列として比較する
                //for (int materials_count = 0; materials_count < 6 && flag == -1; materials_count++)
                //{
                //    if(Data.straMaterials[materials_count] == "" || Data.straMaterials[materials_count] == null)
                //    {
                //        continue;
                //    }

                    for (int i = 0; i < rows && flag == -1; i++)
                    {
                        csvField = ModuleData.FG.Array[i];


                        for (int j = 1; j < cols; j++)
                        {

                            if (csvField[j] == ""){ break; }

                            if (csvField[j] == Data.straMaterials[index])
                            {
                                flag = i;
                                break;
                            }


                        }


                    }


                //}


                if(flag != -1)
                {
                    bool IsExist = false;
                    csvField = ModuleData.FG.Array[flag];

                    for (int materials_count = 0; materials_count < 6; materials_count++)
                    {
                     
                        if (Data.straMaterials[materials_count] == csvField[0])
                        {
                            IsExist = true;
                            break;
                        }

                    }

                    if (!IsExist)
                    {
                        for (int materials_count = index; materials_count < 6; materials_count++)
                        {
                            if (Data.straMaterials[materials_count] == "" || Data.straMaterials[materials_count] == null)
                            {

                                Data.strMaterials(csvField[0], materials_count);

                                break;
                            }

                        }
                    }


                }

                               
            }

            //終わったらイベントを再登録
            //ReturnValue.PropertyChanged += DekidakaPropertyChanged;

        }

        /// <summary>
        /// 果実に分類される品名かどうかを調べる
        /// </summary>
        /// <param name="sender"></param>
        private void SerchFruits(Dekidaka_Data sender)
        {
            Dekidaka_Data Data = sender as Dekidaka_Data;

            if (Data == null) { return; }


            //処理中はイベントを解除
            //ReturnValue.PropertyChanged -= DekidakaPropertyChanged;

            if (ModuleData.Fruits.Array != null)
            {


                //Listの行数と列数をもらう
                List<string> csvField = new List<string>();
                csvField = ModuleData.Fruits.Array[0];

                int cols = csvField.Count;

                int flag = -1;

                //文字列として比較する


                for (int j = 0; j < cols; j++)
                {

                    if (csvField[j] == "") { break; }

                    if (csvField[j] == Data.strCommodity)
                    {
                        flag = 1;
                        break;
                    }


                }

                if (flag == 1)
                {
                    Data.bKind = true;

                }
                else
                {
                    Data.bKind = false;

                }

                IsFruitModified = false;
                strModifiedFruit = "";



            }

            //終わったらイベントを再登録
            //ReturnValue.PropertyChanged += DekidakaPropertyChanged;

        }




        private void Button_Submit_Click(object sender, MouseButtonEventArgs e)
        {
            Button_Submit_Press(sender);
        }

        private void Button_Submit_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = Button_Submit_Press;

            tp.button_TouchUp(sender, e, Prs);


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
        /// 確定ボタンが押されたときの処理
        /// </summary>
        /// <param name="sender"></param>
        private void Button_Submit_Press(object sender)
        {

            //未入力箇所を調べる
            string message = "";

            if(ReturnValue.strCustomar == null || ReturnValue.strCustomar == "") { message = "発注者"; }
            if(ReturnValue.strCommodity == null || ReturnValue.strCommodity == "") { message = "品名"; }
            if(ReturnValue.strContentsOfWork == null || ReturnValue.strContentsOfWork == "") { message = "作業内容"; }
            if(ReturnValue.strEndTime == "0:00") { message = "終了時刻"; }
            if(ReturnValue.strOutput == null || ReturnValue.strOutput == "") { message = "出来高"; }

            if (message != "")
            {
                message = message + "が入力されていませんが続行しますか？";


                if (ShowMessageDlg(message) == MessageBoxResult.Cancel)
                { return; }

            }


            //未入力箇所が無いときは出来高に不正がないか調べる
            if (message == "")
            {

                for (int i = 0; i < 6; i++)
                {
                    if (ReturnValue.iaOutputQuantity[i] != 0 && ReturnValue.iaOutputNumber[i] == 1)
                    {
                        message = "OK";
                        break;

                    }

                }

                if (message != "OK")
                {

                    if (ShowMessageDlg("出来高に「○○ × 1」がありません。\n端数はありませんか？") == MessageBoxResult.Cancel)
                    { return; }


                }

            }

            //フルーツ種別が変更されていたら、設定のフルーツ種別を追加/削除する
            if(IsFruitModified)
            {
                if (ModuleData.Fruits.Array != null && strModifiedFruit != null && strModifiedFruit != "")
                {

                    List<string> csvField = new List<string>();
                    csvField = ModuleData.Fruits.Array[0];

                    int cols = csvField.Count;

                    int flag = 0;


                    var el =
                        from p in csvField
                        where p == strModifiedFruit
                        select p;

                    

                    if(el.Count<string>() != 0)
                    {
                        flag = 1;
                    }

                    //新規文字列かどうかを確認する
                    //for (int j = 0; j < cols; j++)
                    //{

                    //    if (csvField[j] == strModifiedFruit)
                    //    {
                    //        flag = 1;
                    //        break;
                    //    }

                    //}

                    
                    
                    if(ReturnValue.bKind)
                    {
                        if(flag == 0)
                        {
                            csvField.Add(strModifiedFruit);
                        }

                    }
                    else
                    {
                        if(flag == 1)
                        {
                            csvField.Remove(strModifiedFruit);

                        }


                    }

                    if(flag != -1)
                    {
                        //ArreyListをDataTableに変換してCsvStreamFruisを更新する
                        DataTable TempData = new DataTable();

                        foreach (var str in csvField )
                        {
                            TempData.Columns.Add(str);

                        }

                        DataRow row = TempData.NewRow();

                        for(int i = 0;i < csvField.Count;i++)
                        {
                            row[i] = csvField[i];

                        }

                        TempData.Rows.Add(row);

                        string strcsv = CSVTool.CSVTool.ConvertDataTableToCsvStream(TempData, false);



                        Settings.csvStreamFruit = strcsv;


                    }





                }

            }


            //入力済みフラグを立てる
            ReturnValue.bActionable = true;
            //IsModified = true;


            //出来高データ本体にデータをインポート
            Original.Data_Import(ReturnValue);


            //ReturnValue.PropertyChanged -= DekidakaPropertyChanged;

            this.Close();
        }



        private void button_Cancel_TouchUp(object sender, TouchEventArgs e)
        {

            EventProcess Prs = Button_Cancel_Press;

            tp.button_TouchUp(sender, e, Prs);
            

        }

        private void Button_Cancel_Click(object sender, MouseButtonEventArgs e)
        {

            Button_Cancel_Press(sender);


        }

        /// <summary>
        /// キャンセルボタンを押したときの処理
        /// </summary>
        /// <param name="sender"></param>
        private void Button_Cancel_Press(object sender)
        {

            string message = "現在の変更内容を破棄\nして戻りますか？";

            if (ShowMessageDlg(message) == MessageBoxResult.Cancel)
            { return; }


            //ReturnValue.Data_Import(Original);

            IsModified = false;

            //ReturnValue.PropertyChanged -= DekidakaPropertyChanged;

            this.Close();


        }


        private void button_Delete_TouchUp(object sender, TouchEventArgs e)
        {

            EventProcess Prs = Button_Delete_Press;

            tp.button_TouchUp(sender, e, Prs);


        }

        private void Button_Delete_Click(object sender, MouseButtonEventArgs e)
        {

            Button_Delete_Press(sender);


        }

        /// <summary>
        /// 削除ボタンが押された時の処理
        /// </summary>
        /// <param name="sender"></param>
        private void Button_Delete_Press(object sender)
        {


            string message = "内容が削除されますが\nよろしいですか？";

            if (ShowMessageDlg(message) == MessageBoxResult.Cancel)
            { return; }


            this.Grid_Opa.Visibility = Visibility.Visible;
            ConfirmationWindow conf = new ConfirmationWindow("削除");
            conf.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            conf.ShowDialog();

            this.Grid_Opa.Visibility = Visibility.Collapsed;

            if (conf.result == false) { return; }

            ReturnValue.bActionable = false;

            Original.Data_Import(ReturnValue);

            IsModified = true;

            //ReturnValue.PropertyChanged -= DekidakaPropertyChanged;


            this.Close();


        }


        /// <summary>
        /// 現在時刻ボタンが押されたら現在時刻を設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_StartTime_Click(object sender, MouseButtonEventArgs e)
        {
            ReturnValue.dtStartTime = DateTime.Now;
            IsModified = true;
        }

        private void button_StartTime_TouchUp(object sender, TouchEventArgs e)
        {

            EventProcess Prs = obj =>
            {

                ReturnValue.dtStartTime = DateTime.Now;
                IsModified = true;

            };

            tp.button_TouchUp(sender, e, Prs);


        }


        /// <summary>
        /// 現在時刻ボタンが押されたら現在時刻を設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_EndTime_Click(object sender, MouseButtonEventArgs e)
        {
            ReturnValue.dtEndTime = DateTime.Now;
            IsModified = true;
        }

        private void button_EndTime_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = obj =>
            {

                ReturnValue.dtEndTime = DateTime.Now;
                IsModified = true;

            };

            tp.button_TouchUp(sender, e, Prs);



        }




        /// <summary>
        /// タッチでの選択入力ウィンドウを出す
        /// </summary>
        /// <param name="sendtext">選択ウィンドウに表示する選択肢の種類</param>
        /// <param name="InputEnabled">自由入力が出来るかどうか</param>
        /// <param name="Text">自由入力欄に表示する文字列</param>
        /// <returns>押されたボタンの文字列</returns>
        private string TouchPanelSelectShow(string sendtext, bool InputEnabled = false, string Text = "")
        {
            //選択ウィンドウが既に実体化していたら戻る
            if (sw != null) { return null; }

            sw = new SelectWindow(sendtext, Text);
            sw.swContext.bIsInputButtonEnable = InputEnabled;


            sw.ShowDialog();


            string Returntext = null;

            if (sw.ReturnText != null)
            {
                Returntext = sw.ReturnText;
                IsModified = true;
            }


            //閉じて処理を終えたのでnullにする
            sw = null;

            //返ってきた文字列を返す
            return Returntext;


        }



        //得意先
        private void Customer_TouchUp(object sender, TouchEventArgs e)
        {

            

            TextBox bx = (TextBox)sender;

            string response = TouchPanelSelectShow(bx.Name, true, ReturnValue.strCustomar);

            if (response != null)
            {
                ReturnValue.strCustomar = response;
                if (response != "") { SerchCCC(ReturnValue); }

            }

        }


        private void Customer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            TextBox bx = (TextBox)sender;

            string response = TouchPanelSelectShow(bx.Name, true, ReturnValue.strCustomar);

            if (response != null)
            {
                ReturnValue.strCustomar = response;
                if (response != "") { SerchCCC(ReturnValue); }

            }
        }


        //品名
        private void Commodity_TouchUp(object sender, TouchEventArgs e)
        {
            
            Commodity_getresponse(sender);

        }

        private void Commodity_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            Commodity_getresponse(sender);


        }

        private void Commodity_getresponse(object sender)
        {
            //品名の返答をもらう
            string text = ReturnValue.strCommodity;

            if(DoubleRankResponse(sender, ref text))
            {
                ReturnValue.strCommodity = text;
                if (text != "")
                {
                    SerchCCC(ReturnValue);
                    SerchFruits(ReturnValue);
                }
            }

        }

        /// <summary>
        /// ２階層の返答をもらう
        /// 返り値 true = 入力か削除 false = 戻るで戻った
        /// </summary>
        /// <param name="sender">送信元オブジェクト</param>
        /// <param name="Textbox">返答をもらう文字列への参照</param>
        /// <param name="itemtext">パネルの種類指定、省略時はsenderのNameプロパティでセットされる</param>
        private bool DoubleRankResponse(object sender,ref string Textbox,string itemtext = "")
        {

            
            TextBox bx = (TextBox)sender;

            if (itemtext == "")
            { itemtext = bx.Name; }

            bool returnval = false;

            bool f = true;
            string response = null;


            while (f)
            {
                //1階層目
                response = TouchPanelSelectShow(itemtext, false, Textbox);

                if (response == "") //内容削除時
                {
                    Textbox = response;
                    f = false;
                    returnval = true;
                }
                else if (response == null) //戻るボタン
                {
                    //１階層目で戻ったのでループを抜けてfalseを返す
                    f = false;
                    returnval = false;
                }
                else //文字ボタン
                {
                    //２階層目
                    response = TouchPanelSelectShow(itemtext + ":" + response, true, Textbox);

                    if (response == "") //内容削除ボタン
                    {
                        Textbox = response;
                        f = false;
                        returnval = true;
                    }
                    else if (response == null) //戻るボタン
                    {
                        //なにもしないので１階層目に戻る
                    }
                    else //文字ボタン
                    {
                        Textbox = response;
                        f = false;
                        returnval = true;

                    }

                }

            }

            return returnval;

        }



        //産地
        private void ProductionArea_TouchUp(object sender, TouchEventArgs e)
        {

            
            TextBox bx = (TextBox)sender;

            string response = TouchPanelSelectShow(bx.Name, true, ReturnValue.strProductionArea);

            if (response != null)
            {
                ReturnValue.strProductionArea = response;
                if (response != "") { SerchCCC(ReturnValue); }
            }

        }

        private void ProductionArea_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox bx = (TextBox)sender;

            string response = TouchPanelSelectShow(bx.Name, true, ReturnValue.strProductionArea);

            if (response != null)
            {
                ReturnValue.strProductionArea = response;
                if (response != "") { SerchCCC(ReturnValue); }
            }

        }


        //作業内容
        private void ContentsOfWork_TouchUp(object sender, TouchEventArgs e)
        {
            

            ContentsOfWork_getresponse(sender);

        }

        private void ContentsOfWork_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            ContentsOfWork_getresponse(sender);
                

        }

        private void ContentsOfWork_getresponse(object sender)
        {
            ////作業内容の返答をもらう

            string text = ReturnValue.strContentsOfWork;

            if (DoubleRankResponse(sender, ref text))
            {
                ReturnValue.strContentsOfWork = text;
                if (text != "") { SerchCCC(ReturnValue); }

            }


        }


        //資材
        private void Materials_TouchUp(object sender, TouchEventArgs e)
        {

            

            Materials_getresponse(sender); //丸投げする

        }

        private void Materials_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            Materials_getresponse(sender); //丸投げする


        }

        private void Materials_getresponse(object sender)
        {
            ////資材NoのNameで識別して返答をセットする

            TextBox bx = (TextBox)sender;

            string itemtext = bx.Name.Substring(0, 9); //"Materials0" -> "Materials"
            int index = IndexSerch(bx); //"Materials0" -> "0"


            string text = ReturnValue.straMaterials[index];

            if (DoubleRankResponse(sender, ref text, itemtext))
            {
                ReturnValue.strMaterials(text,index);
                if(text != "") { SerchMaterials(ReturnValue,index); }
                else { ReturnValue.iMaterialsNumber(1, index); }
            }


        }



        /// <summary>
        /// テンキーボードを表示する
        /// </summary>
        /// <param name="value">初期表示の数字</param>
        /// <returns>入力結果</returns>
        private int TouchPanelTenkeyShow(int value = 0) 
        {

            this.Grid_Opa.Visibility = Visibility.Visible;


            //MessageBox.Show("ダブルクリック");

            //var txtbx_sender = (TextBox)sender;

            //string sendtext = txtbx_sender.Name;


            TenKeyBord tw = new TenKeyBord(value);


            //tw.Topmost = true;

            tw.ShowDialog();

            IsModified = true;

            this.Grid_Opa.Visibility = Visibility.Collapsed;


            return tw.result;


        }


        
        /// <summary>
        /// テキストボックスのNameプロパティから数字を検索して最初にマッチした数値を返す
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private int IndexSerch(TextBox sender)
        {

            int number = -1;

            System.Text.RegularExpressions.Regex r =
                new System.Text.RegularExpressions.Regex(@"[0-9]+", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            //TextBox1.Text内で正規表現と一致する対象を1つ検索
            System.Text.RegularExpressions.Match m = r.Match(sender.Name);

            if (m.Success)
            {
                int.TryParse(m.Value, out number);

            }

            return number;



        }

        private void OutputShipmext_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //OutputShipment


            string itemtext = "OutputShipment";
            int index = IndexSerch((TextBox)sender);


            string response = TouchPanelSelectShow(itemtext, true, ReturnValue.straShipment[index]);

            if (response != null)
            {
                ReturnValue.strShipment(response, index);

            }

        }

        private void OutputShipmext_TouchUp(object sender, TouchEventArgs e)
        {


            string itemtext = "OutputShipment";
            int index = IndexSerch((TextBox)sender);


            string response = TouchPanelSelectShow(itemtext, true, ReturnValue.straShipment[index]);

            if (response != null)
            {
                ReturnValue.strShipment(response, index);

            }

        }


        private void OutputQuantity_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            OutputQuantity_Press(sender);


        }

        private void OutputQuantity_TouchUp(object sender, TouchEventArgs e)
        {

            OutputQuantity_Press(sender);


        }

        private void OutputQuantity_Press(object sender)
        {

            TextBox tb = (TextBox)sender;

            Brush Bc = tb.Background;

            tb.Background = SelectedBg;

            int index = IndexSerch(tb);

            if (index == -1) { return; }

            int result = -1;

            result = TouchPanelTenkeyShow(ReturnValue.iaOutputQuantity[index]);



            if (result != -1)
            {
                ReturnValue.iOutputQuantity(result, index);
            }

            tb.Background = Bc;


        }


        private void OutputNumber_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OutputNumber_Press(sender);

        }

        private void OutputNumber_TouchUp(object sender, TouchEventArgs e)
        {

            OutputNumber_Press(sender);
        }

        private void OutputNumber_Press(object sender)
        {

            TextBox tb = (TextBox)sender;

            Brush Bc = tb.Background;

            tb.Background = SelectedBg;

            int index = IndexSerch(tb);

            if (index == -1) { return; }

            int result = -1;

            result = TouchPanelTenkeyShow(ReturnValue.iaOutputNumber[index]);



            if (result != -1)
            {
                ReturnValue.iOutputNumber(result, index);
            }

            tb.Background = Bc;

        }


        private void MaterialsNumber_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MaterialsNumber_Press(sender);

        }

        private void MaterialsNumber_TouchUp(object sender, TouchEventArgs e)
        {

            MaterialsNumber_Press(sender);

        }

        private void MaterialsNumber_Press(object sender)
        {

            TextBox tb = (TextBox)sender;

            Brush Bc = tb.Background;

            tb.Background = SelectedBg;

            int index = IndexSerch(tb);

            if (index == -1) { return; }

            int result = -1;

            result = TouchPanelTenkeyShow(ReturnValue.iaMaterialsNumber[index]);



            if (result != -1)
            {
                ReturnValue.iMaterialsNumber(result, index);

            }

            tb.Background = Bc;

        }


        private void StartTime_TouchUp(object sender, TouchEventArgs e)
        {


            TextBox tb = (TextBox)sender;

            Brush Bc = tb.Background;

            tb.Background = SelectedBg;

            DateTime result = GetInputTime(ReturnValue.dtStartTime);
            DateTime None = new DateTime(1, 1, 1);

            if(result != None)
            {
                ReturnValue.dtStartTime = result;
            }

            tb.Background = Bc;


        }

        private void EndTime_TouchUp(object sender, TouchEventArgs e)
        {

            TextBox tb = (TextBox)sender;

            Brush Bc = tb.Background;

            tb.Background = SelectedBg;

            DateTime result = GetInputTime(ReturnValue.dtEndTime);
            DateTime None = new DateTime(1, 1, 1);

            if (result != None)
            {
                ReturnValue.dtEndTime = result;
            }

            tb.Background = Bc;

        }

        /// <summary>
        /// 時刻入力ウィンドウを開いて返答をもらう
        /// </summary>
        /// <param name="dtTime"></param>
        /// <returns></returns>
        private DateTime GetInputTime(DateTime dtTime)
        {

            this.Grid_Opa.Visibility = Visibility.Visible;

            TimeInput tiw = new TimeInput(dtTime);

            //tiw.Topmost = true;

            tiw.ShowDialog();


            this.Grid_Opa.Visibility = Visibility.Collapsed;

            DateTime resultTime;

            if (tiw.strTime != null)
            {
                DateTime.TryParse(tiw.strTime, out resultTime);
                IsModified = true;
                return resultTime;
            }

            resultTime = new DateTime(1, 1, 1);
            return  resultTime;


        }

        private void GroupNo_TouchUp(object sender, TouchEventArgs e)
        {
            

            int result = TouchPanelTenkeyShow(ReturnValue.iPartGroup);

            if(result > -1 && result < 11)
            {
                ReturnValue.iPartGroup = result;

            }


        }

        private void PartNumber_TouchUp(object sender, TouchEventArgs e)
        {

            

            int result = TouchPanelTenkeyShow(ReturnValue.iPartNumber);

            if (result != -1)
            {
                ReturnValue.iPartNumber = result;

            }


        }

        private void WriteMember_TouchUp(object sender, TouchEventArgs e)
        {
            

            WriteMember_getresponse(sender); //丸投げ


        }

        /// <summary>
        /// 記入者の返答をもらう
        /// </summary>
        /// <param name="sender"></param>
        private void WriteMember_getresponse(object sender)
        {
            ////記入者の返答をもらう

            TextBox bx = (TextBox)sender;

            string itemtext = bx.Name;

            string response = TouchPanelSelectShow(itemtext, true, ReturnValue.strWriteMember);


            if (response != null)
            {
                ReturnValue.strWriteMember = response;

             }
        }

        private void Size_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Size_getresponse(sender);

        }

        private void Size_TouchUp(object sender, TouchEventArgs e)
        {

            
            Size_getresponse(sender);

        }

        private void Size_getresponse(object sender)
        {
            ////サイズの返答をもらう

            TextBox bx = (TextBox)sender;

            string itemtext = bx.Name;

            string response = TouchPanelSelectShow(itemtext, true, ReturnValue.strSize);


            if (response != null)
            {
                ReturnValue.strSize = response;
                if (response != "") { SerchCCC(ReturnValue); }
            }
        }



        private void checkBox_Fruit_Checked(object sender, RoutedEventArgs e)
        {
            //IsModified = true;
        }

        private void checkBox_Fruit_Unchecked(object sender, RoutedEventArgs e)
        {
            //IsModified = true;
        }



        private void checkBox_MultiDay_Unchecked(object sender, RoutedEventArgs e)
        {
            checkBox_MultiDay_Change(sender);
        }

        private void checkBox_MultiDay_Checked(object sender, RoutedEventArgs e)
        {
            checkBox_MultiDay_Change(sender);
        }

        private void checkBox_MultiDay_Change(object sender)
        {

            //ReturnValue.bMulti = ReturnValue.bMulti == true ? false : true;

            this.OutputShipment0.IsEnabled = ReturnValue.bMulti;
            this.OutputShipment1.IsEnabled = ReturnValue.bMulti;
            this.OutputShipment2.IsEnabled = ReturnValue.bMulti;
            this.OutputShipment3.IsEnabled = ReturnValue.bMulti;
            this.OutputShipment4.IsEnabled = ReturnValue.bMulti;
            this.OutputShipment5.IsEnabled = ReturnValue.bMulti;
            //IsModified = true;


        }

        private void Arrival_TouchUp(object sender, TouchEventArgs e)
        {
            Arrival_getresponse(sender);
        }

        private void Arrival_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Arrival_getresponse(sender);

        }

        private void Arrival_getresponse(object sender)
        {


            int result = TouchPanelTenkeyShow(ReturnValue.iArrival);



            if (result != -1)
            {
                ReturnValue.iArrival = result;

            }


        }


        private void button_TouchDown(object sender, TouchEventArgs e)
        {

            tp.button_TouchDown(sender, e);
            

        }

        private void CheckBox_TouchDown(object sender, TouchEventArgs e)
        {

            //タッチされたチェックボックスの操作による切り替えを無効にする
            CheckBox ch = (CheckBox)sender;
            ch.IsHitTestVisible = false;

            tp.button_TouchDown(sender, e);


        }


        private void checkBox_Fruit_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs =  obj => {

                ReturnValue.bKind = ReturnValue.bKind == true ? false : true;
                IsModified = true;
                if (IsFruitModified)
                {
                    IsFruitModified = false;
                    strModifiedFruit = "";
                }
                else
                {
                    IsFruitModified = true;
                    strModifiedFruit = ReturnValue.strCommodity;
                }
            };

            tp.button_TouchUp(sender, e, Prs);


            //無効化されていた切り替えを有効に戻す

            CheckBox ch = (CheckBox)sender;
            ch.IsHitTestVisible = true;



        }

        private void checkBox_Fruit_Click(object sender, RoutedEventArgs e)
        {
            IsModified = true;

            if (IsFruitModified)
            {
                IsFruitModified = false;
                strModifiedFruit = "";
            }
            else
            {
                IsFruitModified = true;
                strModifiedFruit = ReturnValue.strCommodity;
            }
        }


        private void checkBox_MultiDay_TouchUp(object sender, TouchEventArgs e)
        {

            EventProcess Prs =  obj => {

                ReturnValue.bMulti = ReturnValue.bMulti == true ? false : true;
                IsModified = true;
            };

            tp.button_TouchUp(sender, e, Prs);


            //無効化されていた切り替えを有効に戻す
            CheckBox ch = (CheckBox)sender;
            ch.IsHitTestVisible = true;




        }

        private void checkBox_MultiDay_Click(object sender, RoutedEventArgs e)
        {

            IsModified = true;

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

        private void Button_Print_Press(object sender)
        {

            this.Grid_Opa.Visibility = Visibility.Visible;

            TagEdit te = new TagEdit(ReturnValue);

            te.ShowDialog();

            this.Grid_Opa.Visibility = Visibility.Collapsed;


        }


    }
}
