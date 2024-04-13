using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetwork.NetworkModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NeuralNetworkTests
{
    [TestClass]
    public class NeuralNetworkTests
    {
        [TestMethod]
        public void HeartDataSetTest()
        {
            var text = File.ReadAllText("HeartDataset.txt");
            var dataSets = JsonConvert.DeserializeObject<List<DataSet>>(text);
            var input = dataSets[0];
            dataSets.Remove(input);

            var neuralNetwork = new Network(13, new int[] { 6, 5, 4, 3, 2 }, 1);
            var minError = 0.1;

            neuralNetwork.Train(dataSets, minError);
            var outputs = neuralNetwork.Compute(input.Values);

            Assert.AreEqual(input.Targets.First(), Math.Round(outputs.First(), 1));
        }
    }
}
