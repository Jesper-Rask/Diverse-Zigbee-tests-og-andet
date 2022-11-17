namespace ZigbeeModels
{
    public class TempAndHumiSensor : BatteryPoweredDevice
    {
        public double humidity { get; set; }
        public double temperature { get; set; }
    }
}