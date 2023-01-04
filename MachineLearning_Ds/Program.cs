using CsvHelper;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;

namespace NullValues
{
    class Program
    {
        static void Main(string[] args)
        {
            int sumOfAge = 0, sumOfSalary = 0, meanOfSalary;
            List<HousingData> itemlist = new List<HousingData>();
            var lines = System.IO.File.ReadAllLines("D:\\MachineLearning\\MachineLearning_Ds\\data_preprocessing.csv").Skip(1).TakeWhile(t => t != null);
            var country = new List<string>();
            foreach (string item in lines)
            {
                var values = item.Split(',');
                //need to check which type of vehicle is it
                if (values[0] == "France")
                {
                    itemlist.Add(new HousingData()
                    {
                        Country = values[0],
                        Age = int.Parse(values[1]),
                        Salary = int.Parse(values[2]),
                        Purchased =values[3]
                    });
                }
                else if (values[0] == "Spain")
                {
                    itemlist.Add(new HousingData()
                    {
                        Country = values[0],
                        Age = int.Parse(values[1]),
                        Salary = int.Parse(values[2]),
                        Purchased = values[3]
                    });
                }
                else if (values[0] == "Germany")
                {
                    itemlist.Add(new HousingData()
                    {
                        Country = values[0],
                        Age = int.Parse(values[1]),
                        Salary = int.Parse(values[2]),
                        Purchased = values[3]
                    });
                }

            }
            foreach(var item in itemlist)
            {
                Console.WriteLine(item.Country   +" "+item.Age+"  "+item.Salary+"  "+item.Purchased);
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
               foreach(var item in itemlist)
                {
                    if(item.Age==0)
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
                    // File.WriteAllText( path, itemlist);
                    cw.WriteRecords(itemlist);
                }
            }

            //for (int i = 0; i < filePath.Length; i++)
            //{
            //    string[] value = filePath[i].Split(',');
            //    country.Add(value[i]);

            //    //country.Add(value[2]);

            //}

            //for (int i = 0; i < country.Count; i++)
            //{
            //    Console.WriteLine(country[i]);
            //    sumOfAge = sumOfAge + int.Parse(country[i]);

            //}
            //Console.WriteLine("Sum=" + sumOfAge);
            //var mean = sumOfAge / (filePath.Length - 1);
            //Console.WriteLine("Age Mean=" + mean);

            //for (int i = 0; i < country.Count; i++)
            //{
            //    if (int.Parse(country[i]) == 0)
            //    {
            //        country[i] = mean.ToString();

            //    }

            //    Console.WriteLine(country[i] + "\t");
            //}
            //File.ReadAllLines("D:\\MachineLearning\\MachineLearning_Ds\\data_preprocessing.csv");





            //var context = new MLContext();

            //IDataView data = context.Data.LoadFromTextFile<HousingData>("D:\\MachineLearning\\MachineLearning_Ds\\data_preprocessing.csv", hasHeader: true, separatorChar: ',');

            ////To print the content of data that is been loaded from csv file
            //var preview = data.Preview();
            //foreach (var col in preview.Schema)
            //{
            //    if (col.Name == "Features")
            //    {
            //        continue;
            //    }
            //    Console.Write(col.Name + "\t");
            //}
            //Console.WriteLine();

            ////Row view
            //foreach (var row in preview.RowView)
            //{
            //    Console.WriteLine();
            //    foreach (var col in row.Values)
            //    {
            //        if (col.Key == "Features")
            //        {
            //            continue;
            //        }
            //        Console.Write($"{col.Value}\t");
            //    }
            //}
            //Console.WriteLine("\nCount: {0}\n", preview.RowView.Length);


            ////Column view
            //foreach (var row in preview.ColumnView)
            //{
            //    string ColumnName = row.Column.Name.ToString();
            //    if (ColumnName == "Age")
            //    {
            //        foreach (var col in row.Values)
            //        {
            //            int ageValue = Convert.ToInt32(col);
            //            sumOfAge += ageValue;
            //        }
            //    }
            //    if (ColumnName == "Salary")
            //    {
            //        foreach (var col in row.Values)
            //        {
            //            salaryValue = Convert.ToInt32(col);
            //            sumOfSalary += salaryValue;
            //        }
            //    }
            //}
            //meanOfSalary = sumOfSalary / preview.RowView.Length;
            //meanOfAge = sumOfAge / preview.RowView.Length;
            //Console.WriteLine("Sum of Salary : {0} \t Mean Of Salary : {1} \nSum of Age : {2} \t Mean Of Age : {3}", sumOfSalary, meanOfSalary, sumOfAge, meanOfAge);
            //foreach (var row in preview.ColumnView)
            //{
            //    string ColumnName = row.Column.Name.ToString();
            //    if (ColumnName == "Age")
            //    {
            //        foreach (var col1 in row.Values)
            //        {
            //            if (col1.Equals(Convert.ToSingle(0)))
            //            {
            //                ColumnName = meanOfAge.ToString();
            //                Console.WriteLine(ColumnName);
            //            }
            //            //   col= Convert.ToInt64(meanOfAge);
            //        }
            //    }
            //    if (ColumnName == "Salary")
            //    {
            //        foreach (var col in row.Values)
            //        {
            //            salaryValue = Convert.ToInt32(col);
            //            //Calculating the sum of all values in slaary column
            //            sumOfSalary += salaryValue;
            //        }
            //    }
            //}
            //var preview1 = data.Preview();
            //foreach (var col in preview1.Schema)
            //{
            //    if (col.Name == "Features")
            //    {
            //        continue;
            //    }
            //    Console.Write(col.Name + "\t");
            //}
            //Console.WriteLine();

            ////Row view
            //foreach (var row in preview.RowView)
            //{
            //    Console.WriteLine();
            //    foreach (var col in row.Values)
            //    {
            //        if (col.Key == "Features")
            //        {
            //            continue;
            //        }
            //        Console.Write($"{col.Value}\t");
            //    }
            //}
            //Console.WriteLine("\nCount: {0}\n", preview.RowView.Length);


            ////Column view
            //foreach (var row in preview.ColumnView)
            //{
            //    string ColumnName = row.Column.Name.ToString();
            //    if (ColumnName == "Age")
            //    {
            //        foreach (var col in row.Values)
            //        {
            //            int ageValue = Convert.ToInt32(col);
            //            sumOfAge += ageValue;
            //        }
            //    }
            //    if (ColumnName == "Salary")
            //    {
            //        foreach (var col in row.Values)
            //        {
            //            salaryValue = Convert.ToInt32(col);
            //            sumOfSalary += salaryValue;
            //        }
            //    }
            //}

        }
    }
}