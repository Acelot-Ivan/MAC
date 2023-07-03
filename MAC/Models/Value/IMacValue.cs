using System;
using MAC.ViewModels.Services.SerialPort;

namespace MAC.Models.Value
{
    public interface IMacValue
    {
        /// <summary>
        /// Тип проверяемого значения
        /// </summary>
        TypeMeasurement TypeMeasurement { get; set; }

        /// <summary>
        /// Проверяемое значение типа  TypeMeasurement
        ///Ом: 30, 85, 110, 155, 190Ом
        ///Вольт: 0.345, 1.325, 2.550, 3.775, 4.755
        ///Гц: 50, 250, 500, 750, 1000
        /// </summary>
        int ValueMeasurement { get; set; }

        /// <summary>
        /// Итоговое полученное значение тестирования КС
        /// </summary>
        decimal? ResultValue { get; set; }

        /// <summary>
        /// Будет ли проводится тестирование данного значения
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// Показывает, проверяется ли сейчас это значение , для его подсветки
        /// </summary>
        bool IsСheckedNow { get; set; }

        /// <summary>
        /// Флаг для биндинга на итерфейс, что бы подсветить ячейку по результату проверки валидности.
        /// Необходим, что бы не подсвечивать ячейки, которые еще не проверялись , либо не будут проверяться.
        /// </summary>
        bool IsVerified { get; set; }

        /// <summary>
        /// После получения результата, проверка его валидности.
        /// И подсветка результата красным или зеленым в зависимости от результата проверки
        /// </summary>
        bool IsValidResult { get; set; }

        /// <summary>
        /// При включении учета погрешности при получении результата теста,
        /// свойство прибавляется ResultValue
        /// </summary>
        decimal ErrorValue { get; set; }

        /// <summary>
        /// Время выполнения измерения
        /// </summary>
        /// <returns></returns>
        TimeSpan TimeMeasurements { get; set; }

        /// <summary>
        /// Установка нужных настроек на Fluke  по TypeMeasurement and ValueMeasurement
        /// </summary>
        void SetFlukeSettings(FlukeSerialPort fluke , bool isOnOper = true);

        /// <summary>
        /// Проверка отклонения полученного результата от нормы
        /// </summary>
        /// <param name="resultValue"></param>
        /// <returns></returns>
        bool CheckedValidationDifferenceValue(decimal resultValue);


    }

    /// <summary>
    /// Типы значений : Ом , Вольт , Частота
    /// </summary>
    public enum TypeMeasurement
    {
        Ohm,
        V,
        Hz
    }
}