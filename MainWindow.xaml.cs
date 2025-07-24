using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double maxI = InputData.InputValue(MaxI, ComboBoxMaxI, "Некорректное значение тока.", Converter.AmpereUnits);
                double trackThickness = InputData.InputValue(TrackThickness, ComboBoxTrackThickness, "Некорректное значение толщины дорожки.", Converter.ThicknessUnits);
                double trackLength = InputData.InputValue(TrackLength, ComboBoxTrackLength, "Некорректное значение длины дорожки.", Converter.LengthUnits);
                double deltaT = InputData.InputTemperature(TempRise, ComboBoxTempRise, "Некорректное значение повышения температуры", allowZero: false);
                double ambT = InputData.InputTemperature(AmbTemp, ComboBoxAmbTemp, "Некорректное значение температуры окружающей среды");

                var result = Calculate.Calculator(maxI, trackThickness, trackLength, deltaT, ambT);

                double widthEx = Converter.Convert(((ComboBoxItem)ComboBoxUW.SelectedItem)?.Content?.ToString() ?? "", result.WidthExMil, Converter.WidthUnits);
                double widthIn = Converter.Convert(((ComboBoxItem)ComboBoxUW.SelectedItem)?.Content?.ToString() ?? "", result.WidthInMil, Converter.WidthUnits);

                ExTrackTemp.Text = result.FinalTemp.ToString("F1");
                InTrackTemp.Text = result.FinalTemp.ToString("F1");

                ExTrackWidth.Text = widthEx.ToString("F3");
                InTrackWidth.Text = widthIn.ToString("F3");

                ExResist.Text = result.ResistanceEx.ToString("F5");
                InResist.Text = result.ResistanceIn.ToString("F5");

                ExVolt.Text = result.VoltageEx.ToString("F5");
                InVolt.Text = result.VoltageIn.ToString("F5");

                ExPow.Text = result.PowerEx.ToString("F5");
                InPow.Text = result.PowerIn.ToString("F5");
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