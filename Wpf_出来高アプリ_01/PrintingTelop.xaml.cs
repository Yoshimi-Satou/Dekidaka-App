using DekiDaka_Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Printing;
using System.Windows.Xps;
using Wpf_Dekidaka_app.Bind;

namespace Wpf_Dekidaka_app
{
    /// <summary>
    /// PrintingTelop.xaml の相互作用ロジック
    /// </summary>
    public partial class Telop_Control : UserControl
    {
        /// <summary>
        /// データコンテキスト
        /// </summary>
        private TelopCx TelopData = new TelopCx();

        /// <summary>
        /// 印刷内容を表現するユーザーコントロール
        /// </summary>
        /// <param name="strCustomer">発注者</param>
        /// <param name="strConOfWork">作業内容</param>
        /// <param name="strOutput">出来高</param>
        /// <param name="strDayArea">産地と日付</param>
        public Telop_Control(string strCustomer = "発注者", string strConOfWork = "作業内容", string strOutput = "出来高" ,string strDayArea = "")
        {
            //データコンテキストの設定

            TelopData.strCustomer = strCustomer;
            TelopData.strConOfWork = strConOfWork;
            TelopData.strOutput = strOutput;
            TelopData.strDayArea = strDayArea;

            InitializeComponent();

            DataContext = TelopData;

        }


    }

    /// <summary>
    /// 印刷用データテンプレートを設定によって切り替えるセレクタオーバーライドクラス
    /// 本体はxamlで定義、実体化する
    /// </summary>
    public class TelopTemplateSelector : DataTemplateSelector
    {

        public DataTemplate Portrait { get; set; }
        public DataTemplate Landscape { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            
            if (Settings.PrintLandscape)
                return Landscape;
            else
                return Portrait;
        }


    }





    /// <summary>
    /// 実際に印刷作業をオペレーションするクラス
    /// </summary>
    public class PrintingTelop
    {

        /// <summary>
        /// 印刷するドキュメントを入れるオブジェクト
        /// </summary>
        protected FixedDocument fixedDoc = null; //作成時に実体化する

        protected Dekidaka_Data Dekidaka = null; //コンストラクタで作成時に実体化する

        protected LocalPrintServer ps = new LocalPrintServer();
        protected PrintQueue pq = null;
        protected PrintTicket Pt = null;

        protected double PageWidth = 0;
        protected double PageHeight = 0;

        protected bool IsPrintSetting = false;

        /// <summary>
        /// コンストラクタ
        /// 出来高を取得、印刷関連オブジェクトの設定をする
        /// </summary>
        /// <param name="Dekidaka">出来高データ</param>
        public PrintingTelop(Dekidaka_Data orgDekidaka)
        {

            Dekidaka = orgDekidaka; //出来高データの保持


            //プリンタの印刷範囲を取得する為に関連オブジェクトを作成する
            pq = null;
            Pt = null;

            //Settings.PrintDlg = new PrintDialog();

            //ローカルプリントサーバーから設定内容のプリンタキューと印刷設定チケットを取得する
            IsPrintSetting = GetPrinterSetting(ps, ref pq, ref Pt);


            //印刷可能範囲を計算して保持する
            PageImageableArea Pa = pq.GetPrintCapabilities().PageImageableArea;

            //double PageWidtha = Pa.OriginWidth +  Pa.ExtentWidth;
            //double PageHeighta = Pa.OriginHeight + Pa.ExtentHeight;
            PageWidth = (double)Pt.PageMediaSize.Width - Pa.OriginWidth;
            PageHeight = (double)Pt.PageMediaSize.Height - Pa.OriginHeight;

            //MessageBox.Show("1 Width:" + PageWidth.ToString() + "  Height:" + PageHeight.ToString() +
            //                "\n2 Width:" + PageWidtha.ToString() + " Height:" + PageHeighta.ToString() +
            //                "\nO Width:" + Pa.OriginWidth.ToString() + " Height:" + Pa.OriginHeight.ToString() +
            //                "\nE Width:" + Pa.ExtentWidth.ToString() + " Height:" + Pa.ExtentHeight.ToString() +
            //                "\nP Width:" + Pt.PageMediaSize.Width.ToString() + " Height:" + Pt.PageMediaSize.Height.ToString());

        }

        public PrintingTelop()
        {
            Dekidaka = new Dekidaka_Data();

            //プリンタの印刷範囲を取得する為に関連オブジェクトを作成する
            pq = null;
            Pt = null;

            //Settings.PrintDlg = new PrintDialog();

            //ローカルプリントサーバーから設定内容のプリンタキューと印刷設定チケットを取得する
            IsPrintSetting = GetPrinterSetting(ps, ref pq, ref Pt);


            //印刷可能範囲を計算して保持する
            PageImageableArea Pa = pq.GetPrintCapabilities().PageImageableArea;

            //double PageWidtha = Pa.OriginWidth +  Pa.ExtentWidth;
            //double PageHeighta = Pa.OriginHeight + Pa.ExtentHeight;
            PageWidth = (double)Pt.PageMediaSize.Width - Pa.OriginWidth;
            PageHeight = (double)Pt.PageMediaSize.Height - Pa.OriginHeight;

            //MessageBox.Show("1 Width:" + PageWidth.ToString() + "  Height:" + PageHeight.ToString() +
            //                "\n2 Width:" + PageWidtha.ToString() + " Height:" + PageHeighta.ToString() +
            //                "\nO Width:" + Pa.OriginWidth.ToString() + " Height:" + Pa.OriginHeight.ToString() +
            //                "\nE Width:" + Pa.ExtentWidth.ToString() + " Height:" + Pa.ExtentHeight.ToString() +
            //                "\nP Width:" + Pt.PageMediaSize.Width.ToString() + " Height:" + Pt.PageMediaSize.Height.ToString());

        }



        /// <summary>
        /// テロップのドキュメントを設定して作成する
        /// </summary>
        public void TelopSetting()
        {

            //ドキュメントを実体化する
            fixedDoc = new FixedDocument();


            //横向き印刷設定
            bool LandScape = Settings.PrintLandscape;

            //
            if (Dekidaka.strContentsOfWork == null || Dekidaka.strCommodity == null )
            {
                OKCancelDlg re = new OKCancelDlg("品名か作業内容に入力がありません");

                re.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                re.ShowDialog();

                return;

            }



            //複数日分の出来高の場合
            if (Dekidaka.bMulti)
            {

                //関連オブジェクトの定義
                PageContent[] pageContent = new PageContent[6];
                FixedPage[] fixedPage = new FixedPage[6];
                Telop_Control[] tc = new Telop_Control[6];


                //ページ数のカウント
                int TpCount = 0;

                int[] ShipmentBlock = new int[6];
                int Blockcount = 0;
                string Blockstr = "";

                //日付入力を調べてブロックを分ける

                for(int i = 0; i < 6; i++)
                {
                    if(Dekidaka.straShipment[i] != "" && Dekidaka.straShipment[i] != null && Dekidaka.straShipment[i] != Blockstr)
                    {
                        Blockstr = Dekidaka.straShipment[i];
                        ShipmentBlock[Blockcount] = i;
                        Blockcount++;

                    }

                }

                //日付有効なのに日付項目が未記入の時
                if(Blockcount == 0)
                {
                    OKCancelDlg re = new OKCancelDlg("日付項目無しで出力しますか？");

                    re.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                    re.ShowDialog();

                    if (re.result == MessageBoxResult.Cancel) return;

                    //キャンセルじゃなければ印刷用の設定
                    ShipmentBlock[0] = 0;
                    Blockcount = 1;

                }

                //出来高データの出来高を調べてデータを作成する
                for (int i = 0;i < Blockcount; i++)
                {

                    if(Dekidaka.iaOutputNumber[ShipmentBlock[i]] != 0)
                    {

                        int outputrows = 1;

                        if (i < Blockcount - 1)
                        {
                            outputrows = ShipmentBlock[i + 1] - ShipmentBlock[i];
                        }
                        else
                        {
                            outputrows = 6 - ShipmentBlock[i];
                        }

                        string strDayArea;

                        DateTime dt;
                        bool IsDateTime = DateTime.TryParse(Dekidaka.straShipment[ShipmentBlock[i]], out dt);


                        if (LandScape)
                        {
                            strDayArea = Dekidaka.strProductionArea + (Dekidaka.strProductionArea != "" && Dekidaka.strProductionArea != null ? "産 " : " ") +
                                         Dekidaka.straShipment[ShipmentBlock[i]] + (IsDateTime ? "分" : "");

                        }
                        else
                        {
                            strDayArea = Dekidaka.strProductionArea + (Dekidaka.strProductionArea != "" && Dekidaka.strProductionArea != null ? "産\n" : "") +
                                         Dekidaka.straShipment[ShipmentBlock[i]] + (IsDateTime ? "分" : "");
                        }



                        //出来高文字列を作成
                        string Output = "";
                        int OutputNum = 0;

                        if (LandScape)
                        {
                            for (int Count = ShipmentBlock[i]; Count < ShipmentBlock[i] + outputrows; Count++)
                            {
                                //出来高が3項目なら改行をして連結する
                                if (Count % 3 == 2)
                                {

                                    if (Dekidaka.iaOutputNumber[Count] != 0 && Dekidaka.iaOutputQuantity[Count] != 0)
                                    {

                                        Output = Output + Dekidaka.iaOutputQuantity[Count].ToString() + "×" + Dekidaka.iaOutputNumber[Count].ToString() + "\n";
                                        OutputNum++;

                                    }

                                }
                                else
                                {

                                    //それ以外ならカンマでつなげる
                                    if (Dekidaka.iaOutputNumber[Count] != 0 && Dekidaka.iaOutputQuantity[Count] != 0)
                                    {

                                        Output = Output + Dekidaka.iaOutputQuantity[Count].ToString() + "×" + Dekidaka.iaOutputNumber[Count].ToString() + ", ";
                                        OutputNum++;

                                    }

                                }



                            }




                        }
                        else
                        { 
                            for (int j = 0; j < outputrows; j++)
                            {

                                if (Dekidaka.iaOutputNumber[ShipmentBlock[i] + j] != 0)
                                {
                               
                                    Output = Output + string.Format("{0}×{1}\n", Dekidaka.iaOutputQuantity[ShipmentBlock[i] + j], Dekidaka.iaOutputNumber[ShipmentBlock[i] + j]);

                                }


                            }
                            

                        }

                        //中身があれば最後の改行かカンマを削除
                        if (Output != "")
                        {

                            //最後の改行もしくはカンマを除去する
                            if (Output.Substring(Output.Count() - 1, 1) == "\n")
                            {
                                Output = Output.Substring(0, Output.Count() - 1); //"\n"１文字を除去
                            }
                            else
                            {
                                Output = Output.Substring(0, Output.Count() - 2); //", "２文字を除去
                            }

                        }



                        //ページとコンテントを初期化
                        fixedPage[TpCount] = new FixedPage();
                        pageContent[TpCount] = new PageContent();


                        //作業内容文字列を作成
                        string CoW = "";
                        
                        if (LandScape)
                        {

                            CoW = Dekidaka.strCommodity + " " + (Dekidaka.strSize == "" || Dekidaka.strSize == null ? "" : Dekidaka.strSize + ", ") + Dekidaka.strContentsOfWork.Split('(')[0];

                        }
                        else
                        {

                            CoW = Dekidaka.strCommodity + "\n" + (Dekidaka.strSize == "" || Dekidaka.strSize == null ? "" : Dekidaka.strSize + ", ") + Dekidaka.strContentsOfWork.Split('(')[0];

                        }


                        //印刷内容を作成する
                        tc[TpCount] = new Telop_Control(Dekidaka.strCustomar, CoW, Output, strDayArea);


                        //印刷可能範囲に合わせてViewboxで拡大縮小する
                        if (LandScape)
                        {
                            tc[TpCount].ViewBox.Height = PageWidth;
                            tc[TpCount].ViewBox.Width = PageHeight;

                        }
                        else
                        {
                            tc[TpCount].ViewBox.Height = PageHeight;
                            tc[TpCount].ViewBox.Width = PageWidth;
                        }

                        //ページに追加する
                        fixedPage[TpCount].Children.Add(tc[TpCount]);


                        //fixedPage[TpCount].Arrange(new Rect(0, 0, PageHeight, PageWidth));
                        //ページの大きさを設定し直す
                        if (LandScape)
                        {
                            //fixedPage[TpCount].Arrange(new Rect(0, 0, PageWidth, PageHeight)); //これ付けると最後のページ以外が空白になる、なぜだ？
                            fixedPage[TpCount].UpdateLayout();
                            fixedPage[TpCount].Width = PageHeight;
                            fixedPage[TpCount].Height = PageWidth;


                        }
                        else
                        {
                            fixedPage[TpCount].Width = PageWidth;
                            fixedPage[TpCount].Height = PageHeight;
                            fixedPage[TpCount].UpdateLayout();
                        }


                        //ページをコンテントに追加する
                        ((IAddChild)pageContent[TpCount]).AddChild(fixedPage[TpCount]);

                        //コンテントをドキュメントに追加する
                        fixedDoc.Pages.Add(pageContent[TpCount]);

                        //ページのカウントを増やす
                        TpCount++;



                    }




                }

                if (LandScape) { Pt.PageOrientation = System.Printing.PageOrientation.Landscape; }
                Pt.CopyCount = 1;


            }
            else //単一出来高の時
            {

                //関連オブジェクトを作成
                var pageContent = new PageContent();
                var fixedPage = new FixedPage();

                //出来高内容
                string CoW = "";
                string strDayArea = Dekidaka.strProductionArea + (Dekidaka.strProductionArea != "" && Dekidaka.strProductionArea != null ? "産 " : " ");
                string Output = null;



                //横向き印刷時
                if (LandScape)
                {

                    CoW = Dekidaka.strCommodity + " " + (Dekidaka.strSize == "" || Dekidaka.strSize == null ? "" : Dekidaka.strSize + ", ") + Dekidaka.strContentsOfWork.Split('(')[0];
                    int OutputNum = 0;

                    //出来高に入力があれば連結してひとつの文字列にする
                    for (int Count = 0; Count < Dekidaka.iaOutputQuantity.Length; Count++)
                    {
                        //出来高が3項目なら改行をして連結する
                        if (Count % 3 == 2)
                        {

                            if (Dekidaka.iaOutputNumber[Count] != 0 && Dekidaka.iaOutputQuantity[Count] != 0)
                            {

                                Output = Output + Dekidaka.iaOutputQuantity[Count].ToString() + "×" + Dekidaka.iaOutputNumber[Count].ToString() + "\n";
                                OutputNum++;

                            }

                        }
                        else
                        {

                            //それ以外ならカンマでつなげる
                            if (Dekidaka.iaOutputNumber[Count] != 0 && Dekidaka.iaOutputQuantity[Count] != 0)
                            {

                                Output = Output + Dekidaka.iaOutputQuantity[Count].ToString() + "×" + Dekidaka.iaOutputNumber[Count].ToString() + ", ";
                                OutputNum++;

                            }

                        }



                    }

                    //中身があれば最後の改行かカンマを削除
                    if (Output != null)
                    {
                        //出来高が2つ以下ならカンマを改行に置換する
                        if (OutputNum < 3)
                        {
                            Output = Output.Replace(", ", "\n");
                        }

                        //最後の改行もしくはカンマを除去する
                        if (Output.Substring(Output.Count() - 1, 1) == "\n")
                        {
                            Output = Output.Substring(0, Output.Count() - 1); //"\n"１文字を除去
                        }
                        else
                        {
                            Output = Output.Substring(0, Output.Count() - 2); //", "２文字を除去
                        }

                    }



                }
                else //横向きじゃないとき
                {

                    CoW = Dekidaka.strCommodity + "\n" + (Dekidaka.strSize == "" || Dekidaka.strSize == null ? "" : Dekidaka.strSize + ", ") + Dekidaka.strContentsOfWork.Split('(')[0];

                    if (Dekidaka.strOutput != null)
                    {
                        //カンマを全て改行に置換
                        Output = Dekidaka.strOutput.Replace(", ", "\n");

                        //最後の文字が改行なら除去
                        if (Output.Substring(Output.Count() - 1, 1) == "\n")
                        {
                            Output = Output.Substring(0, Output.Count() - 1); //"\n"１文字を除去
                        }
                    }
                    else
                    { Output = ""; }



                }

                //印刷内容を作成する
                Telop_Control tc = new Telop_Control(Dekidaka.strCustomar, CoW, Output, strDayArea);


                //印刷可能範囲に合わせてViewboxで拡大縮小する
                if(LandScape)
                {
                    tc.ViewBox.Height = PageWidth;
                    tc.ViewBox.Width = PageHeight;
                }
                else
                {
                    tc.ViewBox.Height = PageHeight;
                    tc.ViewBox.Width = PageWidth;
                }



                //ページに追加する
                fixedPage.Children.Add(tc);

                //ページの大きさを設定し直す
                //fixedPage.Arrange(new Rect(0, 0, PageHeight, PageWidth));
                if (LandScape)
                {
                    //fixedPage.Arrange(new Rect(0, 0, PageWidth, PageHeight));
                    fixedPage.UpdateLayout();
                    fixedPage.Width = PageHeight;
                    fixedPage.Height = PageWidth;
                }
                else
                {
                    fixedPage.Width = PageWidth;
                    fixedPage.Height = PageHeight;
                    fixedPage.UpdateLayout();
                }





                //ページをコンテントに追加する
                ((IAddChild)pageContent).AddChild(fixedPage);

                //コンテントをドキュメントに追加する
                fixedDoc.Pages.Add(pageContent);

                if (LandScape) { Pt.PageOrientation = System.Printing.PageOrientation.Landscape; }

                Pt.CopyCount = 1;


            }


        }

        /// <summary>
        /// 印刷を実行するメソッド
        /// </summary>
        public void Print()
        {
            //もしドキュメントが作成されていなかったら戻る
            if(fixedDoc == null || fixedDoc.Pages.Count == 0) { return; }


            //関連オブジェクトの作成
            XpsDocumentWriter xpsdw;
            

            //設定から印刷設定を読み出して印刷を実行する
            try
            {


                xpsdw = PrintQueue.CreateXpsDocumentWriter(pq);

                
                xpsdw.Write(fixedDoc, Pt);

                //MessageBox.Show("Pt.CopyCount=" + Pt.CopyCount.ToString() + "\nSettings.PrintTicketSetting.CopyCount=" + Settings.PrintTicketSetting.CopyCount.ToString());

                //印刷部数を1に戻す
                //Pt.CopyCount = 1;

            }
            catch
            {
                MessageBox.Show("プリントエラー");
            }


        }

        /// <summary>
        /// 設定を読み出して、印刷設定を取得する
        /// </summary>
        /// <param name="ps">ローカルプリントサーバー</param>
        /// <param name="pq">設定を入力するプリントキューへの参照</param>
        /// <param name="pt">設定を入力するプリントチケットへの参照</param>
        /// <returns>true:全て設定成功 false:システムデフォルト設定が混じった</returns>
        protected bool GetPrinterSetting(LocalPrintServer ps, ref PrintQueue pq,  ref  PrintTicket pt)
        {

            if (Settings.PrinterName != "")
            {
                try
                {
                    //PrinterName設定のキューを取得する
                    pq = ps.GetPrintQueue(Settings.PrinterName);

                    //PrintTicket設定に中身があれば取得する
                    if (Settings.PrintTicketSetting != null)
                    {
                        pt = Settings.PrintTicketSetting.Clone();

                        //両方取得できたらアプリ設定からの印刷設定取得に成功として返す
                        return true;
                    }
                    else
                    {
                        //中身がなければキューのデフォルト設定を取得する
                        pt = ps.DefaultPrintQueue.UserPrintTicket.Clone();
                        
                    }


                }
                catch
                {
                    //失敗したらシステムのデフォルト設定を取得する
                    MessageBox.Show("設定したプリンタが見つかりません");
                    pq = ps.DefaultPrintQueue;
                    pt = ps.DefaultPrintQueue.UserPrintTicket.Clone();
                }


            }
            else
            {
                //プリンタ設定がなければシステムのデフォルト設定を取得する
                pq = ps.DefaultPrintQueue;
                pt = ps.DefaultPrintQueue.UserPrintTicket.Clone();
            }

            //ここまで来たらアプリ設定からの印刷設定取得に失敗している
            return false;
        }


    }
}
