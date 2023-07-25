using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using MAC.Models;
using MAC.ViewModels.Base;
using MAC.ViewModels.Services.SerialPort;

namespace MAC.ViewModels
{
    public class SelectActiveMacVm
    {
        public Action CloseWindow;
        public RelayCommand ContinueCommand => new RelayCommand(Continue, ContinueValidation);
        public RelayCommand CancelCommand => new RelayCommand(Cancel);
        public RelayCommand GetSerialNumberMacCommand => new RelayCommand(GetSerialNumber);
        public ObservableCollection<ComConnectItem> MacItems { get; set; }

        object locker = new object();
        public bool IsContinue { get; set; }

        public SelectActiveMacVm(IEnumerable<ComConnectItem> macItems, CommutatorSerialPort comm)
        {
            MacItems = new ObservableCollection<ComConnectItem>(macItems);

            Task.Run(async () => await Task.Run(() =>
            {

                comm.OpenCommPort();

                foreach (var item in MacItems)
                {
                    comm.OnPowerIndex(item.Number);
                    Thread.Sleep(200);
                    item.CheckComConnectAsyncGetSerial();
                    comm.OffCommAll();
                    Thread.Sleep(200);
                }

                comm.Close();   
            }));
        }

        private void GetSerialNumber(object obj)
        {
            var comConnectItem = obj as ComConnectItem;

           lock(locker)
           {
               if (comConnectItem?.CheckedResult == true)
               {
                   Task.Run(async () => await Task.Run(() => comConnectItem.GerSerialNumberSc()));
               }
            }
        }

        private void Cancel()
        {
            IsContinue = false;
            CloseWindow();
        }

        private void Continue()
        {
            IsContinue = true;
            CloseWindow();
        }

        private bool ContinueValidation(object obj)
        {
            var isActiveOneSignalController = false;

            foreach (var item in MacItems)
            {
                if (item.IsActiveTest)
                    isActiveOneSignalController = true;
            }

            return isActiveOneSignalController;
        }
    }
}