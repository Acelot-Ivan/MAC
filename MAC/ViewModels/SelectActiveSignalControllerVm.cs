using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MAC.Models;
using MAC.ViewModels.Base;

namespace MAC.ViewModels
{
    public class SelectActiveSignalControllerVm
    {
        public Action CloseWindow;
        public RelayCommand ContinueCommand => new RelayCommand(Continue, ContinueValidation);
        public RelayCommand CancelCommand => new RelayCommand(Cancel);
        public RelayCommand GetSerialNumberScCommand => new RelayCommand(GetSerialNumber);
        public ObservableCollection<ComConnectItem> SignalControllerItems { get; set; }

        object locker = new object();
        public bool IsContinue { get; set; }

        public SelectActiveSignalControllerVm(IEnumerable<ComConnectItem> signalControllerItems)
        {
            SignalControllerItems = new ObservableCollection<ComConnectItem>(signalControllerItems);

            Task.Run(async () => await Task.Run(() =>
            {
                foreach (var item in SignalControllerItems)
                {
                    item.CheckComConnectAsync();
                }
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

            foreach (var item in SignalControllerItems)
            {
                if (item.IsActiveTest)
                    isActiveOneSignalController = true;
            }

            return isActiveOneSignalController;
        }
    }
}