using System.Windows;
using System.Windows.Input;
using Wpf_Dekidaka_app.Bind;

namespace Wpf_Dekidaka_app
{
    /// <summary>
    /// OKCancelDlg.xaml の相互作用ロジック
    /// </summary>
    public partial class OKCancelDlg : Window
    {

        private TouchEventProcessor tp = new TouchEventProcessor();


        public MessageBoxResult result = MessageBoxResult.Cancel;

        public OKCancelDlg(string DlgText)
        {
            DlgData tx = new DlgData(DlgText);
                
            DataContext = tx;

            this.Topmost = true;

            InitializeComponent();



        }

        private void button_ok_Click(object sender, RoutedEventArgs e)
        {

            result = MessageBoxResult.OK;
            this.Close();

        }

        private void button_ok_TouchUp(object sender, TouchEventArgs e)
        {

            EventProcess Prs = obj =>
            {

                result = MessageBoxResult.OK;
                this.Close();

            };

            tp.button_TouchUp(sender, e, Prs);



        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            result = MessageBoxResult.Cancel;
            this.Close();


        }

        private void button_Cancel_TouchUp(object sender, TouchEventArgs e)
        {

            EventProcess Prs = obj =>
            {

                result = MessageBoxResult.Cancel;
                this.Close();

            };

            tp.button_TouchUp(sender, e, Prs);


        }


        private void button_TouchDown(object sender, TouchEventArgs e)
        {

            tp.button_TouchDown(sender,e);

        }



    }
}
