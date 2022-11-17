namespace ZigbeeModels
{
    public class ZigbeeDevice
    {
        public virtual string Name { get; set; } = "";
        public string Id { get; set; } = "";
        public double linkquality { get; set; }
        public DateTime TimeStamp { get; set; } = new DateTime();
    }
}