using System;
using System.IO.Ports;
using System.Runtime.Remoting.Channels;
using System.Threading;
using System.Windows;
using MAC.Models;

namespace MAC.ViewModels.Services.SerialPort
{
    public class CommutatorSerialPort
    {
        private System.IO.Ports.SerialPort _commutator;
        private string _comPort;

        public CommutatorSerialPort(ComConnectItem comConnectItem)
        {
            _comPort = comConnectItem.ComPort;
            //OpenCommPort(comConnectItem.ComPort);
        }

        public void OpenCommPort()
        {
            Close();

            _commutator = null;

            _commutator = new System.IO.Ports.SerialPort
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

            _commutator.Open();
        }

        public void Send(string text)
        {
            while (true)
            {
                try
                {
                    _commutator.WriteLine($"{text}\r\n");
                    Thread.Sleep(500);
                    var x =_commutator.ReadExisting();
                    return;
                }
                catch (Exception ex)
                {
                    GlobalLog.Log.Debug(ex, $"Comm:{_comPort}");
                    int result = (int)MessageBox.Show(
                        "Нажмите Ок, что бы повторить попытку.\r\nНажмите Отмена, что бы перейти на следующую МАС или остановить программу.",
                        $"{ex.Message}({_comPort})",
                        MessageBoxButton.OKCancel
                    );
                    switch (result)
                    {
                        case (int)MessageBoxResult.OK:
                            OpenCommPort();
                            break;
                        case (int)MessageBoxResult.Cancel:
                            throw;
                    }
                }
            }
        }

        /// <summary>
        /// Включаем подачу питания на указанному Mac по индексу
        /// </summary>
        /// <param name="index">Номер МАС от 1 до 6</param>
        public void OnPowerIndex(int index)
        {

            //Для первой МАС(мастер), включение выполняется командой on  без индекса.
            if (index > 6)
                throw new ArgumentException();

            Send($"ch{index}");
        }

        /// <summary>
        /// Включаем напряжение на указанный канал Mac
        /// </summary>
        /// <param name="channel"> Канал Mac от 1 до 6</param>
        public void OnСhannel(int channel)
        {
            Send($"MUX {channel}");
        }

        public void OffCommAll()
        {
            Send($"off");
        }

        /// <summary>
        /// Закрыть порт и освободить ресурсы
        /// </summary>
        public void Close()
        {
            try
            {
                _commutator?.WriteLine("off\r\n");
                Thread.Sleep(400);
                _commutator?.Close();
                _commutator?.Dispose();
            }
            catch
            {
                // ignored
            }
        }
    }
}