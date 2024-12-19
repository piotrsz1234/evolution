using System.CommandLine;
using Evolution.Differential;
using Evolution.Differential.Mutations;

// int[] seeds = [420, 100, 12321313];
// double[] rValues = [0.1, 0.3, 0.5, 0.9, 1];
// double[] fValues = [0.1, 0.5, 0.8, 1,3, 2];
//
// if (!Directory.Exists("outputs"))
// {
//     Directory.CreateDirectory("outputs");
// }
//
// int all = seeds.Length * rValues.Length * fValues.Length * ConsoleHelper.TestFunctionNames.Length * ConsoleHelper.MutationNames.Length;
// int index = 0;
// for (int i = 0; i < seeds.Length; i++)
// {
//     for(int j = 0; j < rValues.Length; j++)
//     {
//         for(int k = 0; k < fValues.Length; k++)
//         {
//             foreach (var functionName in ConsoleHelper.TestFunctionNames)
//             {
//                 foreach (var mutationName in ConsoleHelper.MutationNames)
//                 {
//                     var evolution = new EvolutionEngine(seeds[i], 20, 5, fValues[k], rValues[j], 1000, TestFunctions.GetFunctionByName(functionName), ConsoleHelper.GetMutationByName(mutationName));
//                     evolution.CalculateEvolution($"outputs/{j}_{k}_{functionName}_{mutationName}_{i}.txt");
//                     Console.WriteLine($"{++index}/{all}");
//                 }
//             }
//         }
//     }
// }

var rootCommand = new RootCommand("Differential Evolution Algorithm");
ConsoleHelper.CreateRootCommand(rootCommand);

return rootCommand.Invoke(args);