using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Windows;
using MAC.Models;

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
                    _fluke.WriteLine(text + "\r\n");
                    Thread.Sleep(500);
                    return;
                }
                catch (Exception ex)
                {
                    GlobalLog.Log.Debug(ex, "Fluke:{_comPort}", _comPort);
                    var result = (int) MessageBox.Show(
                        "Нажмите Ок, что бы повторить попытку.\r\nНажмите Отмена, что бы перейти на следующую КС или остановить программу.",
                        $"{ex.Message}({_comPort})",
                        MessageBoxButton.OKCancel
                    );
                    switch (result)
                    {
                        case (int) MessageBoxResult.OK:
                            OpenFlukePort();
                            break;
                        case (int) MessageBoxResult.Cancel:
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

        public void SetOhmValue(int value, bool isOnOper = true)
        {
            var validationOhmList = new List<int> {80, 90, 100, 115, 120, 130, 140,200};
            if (!validationOhmList.Contains(value))
                throw new ArgumentException();

            Send(
                isOnOper
                    ? $"OUT {value} OHM;OPER"
                    : $"OUT {value} OHM"
            );
        }

        public void SetOhmValueCalibration()
        {
            var pointValue = 200;
            SetOhmValue(pointValue);
        }

        public void SetVoltValue(int value, bool isOnOper = true)
        {
            var validationVoltList = new List<int> {1, 2, 3, 4, 5};
            if (!validationVoltList.Contains(value))
                throw new ArgumentException();


            Send(
                isOnOper
                    ? $"OUT {value} V;OPER"
                    : $"OUT {value} V"
            );
        }

        public void SetHzValue(int value, bool isOnOper = true)
        {
            var validationHzList = new List<int> {5, 25, 50, 75, 100};
            if (!validationHzList.Contains(value))
                throw new ArgumentException();


            Send(
                isOnOper
                    ? $"OUT 4 V,{value} HZ;OPER"
                    : $"OUT 4 V,{value} HZ"
            );
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