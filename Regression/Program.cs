using CsvHelper;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Regression
{
    public class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Select\n1)Train\n2)Test\n");
                int option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        TrainData.train();
                        break;
                    case 2:
                        TestData.RemoveData();
                        TestData.Standardization();
                        break;
                }
            }
          
        }

    }

    public class DataPoint
{
        [LoadColumn(0)]
        public float x { get; set; }
        [LoadColumn(1)]
        public float y { get; set; }
}
}
