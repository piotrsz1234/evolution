namespace Evolution.Differential.Mutations
{
    public interface IMutation
    {
        double[] GetMutation(int currentIndex, Random random, List<Subject> population, int subjectDimension, double minValue, double maxValue, double f);
    }
}