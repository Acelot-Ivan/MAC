using System;
using MAC.ViewModels.Base;
using MAC.ViewModels.Services.SerialPort;

namespace MAC.Models.Value
{
    public class OhmValueSc : BaseVm, IScValue
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

        public OhmValueSc(int valueMeasurement, bool isActive)
        {
            TypeMeasurement = TypeMeasurement.Ohm;
            ValueMeasurement = valueMeasurement;
            IsActive = isActive;
        }


        public void SetFlukeSettings(FlukeSerialPort fluke, bool isOnOper)
        {
            fluke.SetOhmValue(ValueMeasurement , isOnOper);
        }

        public bool CheckedValidationDifferenceValue(decimal differenceValue)
        {
            switch (ValueMeasurement)
            {
                case 80:
                    return (0.04M >= differenceValue) && (differenceValue >= -0.04M);
                case 90:
                    return (0.04M >= differenceValue) && (differenceValue >= -0.04M);
                case 100:
                    return (0.04M >= differenceValue) && (differenceValue >= -0.04M);
                case 115:
                    return (0.04M >= differenceValue) && (differenceValue >= -0.04M);
                case 130:
                    return (0.1M >= differenceValue) && (differenceValue >= -0.1M);
                case 140:
                    return (0.1M >= differenceValue) && (differenceValue >= -0.1M);
                default:
                    throw new ArgumentException();
            }
        }

    }
}