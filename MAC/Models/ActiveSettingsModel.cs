using MAC.Properties;
using MAC.ViewModels.Base;

namespace MAC.Models
{
    public class ActiveSettingsModel : BaseVm
    {
        public ActiveSettingsModel()
        {
            Ohm1X1 = Settings.Default.Ch0Ohm1IsActive;
            Ohm2X1 = Settings.Default.Ch0Ohm2IsActive;
            Ohm3X1 = Settings.Default.Ch0Ohm3IsActive;
            Ohm4X1 = Settings.Default.Ch0Ohm4IsActive;
            Ohm5X1 = Settings.Default.Ch0Ohm5IsActive;


            Ohm1X2 = Settings.Default.Ch1Ohm1IsActive;
            Ohm2X2 = Settings.Default.Ch1Ohm2IsActive;
            Ohm3X2 = Settings.Default.Ch1Ohm3IsActive;
            Ohm4X2 = Settings.Default.Ch1Ohm4IsActive;
            Ohm5X2 = Settings.Default.Ch1Ohm5IsActive;


            Ohm1X3 = Settings.Default.Ch2Ohm1IsActive;
            Ohm2X3 = Settings.Default.Ch2Ohm2IsActive;
            Ohm3X3 = Settings.Default.Ch2Ohm3IsActive;
            Ohm4X3 = Settings.Default.Ch2Ohm4IsActive;
            Ohm5X3 = Settings.Default.Ch2Ohm5IsActive;

            V1X4 = Settings.Default.Ch3V1IsActive;
            V2X4 = Settings.Default.Ch3V2IsActive;
            V3X4 = Settings.Default.Ch3V3IsActive;
            V4X4 = Settings.Default.Ch3V4IsActive;
            V5X4 = Settings.Default.Ch3V5IsActive;

            V1X5 = Settings.Default.Ch5V1IsActive;
            V2X5 = Settings.Default.Ch5V2IsActive;
            V3X5 = Settings.Default.Ch5V3IsActive;
            V4X5 = Settings.Default.Ch5V4IsActive;
            V5X5 = Settings.Default.Ch5V5IsActive;

            Hz1X6 = Settings.Default.Ch6Hz1IsActive;
            Hz2X6 = Settings.Default.Ch6Hz2IsActive;
            Hz3X6 = Settings.Default.Ch6Hz3IsActive;
            Hz4X6 = Settings.Default.Ch6Hz4IsActive;
            Hz5X6 = Settings.Default.Ch6Hz1IsActive;
        }


        #region CH0

        private bool _ohm1X1;
        private bool _ohm2X1;
        private bool _ohm3X1;
        private bool _ohm4X1;
        private bool _ohm5X1;


        public bool Ohm1X1
        {
            get => _ohm1X1;
            set
            {
                _ohm1X1 = value;
                OnPropertyChanged(nameof(Ohm1X1));
                Settings.Default.Ch0Ohm1IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ohm2X1
        {
            get => _ohm2X1;
            set
            {
                _ohm2X1 = value;
                OnPropertyChanged(nameof(Ohm2X1));
                Settings.Default.Ch0Ohm2IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ohm3X1
        {
            get => _ohm3X1;
            set
            {
                _ohm3X1 = value;
                OnPropertyChanged(nameof(Ohm3X1));
                Settings.Default.Ch0Ohm3IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ohm4X1
        {
            get => _ohm4X1;
            set
            {
                _ohm4X1 = value;
                OnPropertyChanged(nameof(Ohm4X1));
                Settings.Default.Ch0Ohm4IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ohm5X1
        {
            get => _ohm5X1;
            set
            {
                _ohm5X1 = value;
                OnPropertyChanged(nameof(Ohm5X1));
                Settings.Default.Ch0Ohm5IsActive = value;
                Settings.Default.Save();
            }
        }

        #endregion

        #region CH1

        private bool _ohm1X2;
        private bool _ohm2X2;
        private bool _ohm3X2;
        private bool _ohm4X2;
        private bool _ohm5X2;

        public bool Ohm1X2
        {
            get => _ohm1X2;
            set
            {
                _ohm1X2 = value;
                OnPropertyChanged(nameof(Ohm1X2));
                Settings.Default.Ch1Ohm1IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ohm2X2
        {
            get => _ohm2X2;
            set
            {
                _ohm2X2 = value;
                OnPropertyChanged(nameof(Ohm2X2));
                Settings.Default.Ch1Ohm2IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ohm3X2
        {
            get => _ohm3X2;
            set
            {
                _ohm3X2 = value;
                OnPropertyChanged(nameof(Ohm3X2));
                Settings.Default.Ch1Ohm3IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ohm4X2
        {
            get => _ohm4X2;
            set
            {
                _ohm4X2 = value;
                OnPropertyChanged(nameof(Ohm4X2));
                Settings.Default.Ch1Ohm4IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ohm5X2
        {
            get => _ohm5X2;
            set
            {
                _ohm5X2 = value;
                OnPropertyChanged(nameof(Ohm5X2));
                Settings.Default.Ch1Ohm5IsActive = value;
                Settings.Default.Save();
            }
        }
        #endregion

        #region CH2

        private bool _ohm1X3;
        private bool _ohm2X3;
        private bool _ohm3X3;
        private bool _ohm4X3;
        private bool _ohm5X3;

        public bool Ohm1X3
        {
            get => _ohm1X3;
            set
            {
                _ohm1X3 = value;
                OnPropertyChanged(nameof(Ohm1X3));
                Settings.Default.Ch2Ohm1IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ohm2X3
        {
            get => _ohm2X3;
            set
            {
                _ohm2X3 = value;
                OnPropertyChanged(nameof(Ohm2X3));
                Settings.Default.Ch2Ohm2IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ohm3X3
        {
            get => _ohm3X3;
            set
            {
                _ohm3X3 = value;
                OnPropertyChanged(nameof(Ohm3X3));
                Settings.Default.Ch2Ohm3IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ohm4X3
        {
            get => _ohm4X3;
            set
            {
                _ohm4X3 = value;
                OnPropertyChanged(nameof(Ohm4X3));
                Settings.Default.Ch2Ohm4IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ohm5X3
        {
            get => _ohm5X3;
            set
            {
                _ohm5X3 = value;
                OnPropertyChanged(nameof(Ohm5X3));
                Settings.Default.Ch2Ohm5IsActive = value;
                Settings.Default.Save();
            }
        }

        #endregion

        #region CH3

        private bool _v1X4;
        private bool _v2X4;
        private bool _v3X4;
        private bool _v4X4;
        private bool _v5X4;

        public bool V1X4
        {
            get => _v1X4;
            set
            {
                _v1X4 = value;
                OnPropertyChanged(nameof(V1X4));
                Settings.Default.Ch3V1IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool V2X4
        {
            get => _v2X4;
            set
            {
                _v2X4 = value;
                OnPropertyChanged(nameof(V2X4));
                Settings.Default.Ch3V2IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool V3X4
        {
            get => _v3X4;
            set
            {
                _v3X4 = value;
                OnPropertyChanged(nameof(V3X4));
                Settings.Default.Ch3V3IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool V4X4
        {
            get => _v4X4;
            set
            {
                _v4X4 = value;
                OnPropertyChanged(nameof(V4X4));
                Settings.Default.Ch3V4IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool V5X4
        {
            get => _v5X4;
            set
            {
                _v5X4 = value;
                OnPropertyChanged(nameof(V5X4));
                Settings.Default.Ch3V5IsActive = value;
                Settings.Default.Save();
            }
        }

        #endregion

        #region CH5

        private bool _v1X5;
        private bool _v2X5;
        private bool _v3X5;
        private bool _v4X5;
        private bool _v5X5;

        public bool V1X5
        {
            get => _v1X5;
            set
            {
                _v1X5 = value;
                OnPropertyChanged(nameof(V1X5));
                Settings.Default.Ch5V1IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool V2X5
        {
            get => _v2X5;
            set
            {
                _v2X5 = value;
                OnPropertyChanged(nameof(V2X5));
                Settings.Default.Ch5V2IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool V3X5
        {
            get => _v3X5;
            set
            {
                _v3X5 = value;
                OnPropertyChanged(nameof(V3X5));
                Settings.Default.Ch5V3IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool V4X5
        {
            get => _v4X5;
            set
            {
                _v4X5 = value;
                OnPropertyChanged(nameof(V4X5));
                Settings.Default.Ch5V4IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool V5X5
        {
            get => _v5X5;
            set
            {
                _v5X5 = value;
                OnPropertyChanged(nameof(V5X5));
                Settings.Default.Ch5V5IsActive = value;
                Settings.Default.Save();
            }
        }

        #endregion

        #region CH6

        private bool _hz1X6;
        private bool _hz2X6;
        private bool _hz3X6;
        private bool _hz4X6;
        private bool _hz5X6;

        public bool Hz1X6
        {
            get => _hz1X6;
            set
            {
                _hz1X6 = value;
                OnPropertyChanged(nameof(Hz1X6));
                Settings.Default.Ch6Hz1IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Hz2X6
        {
            get => _hz2X6;
            set
            {
                _hz2X6 = value;
                OnPropertyChanged(nameof(Hz2X6));
                Settings.Default.Ch6Hz2IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Hz3X6
        {
            get => _hz3X6;
            set
            {
                _hz3X6 = value;
                OnPropertyChanged(nameof(Hz3X6));
                Settings.Default.Ch6Hz3IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Hz4X6
        {
            get => _hz4X6;
            set
            {
                _hz4X6 = value;
                OnPropertyChanged(nameof(Hz4X6));
                Settings.Default.Ch6Hz4IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Hz5X6
        {
            get => _hz5X6;
            set
            {
                _hz5X6 = value;
                OnPropertyChanged(nameof(Hz5X6));
                Settings.Default.Ch6Hz1IsActive = value;
                Settings.Default.Save();
            }
        }

        #endregion


        /// <summary>
        /// Флаг показывающий, выбран весь канал X1 или нет.
        /// </summary>
        public bool IsAllCh0Active => Ohm1X1 && Ohm2X1 &&
                                      Ohm3X1 && Ohm4X1 &&
                                      Ohm5X1;

        /// <summary>
        /// Флаг показывающий, выбран весь канал X2 или нет.
        /// </summary>
        public bool IsAllCh1Active => Ohm1X2 && Ohm2X2 &&
                                      Ohm3X2 && Ohm4X2 &&
                                      Ohm5X2;
        /// <summary>
        /// Флаг показывающий, выбран весь канал X3 или нет.
        /// </summary>
        public bool IsAllCh2Active => Ohm1X3 && Ohm2X3 &&
                                      Ohm3X3 && Ohm4X3 &&
                                      Ohm5X3;

        /// <summary>
        /// Флаг показывающий, выбран весь канал X4 или нет.
        /// </summary>
        public bool IsAllCh3Active => V1X4 && V2X4 &&
                                      V3X4 && V4X4 &&
                                      V5X4;
        /// <summary>
        /// Флаг показывающий, выбран весь канал X5 или нет.
        /// </summary>
        public bool IsAllCh5Active => V1X5 && V2X5 &&
                                      V3X5 && V4X5 &&
                                      V5X5;
        /// <summary>
        /// Флаг показывающий, выбран весь канал X6 или нет.
        /// </summary>
        public bool IsAllCh6Active => Hz1X6 && Hz2X6 &&
                                      Hz3X6 && Hz4X6 &&
                                      Hz5X6;
    }
}