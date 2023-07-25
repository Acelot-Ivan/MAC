using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;
using System.Linq;
using MAC.Models;
using MAC.ViewModels.Base;
using MAC.ViewModels.Services;

namespace MAC.ViewModels
{
    public class ConnectSettingsVm : BaseVm
    {
        #region RelayCommand
        //Кнопка под эту команду отключена

        //public RelayCommand CheckComConnectCommand => new RelayCommand(CheckComConnect);

        #endregion

        public double ContentGridHeight { get; set; }
        public double ContentGridWidth { get; set; }
        public bool IsActiveTest { get; set; }

        /// <summary>
        /// Все подключенные comPort
        /// </summary>
        public ObservableCollection<ExistingComPort> AllExistingComPorts { get; set; }

        public ObservableCollection<ComConnectItem> ComConnectItems { get; set; }


        public ConnectSettingsVm(ObservableCollection<ComConnectItem> comConnectItems, double contentGridHeight,
            double contentGridWidth, bool isActiveTest)
        {
            ComConnectItems = comConnectItems;
            ContentGridHeight = contentGridHeight;
            ContentGridWidth = contentGridWidth;
            IsActiveTest = isActiveTest;

            var portList = SerialPort.GetPortNames().ToList();
            portList.Insert(0, MainConst.DefaultComPort);

            AllExistingComPorts =
                new ObservableCollection<ExistingComPort>(portList.Select(item => new ExistingComPort(item)));

            UpdateAvailableComPorts();
        }

        //private void CheckComConnect(object obj)
        //{
        //    var comConnectItem = obj as ComConnectItem;
        //    comConnectItem?.CheckComConnectAsyncGetSerial();
        //}


        /// <summary>
        /// При открытии выпадающего списка , обновляет список доступных comport.
        /// </summary>
        public void UpdateAvailableComPort()
        {
            var newComPorts = SerialPort.GetPortNames().ToList();
            newComPorts.Insert(0, MainConst.DefaultComPort);

            var oldComPort = (from comPort in AllExistingComPorts select comPort.ComPort).ToList();


            //Удаляю старые итемы коллекции, есди их нет в новой
            foreach (var comPort in AllExistingComPorts.ToList())
            {
                if (!newComPorts.Contains(comPort.ComPort))
                {
                    AllExistingComPorts.Remove(comPort);
                }
            }

            //Добавляю в колекцию новые итемы, если они есть в новом списке доступных портов
            foreach (var newComPort in newComPorts)
            {
                if (!oldComPort.Contains(newComPort))
                {
                    AllExistingComPorts.Add(new ExistingComPort(newComPort));
                }
            }
        }


        /// <summary>
        /// Событие выбора пользователем итема из выпадающего списка
        /// </summary>
        public void UpdateAvailableComPorts()
        {
            var useComPort = new List<string>();

            foreach (var item in ComConnectItems)
            {
                if (!useComPort.Contains(item.ComPort))
                    useComPort.Add(item.ComPort);
            }

            foreach (var existingComPort in AllExistingComPorts)
            {
                if (existingComPort.ComPort == MainConst.DefaultComPort)
                {
                    existingComPort.IsUse = false;
                    continue;
                }

                existingComPort.IsUse = useComPort.Contains(existingComPort.ComPort);
            }
        }
    }

    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class ExistingComPort : BaseVm
    {
        public ExistingComPort(string comPort)
        {
            ComPort = comPort;
        }

        public string ComPort { get; set; }
        public bool IsUse { get; set; }
    }
}