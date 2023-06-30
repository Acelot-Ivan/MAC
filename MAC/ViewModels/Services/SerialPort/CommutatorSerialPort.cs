using System;
using System.IO.Ports;
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
                    return;
                }
                catch (Exception ex)
                {
                    GlobalLog.Log.Debug(ex, $"Comm:{_comPort}");
                    int result = (int)MessageBox.Show(
                        "Нажмите Ок, что бы повторить попытку.\r\nНажмите Отмена, что бы перейти на следующую КС или остановить программу.",
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
        /// Включаем подачу питания на указанному КС по индексу
        /// </summary>
        /// <param name="indexSignalController">Номер КС от 1 до 6</param>
        public void OnSignalController(int indexSignalController)
        {

            //Для первой КС(мастер), включение выполняется командой on  без индекса.
            if (indexSignalController > 6)
                throw new ArgumentException();

            Send("voltageon");

            //Для следующих кс(слейв), включение выполняется командой с указанием индекса.
            //Так же, если коммутатор уже ключен, voltageon не обязателен.
            //Индексы слейв кс начинаются с 1 ... и до 5
            if (indexSignalController > 1)
                Send($"voltage {indexSignalController - 1}");
        }

        /// <summary>
        /// Включаем напряжение на указанный канал КС
        /// </summary>
        /// <param name="channel"> Канал КС от 1 до 6</param>
        public void OnСhannel(int channel)
        {
            Send($"MUX {channel}");
        }

        /// <summary>
        /// Закрыть порт и освободить ресурсы
        /// </summary>
        public void Close()
        {
            try
            {
                _commutator?.WriteLine("voltageoff\r\n");
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