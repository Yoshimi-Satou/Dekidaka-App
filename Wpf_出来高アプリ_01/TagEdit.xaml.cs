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
using System.Windows.Shapes;
using DekiDaka_Data;
using Wpf_Dekidaka_app.Bind;


namespace Wpf_Dekidaka_app
{
    /// <summary>
    /// TagEdit.xaml の相互作用ロジック
    /// </summary>
    public partial class TagEdit : Window
    {

        //Tag_Control tag;
        public TagCx TagContext = new TagCx();


        /// <summary>
        /// タッチイベント処理用オブジェクト
        /// </summary>
        private TouchEventProcessor tp = new TouchEventProcessor();


        private Dekidaka_Data DData;


        public TagEdit(Dekidaka_Data Dekidaka)
        {
            TagContext.strCow = Dekidaka.strContentsOfWork.Split('(')[0];
            TagContext.strArea = Dekidaka.strProductionArea + (Dekidaka.strProductionArea != "" && Dekidaka.strProductionArea != null ? "産 " : " ");
            TagContext.strCommodity = Dekidaka.strCommodity;
            TagContext.iQuantity = Dekidaka.iaOutputQuantity[0];
            TagContext.strSize = Dekidaka.strSize;
            TagContext.iCopyCount = 1;

            DData = Dekidaka;

            //tag = new Tag_Control(stryArea, strComodity, CoW, iQuantity);



            InitializeComponent();

            DataContext = TagContext;

            //this.RootGrid.Children.Add(tag);





        }

        private void Button_Print_Click(object sender, RoutedEventArgs e)
        {
            Button_Print_Press(sender);
        }

        private void Button_Print_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = Button_Print_Press;

            tp.button_TouchUp(sender, e, Prs);

        }

        private void Button_Print_Press(object sender)
        {

            //確認ダイアログで枚数を確認させる
            string message = TagContext.iQuantity.ToString() + "入を" + TagContext.iCopyCount.ToString() + "枚印刷します。";

            OKCancelDlg re = new OKCancelDlg(message);

            re.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            re.ShowDialog();

            //返事がキャンセルならなにもせず戻る
            if (re.result == MessageBoxResult.Cancel) { return; }


            StateWindow StateW = new StateWindow();

            StateW.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            StateW.Topmost = true;

            StateW.Show();

            //タグを設定する
            PrintingTag Pt = new PrintingTag(DData);

            Pt.TagSetting(TagContext.iQuantity, TagContext.iCopyCount);

            //印刷する
            Pt.Print();

            StateW.Close();
            StateW = null;

            this.Close();

        }


        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void Button_Cancel_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = obj =>{ this.Close(); };

            tp.button_TouchUp(sender, e, Prs);
        }



        private void Textbox_NumberCopies_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Textbox_NumberCopies_Press(sender);

        }

        private void Textbox_NumberCopies_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = Textbox_NumberCopies_Press;

            tp.button_TouchUp(sender, e, Prs);
        }

        private void Textbox_NumberCopies_Press(object sender)
        {

            int result = TouchPanelTenkeyShow(TagContext.iCopyCount);

            if (result != -1)
            {
                TagContext.iCopyCount = result;

            }


        }

        private int TouchPanelTenkeyShow(int value = 0)
        {
            //MessageBox.Show("ダブルクリック");

            //var txtbx_sender = (TextBox)sender;

            //string sendtext = txtbx_sender.Name;


            TenKeyBord tw = new TenKeyBord(value);


            //tw.Topmost = true;

            tw.ShowDialog();


            return tw.result;
        }

        private void Textbox_Quantity_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Textbox_Quantity_Press(sender);
        }


        private void Textbox_Quantity_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = Textbox_Quantity_Press;

            tp.button_TouchUp(sender, e, Prs);
        }

        private void Textbox_Quantity_Press(object sender)
        {
            int result = TouchPanelTenkeyShow(TagContext.iQuantity);

            if (result != -1)
            {
                TagContext.iQuantity = result;

            }
        }



        private void button_TouchDown(object sender, TouchEventArgs e)
        {

            tp.button_TouchDown(sender, e);


        }





    }
}
