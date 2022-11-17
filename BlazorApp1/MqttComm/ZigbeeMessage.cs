namespace BlazorApp1.MqttComm
{
    public class ZigbeeMessage
    {
        public string Topic { get; set; }
        public string Message { get; set; }
        public ZigbeeMessage(string topic, string message)
        {
            Topic = topic;
            Message = message;
        }
    }
   
}
