using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.NetworkModels
{
    public class ScallingData
    {
        public double Max { get; set; } 
        public double Min { get; set; } 

        public ScallingData(double max, double min) 
        {
            Max = max;
            Min = min;
        }
    }
}
