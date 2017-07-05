using System;
using System.Windows;
using System.Windows.Input;
using Wpf_Dekidaka_app.Bind;

namespace Wpf_Dekidaka_app
{
    /// <summary>
    /// TimeInput.xaml の相互作用ロジック
    /// </summary>
    public partial class TimeInput : Window
    {

        TimeInputCx Dcx = new TimeInputCx();

        public string strTime = null;

        private TouchEventProcessor tp = new TouchEventProcessor();


        public TimeInput(DateTime dtTime)
        {

            Dcx.iHour = dtTime.Hour;
            Dcx.iMinute = dtTime.Minute;

            DataContext = Dcx;



            InitializeComponent();
        }

        private void textBlock_Hour_TouchUp(object sender, TouchEventArgs e)
        {
            textBlock_Hour_Touch(sender);

        }

        private void textBlock_Hour_MouseUp(object sender, MouseButtonEventArgs e)
        {
            textBlock_Hour_Touch(sender);

        }

        private void textBlock_Hour_Touch(object sender)
        {
            TenKeyBord tkw = new TenKeyBord(Dcx.iHour);

            tkw.ShowDialog();

            if (tkw.result < 25 && tkw.result > -1)
            {
                Dcx.iHour = tkw.result;


            }


        }






        private void textBlock_Minute_TouchUp(object sender, TouchEventArgs e)
        {

            textBlock_Minute_Touch(sender);



        }

        private void textBlock_Minute_MouseUp(object sender, MouseButtonEventArgs e)
        {
            textBlock_Minute_Touch(sender);

        }

        private void textBlock_Minute_Touch(object sender)
        {

            TenKeyBord tkw = new TenKeyBord(Dcx.iMinute);

            tkw.ShowDialog();

            if (tkw.result < 60 && tkw.result > -1)
            {
                Dcx.iMinute = tkw.result;


            }




        }




        private void button_Cancel_TouchUp(object sender, TouchEventArgs e)
        {
            EventProcess Prs = obj =>
            {
                this.Close();

            };

            tp.button_TouchUp(sender, e, Prs);


        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }


        private void button_Enter_TouchUp(object sender, TouchEventArgs e)
        {

            EventProcess Prs = obj =>
            {
                strTime = Dcx.iHour.ToString() + ":" + Dcx.iMinute.ToString();

                this.Close();


            };

            tp.button_TouchUp(sender, e, Prs);


        }

        private void button_Enter_Click(object sender, RoutedEventArgs e)
        {
            strTime = Dcx.iHour.ToString() + ":" + Dcx.iMinute.ToString();

            this.Close();

        }

        private void button_TouchDown(object sender, TouchEventArgs e)
        {

            tp.button_TouchDown(sender, e);


        }




    }
}
