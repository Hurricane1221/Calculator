namespace Calculator
{
    public static class Converter
    {
        public static readonly Dictionary<string, double> AmpereUnits = new()
        {
            { "А", 1 },
            { "мА", 0.001 },
            { "мкА", 0.000001 }
        };
        public static readonly Dictionary<string, double> ThicknessUnits = new()
        {
            { "мм", 1 },
            { "унция/фут²", 0.03479 },
            { "мил", 0.0254 },
            { "см", 10 },
            { "мкм", 0.001 },
            { "дюйм", 25.4 }
        };
        public static readonly Dictionary<string, double> LengthUnits = new()
        {
            { "м", 1 },
            { "см", 0.01 },
            { "мм", 0.001 },
            { "мкм", 0.000001 },
            { "мил", 0.0000254 },
            { "дюйм", 0.0254 }
        };
        public static readonly Dictionary<string, double> WidthUnits = new()
        {
            { "мил", 1 },
            { "см", 0.00254 },
            { "мм", 0.0254 },
            { "мкм", 25.4 },
            { "дюйм", 0.001 }
        };
        public static double Convert(string unit, double value, Dictionary<string, double> unitMap)
        {
            if (unitMap.TryGetValue(unit, out double factor))
                return value * factor;
            return value;
        }
    }
}
