using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf_Dekidaka_app.Bind;

namespace Wpf_Dekidaka_app
{
    /// <summary>
    /// TenKeyBord.xaml の相互作用ロジック
    /// </summary>
    public partial class TenKeyBord : Window
    {


        public TenKey tenkey;
        public int result = -1;

        private TouchEventProcessor tp = new TouchEventProcessor();

        private bool IsTouch = false;

        public TenKeyBord(int value = 0)
        {
            tenkey = new TenKey(value);
            
            DataContext = tenkey;


            //if(value != 0)
            //{
            //    result = value;
            //}


            InitializeComponent();


        }

        private void button_10Key_TouchUp(object sender, TouchEventArgs e)
        {

            EventProcess Prs = button_10Key_Touch;

            tp.button_TouchUp(sender, e, Prs);


        }

        //private void button_10Key_Click(object sender, RoutedEventArgs e)
        //{
        //    if(IsTouch) { IsTouch = false; return; }
        //    MessageBox.Show("Click");

        //    button_10Key_Touch(sender);

        //}

        private void button_10Key_Click(object sender, MouseButtonEventArgs e)
        {

            if (IsTouch) { IsTouch = false; return; }

            //MessageBox.Show("Click");
            button_10Key_Touch(sender);
        }


        private void button_10Key_Touch(object sender)
        {
            Button bx = (Button)sender;

            System.Text.RegularExpressions.Regex r =
                new System.Text.RegularExpressions.Regex(@"[0-9]+", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            //正規表現と一致する対象を1つ検索
            System.Text.RegularExpressions.Match m = r.Match(bx.Name);

            //次のように一致する対象をすべて検索することもできる
            //System.Text.RegularExpressions.MatchCollection mc = r.Matches(bx.Name);

            int number;
            int.TryParse(m.Value, out number);

            if (result != -1)
            {
                if (result < (2147483647 / 10) - number)
                    result = result * 10 + number;

            }
            else
            {
                result = number;

            }

            tenkey.iInputNumber = result;

        }



        private void button_Cancel_TouchUp(object sender, TouchEventArgs e)
        {

            EventProcess Prs = obj =>
            {

                result = -1;

                this.Close();

            };

            tp.button_TouchUp(sender, e, Prs);


        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            //if (IsTouch) { IsTouch = false; return; }

            e.Handled = IsTouch;

            IsTouch = false;

            result = -1;

            this.Close();


        }

        private void button_Cancel_Click(object sender, MouseButtonEventArgs e)
        {
            if (IsTouch) { IsTouch = false; return; }
            
            result = -1;

            this.Close();

        }


        private void button_Enter_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = obj =>
            {
                this.Close();

            };

            tp.button_TouchUp(sender, e, Prs);


        }

        private void button_Enter_Click(object sender, RoutedEventArgs e)
        {
            if (IsTouch) { IsTouch = false; return; }
            
            this.Close();
        }

        private void button_Enter_Click(object sender, MouseButtonEventArgs e)
        {
            if (IsTouch) { IsTouch = false; return; }
            
            this.Close();

        }


        private void button_BackSpace_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = button_BackSpace_Touch;

            tp.button_TouchUp(sender, e, Prs);


        }

        private void button_BackSpace_Click(object sender, RoutedEventArgs e)
        {
            if (IsTouch) { IsTouch = false; return; }


            button_BackSpace_Touch(sender);

        }

        private void button_BackSpace_Click(object sender, MouseButtonEventArgs e)
        {
            if (IsTouch) { IsTouch = false; return; }


            button_BackSpace_Touch(sender);

        }


        private void button_BackSpace_Touch(object sender)
        {

 //           if (result == -1) { return; }

            if (result > 9)
            {
                result = result / 10;

            }
            else
            {
                result = 0;

            }


            tenkey.iInputNumber = result;

        }


        private void button_TouchDown(object sender, TouchEventArgs e)
        {

            tp.button_TouchDown(sender, e);
            IsTouch = true;


        }




    }
}
