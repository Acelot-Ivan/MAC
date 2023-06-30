using MAC.Models;
using MAC.ViewModels.Base;

namespace MAC.ViewModels
{
    public class MainSettingsVm : BaseVm
    {
        public RelayCommand SetLogSaveWay => new RelayCommand(SaveLogDialogPath);

        public MainSettingsModel MainSettingsModel { get; set; }
        public double ContentGridHeight { get; set; }
        public double ContentGridWidth { get; set; }
        public bool IsActiveTest { get; set; }

        public MainSettingsVm(MainSettingsModel mainSettingsModel, double contentGridHeight, double contentGridWidth, bool isActiveTest)
        {
            MainSettingsModel = mainSettingsModel;
            ContentGridWidth = contentGridWidth;
            ContentGridHeight = contentGridHeight;
            IsActiveTest = isActiveTest;
        }


        private void SaveLogDialogPath()
        {
            var folderBrowser = new System.Windows.Forms.FolderBrowserDialog();

            folderBrowser.ShowDialog();

            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                MainSettingsModel.FullLogPath = folderBrowser.SelectedPath;
            }
        }

    }
}
