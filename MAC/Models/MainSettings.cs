using MAC.Properties;
using System;
using System.Collections.Generic;
using System.Windows;

namespace MAC.Models
{
    public class MainSettingsModel : ViewModels.Base.BaseVm
    {

        public List<ScVersionList> ScVersionList { get; set; } = new List<ScVersionList>
        {
            new ScVersionList("До 12.1.9 (Включительно)", ScVersion.Old),
            new ScVersionList("С 12.1.11 (Включительно)", ScVersion.New),
        };

        public MainSettingsModel()
        {
            TimeOutOhm = Settings.Default.TimeOutOhm;
            TimeOutV = Settings.Default.TimeOutV;
            TimeOutHz = Settings.Default.TimeOutHz;
            FullLogPath = Settings.Default.FullLogPath;
            IsUseAverageValue = Settings.Default.IsUseAverageValue;
            CountAverageValue = Settings.Default.CountAverageValue;
            ScVersion = (ScVersion)Settings.Default.ScVersion;
            IsOnCalibration = Settings.Default.IsOnCalibration;

        }

        private bool _isOnCalibration;

        public bool IsOnCalibration
        {
            get => _isOnCalibration;
            set
            {
                _isOnCalibration = value;
                OnPropertyChanged(nameof(IsOnCalibration));
                Settings.Default.IsOnCalibration = value;
                Settings.Default.Save();
            }
        }


        #region TimeOut

        private int _timeOutOhm;

        /// <summary>
        /// Время ожидания перед снятием значения Ohm в секундах
        /// </summary>
        public int TimeOutOhm
        {
            get => _timeOutOhm;
            set
            {
                if (value >= 10)
                {
                    _timeOutOhm = value;
                    OnPropertyChanged(nameof(TimeOutOhm));
                    Settings.Default.TimeOutOhm = value;
                }
                else
                {
                    MessageBox.Show("Минимальное значение времени тестирования 10 сек");
                    _timeOutOhm = 10;
                    OnPropertyChanged(nameof(TimeOutOhm));
                    Settings.Default.TimeOutOhm = 10;
                }

                Settings.Default.Save();
            }
        }

        private int _timeOutV;

        /// <summary>
        /// Время ожидания перед снятием значения V в секундах
        /// </summary>
        public int TimeOutV
        {
            get => _timeOutV;
            set
            {
                if (value >= 10)
                {
                    _timeOutV = value;
                    OnPropertyChanged(nameof(TimeOutV));
                    Settings.Default.TimeOutV = value;
                }
                else
                {
                    MessageBox.Show("Минимальное значение времени тестирования 10 сек");
                    _timeOutV = 10;
                    OnPropertyChanged(nameof(TimeOutV));
                    Settings.Default.TimeOutV = 10;
                }

                Settings.Default.Save();
            }
        }

        private int _timeOutHz;

        /// <summary>
        /// Время ожидания перед снятием значения Hz в секундах
        /// </summary>
        public int TimeOutHz
        {
            get => _timeOutHz;
            set
            {
                if (value >= 10)
                {
                    _timeOutHz = value;
                    OnPropertyChanged(nameof(TimeOutHz));
                    Settings.Default.TimeOutHz = value;
                }
                else
                {
                    MessageBox.Show("Минимальное значение времени тестирования 10 сек");
                    _timeOutHz = 10;
                    OnPropertyChanged(nameof(TimeOutHz));
                    Settings.Default.TimeOutHz = 10;
                }

                Settings.Default.Save();
            }
        }

        #endregion

        #region Path

        private string _fullLogPath;

        /// <summary>
        /// Путь сохранения лога общения с МАС
        /// </summary>
        public string FullLogPath
        {
            get => _fullLogPath;
            set
            {
                _fullLogPath = value;
                OnPropertyChanged(nameof(FullLogPath));
                Settings.Default.FullLogPath = value;
                Settings.Default.Save();
            }
        }

        #endregion

        #region If use Average Value for measurement

        private bool _isUseAverageValue;

        /// <summary>
        /// При снятии значений измерения, записывать несколько значений и считать среднее.
        /// </summary>
        public bool IsUseAverageValue
        {
            get => _isUseAverageValue;
            set
            {
                _isUseAverageValue = value;
                OnPropertyChanged(nameof(IsUseAverageValue));
                Settings.Default.IsUseAverageValue = value;
                Settings.Default.Save();
            }
        }

        private int _countAverageValue;

        /// <summary>
        /// Кол-во значений для вычисления среднего
        /// </summary>
        public int CountAverageValue
        {
            get => _countAverageValue;
            set
            {
                if (value >= 2 && value <= 10)
                {
                    _countAverageValue = value;
                    OnPropertyChanged(nameof(CountAverageValue));
                    Settings.Default.CountAverageValue = value;
                }
                else
                {
                    MessageBox.Show("Кол-во измерений для среднего значения, может быть от 2 до 10");
                    _countAverageValue = 2;
                    OnPropertyChanged(nameof(CountAverageValue));
                    Settings.Default.CountAverageValue = 2;
                }

                Settings.Default.Save();
            }
        }

        #endregion

        #region ScVersion

        private ScVersion _scVersion;

        public ScVersion ScVersion
        {
            get => _scVersion;
            set
            {
                _scVersion = value;
                OnPropertyChanged(nameof(ScVersion));
                Settings.Default.ScVersion = Convert.ToInt32(ScVersion);
                Settings.Default.Save();
            }
        }

        #endregion
    }

    public class ScVersionList
    {
        public ScVersionList(string name, ScVersion scVersion)
        {
            ScVersion = scVersion;
            Name = name;
        }

        public ScVersion ScVersion { get; set; }
        public string Name { get; set; }
    }
}