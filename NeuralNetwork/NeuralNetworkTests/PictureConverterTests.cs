using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetwork.Helpers;
using System;

namespace NeuralNetworkTests
{
    [TestClass]
    public class PictureConverterTests
    {
        [TestMethod()]
        public void ConvertTest()
        {
            var converter = new PictureConverter();
            var parasitized = converter.Convert("Images\\Parasitized.png");
            var unParasitized = converter.Convert("Images\\Unparasitized.png");

            converter.Save("Images\\ConvertedParasitied.png", parasitized);
            converter.Save("Images\\ConvertedUnparasitied.png", unParasitized);
        }

        [TestMethod()]
        public void ConvertTest2()
        {
            var count = 100;
            var parasitizedPath = @"C:\Users\user\Downloads\archive\cell_images\cell_images\Parasitized";
            var unparasitizedPath = @"C:\Users\user\Downloads\archive\cell_images\cell_images\Uninfected";


            var parasitizedDatasets = ImportHelper.ImportImageDataset(parasitizedPath, count, 1);
            var unParasitizedDatasets = ImportHelper.ImportImageDataset(unparasitizedPath, count, 1);

            var converter = new PictureConverter();
            for (int i = 0; i < parasitizedDatasets.Count; i++)
            {            
                converter.Save($"Cells\\Parasitized\\image{i}.png", parasitizedDatasets[i].Values);
                converter.Save($"Cells\\Uninfected\\image{i}.png", unParasitizedDatasets[i].Values);
            }
        }
    }
}
