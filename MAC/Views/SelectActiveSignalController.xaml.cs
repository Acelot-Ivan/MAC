using MAC.ViewModels;

namespace MAC.Views
{
    /// <summary>
    /// Логика взаимодействия для SelectActiveSignalController.xaml
    /// </summary>
    public partial class SelectActiveSignalController
    {
        public SelectActiveMacVm ViewModels => (SelectActiveMacVm)DataContext;
        public SelectActiveSignalController(SelectActiveMacVm selectActiveMacVm)
        {
            InitializeComponent();

            DataContext = selectActiveMacVm;

            ViewModels.CloseWindow = CloseWindow;
        }

        private void CloseWindow()
        {
            Close();
        }
    }
}
