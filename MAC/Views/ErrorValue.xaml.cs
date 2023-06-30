using MAC.ViewModels;

namespace MAC.Views
{
    /// <summary>
    /// Логика взаимодействия для ErrorValue.xaml
    /// </summary>
    public partial class ErrorValue
    {
        public ErrorValue(ErrorValueVm errorValueVm)
        {
            InitializeComponent();
            DataContext = errorValueVm;
        }
    }
}
