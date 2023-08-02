using MAC.Models;
using MAC.ViewModels.Base;
using MAC.ViewModels.Services.SerialPort;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace MAC.ViewModels
{
    public class SelectActiveMacVm
    {
        public Action CloseWindow;
        public RelayCommand ContinueCommand => new RelayCommand(Continue, ContinueValidation);
        public RelayCommand CancelCommand => new RelayCommand(Cancel);

        public RelayCommand CheckMacCommand => new RelayCommand(CheckMac, IsCheckNowValidation);
        public ObservableCollection<ComConnectItem> MacItems { get; set; }
        private readonly CommutatorSerialPort _comm;
        public bool IsContinue { get; set; }

        /// <summary>
        /// Флаг отвечающий за доступ к функции проверки мас
        /// Если true, то проверка уже идет и запускать еще раз нельзя
        /// </summary>
        public bool IsCheckNow { get; set; }

        public SelectActiveMacVm(IEnumerable<ComConnectItem> macItems, CommutatorSerialPort comm)
        {
            MacItems = new ObservableCollection<ComConnectItem>(macItems);
            _comm = comm;
            CheckMac();
        }

        private bool IsCheckNowValidation(object obj) => !IsCheckNow;

        /// <summary>
        /// Проверка валидности и запрос серийного номера
        /// </summary>
        private void CheckMac()
        {
            Task.Run(async () =>
            {
                await Task.Run(() =>
                {
                    IsCheckNow = true;
                    _comm.OpenCommPort();

                    foreach (var item in MacItems)
                    {
                        item.Name = "Укажите имя";
                        item.CheckedResult = false;


                        _comm.OnPowerIndex(item.Number);
                        Thread.Sleep(200);
                        item.CheckComConnectAsyncGetSerial();
                        _comm.OffCommAll();
                        Thread.Sleep(200);
                    }

                    _comm.Close();

                    IsCheckNow = false;
                });
            });
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