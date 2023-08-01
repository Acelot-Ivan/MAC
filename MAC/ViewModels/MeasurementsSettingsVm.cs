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
            ActiveSettings.Ohm1X1 = newValue;
            ActiveSettings.Ohm2X1 = newValue;
            ActiveSettings.Ohm3X1 = newValue;
            ActiveSettings.Ohm4X1 = newValue;
            ActiveSettings.Ohm5X1 = newValue;
        }

        private void IsAllCh1Click()
        {
            var isAllActive = !ActiveSettings.IsAllCh1Active;
            ActiveSettings.Ohm1X2 = isAllActive;
            ActiveSettings.Ohm2X2 = isAllActive;
            ActiveSettings.Ohm3X2 = isAllActive;
            ActiveSettings.Ohm4X2 = isAllActive;
            ActiveSettings.Ohm5X2 = isAllActive;
        }

        private void IsAllCh2Click()
        {
            var isAllActive = !ActiveSettings.IsAllCh2Active;
            ActiveSettings.Ohm1X3 = isAllActive;
            ActiveSettings.Ohm2X3 = isAllActive;
            ActiveSettings.Ohm3X3 = isAllActive;
            ActiveSettings.Ohm4X3 = isAllActive;
            ActiveSettings.Ohm5X3 = isAllActive;
        }

        private void IsAllCh3Click()
        {
            var isAllActive = !ActiveSettings.IsAllCh3Active;
            ActiveSettings.V1X4 = isAllActive;
            ActiveSettings.V2X4 = isAllActive;
            ActiveSettings.V3X4 = isAllActive;
            ActiveSettings.V4X4 = isAllActive;
            ActiveSettings.V5X4 = isAllActive;
        }

        private void IsAllCh5Click()
        {
            var isAllActive = !ActiveSettings.IsAllCh5Active;
            ActiveSettings.V1X5 = isAllActive;
            ActiveSettings.V2X5 = isAllActive;
            ActiveSettings.V3X5 = isAllActive;
            ActiveSettings.V4X5 = isAllActive;
            ActiveSettings.V5X5 = isAllActive;
        }

        private void IsAllCh6Click()
        {
            var isAllActive = !ActiveSettings.IsAllCh6Active;
            ActiveSettings.Hz1X6 = isAllActive;
            ActiveSettings.Hz2X6 = isAllActive;
            ActiveSettings.Hz3X6 = isAllActive;
            ActiveSettings.Hz4X6 = isAllActive;
            ActiveSettings.Hz5X6 = isAllActive;
        }
    }
}