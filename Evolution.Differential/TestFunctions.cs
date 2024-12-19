using System.Runtime.CompilerServices;

namespace Evolution.Differential
{
    public class TestFunctions
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static double RastriginFunction(double[] input)
        {
            const double a = 10;
            var result = a * input.Length;

            foreach (var t in input)
            {
                result += t * t - a * Math.Cos(2 * Math.PI * t);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static double SphereFunction(double[] input)
        {
            double result = 0;

            foreach (var i in input)
            {
                result += i * i;
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static double RosenbrockFunction(double[] input)
        {
            double result = 0;
            for (int i = 0; i < input.Length - 1; i++)
            {
                result += 100 * Math.Pow(input[i + 1] - input[i] * input[i], 2) + Math.Pow(1 - input[i], 2);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static double StyblinskiTangFunction(double[] input)
        {
            double result = 0;

            foreach (var item in input)
            {
                result += Math.Pow(item, 4) - 16 * Math.Pow(item, 2) + 5 * item;
            }

            return result / 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static double AckleyFunction(double[] input)
        {
            const double a = 20;
            const double b = 0.2;
            const double c = 2 * Math.PI;

            double d = 1 / (double)input.Length;
            var sumOfSquares = input.Sum(x => Math.Pow(x, 2));
            var sumOfCos = input.Sum(x => Math.Cos(c * x));

            return -a * Math.Exp(-b * Math.Sqrt(d * sumOfSquares)) - Math.Exp(d * sumOfCos) + a + Math.Exp(1);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static double GriewankFunction(double[] input)
        {
            var sum = input.Sum(x => Math.Pow(x, 2) / 4000);
            double mul = 1;
            for (int i = 0; i < input.Length; i++)
            {
                mul *= Math.Cos(input[i] / Math.Sqrt(i + 1));
            }

            return sum - mul + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static double SchwefelFunction(double[] input)
        {
            double result = 418.9829 * input.Length;
            foreach (var item in input)
            {
                result -= item * Math.Sin(Math.Sqrt(Math.Abs(item)));
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static double RotatedHyperEllipsoidFunction(double[] input)
        {
            double result = 0;
            foreach (var unused in input)
            {
                foreach (var j in input)
                {
                    result += j * j;
                }
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static double SumOfDifferentPowersFunction(double[] input)
        {
            double result = 0;
            for (int i = 0; i < input.Length; i++)
            {
                result += Math.Pow(input[i], i + 2);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static double SumOfSquaresFunction(double[] input)
        {
            double result = 0;
            for (int i = 0; i < input.Length; i++)
            {
                result += (i + 1) * input[i] * input[i];
            }

            return result;
        }

        public static (double MinValue, double MaxValue) GetRange(Func<double[], double> function)
        {
            if (function == RastriginFunction)
            {
                return (-5.12, 5.12);
            }

            if (function == SphereFunction)
            {
                return (-5.12, 5.12);
            }

            if (function == RosenbrockFunction)
            {
                return (-5, 10);
            }

            if (function == AckleyFunction)
            {
                return (-32.768, 32.768);
            }

            if (function == GriewankFunction)
            {
                return (-600, 600);
            }

            if (function == SchwefelFunction)
            {
                return (-500, 500);
            }

            if (function == RotatedHyperEllipsoidFunction)
            {
                return (-65.536, 65.536);
            }

            if (function == SumOfDifferentPowersFunction)
            {
                return (-1, 1);
            }

            if (function == SumOfSquaresFunction)
            {
                return (-10, 10);
            }

            if (function == StyblinskiTangFunction)
            {
                return (-5, 5);
            }
            
            return (-5.12, 5.12);
        }

        public static Func<double[], double> GetFunctionByName(string name)
        {
            switch (name)
            {
                case "Rastrigin":
                    return RastriginFunction;
                case "Sphere":
                    return SphereFunction;
                case "Rosenbrock":
                    return RosenbrockFunction;
                case "StyblinskiTang":
                    return StyblinskiTangFunction;
                case "Ackley":
                    return AckleyFunction;
                case "Griewank":
                    return GriewankFunction;
                case "Schwefel":
                    return SchwefelFunction;
                case "RotatedHyperEllipsoid":
                    return RotatedHyperEllipsoidFunction;
                case "SumOfDifferentPowers":
                    return SumOfDifferentPowersFunction;
                case "SumOfSquares":
                    return SumOfSquaresFunction;
                default:
                    throw new ArgumentException("Unknown function: " + name);
            }
        }
    }
}