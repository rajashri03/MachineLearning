using Microsoft.ML.Data;
using System.Collections.Generic;

namespace NullValues
{
    public class CountryData
    {
        [LoadColumn(0)]
        public string Country { get; set; }

        [LoadColumn(1)]
        public int Age { get; set; }

        [LoadColumn(2)]
        public int Salary { get; set; }

        [LoadColumn(3)]
        public string Purchased { get; set; }


    }
    public class CountryData2
    {

        [LoadColumn(0)]
        public float Age { get; set; }

        [LoadColumn(1)]
        public float Salary { get; set; }

        [LoadColumn(2)]
        public float purchasedList { get; set; }

        [LoadColumn(3)]
        public float Country_France { get; set; }

        [LoadColumn(4)]
        public float Country_Spain { get; set; }

        [LoadColumn(5)]
        public float Country_Germany { get; set; }
    }


}