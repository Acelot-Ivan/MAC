using MAC.ViewModels.Base;
using MAC.Properties;

namespace MAC.Models
{
    public class ActiveSettingsModel : BaseVm
    {
        public ActiveSettingsModel()
        {
            Ch0Ohm80 = Settings.Default.Ch0Ohm80IsActive;
            Ch0Ohm90 = Settings.Default.Ch0Ohm90IsActive;
            Ch0Ohm100 = Settings.Default.Ch0Ohm100IsActive;
            Ch0Ohm115 = Settings.Default.Ch0Ohm115IsActive;
            Ch0Ohm130 = Settings.Default.Ch0Ohm130IsActive;
            Ch0Ohm140 = Settings.Default.Ch0Ohm140IsActive;

            Ch1Ohm80 = Settings.Default.Ch1Ohm80IsActive;
            Ch1Ohm90 = Settings.Default.Ch1Ohm90IsActive;
            Ch1Ohm100 = Settings.Default.Ch1Ohm100IsActive;
            Ch1Ohm115 = Settings.Default.Ch1Ohm115IsActive;
            Ch1Ohm130 = Settings.Default.Ch1Ohm130IsActive;
            Ch1Ohm140 = Settings.Default.Ch1Ohm140IsActive;

            Ch2Ohm80 = Settings.Default.Ch2Ohm80IsActive;
            Ch2Ohm90 = Settings.Default.Ch2Ohm90IsActive;
            Ch2Ohm100 = Settings.Default.Ch2Ohm100IsActive;
            Ch2Ohm115 = Settings.Default.Ch2Ohm115IsActive;
            Ch2Ohm130 = Settings.Default.Ch2Ohm130IsActive;
            Ch2Ohm140 = Settings.Default.Ch2Ohm140IsActive;

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

            Ch6Hz5 = Settings.Default.Ch6Hz5IsActive;
            Ch6Hz25 = Settings.Default.Ch6Hz25IsActive;
            Ch6Hz50 = Settings.Default.Ch6Hz50IsActive;
            Ch6Hz75 = Settings.Default.Ch6Hz75IsActive;
            Ch6Hz100 = Settings.Default.Ch6Hz100IsActive;
        }


        #region CH0

        private bool _ch0Ohm80;
        private bool _ch0Ohm90;
        private bool _ch0Ohm100;
        private bool _ch0Ohm115;
        private bool _ch0Ohm130;
        private bool _ch0Ohm140;

        public bool Ch0Ohm80
        {
            get => _ch0Ohm80;
            set
            {
                _ch0Ohm80 = value;
                OnPropertyChanged(nameof(Ch0Ohm80));
                Settings.Default.Ch0Ohm80IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch0Ohm90
        {
            get => _ch0Ohm90;
            set
            {
                _ch0Ohm90 = value;
                OnPropertyChanged(nameof(Ch0Ohm90));
                Settings.Default.Ch0Ohm90IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch0Ohm100
        {
            get => _ch0Ohm100;
            set
            {
                _ch0Ohm100 = value;
                OnPropertyChanged(nameof(Ch0Ohm100));
                Settings.Default.Ch0Ohm100IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch0Ohm115
        {
            get => _ch0Ohm115;
            set
            {
                _ch0Ohm115 = value;
                OnPropertyChanged(nameof(Ch0Ohm115));
                Settings.Default.Ch0Ohm115IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch0Ohm130
        {
            get => _ch0Ohm130;
            set
            {
                _ch0Ohm130 = value;
                OnPropertyChanged(nameof(Ch0Ohm130));
                Settings.Default.Ch0Ohm130IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch0Ohm140
        {
            get => _ch0Ohm140;
            set
            {
                _ch0Ohm140 = value;
                OnPropertyChanged(nameof(Ch0Ohm140));
                Settings.Default.Ch0Ohm140IsActive = value;
                Settings.Default.Save();
            }
        }

        #endregion

        #region CH1

        private bool _ch1Ohm80;
        private bool _ch1Ohm90;
        private bool _ch1Ohm100;
        private bool _ch1Ohm115;
        private bool _ch1Ohm130;
        private bool _ch1Ohm140;

        public bool Ch1Ohm80
        {
            get => _ch1Ohm80;
            set
            {
                _ch1Ohm80 = value;
                OnPropertyChanged(nameof(Ch1Ohm80));
                Settings.Default.Ch1Ohm80IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch1Ohm90
        {
            get => _ch1Ohm90;
            set
            {
                _ch1Ohm90 = value;
                OnPropertyChanged(nameof(Ch1Ohm90));
                Settings.Default.Ch1Ohm90IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch1Ohm100
        {
            get => _ch1Ohm100;
            set
            {
                _ch1Ohm100 = value;
                OnPropertyChanged(nameof(Ch1Ohm100));
                Settings.Default.Ch1Ohm100IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch1Ohm115
        {
            get => _ch1Ohm115;
            set
            {
                _ch1Ohm115 = value;
                OnPropertyChanged(nameof(Ch1Ohm115));
                Settings.Default.Ch1Ohm115IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch1Ohm130
        {
            get => _ch1Ohm130;
            set
            {
                _ch1Ohm130 = value;
                OnPropertyChanged(nameof(Ch1Ohm130));
                Settings.Default.Ch1Ohm130IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch1Ohm140
        {
            get => _ch1Ohm140;
            set
            {
                _ch1Ohm140 = value;
                OnPropertyChanged(nameof(Ch1Ohm140));
                Settings.Default.Ch1Ohm140IsActive = value;
                Settings.Default.Save();
            }
        }

        #endregion

        #region CH2

        private bool _ch2Ohm80;
        private bool _ch2Ohm90;
        private bool _ch2Ohm100;
        private bool _ch2Ohm115;
        private bool _ch2Ohm130;
        private bool _ch2Ohm140;

        public bool Ch2Ohm80
        {
            get => _ch2Ohm80;
            set
            {
                _ch2Ohm80 = value;
                OnPropertyChanged(nameof(Ch2Ohm80));
                Settings.Default.Ch2Ohm80IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch2Ohm90
        {
            get => _ch2Ohm90;
            set
            {
                _ch2Ohm90 = value;
                OnPropertyChanged(nameof(Ch2Ohm90));
                Settings.Default.Ch2Ohm90IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch2Ohm100
        {
            get => _ch2Ohm100;
            set
            {
                _ch2Ohm100 = value;
                OnPropertyChanged(nameof(Ch2Ohm100));
                Settings.Default.Ch2Ohm100IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch2Ohm115
        {
            get => _ch2Ohm115;
            set
            {
                _ch2Ohm115 = value;
                OnPropertyChanged(nameof(Ch2Ohm115));
                Settings.Default.Ch2Ohm115IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch2Ohm130
        {
            get => _ch2Ohm130;
            set
            {
                _ch2Ohm130 = value;
                OnPropertyChanged(nameof(Ch2Ohm130));
                Settings.Default.Ch2Ohm130IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch2Ohm140
        {
            get => _ch2Ohm140;
            set
            {
                _ch2Ohm140 = value;
                OnPropertyChanged(nameof(Ch2Ohm140));
                Settings.Default.Ch2Ohm140IsActive = value;
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

        private bool _ch6Hz5;
        private bool _ch6Hz25;
        private bool _ch6Hz50;
        private bool _ch6Hz75;
        private bool _ch6Hz100;

        public bool Ch6Hz5
        {
            get => _ch6Hz5;
            set
            {
                _ch6Hz5 = value;
                OnPropertyChanged(nameof(Ch6Hz5));
                Settings.Default.Ch6Hz5IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch6Hz25
        {
            get => _ch6Hz25;
            set
            {
                _ch6Hz25 = value;
                OnPropertyChanged(nameof(Ch6Hz25));
                Settings.Default.Ch6Hz25IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch6Hz50
        {
            get => _ch6Hz50;
            set
            {
                _ch6Hz50 = value;
                OnPropertyChanged(nameof(Ch6Hz50));
                Settings.Default.Ch6Hz50IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch6Hz75
        {
            get => _ch6Hz75;
            set
            {
                _ch6Hz75 = value;
                OnPropertyChanged(nameof(Ch6Hz75));
                Settings.Default.Ch6Hz75IsActive = value;
                Settings.Default.Save();
            }
        }

        public bool Ch6Hz100
        {
            get => _ch6Hz100;
            set
            {
                _ch6Hz100 = value;
                OnPropertyChanged(nameof(Ch6Hz100));
                Settings.Default.Ch6Hz100IsActive = value;
                Settings.Default.Save();
            }
        }

        #endregion


        /// <summary>
        /// Флаг показывающий, выбран весь канал Ch0 или нет.
        /// </summary>
        public bool IsAllCh0Active => Ch0Ohm80 && Ch0Ohm90 &&
                                      Ch0Ohm100 && Ch0Ohm115 &&
                                      Ch0Ohm130 && Ch0Ohm140;

        /// <summary>
        /// Флаг показывающий, выбран весь канал Ch1 или нет.
        /// </summary>
        public bool IsAllCh1Active => Ch1Ohm80 && Ch1Ohm90 &&
                                      Ch1Ohm100 && Ch1Ohm115 &&
                                      Ch1Ohm130 && Ch1Ohm140;
        /// <summary>
        /// Флаг показывающий, выбран весь канал Ch2 или нет.
        /// </summary>
        public bool IsAllCh2Active => Ch2Ohm80 && Ch2Ohm90 &&
                                      Ch2Ohm100 && Ch2Ohm115 &&
                                      Ch2Ohm130 && Ch2Ohm140;

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
        public bool IsAllCh6Active => Ch6Hz5 && Ch6Hz25 &&
                                      Ch6Hz50 && Ch6Hz75 &&
                                      Ch6Hz100;
    }
}