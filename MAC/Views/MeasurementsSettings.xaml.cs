using MAC.ViewModels;

namespace MAC.Views
{
    /// <summary>
    /// Логика взаимодействия для MeasurementsSettings.xaml
    /// </summary>
    public partial class MeasurementsSettings
    {
        public MeasurementsSettings(MeasurementsSettingsVm measurementsSettingsVm)
        {
            InitializeComponent();
            DataContext = measurementsSettingsVm;
        }
    }
}
