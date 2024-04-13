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
