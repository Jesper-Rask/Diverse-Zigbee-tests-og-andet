namespace ZigbeeModels
{
    public class BridgeDevice
    {
        public string ieee_address { get; set; }
        public string type { get; set; }
        public int network_address { get; set; }
        public bool supported { get; set; }
        public string friendly_name { get; set; }
        public string model_id { get; set; }
        public Definition definition { get; set; }
        public bool Edit { get; set; }
    }
    public class Definition
    {
        public string model { get; set; }
        public string vendor { get; set; }
        public string description { get; set; }
    }
}
