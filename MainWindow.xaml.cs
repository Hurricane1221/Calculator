using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private double GetValidatedInput(TextBox textBox, string errorMessage)
        {
            if (!double.TryParse(textBox.Text, out double value))
                throw new InvalidOperationException(errorMessage);
            return value;
        }

        private double GetMaxI()
        {
            double maxI = GetValidatedInput(MaxI, "Некорректное значение тока.");
            string unit = ((ComboBoxItem)ComboBoxMaxI.SelectedItem)?.Content?.ToString() ?? "";
            return unit switch
            {
                "мА" => maxI * 0.001,
                "мкА" => maxI * 0.000001,
                _ => maxI
            };
        }

        private double GetTrackThickness()
        {
            double trackThickness = GetValidatedInput(TrackThickness, "Некорректное значение толщины дорожки.");
            string unit = ((ComboBoxItem)ComboBoxTrackThickness.SelectedItem)?.Content?.ToString() ?? "";
            return unit switch
            {
                "унция/фут²" => trackThickness * 0.035,
                "мил" => trackThickness * 0.0254,
                "см" => trackThickness * 10,
                "мм" => trackThickness,
                "мкм" => trackThickness * 0.001,
                "дюйм" => trackThickness * 25.4,
                _ => trackThickness
            };
        }

        private double GetTempRise()
        {
            double tempRise = GetValidatedInput(TempRise, "Некорректное значение температуры.");
            string unit = ((ComboBoxItem)ComboBoxTempRise.SelectedItem)?.Content?.ToString() ?? "";
            return unit switch
            {
                "°K" => tempRise - 273.15,
                _ => tempRise
            };
        }

        private double GetAmbTemp()
        {
            double ambTemp = GetValidatedInput(AmbTemp, "Некорректное значение температуры.");
            string unit = ((ComboBoxItem)ComboBoxAmbTemp.SelectedItem)?.Content?.ToString() ?? "";
            return unit switch
            {
                "°K" => ambTemp - 273.15,
                _ => ambTemp
            };
        }

        private double GetTrackLength()
        {
            double trackLength = GetValidatedInput(TrackLength, "Некорректное значение длины дорожки.");
            string unit = ((ComboBoxItem)ComboBoxTrackLength.SelectedItem)?.Content?.ToString() ?? "";
            return unit switch
            {
                "мил" => trackLength * 0.0000254,
                "см" => trackLength * 0.01,
                "мм" => trackLength * 0.001,
                "мкм" => trackLength * 0.000001,
                "дюйм" => trackLength * 0.0254,
                _ => trackLength
            };
        }

        private double GetComboBoxUW(ref double temp)
        {
            string unit = ((ComboBoxItem)ComboBoxUW.SelectedItem)?.Content?.ToString() ?? "";
            return unit switch
            {
                "мил" => temp,
                "см" => temp * 0.00254,
                "мм" => temp * 0.0254,
                "мкм" => temp * 25.4,
                "дюйм" => temp * 0.001,
                _ => temp
            };
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double maxI = GetMaxI(); // Максимальный ток           
                double trackThickness = GetTrackThickness(); // Толщина дорожки                       
                double trackLength = GetTrackLength(); // Длина дорожки

                double deltaT = GetTempRise();
                double finalT = deltaT + GetAmbTemp();
                ExTrackTemp.Text = finalT.ToString("F1");
                InTrackTemp.Text = finalT.ToString("F1");

                const double kEx = 0.048, kIn = 0.024, b = 0.44, c = 0.725;

                // S – площадь сечения дорожки, мил^2
                double areaEx = Math.Pow(maxI / (kEx * Math.Pow(deltaT, b)), 1 / c);
                double areaIn = Math.Pow(maxI / (kIn * Math.Pow(deltaT, b)), 1 / c);

                // W - ширина дорожки, мил
                double thicknessMil = trackThickness / 0.0254;
                double widthExMil = areaEx / thicknessMil;
                double widthInMil = areaIn / thicknessMil;
                double widthEx = GetComboBoxUW(ref widthExMil);
                double widthIn = GetComboBoxUW(ref widthInMil);
                ExTrackWidth.Text = widthEx.ToString("F3");
                InTrackWidth.Text = widthIn.ToString("F3");

                const double rho = 1.68e-8; // Ом*м

                // Перевод ширины и толщины в метры
                double widthExM = widthExMil * 0.0000254;
                double widthInM = widthInMil * 0.0000254;
                double thicknessM = trackThickness * 0.001;

                double areaExM2 = widthExM * thicknessM;
                double areaInM2 = widthInM * thicknessM;

                // Сопротивление
                double rEx = rho * trackLength / areaExM2;
                double rIn = rho * trackLength / areaInM2;
                ExResist.Text = rEx.ToString("F5");
                InResist.Text = rIn.ToString("F5");

                // Падение напряжения
                double uEx = rEx * maxI;
                double uIn = rIn * maxI;
                ExVolt.Text = uEx.ToString("F5");
                InVolt.Text = uIn.ToString("F5");

                // Рассеиваемая мощность
                double pEx = uEx * maxI;
                double pIn = uIn * maxI;
                ExPow.Text = pEx.ToString("F5");
                InPow.Text = pIn.ToString("F5");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла непредвиденная ошибка:\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}