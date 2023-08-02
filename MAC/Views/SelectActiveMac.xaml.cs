using MAC.ViewModels;

namespace MAC.Views
{
    /// <summary>
    /// Логика взаимодействия для SelectActiveMac.xaml
    /// </summary>
    public partial class SelectActiveMac
    {
        public SelectActiveMacVm ViewModels => (SelectActiveMacVm)DataContext;
        public SelectActiveMac(SelectActiveMacVm selectActiveMacVm)
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
