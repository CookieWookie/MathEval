namespace MathEvaluator.Core
{
    public static class OperationPriorities
    {
        public const int Addition = 10;
        public const int Subtraction = Addition;
        public const int Multiplication = 20;
        public const int Division = Multiplication;
        public const int Modulus = Division;
        public const int Power = 30;
        public const int Function = 40;
    }
}
