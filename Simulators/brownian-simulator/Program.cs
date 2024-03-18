using System;
using System.Security.Cryptography;

/// <summary>
/// Generates random numbers with a gaussian distribution instead of a uniform distribution
/// </summary>
/// <remarks>
/// Generated with the ChatGPT Prompt:
///
/// C# to generate a cryptographically secure random number between -1 and 1 with a Gaussian distribution
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

class Program
{
    static void Main()
    {
        Console.WriteLine($"Day, Price");
        Decimal price = 100;

        for (int i = 0; i < 1000; i++)
        {
            using GaussianRandom gr = new GaussianRandom();
            // Adjust mean and standard deviation to scale the distribution between -1 and 1.
            // You might need to experiment with these values to best fit your requirements,
            // here we use 0 as mean and a standard deviation that will likely keep most
            // values within the desired range, but it's not a guarantee.
            double number = gr.NextGaussian(0, 1.0 / 3);

            // Ensure the generated number is within the desired range
            number = Math.Max(Math.Min(number, 1), -1) / 7.0;

            price = price * (Decimal)(1 + number);

            Console.WriteLine($"{i}, {price}");
        }
    }
}