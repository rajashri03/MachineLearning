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
                    cw.WriteRecords(itemlist);
                }
            }
        }
    }
}