using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regression
{
    internal class LinearRegressor
    {
            private float _b0;
            private float _b1;

            public LinearRegressor()
            {
                _b0 = 0;
                _b1 = 0;
            }

            /// <summary>
            /// Train Linear Regression algoritm.
            /// </summary>
            /// <param name="X">Input Data</param>
            /// <param name="y">Output Data</param>
            public void Fit(float[] X, float[] y)
            {
                var ssxy = X.Zip(y, (a, b) => a * b).Sum() - X.Length * X.Average() * y.Average();
                var ssxx = X.Zip(X, (a, b) => a * b).Sum() - X.Length * X.Average() * X.Average();

                _b1 = ssxy / ssxx;
                _b0 = y.Average() - _b1 * X.Average();
            }

            /// <summary>
            /// Predict new values.
            /// </summary>
            /// <param name="x">Input Data</param>
            /// <returns>Predictions from the trained algoritm.</returns>
            public float[] Predict(float[] x)
            {
                return x.Select(i => _b0 + i * _b1).ToArray();
            }
        }
}
