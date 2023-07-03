using MAC.Models;
using MAC.ViewModels.Base;

namespace MAC.ViewModels
{
    public class MeasurementsSettingsVm : BaseVm
    {
        public double ContentGridHeight { get; set; }
        public double ContentGridWidth { get; set; }


        public ActiveSettingsModel ActiveSettings { get; set; }
        public bool IsActiveTest { get; set; }


        #region AllCheckBox Command

        public RelayCommand IsAllCh0ClickCommand => new RelayCommand(IsAllCh0Click);
        public RelayCommand IsAllCh1ClickCommand => new RelayCommand(IsAllCh1Click);
        public RelayCommand IsAllCh2ClickCommand => new RelayCommand(IsAllCh2Click);
        public RelayCommand IsAllCh3ClickCommand => new RelayCommand(IsAllCh3Click);
        public RelayCommand IsAllCh5ClickCommand => new RelayCommand(IsAllCh5Click);
        public RelayCommand IsAllCh6ClickCommand => new RelayCommand(IsAllCh6Click);

        #endregion

        public MeasurementsSettingsVm(ActiveSettingsModel activeSettings, double contentGridHeight,
            double contentGridWidth, bool isActiveTest)
        {
            ContentGridWidth = contentGridWidth;
            ContentGridHeight = contentGridHeight;
            ActiveSettings = activeSettings;
            IsActiveTest = isActiveTest;
        }


        //Биндинг на значения IsAllCh.. идет в одну сторону. Поэтому чек бокс не меняет значение по клику.
        //Значение меняется внутри команды , вызванным этом кликом.
        private void IsAllCh0Click()
        {
            var newValue = !ActiveSettings.IsAllCh0Active;
            ActiveSettings.Ch0Ohm1 = newValue;
            ActiveSettings.Ch0Ohm2 = newValue;
            ActiveSettings.Ch0Ohm3 = newValue;
            ActiveSettings.Ch0Ohm4 = newValue;
            ActiveSettings.Ch0Ohm5 = newValue;
        }

        private void IsAllCh1Click()
        {
            var isAllActive = !ActiveSettings.IsAllCh1Active;
            ActiveSettings.Ch1Ohm1 = isAllActive;
            ActiveSettings.Ch1Ohm2 = isAllActive;
            ActiveSettings.Ch1Ohm3 = isAllActive;
            ActiveSettings.Ch1Ohm4 = isAllActive;
            ActiveSettings.Ch1Ohm5 = isAllActive;
        }

        private void IsAllCh2Click()
        {
            var isAllActive = !ActiveSettings.IsAllCh2Active;
            ActiveSettings.Ch2Ohm1 = isAllActive;
            ActiveSettings.Ch2Ohm2 = isAllActive;
            ActiveSettings.Ch2Ohm3 = isAllActive;
            ActiveSettings.Ch2Ohm4 = isAllActive;
            ActiveSettings.Ch2Ohm5 = isAllActive;
        }

        private void IsAllCh3Click()
        {
            var isAllActive = !ActiveSettings.IsAllCh3Active;
            ActiveSettings.Ch3V1 = isAllActive;
            ActiveSettings.Ch3V2 = isAllActive;
            ActiveSettings.Ch3V3 = isAllActive;
            ActiveSettings.Ch3V4 = isAllActive;
            ActiveSettings.Ch3V5 = isAllActive;
        }

        private void IsAllCh5Click()
        {
            var isAllActive = !ActiveSettings.IsAllCh5Active;
            ActiveSettings.Ch5V1 = isAllActive;
            ActiveSettings.Ch5V2 = isAllActive;
            ActiveSettings.Ch5V3 = isAllActive;
            ActiveSettings.Ch5V4 = isAllActive;
            ActiveSettings.Ch5V5 = isAllActive;
        }

        private void IsAllCh6Click()
        {
            var isAllActive = !ActiveSettings.IsAllCh6Active;
            ActiveSettings.Ch6Hz1 = isAllActive;
            ActiveSettings.Ch6Hz2 = isAllActive;
            ActiveSettings.Ch6Hz3 = isAllActive;
            ActiveSettings.Ch6Hz4 = isAllActive;
            ActiveSettings.Ch6Hz5 = isAllActive;
        }
    }
}