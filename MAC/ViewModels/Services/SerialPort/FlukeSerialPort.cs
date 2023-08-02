using MAC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Threading;
using System.Windows;

namespace MAC.ViewModels.Services.SerialPort
{
    public class FlukeSerialPort
    {
        private System.IO.Ports.SerialPort _fluke;
        private readonly string _comPort;

        /// <summary>
        /// Создать объект SerialPort для  Fluke  и открыть порт
        /// </summary>
        /// <param name="comConnectItem"></param>
        public FlukeSerialPort(ComConnectItem comConnectItem)
        {
            _comPort = comConnectItem.ComPort;
            //OpenFlukePort(comConnectItem.ComPort);
        }


        public void OpenFlukePort()
        {
            Close();

            _fluke = null;

            _fluke = new System.IO.Ports.SerialPort
            {
                PortName = _comPort,
                BaudRate = 9600,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
                ReadTimeout = 5000,
                WriteTimeout = 5000
            };

            _fluke.Open();
        }

        public void Send(string text)
        {
            while (true)
            {
                try
                {
                    _fluke.WriteLine(text + "\r");
                    Thread.Sleep(500);
                    return;
                }
                catch (Exception ex)
                {
                    GlobalLog.Log.Debug(ex, "Fluke:{_comPort}", _comPort);
                    var result = (int)MessageBox.Show(
                        "Нажмите Ок, что бы повторить попытку.\r\nНажмите Отмена, что бы перейти на следующую МАС или остановить программу.",
                        $"{ex.Message}({_comPort})",
                        MessageBoxButton.OKCancel
                    );
                    switch (result)
                    {
                        case (int)MessageBoxResult.OK:
                            OpenFlukePort();
                            break;
                        case (int)MessageBoxResult.Cancel:
                            throw;
                    }
                }
            }
        }

        /// <summary>
        /// Выключить подачу напряжения с Fluke
        /// </summary>
        public void FlukeOff()
        {
            Send("STBY");
        }

        /// <summary>
        /// 30, 85, 110, 155, 190Ом
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException"></exception>
        public void SetOhmValue(decimal value)
        {
            var validationOhmList = new List<decimal> { 30, 85, 110, 115, 155, 190, 200 };
            if (!validationOhmList.Contains(value))
                throw new ArgumentException();

            Send($"OUT {value} OHM;OPER");
        }

        public void SetOhmValueCalibration()
        {
            var pointValue = 200;
            SetOhmValue(pointValue);
        }

        /// <summary>
        /// 0.345, 1.325, 2.550, 3.775, 4.755
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException"></exception>
        public void SetVoltValue(decimal value)
        {
            var validationVoltList = new List<decimal> { 0.345m, 1.325m, 2.550m, 3.775m, 4.755m };
            if (!validationVoltList.Contains(value))
                throw new ArgumentException();

            var valueFix = value.ToString(CultureInfo.InvariantCulture).Replace(",", ".");

            Send($"OUT {valueFix} V;OPER");
        }

        /// <summary>
        /// 50, 250, 500, 750, 1000
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException"></exception>
        public void SetHzValue(decimal value)
        {
            var validationHzList = new List<decimal> { 50, 250, 500, 750, 1000 };
            if (!validationHzList.Contains(value))
                throw new ArgumentException();


            Send($"OUT 4 V,{value} HZ;DC_OFFSET +2 V;WAVE SQUARE;OPER");
        }

        /// <summary>
        /// Закрыть порт и освободить ресурсы
        /// </summary>
        public void Close()
        {
            try
            {
                //Выключаю Fluke

                _fluke?.WriteLine("*RST\r\n");
                Thread.Sleep(200);

                _fluke?.Close();
                _fluke?.Dispose();
            }
            catch
            {
                // ignored
            }
        }
    }
}