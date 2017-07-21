using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace Wpf_Dekidaka_app.Bind
{

    public class DlgData : INotifyPropertyChanged
    {


        #region INotifyPropertyChanged メンバ 

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion


        public DlgData(string value = "")
        {
            DisplayText = value;



        }

        private string _DisplayText;
        public string DisplayText
        {
            get { return _DisplayText; }

            set
            {
                _DisplayText = value;

                OnPropertyChanged("DisplayText");

            }


        }


    }



    public class MwContext : INotifyPropertyChanged
    {


        #region INotifyPropertyChanged メンバ 

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion




        //public string mwWriteMember { get; set; }
        private string _mwWriteMember;
        public string mwWriteMember //記入者
        {
            get { return _mwWriteMember; }



            set
            {
                _mwWriteMember = value;
                OnPropertyChanged("mwWriteMember");

            }


        }

        private int _mwPartGroupNo;
        public int mwPartGroupNo //班番号
        {
            get { return _mwPartGroupNo; }



            set
            {
                _mwPartGroupNo = value;
                OnPropertyChanged("mwPartGroupNo");

            }


        }

        private int _mwPartNumber;
        public int mwPartNumber //人数
        {
            get { return _mwPartNumber; }



            set
            {
                _mwPartNumber = value;
                OnPropertyChanged("mwPartNumber");

            }


        }

        //public DateTime InputDate { get; set; }
        private DateTime _mwInputDate;
        public DateTime mwInputDate
        {
            get { return _mwInputDate; }



            set
            {
                _mwInputDate = value;
                OnPropertyChanged("mwInputDate");

            }




        }


        public MwContext()
        {

        }

    }


    public class PreferenceContext : INotifyPropertyChanged
    {


        #region INotifyPropertyChanged メンバ 

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion




        private string _pwSaveFolderPath;
        public string pwSaveFolderPath
        {
            get { return _pwSaveFolderPath; }



            set
            {
                _pwSaveFolderPath = value;
                OnPropertyChanged("pwSaveFolderPath");

            }

        }

        private string _pwBackFolderPath;
        public string pwBackFolderPath
        {
            get { return _pwBackFolderPath; }



            set
            {
                _pwBackFolderPath = value;
                OnPropertyChanged("pwBackFolderPath");

            }

        }

        private bool _pwPrintLandscape;
        public bool pwPrintLandscape
        {
            get { return _pwPrintLandscape; }



            set
            {
                _pwPrintLandscape = value;
                OnPropertyChanged("pwPrintLandscape");

            }

        }

        private string[] _straBackUpGen;
        public string[] straBackUpGen
        {
            get { return _straBackUpGen; }

            set
            {
                _straBackUpGen = value;
                OnPropertyChanged("straBackUpGen");

            }

        }

        private int _iBackUpGen;
        public int iBackUpGen
        {
            get { return _iBackUpGen; }

            set
            {
                if (value < 0)
                {
                    _iBackUpGen = 1;
                }
                else if (value > 9)
                {
                    _iBackUpGen = 9;
                }
                else
                {
                    _iBackUpGen = value;
                }


            }

        }




        public PreferenceContext()
        {
            _iBackUpGen = 0;
            _straBackUpGen = new string[] {"1","2","3","4","5","6","7","8","9","10" };
        }

    }


    public class SelectionData : INotifyPropertyChanged
    {


        #region INotifyPropertyChanged メンバ 

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion

        public int iDataIndexNumber;

        public int iMaxIndexNo;




        public SelectionData(int MaxindexNumber = 48)
        {
            iDataIndexNumber = MaxindexNumber;

            _straButtonText = new string[iDataIndexNumber];
            _intaButtonColor = new int[iDataIndexNumber];
            _BlBottonColor = new Brush[iDataIndexNumber];
            _baIsButtonEnable = new bool[iDataIndexNumber];


            for (int i = 0; i < iDataIndexNumber; i++)
            {
                _straButtonText[i] = "";// i.ToString();

                _baIsButtonEnable[i] = false;

                _intaButtonColor[i] = 0;


            }

            FlushBottonColor();

            iMaxIndexNo = iDataIndexNumber - 1;

            _bIsInputButtonEnable = false;


        }

        private string[] _straButtonText;// = new string[iDataIndexNumber];//ボタンのテキスト

        public string[] straButtonText
        {
            get { return _straButtonText; }

            set
            {
                _straButtonText = value;
                OnPropertyChanged("strButtonText");



            }



        }

        public bool strButtonText(string value, int index)
        {
            if (index < iDataIndexNumber && index >= 0)
            {
                _straButtonText[index] = value;
                OnPropertyChanged("strButtonText");

                return true;

            }



            return false;

        }


        //ボタンの色番号
        private int[] _intaButtonColor;// = new int[iDataIndexNumber];

        public int[] intaButtonColor
        {
            get { return _intaButtonColor; }

            set
            {
                intaButtonColor = value;
                OnPropertyChanged("intButtonColor");



            }



        }

        public bool intButtonColor(int value, int index)
        {
            if (index < iDataIndexNumber && index >= 0)
            {
                _intaButtonColor[index] = value;
                OnPropertyChanged("intButtonColor");

                return true;

            }



            return false;

        }

        //ボタンの色の実体
        private Brush[] _BlBottonColor;// = new Brush[iDataIndexNumber];

        public Brush[] BlBottonColor
        {
            get { return _BlBottonColor; }

            set
            {
                _BlBottonColor = value;
                OnPropertyChanged("BlBottonColor");



            }



        }

        //ボタンの色番号の中身の色を定義する配列
        private Color[] _ColoraButtonColorIndex = {   Color.FromRgb(0xE4, 0xE4, 0xE4), //デフォルト灰色
                                                    Color.FromRgb(0xD8, 0xFF, 0xD8), //緑
                                                    Color.FromRgb(0xFF, 0xD8, 0xD8), //赤
                                                    Color.FromRgb(0xD8, 0xD8, 0xFF), //青
                                                    Color.FromRgb(0xFF, 0xD8, 0xFF), //マゼンダ
                                                    Color.FromRgb(0xFF, 0xFF, 0xD8), //黄
                                                    Color.FromRgb(0xD8, 0xFF, 0xFF), //シアン
                                                    Color.FromRgb(0xFF, 0xFF, 0xFF) }; //白


        //ボタンの色番号から全部のボタンの色を設定するメソッド
        public void FlushBottonColor()
        {
            for (int i = 0; i < iDataIndexNumber; i++)
            {
                if (_intaButtonColor[i] < _ColoraButtonColorIndex.Count<Color>())
                {
                    _BlBottonColor[i] = new SolidColorBrush(_ColoraButtonColorIndex[_intaButtonColor[i]]);

                }
                else
                {
                    _BlBottonColor[i] = new SolidColorBrush(_ColoraButtonColorIndex[0]);
                }
            }

            OnPropertyChanged("BlBottonColor");

        }




        private bool[] _baIsButtonEnable;// = new bool[iDataIndexNumber];//ボタン有効/無効

        public bool[] baIsButtonEnable
        {
            get { return _baIsButtonEnable; }

            set
            {
                _baIsButtonEnable = value;
                OnPropertyChanged("bIsButtonEnable");



            }



        }


        public bool bIsButtonEnable(bool value, int index)
        {
            if (index < iDataIndexNumber && index >= 0)
            {
                _baIsButtonEnable[index] = value;
                OnPropertyChanged("baIsButtonEnable");

                return true;

            }



            return false;

        }


        private bool _bIsInputButtonEnable;//入力ボタン有効/無効

        public bool bIsInputButtonEnable
        {
            get { return _bIsInputButtonEnable; }

            set
            {
                _bIsInputButtonEnable = value;
                OnPropertyChanged("bIsInputButtonEnable");



            }


        }

    }


    public class TenKey : INotifyPropertyChanged
    {


        #region INotifyPropertyChanged メンバ 

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion




        public TenKey(int value = 0)
        {



            iInputNumber = value;




        }


        private int _iInputNumber;
        public int iInputNumber
        {
            get { return _iInputNumber; }

            set
            {
                _iInputNumber = value;
                OnPropertyChanged("iInputNumber");

            }


        }


    }


    public class TimeInputCx : INotifyPropertyChanged
    {


        #region INotifyPropertyChanged メンバ 

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion




        public TimeInputCx()
        {



            iHour = 0;
            iMinute = 0;




        }


        private int _iHour;
        public int iHour
        {
            get { return _iHour; }

            set
            {
                _iHour = value;
                OnPropertyChanged("iHour");

            }


        }

        private int _iMinute;
        public int iMinute
        {
            get { return _iMinute; }

            set
            {
                _iMinute = value;
                OnPropertyChanged("iMinute");

            }


        }



    }


    public class TelopCx : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged メンバ 

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion




        public TelopCx()
        {
            strCustomer = "得意先";
            strConOfWork = "作業内容";
            strOutput = "出来高";


        }


        private String _strCustomer;
        public string strCustomer
        {
            get { return _strCustomer; }

            set
            {
                _strCustomer = value;
                OnPropertyChanged("strCustomer");

            }


        }


        private String _strConOfWork;
        public string strConOfWork
        {
            get { return _strConOfWork; }

            set
            {
                _strConOfWork = value;
                OnPropertyChanged("strConOfWork");

            }


        }


        private String _strOutput;
        public string strOutput
        {
            get { return _strOutput; }

            set
            {
                _strOutput = value;
                OnPropertyChanged("strOutput");

            }


        }

        private String _strDayArea;
        public string strDayArea
        {
            get { return _strDayArea; }

            set
            {
                _strDayArea = value;
                OnPropertyChanged("strDayArea");

            }


        }




    }


    public class TagCx : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged メンバ 

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion


        public TagCx()
        {
            _strArea = "";
            _strCommodity = "";
            _strCow = "";
            _iQuantity = 0;

        }

        private string _strArea;

        public string strArea
        {
            get { return _strArea; }


            set
            {
                _strArea = value;

                OnPropertyChanged("strArea");
            }

        }

        private string _strCommodity;
        public string strCommodity
        {
            get { return _strCommodity; }


            set
            {
                _strCommodity = value;

                OnPropertyChanged("strCommodity");
            }
        }


        private string _strCow;
        public string strCow
        {
            get { return _strCow; }


            set
            {
                _strCow = value;

                OnPropertyChanged("strCow");
            }
        }

        private string _strSize;
        public string strSize
        {
            get { return _strSize; }


            set
            {
                _strSize = value;

                OnPropertyChanged("strSize");
            }
        }


        private int _iQuantity;
        public int iQuantity
        {
            get { return _iQuantity; }


            set
            {
                _iQuantity = value;

                OnPropertyChanged("iQuantity");
                OnPropertyChanged("strQuantity");
            }


        }


        public string strQuantity
        {
            get {
                
                return _iQuantity == 0 ? "     入" : _iQuantity.ToString() + " 入";
            }


            set
            {
                //_strQuantity = value;

                OnPropertyChanged("strQuantity");
            }
        }


        private bool _IsDialog = true;
        public bool IsDialog
        {
            get { return _IsDialog; }

            set
            {
                _IsDialog = value;

                OnPropertyChanged("IsDialog");
            }

        }

        private int _iCopyCount;
        public int iCopyCount
        {
            get { return _iCopyCount; }


            set
            {
                _iCopyCount = value;

                OnPropertyChanged("iCopyCount");

            }


        }



    }


    public class StateWindowCx : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged メンバ 

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion


        private string _strDisplayMessage;

        public string strDisplayMessage
        {
            get { return _strDisplayMessage; }


            set
            {
                _strDisplayMessage = value;
                OnPropertyChanged("strDisplayMessage");
            }


        }

        public StateWindowCx()
        {

            strDisplayMessage = "処理しています";

        }



    }


}


namespace Wpf_Dekidaka_app.Bind.Converters
{

    class BrsConverter : IValueConverter
    {

        private Brush[] Brs = { new SolidColorBrush(Color.FromRgb(0xF4, 0xF4, 0xF5)), //入力済み欄の色　グレー
                                new SolidColorBrush(Color.FromRgb(0xFF, 0xCA, 0xE0))  //未入力欄の色　ピンク
                              };

        //private string[] Brs = { "#FFF4F4F5", "#FFFFCAE0" };



        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if(value == null) { return Brs[1]; }

            string ContextStr = "";
            if (value is int)
            {
                int val = (int)value;
                ContextStr = val == 0 ? "" : "Input";
            }
            else if(value is bool)
            {
                ContextStr = "input";
            }
            else if(value is string)
            {
                
                ContextStr = (string)value;

                if(ContextStr == "0:00")
                { ContextStr = ""; } 

            }
            else
            {
                ContextStr = "input";
            }



            return Brs[ContextStr == "" ? 1 : 0];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }




    }





}