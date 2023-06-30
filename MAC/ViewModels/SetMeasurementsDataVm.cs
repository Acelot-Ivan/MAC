﻿using System;
using System.Collections.Generic;
using MAC.Models;
using MAC.ViewModels.Base;

namespace MAC.ViewModels
{
    public class SetMeasurementsDataVm : BaseVm
    {
        public Action CloseWindow;
        public MeasurementsData MeasurementsData { get; set; }

        public bool IsContinue { get; set; }

        public List<ListTypeVerification> TypeVerificationsList { get; set; } = new List<ListTypeVerification>
        {
            new ListTypeVerification
            {
                TypeVerification = TypeVerification.Periodic, Description = "Переодическая"
            },
            new ListTypeVerification
            {
                TypeVerification = TypeVerification.Primary, Description = "Первичная"
            }
        };


        public RelayCommand ContinueCommand => new RelayCommand(Continue);
        public RelayCommand CancelCommand => new RelayCommand(Cancel);

        public SetMeasurementsDataVm(ref MeasurementsData measurementsData)
        {
            MeasurementsData = measurementsData;
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
    }
}