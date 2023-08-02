using System;
using System.IO.Ports;
using System.Threading;

namespace MAC.ViewModels.Services.SerialPort
{
    public class SerialPortValidationChecker
    {
        private System.IO.Ports.SerialPort _serialPort;

        private void Open(string portName, int baudRate, ComType comType)
        {
            _serialPort = new System.IO.Ports.SerialPort
            {
                PortName = portName,
                BaudRate = baudRate,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
                ReadTimeout = 5000,
                WriteTimeout = 5000
            };

            _serialPort.Open();

            Thread.Sleep(200);

            if (comType == ComType.Mac)
            {
                _serialPort.WriteLine("close\r\n");
            }

        }

        public bool StartCheck(string portName, string comName)
        {
            string getData;
            var comType = GetComType(comName);

            Open(portName, comType == ComType.Commutator ? 115200 : 9600, comType);


            switch (comType)
            {
                case ComType.Mac:
                    Thread.Sleep(200);
                    _serialPort.WriteLine("open mas *\r\n");
                    Thread.Sleep(200);
                    _serialPort.WriteLine("close\r\n");
                    Thread.Sleep(100);
                    getData = ReadData();
                    if (getData == null) return false;
                    if (getData.Contains("SESSION"))
                    {
                        CloseConnect();
                        return true;
                    }
                    break;
                case ComType.Fluke:
                    _serialPort.WriteLine("Hello");
                    Thread.Sleep(200);
                    getData = ReadData();
                    if (getData == null) return false;
                    if (getData.Contains("Fault") && getData.Contains("Unknown command"))
                    {
                        CloseConnect();
                        return true;
                    }
                    break;
                case ComType.Commutator:
                    _serialPort.WriteLine("\r\n");
                    Thread.Sleep(200);
                    _serialPort.WriteLine("Who\r\n");
                    Thread.Sleep(200);
                    getData = ReadData();
                    if (getData == null) return false;
                    if (getData.Contains("MAC") && getData.Contains("KOMMYTATOP"))
                    {
                        CloseConnect();
                        return true;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }



            CloseConnect();
            return false;
        }

        private string ReadData()
        {
            //25 * 200 = 5000 мс ожидания чтения.
            var countRequest = 25;

            while (countRequest > 0)
            {
                if (_serialPort.BytesToRead > 0)
                {
                    break;
                }

                Thread.Sleep(200);
                countRequest--;

                if (countRequest <= 0)
                {
                    return null;
                }
            }

            var getData = _serialPort.ReadExisting();
            return getData;
        }

        private ComType GetComType(string portName)
        {
            if (portName.Contains(MainConst.NameTypeSignalController))
                return ComType.Mac;
            if (portName.Contains(MainConst.NameTypeFluke))
                return ComType.Fluke;
            if (portName.Contains(MainConst.NameTypeComm))
                return ComType.Commutator;
            throw new Exception($"Недопустимое значение {typeof(ComType).Name}");
        }

        private void CloseConnect()
        {
            _serialPort.Close();
            _serialPort.Dispose();
        }
    }

    public enum ComType
    {
        Mac,
        Fluke,
        Commutator
    }
}