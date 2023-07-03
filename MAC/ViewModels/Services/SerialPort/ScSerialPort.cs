using System;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Windows;
using MAC.Models;

namespace MAC.ViewModels.Services.SerialPort
{
    public class ScSerialPort123
    {
        private System.IO.Ports.SerialPort _signalController;

        private string _fullData = string.Empty;
        private string _currentData = string.Empty;
        private readonly string _comPort;

        public ScSerialPort123(ComConnectItem comConnectItem)
        {
            _comPort = comConnectItem.ComPort;
        }

        public void OpenSerialPort()
        {
            Close();

            _signalController = null;

            _signalController = new System.IO.Ports.SerialPort
            {
                PortName = _comPort,
                BaudRate = 115200,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
                ReadTimeout = 5000,
                WriteTimeout = 5000
            };

            _signalController.Open();

            _signalController.DataReceived += DataReceivedHandlerSignalController;

            _signalController.WriteLine("");
            _signalController.WriteLine("close\r");
        }


        /// <summary>
        /// Закрываю порт, отписываюсь от его просылушивания и освобождаю ресурсы
        /// </summary>
        public void Close()
        {
            try
            {
                //Закрываю сессию

                _signalController?.WriteLine("\r");
                Thread.Sleep(200);
                _signalController?.WriteLine("close\r");
                Thread.Sleep(200);

                if (_signalController != null)
                {
                    _signalController.DataReceived -= DataReceivedHandlerSignalController;
                }

                _signalController?.Close();
                _signalController?.Dispose();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void Send(string text)
        {
            while (true)
            {
                try
                {
                    _signalController.WriteLine(text + "\r");
                    Thread.Sleep(500);
                    return;
                }
                catch (Exception ex)
                {
                    GlobalLog.Log.Debug(ex, "KC:{_comPort}", _comPort);


                    int result = (int) MessageBox.Show(
                        "Нажмите Ок, что бы повторить попытку.\r\nНажмите Отмена, что бы перейти на следующую КС или остановить программу.",
                        $"{ex.Message} ({_comPort})",
                        MessageBoxButton.OKCancel
                    );
                    switch (result)
                    {
                        case (int) MessageBoxResult.OK:
                            OpenSerialPort();
                            break;
                        case (int) MessageBoxResult.Cancel:
                            throw;
                    }
                }
            }
        }

        /// <summary>
        /// Метод для новой калибровки. Без /n
        /// </summary>
        public void SendWithOutN(string text)
        {
            if (_signalController.IsOpen)
            {
                _signalController.WriteLine(text + "\r");
                Thread.Sleep(500);
            }
            else
            {
                throw new Exception(
                    "Сделать обработку ошибки, если порт отвалился. Например дать возможность переподключить.");
            }
        }

        public void SendEnter()
        {
            if (_signalController.IsOpen)
            {
                var returnCorrect = new byte[] {0x0d};
                _signalController.Write(returnCorrect, 0, 1);
                Thread.Sleep(500);
            }
            else
            {
                throw new Exception(
                    "Сделать обработку ошибки, если порт отвалился. Например дать возможность переподключить.");
            }
        }


        /// <summary>
        /// Метод запроса серийного номера КС.
        /// Запрос работает только на новых прошивках КС , 
        /// с 12.1.10
        /// </summary>
        public string GetSerialNumberSc()
        {
            OpenSession();

            _currentData = string.Empty;

            //Небольшое ожидание, что бы очистить переменную от мусора _currentData
            Thread.Sleep(300);

            Send("SNUM");

            var serialNumber = _currentData.Replace(">", "").Replace("\t", "").Replace("\r", "").Replace("\n", "")
                .Replace("SNUM", "").Replace("SERIAL NUMBER:", "").Replace(" ", "");

            Send("");
            Send("close");


            return serialNumber;
        }

        public (ScVersion, Version) GetVersionSc()
        {
            OpenSession();

            _currentData = string.Empty;
            //Небольшое ожидание, что бы очистить переменную от мусора _currentData
            Thread.Sleep(300);

            Send("VER");


            var oldUpdateVersion = new Version(12, 1, 9);
            var newUpdateVersion = new Version(12, 1, 11);


            var stringVersion = new string(_currentData.Where(o => char.IsDigit(o) || o == '.').ToArray());

            var currentVersion = new Version(stringVersion);

            Send("");
            Send("close");

            if (currentVersion <= oldUpdateVersion)
                return (ScVersion.Old, currentVersion);
            if (currentVersion >= newUpdateVersion)
                return (ScVersion.New, currentVersion);

            throw new Exception("Невалидное значение версии Контролера сигналов");
        }

        #region Send

        /// <summary>
        /// Открываю сессию с КС
        /// </summary>
        public void OpenSession()
        {
            Send("close");
            Thread.Sleep(1500);
            ClearCurrentData();
            var countAttempt = 0;

            while (countAttempt != 3)
            {
                if (countAttempt > 0)
                {
                    Send("close");
                    Thread.Sleep(5000);
                }

                Send("open");
                Thread.Sleep(1000);

                var content = _currentData.Replace(" ", "");

                if (content.Contains("SESSIONOPENED"))
                {
                    return;
                }

                countAttempt++;
            }

            throw new Exception("Не удалось открыть сессию");
        }

        /// <summary>
        /// Запуск теста и очистка данных предыдущего теста
        /// </summary>
        public void StartTest()
        {
            //Очищаю данные последнего теста, что бы записать новые
            ClearCurrentData();
            Thread.Sleep(1000);
            Send("test");
        }

        /// <summary>
        /// Остановка теста
        /// </summary>
        public void StopTest()
        {
            //Для остановки можно использовать почти любой набор символов
            Send("");
        }

        /// <summary>
        /// Закрытие сессии
        /// </summary>
        public void CloseSession()
        {
            Send("close");
        }

        #endregion


        public string GetCurrentData()
        {
            return _currentData;
        }

        /// <summary>
        /// Очистка переменной хранящей данные последнего теста 
        /// </summary>
        private void ClearCurrentData()
        {
            _currentData = string.Empty;
        }

        public string GetFullData()
        {
            return _fullData;
        }

        public void ClearFullData()
        {
            _fullData = string.Empty;
        }


        private void DataReceivedHandlerSignalController(object sender, SerialDataReceivedEventArgs e)
        {
            var inData = _signalController.ReadExisting();
            _fullData += inData;
            _currentData += inData;
        }
    }
}