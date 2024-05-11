using Microsoft.VisualStudio.TestTools.UnitTesting;
using MLModel_Heart;
using NeuralNetwork.Helpers;
using NeuralNetwork.NetworkModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkTests
{
    [TestClass]
    public class CompareTests
    {
        [TestMethod]
        public void Compare()
        {
            var text = File.ReadAllText("HeartDataset.txt");
            var dataSets = JsonConvert.DeserializeObject<List<DataSet>>(text);

            var network = ImportHelper.ImportNetwork("D:\\с# work\\Практика\\NeuralNetwork\\NeuralNetwork\\NeuralNetworkTests\\bin\\Debug\\WellTrainedNetwork.txt");

            var predicts = new IOrderedEnumerable<KeyValuePair<string, double>>[dataSets.Count];
            for (var i = 0; i < dataSets.Count; i++)
            {
                var outputs = network.Compute(dataSets[i].Values);

                var result = new KeyValuePair<string, double>[] { new KeyValuePair<string, double>("1", outputs.First()), new KeyValuePair<string, double>("0", 1 - outputs.First()) };
                predicts[i] = result.OrderByDescending(r => r.Value);
            }

            var mlNetDataset = new MLModel1.ModelInput[dataSets.Count];
            for (var j =0;j< dataSets.Count;j++) 
            {
                MLModel1.ModelInput sampleData = new MLModel1.ModelInput();
                var properties = sampleData.GetType().GetProperties();
                for (var i = 0; i < dataSets[j].Values.Length; i++)
                {
                    properties[i].SetValue(sampleData, (float)dataSets[j].Values[i]);
                }
                mlNetDataset[j] = sampleData;
            }

            var mlNetPredicts = new IOrderedEnumerable<KeyValuePair<string,float>>[dataSets.Count];
            for(var i=0;i < mlNetDataset.Length;i++)
            {
                mlNetPredicts[i] = MLModel1.PredictAllLabels(mlNetDataset[i]);
            }

            Debug.WriteLine("---------------------------------------------------------------------------------------------");
            for (var i =0;i< mlNetPredicts.Length;i++)
            {
                Debug.WriteLine($"Own Network: {predicts[i].First().Key} {predicts[i].First().Value} | {predicts[i].Last().Key} {predicts[i].Last().Value} | expected: {dataSets[i].Targets.First()}");
                Debug.WriteLine($"ML.Net: {mlNetPredicts[i].First().Key} {mlNetPredicts[i].First().Value} |  {mlNetPredicts[i].Last().Key} {mlNetPredicts[i].Last().Value} | expected: {dataSets[i].Targets.First()}");
                Debug.WriteLine("---------------------------------------------------------------------------------------------");
            }
        }
    }
}
