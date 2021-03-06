﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections;
using Wpf_Dekidaka_app.Bind;
using System.Data;
using System.Collections.Generic;

namespace Wpf_Dekidaka_app
{
    /// <summary>
    /// SelectWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SelectWindow : Window
    {

        public string ReturnText { get; set; } //返す文字列

        public SelectionData swContext;

        public bool result = false;

        private string Sentext = "";

        private TouchEventProcessor tp = new TouchEventProcessor();

        public SelectWindow(string sendedtext,string InputText = "")
        {


            //データコンテキスト用のデータの用意
            swContext = new SelectionData(48);


            Sentext = sendedtext;



            //パネルデータに実体があれば読み出して設定
            if (ModuleData.Panel.Array != null)
            {


                List<List<string>> csvData = ModuleData.Panel.Array;
                DataTable csvTable = ModuleData.Panel.Table;


                //csvの行数を保持 （csvData[index]が一行分のデータ）
                int csvRow = csvTable.Rows.Count;
                int TargetColNo = -1; //目的のデータの列番号を入れる変数


                ////csvFieldでcsvData行の中の列セルにアクセスする
                //List<string> csvField = csvData[0];//1行目にアクセスして、データのヘッダ名を調べる

                ////親ウィンドウから送られた文字列でヘッダ名を検索する
                //for (int i = 0; i < csvField.Count; i++)
                //{
                //    string FieldText = (string)csvField[i];
                //    if (FieldText == sendedtext)
                //    {
                //        TargetColNo = i;
                //        break;

                //    }

                //}




                DataTable PanelData = new DataTable();

                try
                {
                    //パネルデータを抽出テキストの列で絞り込む
                    PanelData = csvTable.DefaultView.ToTable("PanelData", false, Sentext);
                }
                catch
                {
                    //見つからなかったらメッセージを表示
                    MessageBox.Show(sendedtext + "のデータが見つかりません");
                    TargetColNo = 0;

                }



                if (TargetColNo != 0)
                {

                    int count = 0;

                    foreach (DataRow drEle in PanelData.AsEnumerable())
                    {

                        string ftext = drEle.Field<string>(Sentext);

                        if (ftext != "")
                        {
                            swContext.bIsButtonEnable(true, count);

                            string[] txdata = ftext.Split('%');

                            swContext.strButtonText(txdata[0], count);

                            if (txdata.Count<string>() > 1)
                            {
                                int color = 0;
                                if (int.TryParse(txdata[1], out color))
                                {
                                    swContext.intButtonColor(color, count);
                                }
                                else
                                { swContext.intButtonColor(0, count); }
                            }
                            else
                            { swContext.intButtonColor(0, count); }


                        }

                        count++;
                        if (count > 47) break;


                    }



                    //拡張パネル設定
                    if(ModuleData.ExtPanel.Array != null)
                    {
                        csvData = ModuleData.ExtPanel.Array;

                        List<string> csvField;

                        //親ウィンドウから送られた文字列で拡張データのヘッダ名を検索する
                        int TargetRowNo = -1;

                        for (int i = 0; i < csvData.Count; i++)
                        {
                            csvField = csvData[i];
                            string FieldText = csvField[0];
                            if (FieldText == sendedtext)
                            {
                                TargetRowNo = i;
                                break;

                            }

                        }

                        //見つかったらパネルに設定
                        if(TargetRowNo != -1)
                        {
                            csvField = csvData[TargetRowNo];

                            for (int i = 1; i < csvField.Count; i++)
                            {

                                string ftext = csvField[i];

                                if (ftext != "")
                                {
                                    swContext.bIsButtonEnable(true, i + 43);

                                    string[] txdata = ftext.Split('%');

                                    swContext.strButtonText(txdata[0], i + 43);

                                    if (txdata.Count<string>() > 1)
                                    {
                                        int color = 0;
                                        if (int.TryParse(txdata[1], out color))
                                        {
                                            swContext.intButtonColor(color, i + 43);
                                        }
                                        else
                                        { swContext.intButtonColor(0, i + 43); }
                                    }
                                    else
                                    { swContext.intButtonColor(0, i + 43); }


                                }

                            }

                        }

                    }

                }


                swContext.FlushBottonColor();

            }
            else //パネルデータ無し
            {
                MessageBox.Show("パネルデータがありません");

            }


            //データコンテキストに設定
            DataContext = swContext;


            InitializeComponent();

            if (InputText != "") { this.textBox_Input.Text = InputText; }
            else { this.textBox_Input.Text = "自由入力"; }

            result = true;




        }

        private void Button_Back_Click(object sender, MouseButtonEventArgs e)
        {

            ReturnText = null;
            this.Close();

        }

        private void Button_Back_TouchDown(object sender, TouchEventArgs e)
        {

            EventProcess Prs = obj =>
            {

                ReturnText = null;
                this.Close();

            };

            tp.button_TouchUp(sender, e, Prs);



        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Button bt = (Button)sender;

            ReturnText = (string)bt.Content.ToString();
            //MessageBox.Show(swContext.straButtonText[0].ToString());

            this.Close();


        }



        private void button_select_TouchUp(object sender, TouchEventArgs e)
        {

            EventProcess Prs = obj =>
            {

                Button bt = (Button)sender;

                ReturnText = (string)bt.Content.ToString();
                //MessageBox.Show(swContext.straButtonText[0].ToString());

                this.Close();

            };

            tp.button_TouchUp(sender, e, Prs);




        }


        private void Button_Enter_Click(object sender, MouseButtonEventArgs e)
        {

            this.Close();

        }

        private void Button_Enter_TouchDown(object sender, TouchEventArgs e)
        {

            EventProcess Prs = obj =>
            {

                this.Close();

            };

            tp.button_TouchUp(sender, e, Prs);

        }

        private void Button_Enter_TouchEnter(object sender, TouchEventArgs e)
        {
            


            this.Close();
        }

        private void Button_Input_Click(object sender, MouseButtonEventArgs e)
        {

            Button_Input_Press(sender);

        }

        private void Button_Input_TouchUp(object sender, TouchEventArgs e)
        {

            EventProcess Prs = Button_Input_Press;

            tp.button_TouchUp(sender, e, Prs);



        }

        /// <summary>
        /// 自由入力のボタンを押した時の処理、設定に履歴も書き込む
        /// </summary>
        /// <param name="sender"></param>
        private void Button_Input_Press(object sender)
        {

            if (this.textBox_Input.Text != "自由入力")
            {
                ReturnText = this.textBox_Input.Text;

                //設定に自由入力履歴を書き込む
                if(Sentext != "ProductionArea")
                {


                    //拡張パネル設定
                    if (ModuleData.ExtPanel.Array == null)
                    {
                        //拡張パネルデータが無いなら作成する

                        ModuleData.ExtPanel.Array = new List<List<string>>();

                        List<string> addfield = new List<string>();

                        addfield.Add(Sentext);
                        addfield.Add(ReturnText);
                        addfield.Add("");
                        addfield.Add("");
                        addfield.Add("");



                        ModuleData.ExtPanel.Array.Add(addfield);


                    }
                    else
                    { 


                        List<List<string>> csvData = ModuleData.ExtPanel.Array;
                        List<string> csvField;


                        //親ウィンドウから送られた文字列で拡張データのヘッダ名を検索する
                        int TargetRowNo = -1;

                        for (int i = 0; i < csvData.Count; i++)
                        {
                            csvField = csvData[i];
                            string FieldText = csvField[0];
                            if (FieldText == Sentext)
                            {
                                TargetRowNo = i;
                                break;

                            }

                        }


                        //見つからなかったら拡張データ行を作成して1項目目に入力文字列を追加する
                        if(TargetRowNo == -1)
                        {
                            List<string> addfield = new List<string>();

                            addfield.Add(Sentext);
                            addfield.Add(ReturnText);
                            addfield.Add("");
                            addfield.Add("");
                            addfield.Add("");

                            csvData.Add(addfield);

                        }
                        else
                        {
                            csvField = csvData[TargetRowNo];

                            bool IsExist = false;

                            for(int i = 1; i < 5;i++)
                            {
                                if(csvField[i] == ReturnText)
                                {
                                    IsExist = true;
                                    break;
                                }
                            }

                            for(int i = 0; i < 44; i++)
                            {

                                if (swContext.straButtonText[i] == ReturnText)
                                {
                                    IsExist = true;
                                    break;
                                }

                            }


                            if (!IsExist)
                            {

                                //項目をひとつ後ろにずらす、最後の項目は消える
                                for (int i = 4; i > 1; i--)
                                {
                                    csvField[i] = csvField[i - 1];

                                }

                                //1項目目に入力文字列をセットする
                                csvField[1] = ReturnText;


                            }


                        }





                        //Sentext

                    }


                }

                //ArreyListをDataTableに変換してCsvStreamFruisを更新する
                DataTable TempData = new DataTable();

                List<string> field = ModuleData.ExtPanel.Array[0];

                foreach (var Col in field)
                {
                    TempData.Columns.Add(Col);
                }

                

                foreach(List<string> el in ModuleData.ExtPanel.Array)
                { 

                    DataRow row = TempData.NewRow();

                    for (int i = 0; i < el.Count; i++)
                    {
                        row[i] = (el[i]);

                    }

                    TempData.Rows.Add(row);

                    
                }

                
                string strcsv = CSVTool.CSVTool.ConvertDataTableToCsvStream(TempData, false);



                Settings.csvStreamPanelData = strcsv;


            }
            else { ReturnText = null; }

            
            this.Close();

        }



        private void textBox_Input_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = obj =>
            {
                TextBox tbx = (TextBox)obj;

                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(tbx), tbx);

            };

            tp.button_TouchUp(sender, e, Prs);

        }

        private void Button_Del_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = obj =>
            {

                ReturnText = "";
                this.Close();

            };

            tp.button_TouchUp(sender, e, Prs);



        }

        private void Button_Del_Click(object sender, MouseButtonEventArgs e)
        {
            ReturnText = "";
            this.Close();

        }

        private void textBox_Input_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.textBox_Input.Text == "自由入力")
            {
                this.textBox_Input.Text = "";
            }


        }

        private void textBox_Input_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.textBox_Input.Text == "")
            {
                this.textBox_Input.Text = "自由入力";
            }



        }


        private void button_TouchDown(object sender, TouchEventArgs e)
        {

            tp.button_TouchDown(sender, e);

        }




    }
}
