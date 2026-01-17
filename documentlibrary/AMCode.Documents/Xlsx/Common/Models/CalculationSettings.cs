namespace AMCode.Documents.Xlsx
{
    public class CalculationSettings
    {
        public CalculationMode Mode { get; set; } = CalculationMode.Automatic;
        public CalculationOrder Order { get; set; } = CalculationOrder.Natural;
        public int MaxIterations { get; set; } = 100;
        public double MaxChange { get; set; } = 0.001;
        public bool EnableIterativeCalculation { get; set; }
    }
}
