using MAC.ViewModels.Base;
using MAC.Properties;

namespace MAC.Models
{
    public class ActiveSettingsModel : BaseVm
    {
        public ActiveSettingsModel()
        {
            Ch0Ohm1 = Settings.Default.Ch0Ohm80IsActive;
            Ch0Ohm2 = Settings.Default.Ch0Ohm90IsActive;
            Ch0Ohm3 = Settings.Default.Ch0Ohm100IsActive;
            Ch0Ohm4 = Settings.Default.Ch0Ohm115IsActive;
            Ch0Ohm5 = Settings.Default.Ch0Ohm130IsActive;


            Ch1Ohm1 = Settings.Default.Ch1Ohm80IsActive;
            Ch1Ohm2 = Settings.Default.Ch1Ohm90IsActive;
            Ch1Ohm3 = Settings.Default.Ch1Ohm100IsActive;
            Ch1Ohm4 = Settings.Default.Ch1Ohm115IsActive;
            Ch1Ohm5 = Settings.Default.Ch1Ohm130IsActive;


            Ch2Ohm1 = Settings.Default.Ch2Ohm80IsActive;
            Ch2Ohm2 = Settings.Default.Ch2Ohm90IsActive;
            Ch2Ohm3 = Settings.Default.Ch2Ohm100IsActive;
            Ch2Ohm4 = Settings.Default.Ch2Ohm115IsActive;
            Ch2Ohm5 = Settings.Default.Ch2Ohm130IsActive;

            Ch3V1 = Settings.Default.Ch3V1IsActive;
            Ch3V2 = Settings.Default.Ch3V2IsActive;
            Ch3V3 = Settings.Default.Ch3V3IsActive;
            Ch3V4 = Settings.Default.Ch3V4IsActive;
            Ch3V5 = Settings.Default.Ch3V5IsActive;

            Ch5V1 = Settings.Default.Ch5V1IsActive;
            Ch5V2 = Settings.Default.Ch5V2IsActive;
            Ch5V3 = Settings.Default.Ch5V3IsActive;
            Ch5V4 = Settings.Default.Ch5V4IsActive;
            Ch5V5 = Settings.Default.Ch5V5IsActive;

            Ch6Hz1 = Settings.Default.Ch6Hz5IsActive;
            Ch6Hz2 = Settings.Default.Ch6Hz25IsActive;
            Ch6Hz3 = Settings.Default.Ch6Hz50IsActive;
            Ch6Hz4 = Settings.Default.Ch6Hz75IsActive;
            Ch6Hz5 = Settings.Default.Ch6Hz100IsActive;
        }


        #region CH0

        private bool _ch0Ohm1;
        private bool _ch0Ohm2;
        private bool _ch0Ohm3;
        private bool _ch0Ohm4;
        private bool _ch0Ohm5;


        public bool Ch0Ohm1
        {
            get => _ch0Ohm1;
            set
            {
                _ch0Ohm1 = value;
                OnPropertyChanged(nameof(Ch0Ohm1));
                Settings.Default.Ch0Ohm80IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch0Ohm2
        {
            get => _ch0Ohm2;
            set
            {
                _ch0Ohm2 = value;
                OnPropertyChanged(nameof(Ch0Ohm2));
                Settings.Default.Ch0Ohm90IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch0Ohm3
        {
            get => _ch0Ohm3;
            set
            {
                _ch0Ohm3 = value;
                OnPropertyChanged(nameof(Ch0Ohm3));
                Settings.Default.Ch0Ohm100IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch0Ohm4
        {
            get => _ch0Ohm4;
            set
            {
                _ch0Ohm4 = value;
                OnPropertyChanged(nameof(Ch0Ohm4));
                Settings.Default.Ch0Ohm115IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch0Ohm5
        {
            get => _ch0Ohm5;
            set
            {
                _ch0Ohm5 = value;
                OnPropertyChanged(nameof(Ch0Ohm5));
                Settings.Default.Ch0Ohm130IsActive = value;
                Settings.Default.Save();
            }
        }

        #endregion

        #region CH1

        private bool _ch1Ohm1;
        private bool _ch1Ohm2;
        private bool _ch1Ohm3;
        private bool _ch1Ohm4;
        private bool _ch1Ohm5;

        public bool Ch1Ohm1
        {
            get => _ch1Ohm1;
            set
            {
                _ch1Ohm1 = value;
                OnPropertyChanged(nameof(Ch1Ohm1));
                Settings.Default.Ch1Ohm80IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch1Ohm2
        {
            get => _ch1Ohm2;
            set
            {
                _ch1Ohm2 = value;
                OnPropertyChanged(nameof(Ch1Ohm2));
                Settings.Default.Ch1Ohm90IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch1Ohm3
        {
            get => _ch1Ohm3;
            set
            {
                _ch1Ohm3 = value;
                OnPropertyChanged(nameof(Ch1Ohm3));
                Settings.Default.Ch1Ohm100IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch1Ohm4
        {
            get => _ch1Ohm4;
            set
            {
                _ch1Ohm4 = value;
                OnPropertyChanged(nameof(Ch1Ohm4));
                Settings.Default.Ch1Ohm115IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch1Ohm5
        {
            get => _ch1Ohm5;
            set
            {
                _ch1Ohm5 = value;
                OnPropertyChanged(nameof(Ch1Ohm5));
                Settings.Default.Ch1Ohm130IsActive = value;
                Settings.Default.Save();
            }
        }
        #endregion

        #region CH2

        private bool _ch2Ohm1;
        private bool _ch2Ohm2;
        private bool _ch2Ohm3;
        private bool _ch2Ohm4;
        private bool _ch2Ohm5;

        public bool Ch2Ohm1
        {
            get => _ch2Ohm1;
            set
            {
                _ch2Ohm1 = value;
                OnPropertyChanged(nameof(Ch2Ohm1));
                Settings.Default.Ch2Ohm80IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch2Ohm2
        {
            get => _ch2Ohm2;
            set
            {
                _ch2Ohm2 = value;
                OnPropertyChanged(nameof(Ch2Ohm2));
                Settings.Default.Ch2Ohm90IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch2Ohm3
        {
            get => _ch2Ohm3;
            set
            {
                _ch2Ohm3 = value;
                OnPropertyChanged(nameof(Ch2Ohm3));
                Settings.Default.Ch2Ohm100IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch2Ohm4
        {
            get => _ch2Ohm4;
            set
            {
                _ch2Ohm4 = value;
                OnPropertyChanged(nameof(Ch2Ohm4));
                Settings.Default.Ch2Ohm115IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch2Ohm5
        {
            get => _ch2Ohm5;
            set
            {
                _ch2Ohm5 = value;
                OnPropertyChanged(nameof(Ch2Ohm5));
                Settings.Default.Ch2Ohm130IsActive = value;
                Settings.Default.Save();
            }
        }

        #endregion

        #region CH3

        private bool _ch3V1;
        private bool _ch3V2;
        private bool _ch3V3;
        private bool _ch3V4;
        private bool _ch3V5;

        public bool Ch3V1
        {
            get => _ch3V1;
            set
            {
                _ch3V1 = value;
                OnPropertyChanged(nameof(Ch3V1));
                Settings.Default.Ch3V1IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch3V2
        {
            get => _ch3V2;
            set
            {
                _ch3V2 = value;
                OnPropertyChanged(nameof(Ch3V2));
                Settings.Default.Ch3V2IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch3V3
        {
            get => _ch3V3;
            set
            {
                _ch3V3 = value;
                OnPropertyChanged(nameof(Ch3V3));
                Settings.Default.Ch3V3IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch3V4
        {
            get => _ch3V4;
            set
            {
                _ch3V4 = value;
                OnPropertyChanged(nameof(Ch3V4));
                Settings.Default.Ch3V4IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch3V5
        {
            get => _ch3V5;
            set
            {
                _ch3V5 = value;
                OnPropertyChanged(nameof(Ch3V5));
                Settings.Default.Ch3V5IsActive = value;
                Settings.Default.Save();
            }
        }

        #endregion

        #region CH5

        private bool _ch5V1;
        private bool _ch5V2;
        private bool _ch5V3;
        private bool _ch5V4;
        private bool _ch5V5;

        public bool Ch5V1
        {
            get => _ch5V1;
            set
            {
                _ch5V1 = value;
                OnPropertyChanged(nameof(Ch5V1));
                Settings.Default.Ch5V1IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch5V2
        {
            get => _ch5V2;
            set
            {
                _ch5V2 = value;
                OnPropertyChanged(nameof(Ch5V2));
                Settings.Default.Ch5V2IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch5V3
        {
            get => _ch5V3;
            set
            {
                _ch5V3 = value;
                OnPropertyChanged(nameof(Ch5V3));
                Settings.Default.Ch5V3IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch5V4
        {
            get => _ch5V4;
            set
            {
                _ch5V4 = value;
                OnPropertyChanged(nameof(Ch5V4));
                Settings.Default.Ch5V4IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch5V5
        {
            get => _ch5V5;
            set
            {
                _ch5V5 = value;
                OnPropertyChanged(nameof(Ch5V5));
                Settings.Default.Ch5V5IsActive = value;
                Settings.Default.Save();
            }
        }

        #endregion

        #region CH6

        private bool _ch6Hz1;
        private bool _ch6Hz2;
        private bool _ch6Hz3;
        private bool _ch6Hz4;
        private bool _ch6Hz5;

        public bool Ch6Hz1
        {
            get => _ch6Hz1;
            set
            {
                _ch6Hz1 = value;
                OnPropertyChanged(nameof(Ch6Hz1));
                Settings.Default.Ch6Hz5IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch6Hz2
        {
            get => _ch6Hz2;
            set
            {
                _ch6Hz2 = value;
                OnPropertyChanged(nameof(Ch6Hz2));
                Settings.Default.Ch6Hz25IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch6Hz3
        {
            get => _ch6Hz3;
            set
            {
                _ch6Hz3 = value;
                OnPropertyChanged(nameof(Ch6Hz3));
                Settings.Default.Ch6Hz50IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch6Hz4
        {
            get => _ch6Hz4;
            set
            {
                _ch6Hz4 = value;
                OnPropertyChanged(nameof(Ch6Hz4));
                Settings.Default.Ch6Hz75IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch6Hz5
        {
            get => _ch6Hz5;
            set
            {
                _ch6Hz5 = value;
                OnPropertyChanged(nameof(Ch6Hz5));
                Settings.Default.Ch6Hz100IsActive = value;
                Settings.Default.Save();
            }
        }

        #endregion


        /// <summary>
        /// Флаг показывающий, выбран весь канал Ch0 или нет.
        /// </summary>
        public bool IsAllCh0Active => Ch0Ohm1 && Ch0Ohm2 &&
                                      Ch0Ohm3 && Ch0Ohm4 &&
                                      Ch0Ohm5;

        /// <summary>
        /// Флаг показывающий, выбран весь канал Ch1 или нет.
        /// </summary>
        public bool IsAllCh1Active => Ch1Ohm1 && Ch1Ohm2 &&
                                      Ch1Ohm3 && Ch1Ohm4 &&
                                      Ch1Ohm5;
        /// <summary>
        /// Флаг показывающий, выбран весь канал Ch2 или нет.
        /// </summary>
        public bool IsAllCh2Active => Ch2Ohm1 && Ch2Ohm2 &&
                                      Ch2Ohm3 && Ch2Ohm4 &&
                                      Ch2Ohm5;

        /// <summary>
        /// Флаг показывающий, выбран весь канал Ch3 или нет.
        /// </summary>
        public bool IsAllCh3Active => Ch3V1 && Ch3V2 &&
                                      Ch3V3 && Ch3V4 &&
                                      Ch3V5;
        /// <summary>
        /// Флаг показывающий, выбран весь канал Ch5 или нет.
        /// </summary>
        public bool IsAllCh5Active => Ch5V1 && Ch5V2 &&
                                      Ch5V3 && Ch5V4 &&
                                      Ch5V5;
        /// <summary>
        /// Флаг показывающий, выбран весь канал Ch6 или нет.
        /// </summary>
        public bool IsAllCh6Active => Ch6Hz1 && Ch6Hz2 &&
                                      Ch6Hz3 && Ch6Hz4 &&
                                      Ch6Hz5;
    }
}