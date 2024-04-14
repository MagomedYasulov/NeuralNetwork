using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            var results = new List<double[]>();
            foreach(var dataSet in dataSets)
            {
                var outputs = neuralNetwork.Compute(dataSet.Values);
                results.Add(outputs);
            }
           
            for(var i=0; i< results.Count;i++)
            {
                Assert.AreEqual(dataSets[i].Targets.First(), Math.Round(results[i].First(), 1));
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

            var results = new List<double[]>();
            foreach (var dataSet in dataSets)
            {
                var outputs = neuralNetwork.Compute(dataSet.Values);
                results.Add(outputs);
            }

            for (var i = 0; i < results.Count; i++)
            {
                Assert.AreEqual(dataSets[i].Targets.First(), Math.Round(results[i].First(), 1));
            }
        }


        [TestMethod]
        public void HeartDataSetTest()
        {
            var text = File.ReadAllText("HeartDataset.txt");
            var dataSets = JsonConvert.DeserializeObject<List<DataSet>>(text);
            var input = dataSets[0];
            dataSets.Remove(input);

            var neuralNetwork = new Network(13, new int[] { 12, 10, 8, 6, 4 }, 1);
            var minError = 0.03; // - 3% error

            neuralNetwork.Train(dataSets, minError);
            var outputs = neuralNetwork.Compute(input.Values);

            //ExportHelper.ExportNetwork(neuralNetwork);

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

            var hiddenLayersCount = 50;//parasitizedImageInput.Length / 4;
            var hiddenLayers = new int[hiddenLayersCount - 1];
            
            for (int i=0; i< hiddenLayersCount - 1; i++)
            {
                hiddenLayers[i] = hiddenLayersCount - i;
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
