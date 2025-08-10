using MauiApp2.Models;
using NUnit.Framework;

namespace MauiApp2.Tests
{
    [TestFixture]
    public class BrownianMotionModelTests
    {
        [Test]
        public void GenerateBrownianMotion_ReturnsArrayWithCorrectSize()
        {
            double sigma = 0.2;
            double mean = 0.01;
            double initialPrice = 100;
            int numDays = 252;

            double[] prices = BrownianMotionModel.GenerateBrownianMotion(sigma, mean, initialPrice, numDays);

            Assert.That(prices.Length, Is.EqualTo(numDays));
        }

        [Test]
        public void GenerateBrownianMotion_FirstPriceIsInitialPrice()
        {
            double sigma = 0.2;
            double mean = 0.01;
            double initialPrice = 100;
            int numDays = 252;

            double[] prices = BrownianMotionModel.GenerateBrownianMotion(sigma, mean, initialPrice, numDays);

            Assert.That(prices[0], Is.EqualTo(initialPrice));
        }

        [Test]
        public void GenerateBrownianMotion_PricesAreNotNegative()
        {
            double sigma = 0.2;
            double mean = 0.01;
            double initialPrice = 100;
            int numDays = 252;

            double[] prices = BrownianMotionModel.GenerateBrownianMotion(sigma, mean, initialPrice, numDays);

            Assert.That(prices.All(p => p >= 0));
        }
    }
}