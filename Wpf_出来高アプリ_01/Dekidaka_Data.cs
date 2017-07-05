using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;

namespace DekiDaka_Data
{


    [Serializable()]
    public class Dekidaka_Data : INotifyPropertyChanged
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





        //
        //   メンバを増やしたらインポートメソッドも修正すること
        //   MainWindow.xaml.csのSaveTempData()も要修正
        //

        private const int MaxIndex = 6;



        //ボタンの色番号の中身の色を定義する配列 #BFE8FFDB
        private Color[] _ColoraButtonColorIndex = { Color.FromArgb(0xBF,0xE8, 0xFF, 0xDB), //デフォルト灰色
                                                    Color.FromArgb(0xBF,0xFF, 0xD8, 0xD8) }; //赤
        /// <summary>
        /// 印刷ボタンの色
        /// </summary>
        private Brush _BlPrintBottonColor;

        public Brush BlPrintBottonColor
        {
            get { return _BlPrintBottonColor; }

            set
            {
                _BlPrintBottonColor = value;

                OnPropertyChanged("BlPrintBottonColor");



            }

        }

        /// <summary>
        /// 印刷ボタンの色を設定する
        /// </summary>
        /// <param name="ColorID"></param>
        public void SetPrintBottonColor(int ColorID)
        {
            if (ColorID > -1 && ColorID < 2)
            {
                _BlPrintBottonColor = new SolidColorBrush(_ColoraButtonColorIndex[ColorID]);

                OnPropertyChanged("BlPrintBottonColor");
            }

        }



        private string[] _straShipment = new string[MaxIndex];
        /// <summary>
        /// 出荷日
        /// </summary>
        public string[] straShipment
        {
            get { return _straShipment; }



            set
            {
                _straShipment = value;
                OnPropertyChanged("straShipment");

            }

        }

        /// <summary>
        /// 出荷日を設定する
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool strShipment(string value, int index) 
        {

            if (index < MaxIndex && index >= 0)
            {
                _straShipment[index] = value;

                OnPropertyChanged("straShipment");

                return true;
            }


            return false;


        }





        private int _iArrival;
        /// <summary>
        /// 品物の入荷数
        /// </summary>
        public int iArrival     //入荷数
        {
            get { return _iArrival; }

            set
            {
                _iArrival = value;
                OnPropertyChanged("iArrival");

            }


        }



        private bool _bMulti;
        /// <summary>
        /// イオンの複数日分の出来高対応用フラグ
        /// </summary>
        public bool bMulti //イオンの複数日分フラグ
        {
            get { return _bMulti; }

            set
            {
                _bMulti = value;
                OnPropertyChanged("bMulti");

            }

        }




        private bool _bKind;
        /// <summary>
        /// 種別、果実ならtrue
        /// </summary>
        public bool bKind //種別
        {
            get { return _bKind; }

            set
            {
                _bKind = value;
                OnPropertyChanged("bkind");

            }

        }

        private string _strSize;
        /// <summary>
        /// 品物サイズ
        /// </summary>
        public string strSize //品物サイズ
        {
            get { return _strSize; }

            set
            {
                _strSize = value;
                OnPropertyChanged("strSize");
            }
        }


        private string _strCustomar;
        /// <summary>
        /// 得意先
        /// </summary>
        public string strCustomar //得意先
        {
            get { return _strCustomar; }

            set
            {
                _strCustomar = value;
                OnPropertyChanged("strCustomar");
                //OnPropertyChanged("brsCustomer");
            }
        }




        private string _strWriteMember;
        /// <summary>
        /// 記入者
        /// </summary>
        public string strWriteMember //記入者
        {
            get { return _strWriteMember; }

            set
            {
                _strWriteMember = value;
                OnPropertyChanged("strWriteMember");
            }
        }


        private int _iPartGroup;
        /// <summary>
        /// 班番号
        /// </summary>
        public int iPartGroup //班No
        {
            get { return _iPartGroup; }



            set
            {
                _iPartGroup = value;
                OnPropertyChanged("iPartGroup");

            }


        }



        private int _iPartNumber;
        /// <summary>
        /// パート人数
        /// </summary>
        public int iPartNumber //パート人数
        {
            get { return _iPartNumber; }



            set
            {
                _iPartNumber = value;
                OnPropertyChanged("iPartNumber");

            }


        }

        private int[] _iaOutputQuantity = new int[MaxIndex];
        /// <summary>
        /// 出来高入り数配列
        /// </summary>
        public int[] iaOutputQuantity //出来高の入り数
        {
            get { return _iaOutputQuantity; }



            set
            {
                _iaOutputQuantity = value;
                OnPropertyChanged("strOutput");
                OnPropertyChanged("iaOutputQuantity");

            }


        }

        /// <summary>
        /// 出来高入数に値をセットして変更を通知する
        /// </summary>
        public bool iOutputQuantity(int value, int index) //入り数
        {

            if (index < MaxIndex && index >= 0)
            {
                _iaOutputQuantity[index] = value;

                OnPropertyChanged("strOutput");
                OnPropertyChanged("iaOutputQuantity");

                return true;
            }


            return false;


        }



        private int[] _iaOutputNumber = new int[MaxIndex];
        /// <summary>
        /// 出来高の個数配列
        /// </summary>
        public int[] iaOutputNumber //出来高の×数
        {
            get { return _iaOutputNumber; }



            set
            {
                _iaOutputNumber = value;
                OnPropertyChanged("strOutput");
                OnPropertyChanged("iaOutputNumber");

            }


        }

        /// <summary>
        /// 出来高の個数配列に値をセットして変更を通知する
        /// </summary>
        public bool iOutputNumber(int value, int index) //数
        {

            if (index < MaxIndex && index >= 0)
            {
                _iaOutputNumber[index] = value;

                OnPropertyChanged("strOutput");
                OnPropertyChanged("iaOutputNumber");

                return true;
            }


            return false;


        }





        private string[] _straMaterials = new string[MaxIndex];
        /// <summary>
        /// 資材名配列
        /// </summary>
        public string[] straMaterials //資材
        {
            get { return _straMaterials; }



            set
            {
                _straMaterials = value;
                OnPropertyChanged("strChainedMaterials");
                OnPropertyChanged("straMaterials");

            }


        }

        /// <summary>
        /// 資材名配列に値をセットして変更を通知する
        /// </summary>
        public bool strMaterials(string value,int index) //資材
        {

            if(index < MaxIndex && index >= 0)
            {
                _straMaterials[index] = value;

                OnPropertyChanged("strChainedMaterials");
                OnPropertyChanged("straMaterials");

                return true;
            }


            return false;


        }



        private int[] _iaMaterialsNumber = new int[MaxIndex];
        /// <summary>
        /// 資材使用数配列
        /// </summary>
        public int[] iaMaterialsNumber //資材数
        {
            get { return _iaMaterialsNumber; }



            set
            {
                _iaMaterialsNumber = value;
                OnPropertyChanged("strChainedMaterials");
                OnPropertyChanged("iaMaterialsNumber");

            }


        }

        /// <summary>
        /// 資材使用数配列に値をセットして、変更を通知する
        /// </summary>
        public bool iMaterialsNumber(int value, int index) //資材使用数
        {

            if (index < MaxIndex && index >= 0)
            {
                _iaMaterialsNumber[index] = value;

                OnPropertyChanged("strChainedMaterials");
                OnPropertyChanged("iaMaterialsNumber");

                return true;
            }


            return false;


        }





        private string _strContentsOfWork;
        /// <summary>
        /// 作業内容
        /// </summary>
        public string strContentsOfWork //作業内容
        {
            get { return _strContentsOfWork; }



            set
            {
                _strContentsOfWork = value;
                OnPropertyChanged("strContentsOfWork");

            }


        }



        private string _strCommodity;
        /// <summary>
        /// 品名
        /// </summary>
        public string strCommodity //品名
        {
            get { return _strCommodity; }



            set
            {
                _strCommodity = value;
                OnPropertyChanged("strCommodity");

            }


        }


        private string _strProductionArea;
        /// <summary>
        /// 産地
        /// </summary>
        public string strProductionArea //産地
        {
            get { return _strProductionArea; }



            set
            {
                _strProductionArea = value;
                OnPropertyChanged("strProductionArea");

            }


        }




        private DateTime _dtStartTime;
        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime dtStartTime //開始時間
        {
            get { return _dtStartTime; }

            set
            {
                _dtStartTime = value;
                OnPropertyChanged("strStartTime");

            }

        }

        private DateTime _dtEndTime;
        /// <summary>
        /// 終了時間
        /// </summary>
        public DateTime dtEndTime //終了時間
        {
            get { return _dtEndTime; }

            set
            {
                _dtEndTime = value;
                OnPropertyChanged("strEndTime");

            }

        }

        private int _iUID;
        /// <summary>
        /// ID番号
        /// </summary>
        public int iUID //ID番号
        {
            get { return _iUID; }



            set
            {
                _iUID = value;
                OnPropertyChanged("iUID");

            }


        }








        //バインド用文字列変換メソッド

        /// <summary>
        /// 開始時間を文字列で返す
        /// </summary>
        public string strStartTime //開始時間文字列
        {
            get { return dtStartTime.ToShortTimeString(); }

            set
            {
                DateTime time;

                if(value != null)
                {
                    if(DateTime.TryParse(value,out time))
                    {
                        dtStartTime = time;
                    }
                    
                }

            } 
        }

        /// <summary>
        /// 終了時間を文字列で返す
        /// </summary>
        public string strEndTime //終了時間文字列
        {
            get { return dtEndTime.ToShortTimeString(); }

            set
            {
                DateTime time;

                if (value != null)
                {
                    if (DateTime.TryParse(value, out time))
                    {
                        dtEndTime = time;
                    }

                }

            }

        }




        /// <summary>
        /// 出来高をまとめた文字列を返す、代入時はプロパティ変更通知のみの動作
        /// </summary>
        public string strOutput //出来高をまとめた文字列
        {
            get
            {
                string Output = null;
                int OutputNum = 0;


                //出来高に入力があれば連結してひとつの文字列にする
                for (int Count = 0; Count < iaOutputQuantity.Length; Count++)
                {
                    //2項目目なら改行する
                    if (OutputNum % 2 == 1)
                    {

                        if (iaOutputNumber[Count] != 0 && iaOutputQuantity[Count] != 0)
                        {

                            Output = Output + iaOutputQuantity[Count].ToString() + "×" + iaOutputNumber[Count].ToString() + "\n";

                            OutputNum++;
                        }



                    }
                    else
                    {
                        //1項目目ならカンマでつなげる
                        
                        if (iaOutputNumber[Count] != 0 && iaOutputQuantity[Count] != 0)
                        {

                            Output = Output + iaOutputQuantity[Count].ToString() + "×" + iaOutputNumber[Count].ToString() + ", ";

                            OutputNum++;
                        }

                    }


                }
                

                //中身があれば最後の改行を削除
                if (Output != null)
                {
                    //出来高が3つ以下ならカンマを改行に置換する
                    if (OutputNum < 4)
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



                return Output;



            }


            set { OnPropertyChanged("strOutput");  } //代入不可




        }

        /// <summary>
        /// 資材をまとめた文字列を返す、代入時はプロパティ変更通知のみの動作
        /// </summary>
        public string strChainedMaterials //資材をまとめた文字列
        {
            get
            {
                string Materials = "";

                //資材に入力があれば連結してひとつの文字列にする
                for (int Count = 0; Count < straMaterials.Length; Count++)
                {
                    if (straMaterials[Count] != "")
                    {
                        Materials = Materials + straMaterials[Count] + "\n";
                    }
                }

                //中身があれば最後の改行を削除
                if (Materials != "")
                {
                    Materials = Materials.Substring(0, Materials.Count() - 1);
                }



                return Materials;


                /*    System.Text.StringBuilder Output = new System.Text.StringBuilder();
                    string temp = null;
                    foreach (int Count in iOutputQuantity)
                    {
                        temp = iOutputQuantity[Count].ToString
                        Output.AppendLine(temp + "×");
                        temp = iOutputNumber[Count].ToString
                        Output.AppendLine(temp + "\n");
                    }
                */

            }


            set { OnPropertyChanged("strChainedMaterials");  } //代入不可




        }




        private string _strEditButton; //ボタンラベル
        private bool _bActionable; //記入フラグ


        /// <summary>
        /// ホタンのラベル用
        /// </summary>
        public string strEditButton //ボタンラベル 
        {
            get { return _strEditButton; }

            set {; }



        }

        /// <summary>
        /// データ作成済みフラグとして使用
        /// </summary>
        public bool bActionable //記入フラグ
        {
            get { return _bActionable; }
            set
            {
                if (_bActionable != value)
                {
                    _bActionable = value;
                    _strEditButton = _bActionable ? "編集" : "作成";

                    OnPropertyChanged("strEditButton");
                    OnPropertyChanged("bActionable");

                }
            }
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Dekidaka_Data(int id = 0) //コンストラクタ
        {
            iUID = id;
            bActionable = false;
            bKind = false;
            _strEditButton = "作成";
            
            //資材数の初期値を1に
            for(int i = 0;i < 6;i++)
            {
                _iaMaterialsNumber[i] = 1;
                _straMaterials[i] = "";

            }

            //時間を0:00に
            _dtStartTime = DateTime.Parse("0:00");
            _dtEndTime = DateTime.Parse("0:00");


            _straShipment[0] = DateTime.Today.ToShortDateString().Substring(5, 5);
            _straShipment[1] = "";// DateTime.Today.ToShortDateString().Substring(5, 5);
            _straShipment[2] = DateTime.Today.AddDays(1).ToShortDateString().Substring(5, 5);
            _straShipment[3] = "";//DateTime.Today.AddDays(1).ToShortDateString().Substring(5, 5);
            _straShipment[4] = DateTime.Today.AddDays(2).ToShortDateString().Substring(5, 5);
            _straShipment[5] = "";//DateTime.Today.AddDays(2).ToShortDateString().Substring(5, 5);


            SetPrintBottonColor(1);


        }

        /// <summary>
        /// オブジェクト自身にデータをインポートする
        /// </summary>
        public void Data_Import(Dekidaka_Data source) //参照元の値をコピーする
        {


            this.bActionable = source.bActionable;
            this.bMulti = source.bMulti;
            this.bKind = source.bKind;

            this.dtEndTime = source.dtEndTime;
            this.dtStartTime = source.dtStartTime;

            this.iArrival = source.iArrival;
            this.iPartGroup = source.iPartGroup;
            this.iPartNumber = source.iPartNumber;
            this.iUID = source.iUID;

            this.strCommodity = source.strCommodity;
            this.strContentsOfWork = source.strContentsOfWork;
            this.strCustomar = source.strCustomar;
            this.strEditButton = source.strEditButton;
            this.strProductionArea = source.strProductionArea;
            this.strSize = source.strSize;
            this.strWriteMember = source.strWriteMember;


            this._iaMaterialsNumber = (int[])source._iaMaterialsNumber.Clone();
            this._straMaterials = (string[])source._straMaterials.Clone();
            OnPropertyChanged("strChainedMaterials");//プロパティ変更を通知する


            this._iaOutputQuantity = (int[])source._iaOutputQuantity.Clone();
            this._iaOutputNumber = (int[])source._iaOutputNumber.Clone();
            OnPropertyChanged("strOutput");//プロパティ変更を通知する

            this._straShipment = (string[])source._straShipment.Clone();
            OnPropertyChanged("straShipment");

            this.BlPrintBottonColor = source.BlPrintBottonColor;

        }


        /// <summary>
        /// データを複製して複製の参照を返す
        /// </summary>
        public Dekidaka_Data Clone() //クローン作成メソッド
        {
            Dekidaka_Data cloned = (Dekidaka_Data)MemberwiseClone();

            if (this._iaMaterialsNumber != null)
            {
                cloned._iaMaterialsNumber = (int[])this._iaMaterialsNumber.Clone();
            }

            if (this._straMaterials != null)
            {
                cloned._straMaterials = (string[])this._straMaterials.Clone();
            }

            if (this._iaOutputQuantity != null)
            {
                cloned._iaOutputQuantity = (int[])this._iaOutputQuantity.Clone();
            }

            if (this._iaOutputNumber != null)
            {
                cloned._iaOutputNumber = (int[])this._iaOutputNumber.Clone();
            }

            if (this._straShipment != null)
            {
                cloned._straShipment = (string[])this._straShipment.Clone();
            }



            return cloned;


        }

    }
}

