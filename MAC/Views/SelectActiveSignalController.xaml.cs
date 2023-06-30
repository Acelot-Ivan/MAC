using MAC.ViewModels;

namespace MAC.Views
{
    /// <summary>
    /// Логика взаимодействия для SelectActiveSignalController.xaml
    /// </summary>
    public partial class SelectActiveSignalController
    {
        public SelectActiveSignalControllerVm ViewModels => (SelectActiveSignalControllerVm)DataContext;
        public SelectActiveSignalController(SelectActiveSignalControllerVm selectActiveSignalControllerVm)
        {
            InitializeComponent();

            DataContext = selectActiveSignalControllerVm;

            ViewModels.CloseWindow = CloseWindow;
        }

        private void CloseWindow()
        {
            Close();
        }
    }
}
