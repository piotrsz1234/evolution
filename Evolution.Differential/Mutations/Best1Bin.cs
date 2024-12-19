namespace Evolution.Differential.Mutations
{
    public class Best1Bin : IMutation
    {
        public double[] GetMutation(int currentIndex, Random random, List<Subject> population, int subjectDimension, double minValue, double maxValue, double f)
        {
            double[] mutation = new double[subjectDimension];

            var best = population.OrderByDescending(x => x.FitnessFunction(x.Characteristics)).First();
            int br = population.IndexOf(best);
            int r1, r2;
            do { r1 = random.Next(population.Count); } while (r1 == currentIndex || r1 == br);
            do { r2 = random.Next(population.Count); } while (r2 == currentIndex || r2 == r1 || r2 == br);
            
            for (int j = 0; j < subjectDimension; j++)
            {
                mutation[j] = best.Characteristics[j] + f * (population[r2].Characteristics[j] - population[r1].Characteristics[j]);
                mutation[j] = Math.Clamp(mutation[j], minValue, maxValue);
            }
            
            return mutation;
        }
    }
}