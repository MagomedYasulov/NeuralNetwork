using NeuralNetwork.NetworkModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem
{
    public class SystemController
    {
        public SystemController(Network network) 
        {
            Network = network;
        }

        public Network Network { get; set; }
    }
}
