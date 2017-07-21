using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf_Dekidaka_app
{

    public static class ModuleData
    {
        public static PanelData Panel = new PanelData();

        public static FGData FG = new FGData();

        public static MaterialsData Materials = new MaterialsData();

        public static FruitData Fruits = new FruitData();

        public static ExtPanelData ExtPanel = new ExtPanelData();

        public static void ModuleDataSetup()
        {

            ModuleData.Panel.Array = null;


            //パネルデータを読み込む
            if (ModuleData.Panel.LoadData("PanelData.csv"))
            {
                //読み込めたらパネルデータに日付データを追加する

                //ヘッダカラム
                List<string> field = ModuleData.Panel.Array[0];
                field.Add("OutputShipment");

                //データロウ　日付分
                for (int i = 1; i < 9; i++)
                {
                    field = ModuleData.Panel.Array[i];

                    field.Add(DateTime.Today.AddDays(i - 1).ToShortDateString().Substring(5, 5) + "%5");


                }

                //データロウ　サイズとその他文字列

                string[] size = Settings.OutputShipment.Split(' ');


                for (int i = 9; i < ModuleData.Panel.Array.Count && (i - 9 < size.Length); i++)
                {
                    field = ModuleData.Panel.Array[i];

                    field.Add(size[i - 9]);


                }


                //データロウ　空欄
                for (int i = 9 + size.Length; i < ModuleData.Panel.Array.Count; i++)
                {
                    field = ModuleData.Panel.Array[i];

                    field.Add("");


                }



            }


            //各種データの読み込み
            if (!ModuleData.FG.LoadData("FG_Net.csv"))
            {
                ModuleData.FG.Array = null;
            }

            if (!ModuleData.Materials.LoadData("Materials.csv"))
            {
                ModuleData.Materials.Array = null;
            }

            ModuleData.Fruits.LoadDataFromString(Settings.csvStreamFruit);

            //拡張パネルデータ無ければメンバArreyはnull
            if (Settings.csvStreamPanelData != "") ModuleData.ExtPanel.LoadDataFromString(Settings.csvStreamPanelData);




        }

    }

    public class CsvData
    {

        public List<List<string>> Array = null;

        /// <summary>
        /// データをロードする
        /// </summary>
        /// <param name="path">データファイルのパス</param>
        /// <returns></returns>
        public bool LoadData(string path)
        {

            //PanelData.csvからパネルデータをロードする
            string csvstream = "";

            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("Shift_JIS");
            System.IO.StreamReader csvfile;

            //PanelData.csvを開く
            try
            {
                csvfile = new System.IO.StreamReader(path, enc);
                csvstream = csvfile.ReadToEnd();

                csvfile.Close();

            }
            catch (Exception)
            {
                MessageBox.Show(path + "が開けません");

                return false;


            }

            //csvデータをArrayListに読み込む
            LoadDataFromString(csvstream);

            return true;


        }

        /// <summary>
        /// 文字列からデータを読み込む
        /// </summary>
        /// <param name="csvstream">読み込む文字列</param>
        public void LoadDataFromString(string csvstream)
        {


            //csvデータをArrayListに読み込む
            Array = CSVTool.CSVTool.CsvToList(csvstream);

            return;


        }


    }

    public class PanelData : CsvData
    {

    }

    public class FGData : CsvData
    {

    }

    public class MaterialsData : CsvData
    {

    }

    public class FruitData : CsvData
    {

    }

    public class ExtPanelData : CsvData
    {

    }


}

