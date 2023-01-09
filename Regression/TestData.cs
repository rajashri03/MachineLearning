using CsvHelper;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Regression
{
    internal class TestData
    {
       
        public static void RemoveData()
        {
            MLContext mlcontext = new MLContext();
            var lines = System.IO.File.ReadAllLines("D:\\MachineLearning\\Regression\\test.csv").Skip(1).TakeWhile(t => t != null);

            //IDataView data = mlcontext.Data.LoadFromTextFile<DataPoint>("D:\\MachineLearning\\MachineLearning_Ds\\data_preprocessing.csv", hasHeader: true, separatorChar: ',');
            List<DataPoint> itemlist = new List<DataPoint>();
            // Create a small dataset as an IEnumerable.
            foreach (var item in lines)
            {
                var values = item.Split(',');
                itemlist.Add(new DataPoint()
                {
                    x = float.Parse(values[0]),
                    y = float.Parse(values[1])

                });

            }
            foreach (var item in itemlist)
            {
                Console.WriteLine(item.x + "        " + item.y);
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Before---" + itemlist.Count);
            Console.ResetColor();
            for (int i = 0; i < itemlist.Count; i++)
            {
                foreach (var item in itemlist.ToList())
                {
                    var itemToRemove = itemlist.FirstOrDefault(r => r.x == 0);

                    if (itemToRemove != null)
                        itemlist.Remove(itemToRemove);
                }
            }
            Console.WriteLine("--------------------------------------------------------------------------");
            foreach (var item in itemlist)
            {
                Console.WriteLine(item.x + "        " + item.y);
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("After---" + itemlist.Count);
            Console.ResetColor();
            string testpath = @"D:\MachineLearning\Regression\AfterRemove_test.csv";
            using (StreamWriter sw = new StreamWriter(testpath))
            {
                using (CsvWriter cw = new CsvWriter(sw, CultureInfo.InvariantCulture))
                {
                    cw.WriteRecords(itemlist);
                }
            }
               



        }
        public static void Standardization()
        {
            MLContext mlcontext = new MLContext();
            IDataView datatest= mlcontext.Data.LoadFromTextFile<DataPoint>(@"D:\MachineLearning\Regression\AfterRemove_test.csv", hasHeader: true, separatorChar: ',');
            var columnPairtest = new[]
            {
                new InputOutputColumnPair("x"),
                new InputOutputColumnPair("y")
            };
            var normalizetest = mlcontext.Transforms.NormalizeMeanVariance(columnPairtest,
                fixZero: false);
            var normalizeFixZerotest = mlcontext.Transforms.NormalizeMeanVariance(columnPairtest,
                fixZero: true);

            var normalizeTransformtest = normalizetest.Fit(datatest);
            var transformedDatatest = normalizeTransformtest.Transform(datatest);
            var normalizeFixZeroTransformtest = normalizeFixZerotest.Fit(datatest);
            var fixZeroDatatest = normalizeFixZeroTransformtest.Transform(datatest);
            var xtest = transformedDatatest.GetColumn<float>("x");
            var ytest = transformedDatatest.GetColumn<float>("y");

            List<DataPoint> ListOfAllColumnstest = new List<DataPoint>();
            var previewtest = transformedDatatest.Preview();
            //Headers
            foreach (var col in previewtest.Schema)
            {
                //Console.WriteLine(col.Index); 
                if (((col.Index % 2) == 0))
                {
                    string name = col.Name;
                    Console.Write(col.Name + "\t");
                }
            }

            Console.WriteLine();
            //All values
            //for (int j = 0; j < preview.RowView.Length; j++)
            //{
            //    for (int i = 1; i < 4; i++)
            //    {
            //        if (i % 2 != 0)
            //        {
            //            Console.Write(preview.RowView[j].Values[i].Value + "\t");
            //        }
            //    }
            //    Console.WriteLine();
            //}

            //adding 
            for (int j = 0; j < previewtest.RowView.Length; j++)
            {
                ListOfAllColumnstest.Add(new DataPoint()
                {
                    x = ((float)previewtest.RowView[j].Values[1].Value),
                    y = ((float)previewtest.RowView[j].Values[3].Value)
                });
            }

            foreach (var item in ListOfAllColumnstest)
            {
                Console.WriteLine(item.x + "              " + item.y);
            }
          

        }
    }
}
