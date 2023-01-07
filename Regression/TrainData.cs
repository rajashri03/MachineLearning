using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regression
{
    internal class TrainData
    {
        public static void train()
        {
            var mlcontext = new MLContext();
            var lines = System.IO.File.ReadAllLines("D:\\MachineLearning\\Regression\\train.csv").Skip(1).TakeWhile(t => t != null);

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
            //Console.WriteLine("--------------------------------------------------------------------------");
            //foreach (var item in itemlist)
            //{
            //    Console.WriteLine(item.x + "        " + item.y);
            //}
            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine("After---" + itemlist.Count);
            //Console.ResetColor();
            //string trainpath = @"D:\MachineLearning\Regression\AfterRemove_train.csv";
            //StreamWriter sw = new StreamWriter(trainpath);
            //CsvWriter cw = new CsvWriter(sw, CultureInfo.InvariantCulture);
            //cw.WriteRecords(itemlist);








            IDataView data1 = mlcontext.Data.LoadFromTextFile<DataPoint>(@"D:\MachineLearning\Regression\AfterRemove_train.csv", hasHeader: true, separatorChar: ',');
            var columnPair = new[]
            {
                new InputOutputColumnPair("x"),
                new InputOutputColumnPair("y")
            };
            var normalize = mlcontext.Transforms.NormalizeMeanVariance(columnPair,
                fixZero: false);
            var normalizeFixZero = mlcontext.Transforms.NormalizeMeanVariance(columnPair,
                fixZero: true);

            var normalizeTransform = normalize.Fit(data1);
            var transformedData = normalizeTransform.Transform(data1);
            var normalizeFixZeroTransform = normalizeFixZero.Fit(data1);
            var fixZeroData = normalizeFixZeroTransform.Transform(data1);
            var age = transformedData.GetColumn<float>("x");
            var salary = transformedData.GetColumn<float>("y");

            List<DataPoint> ListOfAllColumns = new List<DataPoint>();
            var preview = transformedData.Preview();
            //Headers
            foreach (var col in preview.Schema)
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
            for (int j = 0; j < preview.RowView.Length; j++)
            {
                ListOfAllColumns.Add(new DataPoint()
                {
                    x = ((float)preview.RowView[j].Values[1].Value),
                    y = ((float)preview.RowView[j].Values[3].Value)
                });
            }

            foreach (var item in ListOfAllColumns)
            {
                Console.WriteLine(item.x + "              " + item.y);
            }

            List<float> xvalue = new List<float>();

            for (int j = 0; j < preview.RowView.Length; j++)
            {
                xvalue.Add((float)preview.RowView[j].Values[0].Value);
            }

            List<float> yvalue = new List<float>();

            for (int j = 0; j < preview.RowView.Length; j++)
            {
                yvalue.Add((float)preview.RowView[j].Values[2].Value);
            }


            float[] X = xvalue.ToArray();
            float[] y = yvalue.ToArray();

            var linearRegressor = new LinearRegressor();
            linearRegressor.Fit(X, y);

            var predictions = linearRegressor.Predict(X);


            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("Predictions:");
            Console.WriteLine($"{string.Join(", ", predictions.Select(p => p.ToString()))}");

            Console.WriteLine("Actual Value:");
            Console.WriteLine($"{string.Join(", ", y.Select(p => p.ToString()))}");
            Console.ReadLine();
        }
    }
}
