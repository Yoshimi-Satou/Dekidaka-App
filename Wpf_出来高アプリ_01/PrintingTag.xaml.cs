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
using DekiDaka_Data;
using Wpf_Dekidaka_app.Bind;
using System.Windows.Markup;

namespace Wpf_Dekidaka_app
{
    /// <summary>
    /// PrintingTag.xaml の相互作用ロジック
    /// </summary>
    public partial class Tag_Control : UserControl
    {
        /// <summary>
        /// タグのデータコンテキスト
        /// </summary>
        public TagCx TagContext = new TagCx();

        /// <summary>
        /// タグの内容を表現するユーザーコントロール
        /// </summary>
        /// <param name="strArea">産地</param>
        /// <param name="strCommodity">品名</param>
        /// <param name="strCow">加工内容</param>
        /// <param name="iQuantity">入数</param>
        public Tag_Control(string strArea = "", string strCommodity = "", string strCow = "", int iQuantity = 0, string strSize = "")
        {
            TagContext.strArea = strArea;
            TagContext.strCommodity = strCommodity;
            TagContext.strCow = strCow;
            TagContext.iQuantity = iQuantity;
            TagContext.strSize = strSize;

            InitializeComponent();

            DataContext = TagContext;

        }

        private void Textbox_NumberCopies_TouchDown(object sender, TouchEventArgs e)
        {

        }

        private void Button_Print_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Textbox_Quantity_TouchDown(object sender, TouchEventArgs e)
        {

        }
    }


    /// <summary>
    /// 印刷用データテンプレートを設定によって切り替えるセレクタオーバーライドクラス
    /// 本体はxamlで定義、実体化する
    /// </summary>
    public class TagTemplateSelector : DataTemplateSelector
    {

        public DataTemplate TagDialog { get; set; }
        public DataTemplate TagPrint { get; set; }




        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            TagCx Tagctrl = (TagCx)item;

            if(Tagctrl == null) { return TagDialog; }

            if (Tagctrl.IsDialog)//Tagctrl.IsDialog)
                return TagDialog;
            else
                return TagPrint;
        }


    }


    public class PrintingTag : PrintingTelop
    {

        private int iNumberCopies = 1;

        public PrintingTag(Dekidaka_Data orgDekidaka) : base(orgDekidaka)
        {
            


        }

        public PrintingTag() : base()
        {



        }

        public void TagSetting(int Quantity,int number)
        {

            if (number > 1)
            {
                iNumberCopies = number;
            }


            //ドキュメントを実体化する
            fixedDoc = new FixedDocument();

            //関連オブジェクトを作成
            PageContent pageContent;
            FixedPage fixedPage;

            Tag_Control tc;

            //出来高内容
            string CoW = Dekidaka.strContentsOfWork.Split('(')[0];
            string stryArea = Dekidaka.strProductionArea + (Dekidaka.strProductionArea != "" && Dekidaka.strProductionArea != null ? "産 " : " ");
            string strComodity = Dekidaka.strCommodity;
            int iQuantity = Quantity;
            string strSize = Dekidaka.strSize;








            //関連オブジェクトを作成
            pageContent = new PageContent();
            fixedPage = new FixedPage();

            //印刷内容を作成する
            tc = new Tag_Control(stryArea, strComodity, CoW, iQuantity, strSize);
            tc.TagContext.IsDialog = false;


            //印刷可能範囲に合わせてViewboxで拡大縮小する
            tc.ViewBox.Height = PageWidth;
            tc.ViewBox.Width = PageHeight;


            //ページに追加する
            fixedPage.Children.Add(tc);

            //ページの大きさを設定し直す
            fixedPage.Arrange(new Rect(0, 0, PageWidth, PageHeight));
            fixedPage.UpdateLayout();

            fixedPage.Width = PageHeight;
            fixedPage.Height = PageWidth;


            //ページをコンテントに追加する
            ((IAddChild)pageContent).AddChild(fixedPage);

            //コンテントをドキュメントに追加する
            fixedDoc.Pages.Add(pageContent);

            Pt.PageOrientation = System.Printing.PageOrientation.Landscape;
            Pt.CopyCount = iNumberCopies;




        }


    }
}




