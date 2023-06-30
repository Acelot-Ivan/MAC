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
            ActiveSettings.Ch0Ohm80 = newValue;
            ActiveSettings.Ch0Ohm90 = newValue;
            ActiveSettings.Ch0Ohm100 = newValue;
            ActiveSettings.Ch0Ohm115 = newValue;
            ActiveSettings.Ch0Ohm130 = newValue;
            ActiveSettings.Ch0Ohm140 = newValue;
        }

        private void IsAllCh1Click()
        {
            var isAllActive = !ActiveSettings.IsAllCh1Active;
            ActiveSettings.Ch1Ohm80 = isAllActive;
            ActiveSettings.Ch1Ohm90 = isAllActive;
            ActiveSettings.Ch1Ohm100 = isAllActive;
            ActiveSettings.Ch1Ohm115 = isAllActive;
            ActiveSettings.Ch1Ohm130 = isAllActive;
            ActiveSettings.Ch1Ohm140 = isAllActive;
        }

        private void IsAllCh2Click()
        {
            var isAllActive = !ActiveSettings.IsAllCh2Active;
            ActiveSettings.Ch2Ohm80 = isAllActive;
            ActiveSettings.Ch2Ohm90 = isAllActive;
            ActiveSettings.Ch2Ohm100 = isAllActive;
            ActiveSettings.Ch2Ohm115 = isAllActive;
            ActiveSettings.Ch2Ohm130 = isAllActive;
            ActiveSettings.Ch2Ohm140 = isAllActive;
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
            ActiveSettings.Ch6Hz5 = isAllActive;
            ActiveSettings.Ch6Hz25 = isAllActive;
            ActiveSettings.Ch6Hz50 = isAllActive;
            ActiveSettings.Ch6Hz75 = isAllActive;
            ActiveSettings.Ch6Hz100 = isAllActive;
        }
    }
}