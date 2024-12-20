using System.CommandLine;
using Evolution.Differential;
using Evolution.Differential.Mutations;

var random = new Random(420);

int[] seeds = Enumerable.Range(0, 50).Select(_ => random.Next()).ToArray();

if (!Directory.Exists("outputs"))
{
    Directory.CreateDirectory("outputs");
}
else
{
    foreach (var file in Directory.GetFiles("outputs"))
    {
        File.Delete(file);
    }
}

int all = seeds.Length * ConsoleHelper.MutationNames.Length * ConsoleHelper.TestFunctionNames.Length;
int index = 0;
for (int i = 0; i < seeds.Length; i++)
{
    foreach (var functionName in ConsoleHelper.TestFunctionNames)
    {
        foreach (var mutationName in ConsoleHelper.MutationNames)
        {
            var evolution = new EvolutionEngine(seeds[i], 20, 5, 0.8, 0.9, 1000, TestFunctions.GetFunctionByName(functionName),
                ConsoleHelper.GetMutationByName(mutationName));
            evolution.CalculateEvolution($"outputs/{functionName}_{mutationName}_{i}.txt");
            Console.Clear();
            Console.WriteLine($"{++index}/{all}");
        }
    }
}

var rootCommand = new RootCommand("Differential Evolution Algorithm");
ConsoleHelper.CreateRootCommand(rootCommand);

//return rootCommand.Invoke(args);