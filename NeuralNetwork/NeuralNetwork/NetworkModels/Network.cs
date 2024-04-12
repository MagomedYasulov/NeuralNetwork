using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;

namespace NeuralNetwork.NetworkModels
{
	public class Network
	{
		#region -- Properties --
		public double LearnRate { get; set; }
		public double Momentum { get; set; }
		public List<Neuron> InputLayer { get; set; }
		public List<List<Neuron>> HiddenLayers { get; set; }
		public List<Neuron> OutputLayer { get; set; }
		public List<ScallingData> ScallingDatas { get; set; }

		#endregion

		#region -- Globals --
		private static readonly Random Random = new Random();
		#endregion

		#region -- Constructor --
		public Network()
		{
			LearnRate = 0;
			Momentum = 0;
			InputLayer = new List<Neuron>();
			HiddenLayers = new List<List<Neuron>>();
			OutputLayer = new List<Neuron>();
			ScallingDatas = new List<ScallingData>(); 
		}

		public Network(int inputSize, int[] hiddenSizes, int outputSize, double? learnRate = null, double? momentum = null)
		{
			LearnRate = learnRate ?? .4;
			Momentum = momentum ?? .9;
			InputLayer = new List<Neuron>();
			HiddenLayers = new List<List<Neuron>>();
			OutputLayer = new List<Neuron>();
			ScallingDatas = new List<ScallingData>();

			for (var i = 0; i < inputSize; i++)
				InputLayer.Add(new Neuron());

			var firstHiddenLayer = new List<Neuron>();
			for (var i = 0; i < hiddenSizes[0]; i++)
				firstHiddenLayer.Add(new Neuron(InputLayer));

			HiddenLayers.Add(firstHiddenLayer);

			for (var i = 1; i < hiddenSizes.Length; i++)
			{
				var hiddenLayer = new List<Neuron>();
				for (var j = 0; j < hiddenSizes[i]; j++)
					hiddenLayer.Add(new Neuron(HiddenLayers[i - 1]));
				HiddenLayers.Add(hiddenLayer);
			}

			for (var i = 0; i < outputSize; i++)
				OutputLayer.Add(new Neuron(HiddenLayers.Last()));
		}
		#endregion

		#region -- Training --
		public void Train(List<DataSet> dataSets, int numEpochs)
		{
            dataSets = Scalling(dataSets);

            for (var i = 0; i < numEpochs; i++)
			{
				foreach (var dataSet in dataSets)
				{
					ForwardPropagate(dataSet.Values);
					BackPropagate(dataSet.Targets);
				}
			}

            ScallingDatas = GetScallingDatas(dataSets);
        }

		public void Train(List<DataSet> dataSets, double minimumError)
		{
			var error = 1.0;
			var numEpochs = 0;
			dataSets = Scalling(dataSets);

            while (error > minimumError && numEpochs < int.MaxValue)
			{
				var errors = new List<double>();
				foreach (var dataSet in dataSets)
				{
					ForwardPropagate(dataSet.Values);
					BackPropagate(dataSet.Targets);
					errors.Add(CalculateError(dataSet.Targets));
				}
				error = errors.Average();
				numEpochs++;
			}

			ScallingDatas = GetScallingDatas(dataSets);
		}

		private void ForwardPropagate(params double[] inputs)
		{
			var i = 0;
			InputLayer.ForEach(a => a.Value = inputs[i++]);
			HiddenLayers.ForEach(a => a.ForEach(b => b.CalculateValue()));
			OutputLayer.ForEach(a => a.CalculateValue());
		}

		private void BackPropagate(params double[] targets)
		{
			var i = 0;
			OutputLayer.ForEach(a => a.CalculateGradient(targets[i++]));
			HiddenLayers.Reverse();
			HiddenLayers.ForEach(a => a.ForEach(b => b.CalculateGradient()));
			HiddenLayers.ForEach(a => a.ForEach(b => b.UpdateWeights(LearnRate, Momentum)));
			HiddenLayers.Reverse();
			OutputLayer.ForEach(a => a.UpdateWeights(LearnRate, Momentum));
		}

		public double[] Compute(params double[] inputs)
		{
			inputs = ScalleInput(inputs);
			ForwardPropagate(inputs);
			return OutputLayer.Select(a => a.Value).ToArray();
		}

		private double CalculateError(params double[] targets)
		{
			var i = 0;
			return OutputLayer.Sum(a => Math.Abs(a.CalculateError(targets[i++])));
		}
		#endregion

		#region -- Helpers --
		public static double GetRandom()
		{
			return 2 * Random.NextDouble() - 1;
		}


        public static List<DataSet> Scalling(List<DataSet> dataSet)
        {
            for (int column = 0; column < dataSet[0].Values.Length; column++)
            {
                var min = dataSet[0].Values[column];
                var max = dataSet[0].Values[column];
                for (var row = 1; row < dataSet.Count; row++)
                {
                    var item = dataSet[row].Values[column];
                    if (item < min)
                    {
                        min = item;
                    }

                    if (item > max)
                    {
                        max = item;
                    }
                }

                var divider = max - min;
                for (var row = 0; row < dataSet.Count; row++)
                {
                    dataSet[row].Values[column] = (dataSet[row].Values[column] - min) / divider;
                }
            }

            return dataSet;
        }

        private List<ScallingData> GetScallingDatas(List<DataSet> dataSets)
		{
			var scallingDatas = new List<ScallingData>();
			for (int column = 0; column < dataSets[0].Values.Length; column++)
			{
				var min = dataSets[0].Values[column];
				var max = dataSets[0].Values[column];
				for (var row = 1; row < dataSets.Count; row++)
				{
					var item = dataSets[row].Values[column];
					if (item < min)
					{
						min = item;
					}

					if (item > max)
					{
						max = item;
					}
				}
				scallingDatas.Add(new ScallingData(max,min));
			}
			return scallingDatas;
        }

		private double[] ScalleInput(double[] inputs)
		{
			for(var i=0; i< inputs.Length; i++)
			{
				if (inputs[i] > ScallingDatas[i].Max)
                    ScallingDatas[i].Max = inputs[i];

                if (inputs[i] < ScallingDatas[i].Min)
                    ScallingDatas[i].Min = inputs[i];

                inputs[i] = (inputs[i] - ScallingDatas[i].Min) / ScallingDatas[i].Max - ScallingDatas[i].Min;
			}
			return inputs;
		}
		#endregion
	}

	#region -- Enum --
	public enum TrainingType
	{
		Epoch,
		MinimumError
	}
	#endregion
}