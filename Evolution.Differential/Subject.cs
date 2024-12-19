using System.Buffers;

namespace Evolution.Differential
{
    public class Subject
    {
        public required double[] Characteristics { get; init; }
        public required Func<double[], double> FitnessFunction { get; init; }

        public static double[] GenerateCharacteristics(Random random, int subjectDimension, double minValue, double maxValue)
        {
            double[] result = new double[subjectDimension];

            for (int i = 0; i < subjectDimension; i++)
            {
                result[i] = random.NextDouble() * (maxValue - minValue) + minValue;
            }
            
            return result;
        }
    }
}