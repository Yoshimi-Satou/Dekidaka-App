using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wpf_Dekidaka_app
{
    /// <summary>
    /// ConfirmationWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        private string _ConfirmText;

        public bool result = false;

        private TouchEventProcessor tp = new TouchEventProcessor();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ConfirmText">確認用文字列</param>
        public ConfirmationWindow(string ConfirmText)
        {
            this.Topmost = true;

            InitializeComponent();

            _ConfirmText = ConfirmText;

            this.textBlock_Info.Text = "続けるには「" + ConfirmText + "」と入力して下さい";


        }

        private void button_Enter_Click(object sender, RoutedEventArgs e)
        {

            button_Enter_check(sender);

        }

        private void button_Enter_TouchUp(object sender, TouchEventArgs e)
        {

            EventProcess Prs = button_Enter_check;

            tp.button_TouchUp(sender, e, Prs);

            
        }

        /// <summary>
        /// テキストを比較して合っていればtrueを返して、ダイアログを閉じる
        /// </summary>
        /// <param name="sender">送信元のオブジェクト</param>
        private void button_Enter_check(object sender)
        {

            if(this.textBox_Confirm.Text == _ConfirmText)
            {
                result = true;

            }
            else
            {
                result = false;

            }

            this.Close();

        }



        private void button_Cancel_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = obj =>
            {
                result = false;

                this.Close();

            };

            tp.button_TouchUp(sender, e, Prs);


        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            result = false;

            this.Close();

        }

        /// <summary>
        /// テキストボックスにフォーカスが移ったときに初期の内容を消去する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Confirm_GotFocus(object sender, RoutedEventArgs e)
        {
            
            if(this.textBox_Confirm.Text == "入力")
            {
                this.textBox_Confirm.Text = "";
            }

        }

        /// <summary>
        /// テキストボックスからフォーカスが移った時に内容がなければ初期文字列を設定する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Confirm_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.textBox_Confirm.Text == "")
            {
                this.textBox_Confirm.Text = "入力";
            }


        }

        /// <summary>
        /// テキストボックス上でタッチされたときにフォーカスをうつす
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Confirm_TouchUp(object sender, TouchEventArgs e)
        {

            EventProcess Prs = obj =>
            {
                TextBox tbx = (TextBox)obj;

                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(tbx), tbx);

            };

            tp.button_TouchUp(sender, e, Prs);



        }


        private void button_TouchDown(object sender, TouchEventArgs e)
        {

            tp.button_TouchDown(sender, e);
        }



    }
}
