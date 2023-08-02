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
        private CommutatorSerialPort Comm;

        object locker = new object();
        public bool IsContinue { get; set; }

        public SelectActiveMacVm(IEnumerable<ComConnectItem> macItems, CommutatorSerialPort comm)
        {
            MacItems = new ObservableCollection<ComConnectItem>(macItems);
            Comm = comm;
        }

        /// <summary>
        /// Проверка валидности и запрос серийного номера
        /// </summary>
        public void CheckMac()
        {
            Task.Run(async () => await Task.Run(() =>
            {

                Comm.OpenCommPort();

                foreach (var item in MacItems)
                {
                    Comm.OnPowerIndex(item.Number);
                    Thread.Sleep(200);
                    item.CheckComConnectAsyncGetSerial();
                    Comm.OffCommAll();
                    Thread.Sleep(200);
                }

                Comm.Close();
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
            var isActiveOneMac = false;

            foreach (var item in MacItems)
            {
                if (item.IsActiveTest)
                    isActiveOneMac = true;
            }

            return isActiveOneMac;
        }
    }
}