using System.Runtime.CompilerServices;
using Evolution.Differential.Mutations;

namespace Evolution.Differential
{
    public class EvolutionEngine
    {
        private readonly Random _random;
        private readonly int _populationSize;
        private readonly int _subjectDimension;
        private readonly double _f;
        private readonly double _recombinationCoefficient;
        private readonly int _maxGenerations;
        private readonly double _minValue;
        private readonly double _maxValue;
        private readonly Func<double[], double> _fitnessFunction;
        private List<Subject> _population;
        private readonly IMutation _mutation;

        public EvolutionEngine(int seed, int populationSize, int subjectDimension, double f, double recombinationCoefficient,
            int maxGenerations, Func<double[], double> fitnessFunction, IMutation mutation)
        {
            _random = new Random(seed);
            _populationSize = populationSize;
            _f = f;
            _recombinationCoefficient = recombinationCoefficient;
            _maxGenerations = maxGenerations;
            (_minValue, _maxValue) = TestFunctions.GetRange(fitnessFunction);
            _mutation = mutation;
            _fitnessFunction = fitnessFunction;
            _subjectDimension = subjectDimension;
            InitiatePopulation();
        }

        private void InitiatePopulation()
        {
            _population = new List<Subject>(_populationSize);

            for (int i = 0; i < _populationSize; i++)
            {
                _population.Add(new Subject()
                {
                    Characteristics = Subject.GenerateCharacteristics(_random, _subjectDimension, _minValue, _maxValue),
                    FitnessFunction = _fitnessFunction,
                });
            }
        }

        public void CalculateEvolution(string? fileName)
        {
            bool writeToFile = !string.IsNullOrWhiteSpace(fileName);

            StreamWriter? writer = null;
            
            if (writeToFile)
            {
                writer = new StreamWriter(File.OpenWrite(fileName!));
                writer.Write("Generation,Fitness\n");
                writer.Write($"0,{GetBestFitness()}\n");
            }
            else
            {
                Console.WriteLine("Generation,Fitness\n");
                Console.WriteLine($"0,{GetBestFitness()}");
            }

            for (int i = 0; i < _maxGenerations; i++)
            {
                MutatePopulation();
                
                if (writeToFile)
                {
                    writer.Write($"{i + 1},{GetBestFitness()}\n");
                }
                else
                {
                    Console.WriteLine($"{i + 1},{GetBestFitness()}");
                }
            }
            
            writer?.Close();
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private void MutatePopulation()
        {
            var newPopulation = new List<Subject>(_populationSize);
            for (int i = 0; i < _populationSize; i++)
            {
                newPopulation.Add(MutateSubject(_population[i], i));
            }

            _population = newPopulation;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private Subject MutateSubject(Subject subject, int currentIndex)
        {
            if (_random.NextDouble() >= _recombinationCoefficient)
                return subject;

            var mutation = _mutation.GetMutation(currentIndex, _random, _population, _subjectDimension, _minValue, _maxGenerations, _f);

            if (_fitnessFunction(mutation) < _fitnessFunction(subject.Characteristics))
            {
                return new Subject()
                {
                    Characteristics = mutation, FitnessFunction = _fitnessFunction
                };
            }

            return subject;
        }

        private double GetBestFitness()
        {
            return _population.Select(x => _fitnessFunction(x.Characteristics)).OrderBy(x => x).First();
        }
    }
}