namespace ZigbeeModels
{
    public abstract class BatteryPoweredDevice : ZigbeeDevice
    {
        public double battery { get; set; }
        public double voltage { get; set; }
    }
}
