using System;
using MAC.ViewModels.Base;
using MAC.ViewModels.Services.SerialPort;

namespace MAC.Models.Value
{
    public class VoltValueSc : BaseVm, IScValue
    {
        public TypeMeasurement TypeMeasurement { get; set; }
        public int ValueMeasurement { get; set; }
        public decimal? ResultValue { get; set; }
        public bool IsActive { get; set; }
        public bool IsСheckedNow { get; set; }
        public bool IsVerified { get; set; }
        public bool IsValidResult { get; set; }
        public decimal ErrorValue { get; set; }
        public TimeSpan TimeMeasurements { get; set; }


        public VoltValueSc(int valueMeasurement, bool isActive)
        {
            TypeMeasurement = TypeMeasurement.V;
            ValueMeasurement = valueMeasurement;
            IsActive = isActive;
        }

        public void SetFlukeSettings(FlukeSerialPort fluke , bool isOnOper)
        {
            fluke.SetVoltValue(ValueMeasurement , isOnOper);
        }

        public bool CheckedValidationDifferenceValue(decimal differenceValue) =>
            0.05m >= differenceValue && differenceValue >= -0.05m;
    }
}