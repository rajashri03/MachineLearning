//using Microsoft.ML;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace MachineLearning_Ds
//{
//    internal class FeatureScaling
//    {
//        public void Scaling()
//        {
//            MLContext cont = new MLContext();

//            var pipeline = cont.
//            // Performs the following operation on a vector X: Y = (X - M) / D, where M is mean and D is either L2 norm, L1 norm or LInf norm.
//            pipeline.Add(new LogMeanVarianceNormalizer("FeatureName"));

//            // Normalizes the data based on the computed mean and variance of the data.
//            pipeline.Add(new MeanVarianceNormalizer("FeatureName") { FixZero = true });

//            // Normalize the columns only if needed.
//            pipeline.Add(new ConditionalNormalizer("FeatureName"));

//            // The values are assigned into equidensity bins and a value is mapped to its bin_number/ number_of_bins.
//            pipeline.Add(new BinNormalizer("FeatureName") { NumBins = 2 });
//        }
//    }
//}
