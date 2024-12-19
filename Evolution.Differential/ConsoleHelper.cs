using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Parsing;
using Evolution.Differential.Mutations;

namespace Evolution.Differential
{
    public static class ConsoleHelper
    {
        public static string[] TestFunctionNames =
        [
            "Rastrigin", "Sphere", "Rosenbrock", "StyblinskiTang", "Ackley",
            "Griewank", "Schwefel", "RotatedHyperEllipsoid", "SumOfDifferentPowers",
            "SumOfSquares"
        ];

        public static string[] MutationNames =
        [
            "rand1", "best1",
            "currenttobest",
            "randtobest"
        ];
        
        public static void CreateRootCommand(RootCommand rootCommand)
        {
            var fOption = new Option<double>(["-w", "--weight"], () => 0.8, "Differential weight of the Evolution Algorithm");
            fOption.AddValidator(result =>
            {
                if (result.GetValueForOption(fOption) > 2 || result.GetValueForOption(fOption) < 0)
                {
                    result.ErrorMessage = "Differential weight must be in range [0, 2]";
                }
            });
            var populationSizeOption = new Option<uint>(["-p", "--population"], "Population size of the Evolution Algorithm")
                { IsRequired = true };
            var dimensionSizeOption = new Option<uint>(["-d", "--dimension"], "Dimension of the Evolution Algorithm") { IsRequired = true };
            var rOption = new Option<double>(["-c", "--crossover"], () => 0.9, "Crossover coefficient of the Evolution Algorithm");
            rOption.AddValidator(result =>
            {
                if (result.GetValueForOption(rOption) > 1 || result.GetValueForOption(rOption) < 0)
                {
                    result.ErrorMessage = "Crossover coefficient must be in range [0, 1]";
                }
            });

            var generationsCountOption = new Option<uint>(["-g", "--generations"], "Number of generations of the Evolution Algorithm")
                { IsRequired = true };

            var testFunctionOption = new Option<string>(["-f", "--function"], "Function to be used for test") { IsRequired = true }.FromAmong(TestFunctionNames);

            var mutationNameOption =
                new Option<string>(["-m", "--mutation"], "Mutation used for test") { IsRequired = true }.FromAmong(MutationNames);
            var seedOption = new Option<int>(["-s", "--seed"], () => 237376647, "Seed for random");
            var fileOption = new Option<FileInfo>(["-o", "--output"], "Output file. If not provided, output will be printed in terminal");
            
            rootCommand.Add(fOption);
            rootCommand.Add(populationSizeOption);
            rootCommand.Add(dimensionSizeOption);
            rootCommand.Add(rOption);
            rootCommand.Add(generationsCountOption);
            rootCommand.Add(testFunctionOption);
            rootCommand.Add(mutationNameOption);
            rootCommand.Add(seedOption);
            rootCommand.Add(fileOption);
            rootCommand.SetHandler((options) =>
            {
                var evolutionEngine = new EvolutionEngine(options.Seed, options.PopulationSize, options.DimensionSize, options.F, options.R, options.GenerationCount, TestFunctions.GetFunctionByName(options.TestFunctionName),
                    GetMutationByName(options.MutationName));
                evolutionEngine.CalculateEvolution(options.FileInfo?.FullName);
            }, new AllOptionsBinder(fOption, populationSizeOption, dimensionSizeOption, rOption, generationsCountOption, testFunctionOption, mutationNameOption, seedOption, fileOption));
        }

        public static IMutation GetMutationByName(string mutationName)
        {
            switch (mutationName)
            {
                case "currenttobest":
                    return new CurrentToBest();
                case "rand1":
                    return new Rand1Bin();
                case "randtobest":
                    return new RandToBest();
                case "best1":
                    return new Best1Bin();
                default:
                    throw new ArgumentException($"Unknown mutation: {mutationName}");
            }
        }
    }

    public class AllOptions
    {
        public double F { get; set; }
        public int PopulationSize { get; set; }
        public int DimensionSize { get; set; }
        public double R { get; set; }
        public int GenerationCount { get; set; }
        public string TestFunctionName { get; set; } = string.Empty;
        public string MutationName { get; set; } = string.Empty;
        public int Seed { get; set; }
        public FileInfo? FileInfo { get; set; }
    }
    
    public class AllOptionsBinder : BinderBase<AllOptions>
    {
        private readonly Option<double> _fOption;
        private readonly Option<uint> _populationSizeOption;
        private readonly Option<uint> _dimensionSizeOption;
        private readonly Option<double> _rOption;
        private readonly Option<uint> _generationsCountOption;
        private readonly Option<string> _testFunctionOption;
        private readonly Option<string> _mutationNameOption;
        private readonly Option<int> _seedOption;
        private readonly Option<FileInfo> _fileOption;

        public AllOptionsBinder(Option<double> fOption, Option<uint> populationSizeOption, Option<uint> dimensionSizeOption, Option<double> rOption, Option<uint> generationsCountOption, Option<string> testFunctionOption, Option<string> mutationNameOption, Option<int> seedOption, Option<FileInfo> fileOption)
        {
            _fOption = fOption;
            _populationSizeOption = populationSizeOption;
            _dimensionSizeOption = dimensionSizeOption;
            _rOption = rOption;
            _generationsCountOption = generationsCountOption;
            _testFunctionOption = testFunctionOption;
            _mutationNameOption = mutationNameOption;
            _seedOption = seedOption;
            _fileOption = fileOption;
        }


        protected override AllOptions GetBoundValue(BindingContext bindingContext) =>
            new AllOptions
            {
                F = bindingContext.ParseResult.GetValueForOption(_fOption),
                PopulationSize = (int)bindingContext.ParseResult.GetValueForOption(_populationSizeOption),
                DimensionSize = (int)bindingContext.ParseResult.GetValueForOption(_dimensionSizeOption),
                R = bindingContext.ParseResult.GetValueForOption(_rOption),
                GenerationCount = (int)bindingContext.ParseResult.GetValueForOption(_generationsCountOption),
                TestFunctionName = bindingContext.ParseResult.GetValueForOption(_testFunctionOption)!,
                MutationName = bindingContext.ParseResult.GetValueForOption(_mutationNameOption)!,
                Seed = bindingContext.ParseResult.GetValueForOption(_seedOption),
                FileInfo = bindingContext.ParseResult.GetValueForOption(_fileOption)
            };
    }
}