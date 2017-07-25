using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Wpf_Dekidaka_app.Bind;

namespace Wpf_Dekidaka_app
{




    /// <summary>
    /// StateWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class StateWindow : Window
    {

        /// <summary>
        /// データコンテキスト
        /// </summary>
        public StateWindowCx cx = new StateWindowCx(); 


        public StateWindow(string strMessage = "お待ち下さい")
        {

            cx.strDisplayMessage = strMessage;

            this.DataContext = cx;

            InitializeComponent();
        }
    }
}
