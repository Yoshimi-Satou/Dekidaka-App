using System.Windows;
using System.Windows.Input;

namespace Wpf_Dekidaka_app
{

    /// <summary>
    /// TouchEventProcessorで使用するデリゲート
    /// </summary>
    delegate void EventProcess(object sender);


    /// <summary>
    /// タッチ操作でのタップ操作の為のヘルパークラス
    /// </summary>
    class TouchEventProcessor
    {

        //System.Media.SoundPlayer TouchSound = new System.Media.SoundPlayer("button.wav");

        public TouchEventProcessor()
        {

        }

        /// <summary>
        /// タッチダウンイベント時に呼び出して、キャプチャやe.Handledを制御する為のメソッド
        /// </summary>
        public void button_TouchDown(object sender, TouchEventArgs e)
        {

            FrameworkElement button = sender as FrameworkElement;

            if (button == null)
            { return; }


            button.CaptureTouch(e.TouchDevice);

            e.Handled = true;


        }

        /// <summary>
        /// ボタンタッチアップイベント時に呼び出して、キャプチャやe.Handledを制御して、指定されたメソッドを実行するメソッド
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">タッチイベントパラメータ</param>
        /// <param name="Prs">実行するメソッド</param>
        /// </summary>
        public void button_TouchUp(object sender, TouchEventArgs e, EventProcess Prs)
        {

            FrameworkElement button = sender as FrameworkElement;

            if (button == null)
            { return; }

            //ポイントがまだ送信元オブジェクトの上にいるか判定する
            TouchPoint tp = e.GetTouchPoint(button);

            Rect bounds = new Rect(new Point(0, 0), button.RenderSize);

            if (bounds.Contains(tp.Position))
            {

                //System.Media.SystemSounds.Beep.Play();
                //TouchSound.Play();

                //指定されたメソッドを実行する
                Prs(sender);


            }



            button.ReleaseTouchCapture(e.TouchDevice);
            e.Handled = true;


        }

    }
}
