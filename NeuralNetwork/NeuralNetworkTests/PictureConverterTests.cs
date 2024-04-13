using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetwork.Helpers;

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
    }
}
