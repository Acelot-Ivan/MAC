using MAC.Properties;
using System;
using System.Threading.Tasks;
using MAC.ViewModels.Base;
using MAC.ViewModels.Services;
using MAC.ViewModels.Services.SerialPort;

namespace MAC.Models
{
    public class ComConnectItem : BaseVm
    {
        public ComConnectItem(string technicalName, int number, string comPort = MainConst.DefaultComPort)
        {
            TechnicalName = technicalName.Contains(MainConst.NameTypeSignalController)
                ? $"{technicalName} - {number}"
                : technicalName;
            Number = number;

            Name = MainConst.DefaultNameSignalController;

            ComPort = comPort;
        }

        /// <summary>
        /// Свойство для отображения Мас , Fluke and Commutator  на интерфейсе. В меню подключений.
        /// </summary>
        public string TechnicalName { get; set; }

        public int Number { get; set; }

        private string _comPort;

        public string ComPort
        {
            get => _comPort;
            set
            {
                if (value == _comPort)
                    return;

                _comPort = string.IsNullOrEmpty(value)
                    ? MainConst.DefaultComPort
                    : value;

                OnPropertyChanged(nameof(ComPort));

                CheckComConnectAsync();

                UpdatePropertySettingsComPort();
            }
        }

        private void UpdatePropertySettingsComPort()
        {
            if (TechnicalName.Contains(MainConst.NameTypeSignalController))
            {
                switch (Number)
                {
                    case 1:
                        Settings.Default.Sc1ComPort = ComPort;
                        break;
                    case 2:
                        Settings.Default.Sc2ComPort = ComPort;
                        break;
                    case 3:
                        Settings.Default.Sc3ComPort = ComPort;
                        break;
                    case 4:
                        Settings.Default.Sc4ComPort = ComPort;
                        break;
                    case 5:
                        Settings.Default.Sc5ComPort = ComPort;
                        break;
                    case 6:
                        Settings.Default.Sc6ComPort = ComPort;
                        break;
                    default:
                        throw new ArgumentException();
                }
            }

            else if (TechnicalName.Contains(MainConst.NameTypeFluke))
                Settings.Default.FlukeComPort = ComPort;

            else if (TechnicalName.Contains(MainConst.NameTypeComm))
                Settings.Default.CommComPort = ComPort;

            Settings.Default.Save();
        }

        /// <summary>
        /// Заводское имя устройства ( точнее серийный номер)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///Флаг отвечает за участие устройства в тестировании.
        /// </summary>
        public bool IsActiveTest { get; set; }


        #region ConnectSettings

        /// <summary>
        /// Результат проверки выбранного ком порта на флюк, коммутатор или мас.
        /// </summary>
        public bool CheckedResult { get; set; }

        /// <summary>
        /// Флаг показывает происходит ли сейчас проверка выбранного ком порта
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Свойство для хранения информации об ошибке подключения.
        /// </summary>
        public Exception ErrorConnect { get; set; }

        /// <summary>
        /// Метод проверки валидности выбранного com port
        /// </summary>
        public void CheckComConnect()
        {
            if (ComPort == MainConst.DefaultComPort)
            {
                CheckedResult = false;
                return;
            }

            ErrorConnect = null;
            IsChecked = true;
            var serialPort = new SerialPortValidationChecker();
            bool resultCheck;
            try
            {
                resultCheck = serialPort.StartCheck(ComPort, TechnicalName);
            }
            catch (Exception e)
            {
                ErrorConnect = e;
                resultCheck = false;
            }

            CheckedResult = resultCheck;
            IsChecked = false;
        }

        /// <summary>
        /// Асинхронный запуск CheckComConnect. Необходимо учитывать, что сеттер завершается раньше чем запущенная из него проверка.
        /// </summary>
        public async void CheckComConnectAsync()
        {
            IsChecked = true;
            await Task.Run(CheckComConnect);
            IsChecked = false;
        }

        /// <summary>
        /// Метод запроса серийного номера Mac.
        /// Только для Mac
        /// </summary>
        public void GerSerialNumberSc()
        {
            var scSerialPort = new MacSerialPort(this);

            scSerialPort.OpenSerialPort();

           Name =  scSerialPort.GetSerialNumberSc();

            scSerialPort.Close();
        }

        #endregion
    }
}