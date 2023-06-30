using System;
using MAC.ViewModels;
using MAC.ViewModels.Services;

namespace MAC.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindowVm ViewModel => (MainWindowVm)DataContext;

        public MainWindow()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                GlobalLog.Log.Error(args.ExceptionObject as Exception, "[{Sender}] Критическая ошибка", sender);
            InitializeComponent();
            DataContext = new MainWindowVm();
        }

    }
}
