using MAC.ViewModels;

namespace MAC.Views
{
    /// <summary>
    /// Логика взаимодействия для MainSettings.xaml
    /// </summary>
    public partial class MainSettings
    {
        public MainSettings(MainSettingsVm mainSettingsVm)
        {
            DataContext = mainSettingsVm;
            InitializeComponent();
        }
    }
}
