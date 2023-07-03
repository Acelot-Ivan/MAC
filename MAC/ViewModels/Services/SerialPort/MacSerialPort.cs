using MAC.Models;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MAC.ViewModels.Services.SerialPort
{
    public class MacSerialPort
    {
        private System.IO.Ports.SerialPort _mac;

        private string _fullData = string.Empty;
        private string _currentData = string.Empty;
        private readonly string _comPort;

        public MacSerialPort(ComConnectItem comConnectItem)
        {
            _comPort = comConnectItem.ComPort;
        }

        public void OpenSerialPort()
        {
            Close();

            _mac = null;

            _mac = new System.IO.Ports.SerialPort
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

            _mac.Open();

            _mac.DataReceived += DataReceivedHandlerSignalController;

            _mac.WriteLine("");
            _mac.WriteLine("close\r");
        }

        /// <summary>
        /// Закрываю порт, отписываюсь от его просылушивания и освобождаю ресурсы
        /// </summary>
        public void Close()
        {
            try
            {
                //Закрываю сессию

                _mac?.WriteLine("\r");
                Thread.Sleep(200);
                _mac?.WriteLine("close\r");
                Thread.Sleep(200);

                if (_mac != null)
                {
                    _mac.DataReceived -= DataReceivedHandlerSignalController;
                }

                _mac?.Close();
                _mac?.Dispose();
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
                    _mac.WriteLine(text + "\r");
                    Thread.Sleep(500);
                    return;
                }
                catch (Exception ex)
                {
                    GlobalLog.Log.Debug(ex, "MAC:{_comPort}", _comPort);


                    int result = (int)MessageBox.Show(
                        "Нажмите Ок, что бы повторить попытку.\r\nНажмите Отмена, что бы перейти на следующую MAC или остановить программу.",
                        $"{ex.Message} ({_comPort})",
                        MessageBoxButton.OKCancel
                    );
                    switch (result)
                    {
                        case (int)MessageBoxResult.OK:
                            OpenSerialPort();
                            break;
                        case (int)MessageBoxResult.Cancel:
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
            if (_mac.IsOpen)
            {
                _mac.WriteLine(text + "\r");
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
            if (_mac.IsOpen)
            {
                var returnCorrect = new byte[] { 0x0d };
                _mac.Write(returnCorrect, 0, 1);
                Thread.Sleep(500);
            }
            else
            {
                throw new Exception(
                    "Сделать обработку ошибки, если порт отвалился. Например дать возможность переподключить.");
            }
        }

        /// <summary>
        /// Метод запроса серийного номера MAC.
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

        public (MacVersion, Version) GetVersionMac()
        {
            OpenSession();

            _currentData = string.Empty;
            //Небольшое ожидание, что бы очистить переменную от мусора _currentData
            Thread.Sleep(300);

            Send("VER");

            var stringVersion = new string(_currentData.Where(o => char.IsDigit(o) || o == '.').ToArray());

            var currentVersion = new Version(stringVersion);

            Send("");
            Send("close");

  
            return (MacVersion.New, currentVersion);
        }

        #region Send

        /// <summary>
        /// Открываю сессию с Mac
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

                Send("open mas *");
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
            //Для Мас используется пробел
            Send(" ");
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
            var inData = _mac.ReadExisting();
            _fullData += inData;
            _currentData += inData;
        }
    }
}
