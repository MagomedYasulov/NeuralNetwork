﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetwork.Helpers;
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
        public void XORTest()
        {
            var dataSets = new List<DataSet>()
            {
                new DataSet(new double[] { 0, 0, }, new double[] { 0 }),
                new DataSet(new double[] { 0, 1, }, new double[] { 1 }),
                new DataSet(new double[] { 1, 0, }, new double[] { 1 }),
                new DataSet(new double[] { 1, 1, }, new double[] { 0 }),
            };

            var neuralNetwork = new Network(2, new int[] { 3, 2 }, 1);
            var minError = 0.01; // - 1% error

            neuralNetwork.Train(dataSets, minError);

            foreach(var dataSet in dataSets)
            {
                Assert.AreEqual(dataSet.Targets.First(), Math.Round(neuralNetwork.Compute(dataSet.Values).First(), 1));
            }
        }

        [TestMethod]
        public void EvenOrOddTest()
        {
            // Output = true если
            // В входных параметрах true - нечетно кол-во или false - четно
            // Output = false если 
            // В входных параметрах true - четно кол-во или false - нечетно
            var dataSets = new List<DataSet>()
            {
                new DataSet(new double[] { 1, 0, 0 }, new double[] { 1 }),
                new DataSet(new double[] { 0, 1, 1 }, new double[] { 0 }),
                new DataSet(new double[] { 1, 0, 1 }, new double[] { 0 }),
                new DataSet(new double[] { 1, 1, 1 }, new double[] { 1 }),
            };

            var neuralNetwork = new Network(3, new int[] { 3, 2 }, 1);
            var minError = 0.01; // - 1% error

            neuralNetwork.Train(dataSets, minError);

            foreach (var dataSet in dataSets)
            {
                Assert.AreEqual(dataSet.Targets.First(), Math.Round(neuralNetwork.Compute(dataSet.Values).First(), 1));
            }
        }

        [TestMethod]
        public void ExportNetworkTest()
        {
            var text = File.ReadAllText("HeartDataset.txt");
            var dataSets = JsonConvert.DeserializeObject<List<DataSet>>(text);

            var network = ImportHelper.ImportNetwork("D:\\с# work\\Практика\\NeuralNetwork\\NeuralNetwork\\NeuralNetworkTests\\bin\\Debug\\WellTrainedNetwork.txt");

            var errors = new double[dataSets.Count];
            for (var i = 0; i < dataSets.Count; i++)
            {
                var outputs = network.Compute(dataSets[i].Values);
                errors[i] = Math.Abs(dataSets[i].Targets.First() - Math.Round(outputs.First(), 2));
            }
        }

        [TestMethod]
        public void HeartDataSetTest()
        {
            var text = File.ReadAllText("HeartDataset.txt");
            var dataSets = JsonConvert.DeserializeObject<List<DataSet>>(text);
            var input = dataSets[0];
            dataSets.Remove(input);

            var neuralNetwork = new Network(13, new int[] { 7, 4 }, 1); 
            var minError = 0.01; // - 1% error

            neuralNetwork.Train(dataSets, minError);
            var outputs = neuralNetwork.Compute(input.Values);

            Assert.AreEqual(input.Targets.First(), Math.Round(outputs.First(), 1));
        }

        [TestMethod]
        public void CellClassificationTest()
        {
            var count = 100;
            var parasitizedPath = @"C:\Users\user\Downloads\archive\cell_images\cell_images\Parasitized";
            var unparasitizedPath = @"C:\Users\user\Downloads\archive\cell_images\cell_images\Uninfected";

            var converter = new PictureConverter();
            var parasitizedImageInput = converter.Convert("Images\\Parasitized.png");
            var unparasitizedImageInput = converter.Convert("Images\\Unparasitized.png");

            var hiddenLayersCount = parasitizedImageInput.Length / 2;
            var hiddenLayers = new int[7];

            var i = 0;
            while(i < hiddenLayers.Length)
            {
                hiddenLayers[i] = hiddenLayersCount;
                hiddenLayersCount /= 2;
                i++;
            }

            var neuralNetwork = new Network(parasitizedImageInput.Length, hiddenLayers, 1);
            var minError = 0.3;

            var datasets = ImportHelper.ImportImageDataset(parasitizedPath, count, 1);
            datasets.AddRange(ImportHelper.ImportImageDataset(unparasitizedPath, count, 0));

            var rnd = new Random();
            rnd.Shuffle(datasets);

            neuralNetwork.Train(datasets, minError);

            var parasitizedOutput = neuralNetwork.Compute(parasitizedImageInput);
            var unparasitizedOutput = neuralNetwork.Compute(unparasitizedImageInput);

            Assert.AreEqual(1, Math.Round(parasitizedOutput.First(),1));
            Assert.AreEqual(0, Math.Round(unparasitizedOutput.First(),1));
        }
    }
}
