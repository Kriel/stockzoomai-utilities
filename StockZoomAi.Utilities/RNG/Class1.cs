using System.Security.Cryptography;

namespace StockZoomAi.Utilities.RNG
{
    /// <summary>
    /// Generates random numbers with a gaussian distribution instead of a uniform distribution
    /// </summary>
    /// <remarks>
    /// Generated with the ChatGPT Prompt:
    ///
    /// C# to generate a cryptographically secure random number between -1 and 1 with a gaussian distribution
    /// </remarks>
    public class GaussianRandom : IDisposable
    {
        private readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

        public double NextGaussian(double mean, double standardDeviation)
        {
            // Use Box-Muller transform to generate two independent standard normally distributed random numbers
            double u1 = 1.0 - GetRandomDouble(); // Subtraction to avoid taking log of 0
            double u2 = GetRandomDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                   Math.Sin(2.0 * Math.PI * u2);

            // Scale and shift the generated value to fit the desired mean and standard deviation
            double randNormal = mean + standardDeviation * randStdNormal;

            return randNormal;
        }

        private double GetRandomDouble()
        {
            // Generate a random number between 0 and 1
            byte[] bytes = new byte[8];
            rng.GetBytes(bytes);
            ulong ul = BitConverter.ToUInt64(bytes, 0);
            return ul / (double)ulong.MaxValue;
        }

        public void Dispose()
        {
            rng.Dispose();
        }
    }
}
