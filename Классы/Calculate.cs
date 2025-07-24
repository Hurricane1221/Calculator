namespace Calculator
{
    public class CalculatorResult
    {
        public double FinalTemp { get; set; }
        public double WidthExMil { get; set; }
        public double WidthInMil { get; set; }
        public double ResistanceEx { get; set; }
        public double ResistanceIn { get; set; }
        public double VoltageEx { get; set; }
        public double VoltageIn { get; set; }
        public double PowerEx { get; set; }
        public double PowerIn { get; set; }
    }
    public static class Calculate
    {
        public static CalculatorResult Calculator(double maxI, double trackThickness, double trackLength, double deltaT, double ambT)
        {
            double finalT = deltaT + ambT;
            const double kEx = 0.048, kIn = 0.024, b = 0.44, c = 0.725;

            double areaExMil = Math.Pow(maxI / (kEx * Math.Pow(deltaT, b)), 1 / c);
            double areaInMil = Math.Pow(maxI / (kIn * Math.Pow(deltaT, b)), 1 / c);

            double trackThicknessUnc = trackThickness / 0.03479;
            double widthExMil = areaExMil / (trackThicknessUnc * 1.378);
            double widthInMil = areaInMil / (trackThicknessUnc * 1.378);

            double widthExMm = widthExMil * 0.0254;
            double widthInMm = widthInMil * 0.0254;

            double rhoStart = 0.0168 * (1 + 0.00393 * (ambT - 20));
            double rhoEnd = rhoStart * (1 + 0.00393 * (finalT - ambT));

            double rEx = (rhoEnd * trackLength) / (widthExMm * trackThickness);
            double rIn = (rhoEnd * trackLength) / (widthInMm * trackThickness);

            double uEx = rEx * maxI;
            double uIn = rIn * maxI;

            return new CalculatorResult
            {
                FinalTemp = finalT,
                WidthExMil = widthExMil,
                WidthInMil = widthInMil,
                ResistanceEx = rEx,
                ResistanceIn = rIn,
                VoltageEx = uEx,
                VoltageIn = uIn,
                PowerEx = uEx * maxI,
                PowerIn = uIn * maxI
            };
        }
    }
}
