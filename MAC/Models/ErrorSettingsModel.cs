using MAC.ViewModels.Base;
using MAC.Properties;

namespace MAC.Models
{
    /// <summary>
    /// Класс для хранения коллекций с настройками
    /// </summary>
    public class ErrorSettingsModel : BaseVm
    {
        public ErrorSettingsModel()
        {
            Sc1Ch0Error = Settings.Default.Sc1Ch0Error;
            Sc1Ch1Error = Settings.Default.Sc1Ch1Error;
            Sc1Ch2Error = Settings.Default.Sc1Ch2Error;
            Sc1Ch3Error = Settings.Default.Sc1Ch3Error;
            Sc1Ch5Error = Settings.Default.Sc1Ch5Error;
            Sc1Ch6Error = Settings.Default.Sc1Ch6Error;

            Sc2Ch0Error = Settings.Default.Sc2Ch0Error;
            Sc2Ch1Error = Settings.Default.Sc2Ch1Error;
            Sc2Ch2Error = Settings.Default.Sc2Ch2Error;
            Sc2Ch3Error = Settings.Default.Sc2Ch3Error;
            Sc2Ch5Error = Settings.Default.Sc2Ch5Error;
            Sc2Ch6Error = Settings.Default.Sc2Ch6Error;

            Sc3Ch0Error = Settings.Default.Sc3Ch0Error;
            Sc3Ch1Error = Settings.Default.Sc3Ch1Error;
            Sc3Ch2Error = Settings.Default.Sc3Ch2Error;
            Sc3Ch3Error = Settings.Default.Sc3Ch3Error;
            Sc3Ch5Error = Settings.Default.Sc3Ch5Error;
            Sc3Ch6Error = Settings.Default.Sc3Ch6Error;

            Sc4Ch0Error = Settings.Default.Sc4Ch0Error;
            Sc4Ch1Error = Settings.Default.Sc4Ch1Error;
            Sc4Ch2Error = Settings.Default.Sc4Ch2Error;
            Sc4Ch3Error = Settings.Default.Sc4Ch3Error;
            Sc4Ch5Error = Settings.Default.Sc4Ch5Error;
            Sc4Ch6Error = Settings.Default.Sc4Ch6Error;

            Sc5Ch0Error = Settings.Default.Sc5Ch0Error;
            Sc5Ch1Error = Settings.Default.Sc5Ch1Error;
            Sc5Ch2Error = Settings.Default.Sc5Ch2Error;
            Sc5Ch3Error = Settings.Default.Sc5Ch3Error;
            Sc5Ch5Error = Settings.Default.Sc5Ch5Error;
            Sc5Ch6Error = Settings.Default.Sc5Ch6Error;

            Sc6Ch0Error = Settings.Default.Sc6Ch0Error;
            Sc6Ch1Error = Settings.Default.Sc6Ch1Error;
            Sc6Ch2Error = Settings.Default.Sc6Ch2Error;
            Sc6Ch3Error = Settings.Default.Sc6Ch3Error;
            Sc6Ch5Error = Settings.Default.Sc6Ch5Error;
            Sc6Ch6Error = Settings.Default.Sc6Ch6Error;
        }


        #region CH0

        private decimal _sc1Ch0Error;
        private decimal _sc2Ch0Error;
        private decimal _sc3Ch0Error;
        private decimal _sc4Ch0Error;
        private decimal _sc5Ch0Error;
        private decimal _sc6Ch0Error;

        public decimal Sc1Ch0Error
        {
            get => _sc1Ch0Error;
            set
            {
                _sc1Ch0Error = value;
                OnPropertyChanged(nameof(Sc1Ch0Error));
                Settings.Default.Sc1Ch0Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc2Ch0Error
        {
            get => _sc2Ch0Error;
            set
            {
                _sc2Ch0Error = value;
                OnPropertyChanged(nameof(Sc2Ch0Error));
                Settings.Default.Sc2Ch0Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc3Ch0Error
        {
            get => _sc3Ch0Error;
            set
            {
                _sc3Ch0Error = value;
                OnPropertyChanged(nameof(Sc3Ch0Error));
                Settings.Default.Sc3Ch0Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc4Ch0Error
        {
            get => _sc4Ch0Error;
            set
            {
                _sc4Ch0Error = value;
                OnPropertyChanged(nameof(Sc4Ch0Error));
                Settings.Default.Sc4Ch0Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc5Ch0Error
        {
            get => _sc5Ch0Error;
            set
            {
                _sc5Ch0Error = value;
                OnPropertyChanged(nameof(Sc5Ch0Error));
                Settings.Default.Sc5Ch0Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc6Ch0Error
        {
            get => _sc6Ch0Error;
            set
            {
                _sc6Ch0Error = value;
                OnPropertyChanged(nameof(Sc6Ch0Error));
                Settings.Default.Sc6Ch0Error = value;
                Settings.Default.Save();
            }
        }

        #endregion

        #region CH1

        private decimal _sc1Ch1Error;
        private decimal _sc2Ch1Error;
        private decimal _sc3Ch1Error;
        private decimal _sc4Ch1Error;
        private decimal _sc5Ch1Error;
        private decimal _sc6Ch1Error;

        public decimal Sc1Ch1Error
        {
            get => _sc1Ch1Error;
            set
            {
                _sc1Ch1Error = value;
                OnPropertyChanged(nameof(Sc1Ch1Error));
                Settings.Default.Sc1Ch1Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc2Ch1Error
        {
            get => _sc2Ch1Error;
            set
            {
                _sc2Ch1Error = value;
                OnPropertyChanged(nameof(Sc2Ch1Error));
                Settings.Default.Sc2Ch1Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc3Ch1Error
        {
            get => _sc3Ch1Error;
            set
            {
                _sc3Ch1Error = value;
                OnPropertyChanged(nameof(Sc3Ch1Error));
                Settings.Default.Sc3Ch1Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc4Ch1Error
        {
            get => _sc4Ch1Error;
            set
            {
                _sc4Ch1Error = value;
                OnPropertyChanged(nameof(Sc4Ch1Error));
                Settings.Default.Sc4Ch1Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc5Ch1Error
        {
            get => _sc5Ch1Error;
            set
            {
                _sc5Ch1Error = value;
                OnPropertyChanged(nameof(Sc5Ch1Error));
                Settings.Default.Sc5Ch1Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc6Ch1Error
        {
            get => _sc6Ch1Error;
            set
            {
                _sc6Ch1Error = value;
                OnPropertyChanged(nameof(Sc6Ch1Error));
                Settings.Default.Sc6Ch1Error = value;
                Settings.Default.Save();
            }
        }

        #endregion

        #region CH2

        private decimal _sc1Ch2Error;
        private decimal _sc2Ch2Error;
        private decimal _sc3Ch2Error;
        private decimal _sc4Ch2Error;
        private decimal _sc5Ch2Error;
        private decimal _sc6Ch2Error;

        public decimal Sc1Ch2Error
        {
            get => _sc1Ch2Error;
            set
            {
                _sc1Ch2Error = value;
                OnPropertyChanged(nameof(Sc1Ch2Error));
                Settings.Default.Sc1Ch2Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc2Ch2Error
        {
            get => _sc2Ch2Error;
            set
            {
                _sc2Ch2Error = value;
                OnPropertyChanged(nameof(Sc2Ch2Error));
                Settings.Default.Sc2Ch2Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc3Ch2Error
        {
            get => _sc3Ch2Error;
            set
            {
                _sc3Ch2Error = value;
                OnPropertyChanged(nameof(Sc3Ch2Error));
                Settings.Default.Sc3Ch2Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc4Ch2Error
        {
            get => _sc4Ch2Error;
            set
            {
                _sc4Ch2Error = value;
                OnPropertyChanged(nameof(Sc4Ch2Error));
                Settings.Default.Sc4Ch2Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc5Ch2Error
        {
            get => _sc5Ch2Error;
            set
            {
                _sc5Ch2Error = value;
                OnPropertyChanged(nameof(Sc5Ch2Error));
                Settings.Default.Sc5Ch2Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc6Ch2Error
        {
            get => _sc6Ch2Error;
            set
            {
                _sc6Ch2Error = value;
                OnPropertyChanged(nameof(Sc6Ch2Error));
                Settings.Default.Sc6Ch2Error = value;
                Settings.Default.Save();
            }
        }

        #endregion

        #region CH3

        private decimal _sc1Ch3Error;
        private decimal _sc2Ch3Error;
        private decimal _sc3Ch3Error;
        private decimal _sc4Ch3Error;
        private decimal _sc5Ch3Error;
        private decimal _sc6Ch3Error;

        public decimal Sc1Ch3Error
        {
            get => _sc1Ch3Error;
            set
            {
                _sc1Ch3Error = value;
                OnPropertyChanged(nameof(Sc1Ch3Error));
                Settings.Default.Sc1Ch3Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc2Ch3Error
        {
            get => _sc2Ch3Error;
            set
            {
                _sc2Ch3Error = value;
                OnPropertyChanged(nameof(Sc2Ch3Error));
                Settings.Default.Sc2Ch3Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc3Ch3Error
        {
            get => _sc3Ch3Error;
            set
            {
                _sc3Ch3Error = value;
                OnPropertyChanged(nameof(Sc3Ch3Error));
                Settings.Default.Sc3Ch3Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc4Ch3Error
        {
            get => _sc4Ch3Error;
            set
            {
                _sc4Ch3Error = value;
                OnPropertyChanged(nameof(Sc4Ch3Error));
                Settings.Default.Sc4Ch3Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc5Ch3Error
        {
            get => _sc5Ch3Error;
            set
            {
                _sc5Ch3Error = value;
                OnPropertyChanged(nameof(Sc5Ch3Error));
                Settings.Default.Sc5Ch3Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc6Ch3Error
        {
            get => _sc6Ch3Error;
            set
            {
                _sc6Ch3Error = value;
                OnPropertyChanged(nameof(Sc6Ch3Error));
                Settings.Default.Sc6Ch3Error = value;
                Settings.Default.Save();
            }
        }

        #endregion

        #region CH5

        private decimal _sc1Ch5Error;
        private decimal _sc2Ch5Error;
        private decimal _sc3Ch5Error;
        private decimal _sc4Ch5Error;
        private decimal _sc5Ch5Error;
        private decimal _sc6Ch5Error;

        public decimal Sc1Ch5Error
        {
            get => _sc1Ch5Error;
            set
            {
                _sc1Ch5Error = value;
                OnPropertyChanged(nameof(Sc1Ch5Error));
                Settings.Default.Sc1Ch5Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc2Ch5Error
        {
            get => _sc2Ch5Error;
            set
            {
                _sc2Ch5Error = value;
                OnPropertyChanged(nameof(Sc2Ch5Error));
                Settings.Default.Sc2Ch5Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc3Ch5Error
        {
            get => _sc3Ch5Error;
            set
            {
                _sc3Ch5Error = value;
                OnPropertyChanged(nameof(Sc3Ch5Error));
                Settings.Default.Sc3Ch5Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc4Ch5Error
        {
            get => _sc4Ch5Error;
            set
            {
                _sc4Ch5Error = value;
                OnPropertyChanged(nameof(Sc4Ch5Error));
                Settings.Default.Sc4Ch5Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc5Ch5Error
        {
            get => _sc5Ch5Error;
            set
            {
                _sc5Ch5Error = value;
                OnPropertyChanged(nameof(Sc5Ch5Error));
                Settings.Default.Sc5Ch5Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc6Ch5Error
        {
            get => _sc6Ch5Error;
            set
            {
                _sc6Ch5Error = value;
                OnPropertyChanged(nameof(Sc6Ch5Error));
                Settings.Default.Sc6Ch5Error = value;
                Settings.Default.Save();
            }
        }

        #endregion

        #region CH6

        private decimal _sc1Ch6Error;
        private decimal _sc2Ch6Error;
        private decimal _sc3Ch6Error;
        private decimal _sc4Ch6Error;
        private decimal _sc5Ch6Error;
        private decimal _sc6Ch6Error;

        public decimal Sc1Ch6Error
        {
            get => _sc1Ch6Error;
            set
            {
                _sc1Ch6Error = value;
                OnPropertyChanged(nameof(Sc1Ch6Error));
                Settings.Default.Sc1Ch6Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc2Ch6Error
        {
            get => _sc2Ch6Error;
            set
            {
                _sc2Ch6Error = value;
                OnPropertyChanged(nameof(Sc2Ch6Error));
                Settings.Default.Sc2Ch6Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc3Ch6Error
        {
            get => _sc3Ch6Error;
            set
            {
                _sc3Ch6Error = value;
                OnPropertyChanged(nameof(Sc3Ch6Error));
                Settings.Default.Sc3Ch6Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc4Ch6Error
        {
            get => _sc4Ch6Error;
            set
            {
                _sc4Ch6Error = value;
                OnPropertyChanged(nameof(Sc4Ch6Error));
                Settings.Default.Sc4Ch6Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc5Ch6Error
        {
            get => _sc5Ch6Error;
            set
            {
                _sc5Ch6Error = value;
                OnPropertyChanged(nameof(Sc5Ch6Error));
                Settings.Default.Sc5Ch6Error = value;
                Settings.Default.Save();
            }
        }

        public decimal Sc6Ch6Error
        {
            get => _sc6Ch6Error;
            set
            {
                _sc6Ch6Error = value;
                OnPropertyChanged(nameof(Sc6Ch6Error));
                Settings.Default.Sc6Ch6Error = value;
                Settings.Default.Save();
            }
        }

        #endregion
    }
}