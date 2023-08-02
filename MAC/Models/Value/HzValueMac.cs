using MAC.ViewModels.Base;
using MAC.ViewModels.Services.SerialPort;
using System;

namespace MAC.Models.Value
{
    public class HzValueMac : BaseVm, IMacValue
    {
        public TypeMeasurement TypeMeasurement { get; set; }
        public decimal ValueMeasurement { get; set; }
        public decimal? ResultValue { get; set; }
        public bool IsActive { get; set; }
        public bool IsСheckedNow { get; set; }
        public bool IsVerified { get; set; }
        public bool IsValidResult { get; set; }
        public decimal ErrorValue { get; set; }
        public TimeSpan TimeMeasurements { get; set; }

        /// <summary>
        /// допустимая погрешность
        /// </summary>
        private const decimal AdmissibleErrorValue = 0.05m;

        public HzValueMac(int valueMeasurement, bool isActive)
        {
            TypeMeasurement = TypeMeasurement.Hz;
            ValueMeasurement = valueMeasurement;
            IsActive = isActive;
        }

        public void SetFlukeSettings(FlukeSerialPort fluke)
        {
            fluke.SetHzValue(ValueMeasurement);
        }

        public bool CheckedValidationDifferenceValue(decimal differenceValue) =>
            AdmissibleErrorValue >= differenceValue && differenceValue >= -AdmissibleErrorValue;
    }
}