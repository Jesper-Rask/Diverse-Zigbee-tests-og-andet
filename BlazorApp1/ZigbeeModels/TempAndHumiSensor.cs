namespace ZigbeeModels
{
    public class TempAndHumiSensor : BatteryPoweredDevice
    {
        public event EventHandler<int> TemperatureChanged;
        public event EventHandler<int> TemperatureAboveHigh;
        public event EventHandler<int> TemperatureBelowHigh;
        public event EventHandler<int> TemperatureAboveLow;
        public event EventHandler<int> TemperatureBelowLow;
        public event EventHandler<int> HumidityChanged;
        public event EventHandler<int> HumidityAboveHigh;
        public event EventHandler<int> HumidityBelowHigh;
        public event EventHandler<int> HumidityAboveLow;
        public event EventHandler<int> HumidityBelowLow;

        private double _temperature;
        private double _humidity;

        public double TemperatureHighLimit { get; set; } = 23;
        public double TemperatureLowLimit { get; set; } = 20;
        public double HumidityHighLimit { get; set; } = 80;
        public double HumidityLowLimit { get; set; } = 75;
        public double humidity
        {
            get { return _humidity; }
            set
            {
                if (value != _humidity)
                {
                    _humidity = value;
                    HumidityChanged?.Invoke(this, -1);
                    if (value < HumidityHighLimit) HumidityBelowHigh?.Invoke(this, -1);
                    if (value < HumidityLowLimit) HumidityBelowLow?.Invoke(this, -1);
                    if (value > HumidityHighLimit) HumidityAboveHigh?.Invoke(this, -1);
                    if (value > HumidityLowLimit) HumidityAboveLow?.Invoke(this, -1);
                }
            }
        }
        public double temperature
        {
            get { return _temperature; }
            set
            {
                if (value != _temperature)
                {
                    _temperature = value;
                    TemperatureChanged?.Invoke(this, -1);
                    if (value < TemperatureHighLimit) TemperatureBelowHigh?.Invoke(this, -1);
                    if (value < TemperatureLowLimit) TemperatureBelowLow?.Invoke(this, -1);
                    if (value > TemperatureHighLimit) TemperatureAboveHigh?.Invoke(this, -1);
                    if (value > TemperatureLowLimit) TemperatureAboveLow?.Invoke(this, -1);
                }
            }
        }
    }
}