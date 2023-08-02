using MAC.ViewModels;

namespace MAC.Views
{
    /// <summary>
    /// Логика взаимодействия для SetMeasurementsData.xaml
    /// </summary>
    public partial class SetMeasurementsData
    {
        public SetMeasurementsDataVm ViewModel => (SetMeasurementsDataVm)DataContext;

        public SetMeasurementsData(SetMeasurementsDataVm setMeasurementsDataVm)
        {
            InitializeComponent();

            DataContext = setMeasurementsDataVm;

            ViewModel.CloseWindow = CloseWindow;
        }


        private void CloseWindow()
        {
            Close();
        }
    }
}