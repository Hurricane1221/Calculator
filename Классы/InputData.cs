using System.Globalization;
using System.Windows.Controls;

namespace Calculator
{
    public static class InputData
    {
        public static bool LocalizeInput(string input, out double value)
        {
            string localized = input.Replace(',', '.');
            return double.TryParse(localized, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
        }
        public static double InputValue(TextBox textBox, ComboBox comboBox, string errorMessage, Dictionary<string, double> unitMap)
        {
            if (!LocalizeInput(textBox.Text, out double value))
                throw new InvalidOperationException(errorMessage);

            string unit = ((ComboBoxItem)comboBox.SelectedItem)?.Content?.ToString() ?? "";
            value = Converter.Convert(unit, value, unitMap);

            if (value <= 0)
                throw new InvalidOperationException($"{errorMessage} Значение должно быть положительным.");

            return value;
        }
        public static double InputTemperature(TextBox textBox, ComboBox comboBox, string errorMessage, bool allowZero = true)
        {
            if (!double.TryParse(textBox.Text, out double value))
                throw new InvalidOperationException(errorMessage);

            string unit = ((ComboBoxItem)comboBox.SelectedItem)?.Content?.ToString() ?? "";
            if (unit == "°K")
                value -= 273.15;

            if (!allowZero && value < 0)
                throw new InvalidOperationException("Температура не может быть отрицательной.");

            if (value < -273.15)
                throw new InvalidOperationException("Температура не может быть ниже абсолютного нуля.");

            return value;
        }
    }
}
