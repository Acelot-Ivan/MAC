namespace MAC.Models
{
    public class MeasurementsData
    {
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        /// <summary>
        /// Поверитель , в месте использования будет сделана коллекция имен или типо того
        /// </summary>
        public  string Verifier { get; set; }
    }
}
