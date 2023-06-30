﻿namespace MAC.Models
{
    public class MeasurementsData
    {
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string Pressure { get; set; }
        public string Voltage { get; set; } = "220";
        public string Frequency { get; set; } = "50";
        public TypeVerification TypeVerification { get; set; }
        /// <summary>
        /// Поверитель , в месте использования будет сделана коллекция имен или типо того
        /// </summary>
        public  string Verifier { get; set; }
    }

    public enum TypeVerification
    {
        Periodic, // Переодическая поверка
        Primary   // Первичная поверка
    }

    /// <summary>
    /// Класс для биндинга коллекции на чекбокс
    /// </summary>
    public class ListTypeVerification
    {
        /// <summary>
        /// Значение
        /// </summary>
        public TypeVerification TypeVerification { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }
    }
}
