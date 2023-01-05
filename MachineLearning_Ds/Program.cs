using CsvHelper;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;

namespace NullValues
{
    class Program
    {

        public static List<CountryData> itemlist = new List<CountryData>();
        static void Main(string[] args)
        {
            int sumOfAge = 0, sumOfSalary = 0, meanOfSalary;
            var lines = System.IO.File.ReadAllLines("D:\\MachineLearning\\MachineLearning_Ds\\data_preprocessing.csv").Skip(1).TakeWhile(t => t != null);

            foreach (string item in lines)
            {
                var values = item.Split(',');
                itemlist.Add(new CountryData()
                {
                    Country = values[0],
                    Age = int.Parse(values[1]),
                    Salary = int.Parse(values[2]),
                    Purchased = values[3]
                });

            }
            foreach (var item in itemlist)
            {
                Console.WriteLine(item.Country + " " + item.Age + "  " + item.Salary + "  " + item.Purchased);
            }
            List<int> purchasedList = new List<int>();
            //printing only purchase
            foreach (var item in itemlist)
            {
                Console.WriteLine(item.Purchased);
                if (item.Purchased == "Yes")
                {
                    purchasedList.Add(1);
                }
                else
                {
                    purchasedList.Add(0);
                }
            }
            foreach (var item in purchasedList)
            {
                Console.WriteLine(item);
            }
            for (int i = 1; i < itemlist.Count; i++)
            {
                foreach (var item in itemlist)
                {
                    sumOfAge = sumOfAge + item.Age;
                    sumOfSalary = sumOfSalary + item.Salary;
                    //  Console.WriteLine(sumOfAge);
                }
                break;
            }
            Console.WriteLine("Sum=" + sumOfAge);
            var mean = sumOfAge / itemlist.Count;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Age Mean=" + mean);
            Console.ResetColor();
            Console.WriteLine("\n");

            Console.WriteLine("Sum=" + sumOfSalary);
            meanOfSalary = sumOfSalary / itemlist.Count;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Salary Mean=" + meanOfSalary);
            Console.ResetColor();
            Console.WriteLine("\n");

            for (int i = 0; i < itemlist.Count; i++)
            {
                foreach (var item in itemlist)
                {
                    if (item.Age == 0)
                    {
                        item.Age = mean;
                    }
                    if (item.Salary == 0)
                    {
                        item.Salary = meanOfSalary;
                    }
                }
            }
            foreach (var item in itemlist)
            {
                Console.WriteLine(item.Country + "     " + item.Age + "     " + item.Salary + "    " + item.Purchased);
            }
            string path = @"D:\MachineLearning\MachineLearning_Ds\data_preprocessing.csv";
            using (StreamWriter sw = new StreamWriter(path))
            {
                using (CsvWriter cw = new CsvWriter(sw, System.Globalization.CultureInfo.InvariantCulture))
                {
                    cw.WriteRecords(itemlist);
                }
            }


            //******************************************Categorical data-one hot encoding***************************

            var context = new MLContext();
            IDataView data = context.Data.LoadFromTextFile<CountryData>("D:\\MachineLearning\\MachineLearning_Ds\\data_preprocessing.csv", hasHeader: true, separatorChar: ',');

            // A pipeline for one hot encoding the Education column.
            var pipeline = context.Transforms.Categorical.OneHotEncoding(
                "EducationOneHotEncoded", "Country");

            var pipeline2 = context.Transforms.Categorical.OneHotEncoding(
                "PurchasedOneHotEncoded", "Purchased");
            // Fit and transform the data.
            IDataView oneHotEncodedData = pipeline.Fit(data).Transform(data);

            // PrintDataColumn(oneHotEncodedData, "PurchasedOneHotEncoded");
            // PrintDataColumn(oneHotEncodedData, "EducationOneHotEncoded");
            //Replacing function by using code here

            List<float> Encode1 = new List<float>();
            List<float> Encode2 = new List<float>();
            List<float> Encode3 = new List<float>();
            var countSelectColumn = oneHotEncodedData.GetColumn<float[]>(
                oneHotEncodedData.Schema["EducationOneHotEncoded"]);

            List<CountryData2> EncodedList1 = new List<CountryData2>();

            Console.WriteLine("EducationOneHotEncoded");
            foreach (var row in countSelectColumn)
            {
                for (var i = 0; i < row.Length; i++)
                {
                    Console.Write($"{row[i]}\t");
                    if (i == 0)
                    {
                        Encode1.Add(row[i]);
                    }
                    if (i == 1)
                    {
                        Encode2.Add(row[i]);
                    }
                    if (i == 2)
                    {
                        Encode3.Add(row[i]);
                    }
                }
                Console.WriteLine();
            }

            // We have 3 slots because there are three categories in the
            // 'Education' column.

            // A pipeline for one hot encoding the Education column (using keying).
            var keyPipeline = context.Transforms.Categorical.OneHotEncoding(
                "EducationOneHotEncoded", "Country",
                OneHotEncodingEstimator.OutputKind.Key);

            // Fit and Transform data.
            oneHotEncodedData = keyPipeline.Fit(data).Transform(data);

            var keyEncodedColumn =
                oneHotEncodedData.GetColumn<uint>("EducationOneHotEncoded");

            Console.WriteLine(
                "One Hot Encoding of single column 'Country', with key type " +
                "output.");

            // One Hot Encoding of single column 'Country', with key type output.

            /*foreach (uint element in keyEncodedColumn)
                Console.WriteLine(element);
            */

            List<uint> CountryEncoded = new List<uint>();

            foreach (uint element in keyEncodedColumn)
            {
                //Console.WriteLine(element);
                CountryEncoded.Add(element);
            }
            //added the encoded values to list
            //foreach (uint element in CountryEncoded)
            //{
            //    Console.WriteLine(element);
            //}

            //Adding column to csv file
            List<string> lines2 = File.ReadAllLines("D:\\MachineLearning\\MachineLearning_Ds\\data_preprocessing.csv").ToList();
            //add new column to the header row
            lines2[0] += ",CountryEncoded";
            int index = 1;
            //add new column value for each row.
            lines2.Skip(1).ToList().ForEach(line =>
            {
                //-1 for header
                lines2[index] += "," + CountryEncoded[index - 1];
                index++;
            });


            //write the new content
            File.WriteAllLines("D:\\MachineLearning\\MachineLearning_Ds\\data_preprocessing.csv", lines2);

            //adding purchased List to csv

            List<string> lines3 = File.ReadAllLines("D:\\MachineLearning\\MachineLearning_Ds\\data_preprocessing.csv").ToList();
            //add new column to the header row
            lines3[0] += ",purchasedList";
            int index1 = 1;
            //add new column value for each row.
            lines3.Skip(1).ToList().ForEach(line =>
            {
                //-1 for header
                lines3[index1] += "," + purchasedList[index1 - 1];
                index1++;
            });

            //Adding country France

            lines3[0] += ",Country_France";
            int index2 = 1;
            //add new column value for each row.
            lines3.Skip(1).ToList().ForEach(line =>
            {
                //-1 for header
                lines3[index2] += "," + Encode1[index2 - 1];
                index2++;
            });
            //Adding Country Spain
            lines3[0] += ",Country_Spain";
            int index3 = 1;
            //add new column value for each row.
            lines3.Skip(1).ToList().ForEach(line =>
            {
                //-1 for header
                lines3[index3] += "," + Encode2[index3 - 1];
                index3++;
            });
            //Adding country Germany
            lines3[0] += ",Country_Germany";
            int index4 = 1;
            //add new column value for each row.
            lines3.Skip(1).ToList().ForEach(line =>
            {
                //-1 for header
                lines3[index4] += "," + Encode3[index4 - 1];
                index4++;
            });


            //write the new content
            File.WriteAllLines("D:\\MachineLearning\\MachineLearning_Ds\\data_preprocessing.csv", lines3);

            List<CountryData2> EncodedList = new List<CountryData2>();
            var lines4 = System.IO.File.ReadAllLines("D:\\MachineLearning\\MachineLearning_Ds\\data_preprocessing.csv").Skip(1).TakeWhile(t => t != null);

            foreach (string item in lines4)
            {
                var values = item.Split(',');
                EncodedList.Add(new CountryData2()
                {
                    Age = int.Parse(values[1]),
                    Salary = int.Parse(values[2]),
                    purchasedList = int.Parse(values[5]),
                    Country_France = int.Parse(values[6]),
                    Country_Spain = int.Parse(values[7]),
                    Country_Germany = int.Parse(values[8])
                });
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Age" + "    " + "Salary" + "  " +"purchasedList" + "  " + "Country_France" + "  " + "Country_Spain" + "  " + "Country_Germany");
            Console.ResetColor();
            foreach (var item in EncodedList)
            {
                Console.WriteLine(item.Age + "\t" + item.Salary + "\t  " + item.purchasedList + "\t\t  " + item.Country_France + "\t\t  " + item.Country_Spain + " \t\t " + item.Country_Germany);
            }

            using (var writer = new StreamWriter("D:\\MachineLearning\\MachineLearning_Ds\\Encoded.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(EncodedList);

            }

            //File.WriteAllLines("C:/Users/Admin/Desktop/WebPractice/MachineLearning/DataPreprocessing1/DataPreprocessingPruchaseList/Encoded.csv", EncodedList);
        }



        private static void PrintDataColumn(IDataView transformedData,
            string columnName)
        {
            var countSelectColumn = transformedData.GetColumn<float[]>(
                transformedData.Schema[columnName]);

            List<CountryData2> EncodedList1 = new List<CountryData2>();

            Console.WriteLine(columnName);
            //foreach (var row in countSelectColumn)
            //{
            //    for (var i = 0; i < row.Length; i++)
            //        Console.Write($"{row[i]}\t");

            //    Console.WriteLine();
            //}
        }
    

    }

}