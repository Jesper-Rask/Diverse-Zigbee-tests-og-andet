namespace ZigbeeModels
{
    public class TempAndHumiSensor : BatteryPoweredDevice
    {
        public event EventHandler TemperatureChanged;
        public event EventHandler TemperatureAboveHigh;
        public event EventHandler TemperatureBelowHigh;
        public event EventHandler TemperatureAboveLow;
        public event EventHandler TemperatureBelowLow;
        public event EventHandler HumidityChanged;
        public event EventHandler HumidityAboveHigh;
        public event EventHandler HumidityBelowHigh;
        public event EventHandler HumidityAboveLow;
        public event EventHandler HumidityBelowLow;

        private double _temperature;
        private double _humidity;

        public double TemperatureHighLimit { get; set; } = 23;
        public double TemperatureLowLimit { get; set; } = 20;
        public double HumidityHighLimit { get; set; } = 70;
        public double HumidityLowLimit { get; set; } = 65;
        public double humidity
        {
            get { return _humidity; }
            set
            {
                if (value != _humidity)
                {
                    _humidity = value;
                    HumidityChanged?.Invoke(this, EventArgs.Empty);
                    if (value < HumidityHighLimit) HumidityBelowHigh?.Invoke(this, EventArgs.Empty);
                    if (value < HumidityLowLimit) HumidityBelowLow?.Invoke(this, EventArgs.Empty);
                    if (value > HumidityHighLimit) HumidityAboveHigh?.Invoke(this, EventArgs.Empty);
                    if (value > HumidityLowLimit) HumidityAboveLow?.Invoke(this, EventArgs.Empty);
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
                    TemperatureChanged?.Invoke(this, EventArgs.Empty);
                    if (value < TemperatureHighLimit) TemperatureBelowHigh?.Invoke(this, EventArgs.Empty);
                    if (value < TemperatureLowLimit) TemperatureBelowLow?.Invoke(this, EventArgs.Empty);
                    if (value > TemperatureHighLimit) TemperatureAboveHigh?.Invoke(this, EventArgs.Empty);
                    if (value > TemperatureLowLimit) TemperatureAboveLow?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}