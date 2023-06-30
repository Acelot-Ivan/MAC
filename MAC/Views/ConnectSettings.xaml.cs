using System;
using System.Windows;
using MAC.ViewModels;

namespace MAC.Views
{
    /// <summary>
    /// Логика взаимодействия для ConnectSettings.xaml
    /// </summary>
    public partial class ConnectSettings
    {
        public ConnectSettingsVm ViewModel => (ConnectSettingsVm)DataContext;

        public ConnectSettings(ConnectSettingsVm connectSettingsVm)
        {
            InitializeComponent();
            DataContext = connectSettingsVm;
        }

        private void Selector_OnSelected(object sender, RoutedEventArgs e)
        {
            ViewModel.UpdateAvailableComPorts();
        }

        private void ComboBox_OnDropDownOpened(object sender, EventArgs e)
        {
            ViewModel.UpdateAvailableComPort();
        }
    }
}