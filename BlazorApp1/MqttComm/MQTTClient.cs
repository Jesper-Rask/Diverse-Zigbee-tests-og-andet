using BlazorApp1.ZigbeeModels;
using System.Text;
using System.Text.Json;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MqttComm
{
    public sealed class MQTTClient
    {
        public List<ZigbeeDevice> zigbeeDevices { get; set; }
        MqttClient? mqttClient;
        public event EventHandler<ZigbeeDevice>? MessageRecieved;

        public MQTTClient()
        {
            zigbeeDevices = ZigbeeDevicesToList();
            Connect(new MQTTConnectionModel());
        }

        private static List<ZigbeeDevice> ZigbeeDevicesToList()
        {          
            Data.zigbeeDevices.Add(new TempAndHumiSensor    { Name = "Sensor Køkken",           Id = "0x00124b0025034367" });
            Data.zigbeeDevices.Add(new TempAndHumiSensor    { Name = "Sensor Kælder Værksted",  Id = "0x00124b0025030228" });
            Data.zigbeeDevices.Add(new TempAndHumiSensor    { Name = "Sensor Vaskerum",         Id = "0x00124b00250338cf" });
            Data.zigbeeDevices.Add(new TempAndHumiSensor    { Name = "Sensor Julies Værelse",   Id = "0x00124b00251ef0ec" }); 
            Data.zigbeeDevices.Add(new TempAndHumiSensor    { Name = "Sensor Soveværelse",      Id = "0x00124b0025046109" });
            Data.zigbeeDevices.Add(new TempAndHumiSensor    { Name = "Sensor Stue",             Id = "0x00124b00250335b1" });
            Data.zigbeeDevices.Add(new TempAndHumiSensor    { Name = "Sensor Kontor",           Id = "0x00124b00251d4787" });
            Data.zigbeeDevices.Add(new TempAndHumiSensor    { Name = "Sensor Systue",           Id = "0x00124b00251515b1" });
            Data.zigbeeDevices.Add(new TempAndHumiSensor    { Name = "Sensor Bad",              Id = "0x00124b00251e034d" });
            Data.zigbeeDevices.Add(new TempAndHumiSensor    { Name = "Sensor Kælder Bad",       Id = "0x00124b0025212f83" });
            Data.zigbeeDevices.Add(new Switch               { Name = "Relæ",                    Id = "0x00124b002343dff3" });
            Data.zigbeeDevices.Add(new PIRSensorModel       { Name = "PIR Sensor",              Id = "0x00124b002508d2ce" });
            Data.zigbeeDevices.Add(new IkeaBulb             { Name = "IKEA Pære Soveværelse",   Id = "0x94deb8fffe575196" });
            Data.zigbeeDevices.Add(new IkeaTryk             { Name = "IKEA Tryk",               Id = "0x540f57fffe3c6bee" });
            Data.zigbeeDevices.Add(new FugaTryk             { Name = "Fuga Tryk Køkken",        Id = "0x000000000171191b" });
            Data.zigbeeDevices.Add(new FugaTryk             { Name = "Fuga Tryk Stue",          Id = "0x000000000171838e" });
            Data.zigbeeDevices.Add(new IkeaBulb             { Name = "IKEA Pære Stue",          Id = "0x086bd7fffe2b0647" });
            Data.zigbeeDevices.Add(new LedPanel             { Name = "Lampe 1 Køkken",          Id = "0x84ba20fffe34518e" });
            Data.zigbeeDevices.Add(new LedPanel             { Name = "Lampe 2 Køkken",          Id = "0x84ba20fffe3451b0" });
            Data.zigbeeDevices.Add(new LedPanel             { Name = "Lampe 3 Køkken",          Id = "0x84ba20fffe344eea" });
            Data.zigbeeDevices.Add(new LedPanel             { Name = "Lampe 1 Stue",            Id = "0x84ba20fffe3450bf" });
            Data.zigbeeDevices.Add(new LedPanel             { Name = "Lampe 2 Stue",            Id = "0x84ba20fffe345d07" });
            Data.zigbeeDevices.Add(new LedPanel             { Name = "Lampe 3 Stue",            Id = "0x84ba20fffe345d90" });
            Data.zigbeeDevices.Add(new LedPanel             { Name = "Lampe 4 Stue",            Id = "0x84ba20fffe345d77" });
            Data.zigbeeDevices.Add(new LedPanel             { Name = "Lampe 5 Stue",            Id = "0x84ba20fffe344f74" });
            return Data.zigbeeDevices;
        }
        public void Publish(string topic, string message)
        {
            Task.Run(() =>
            {
                if (mqttClient is not null && mqttClient.IsConnected)
                {
                    mqttClient.Publish(topic, Encoding.UTF8.GetBytes(message));
                }
            });
        }
        private void Connect(MQTTConnectionModel model)
        {
            Task.Run(() =>
            {
                mqttClient = new MqttClient(model.IpAddress);
                mqttClient.MqttMsgPublishReceived += MqttClient_MqttMsgPublishReceived;
                byte[] QOS = new byte[zigbeeDevices.Count()];
                Array.Fill(QOS, MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE);
                mqttClient.Subscribe(zigbeeDevices.Select(x => "zigbee2mqtt/" + x.Id).ToArray(), QOS);
                mqttClient.Connect(model.Client);
            });
        }
        private void MqttClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Message);
            ZigbeeDevice zig = GetZigbeeDevice(e.Topic, message);
            MessageRecieved?.Invoke(this, zig);
        }
        private ZigbeeDevice GetZigbeeDevice(string topic, string message)
        {
            ZigbeeDevice foundDevice = new();
            bool found = false;

            if (topic == "zigbee2mqtt/bridge/devices")
            {
                Data.bridgeDevices = JsonSerializer.Deserialize<List<BridgeDevice>>(message);
                if (Data.bridgeDevices is null) return new UnKnownType();

                foreach(BridgeDevice device in Data.bridgeDevices)
                {
                    foreach(ZigbeeDevice dev in Data.zigbeeDevices)
                    {
                        if(device.ieee_address == dev.Id)
                        {
                            device.friendly_name = dev.Name;
                            break;
                        }
                    }
                }                  
            }

            foreach (ZigbeeDevice device in zigbeeDevices)
            {
                if (topic.Contains(device.Id))
                {
                    foundDevice = device;
                    found = true;
                    break;
                }
            }

            if (!found) return new UnKnownType();

            switch (foundDevice)
            {
                case TempAndHumiSensor:
                    TempAndHumiSensor? t = JsonSerializer.Deserialize<TempAndHumiSensor>(message);
                    if (t != null)
                    {
                        t.Name = foundDevice.Name;
                        t.Id = foundDevice.Id;
                        return t;
                    }
                    goto default;

                case Switch:
                    Switch? s = JsonSerializer.Deserialize<Switch>(message);
                    if (s != null)
                    {
                        s.Name = foundDevice.Name;
                        s.Id = foundDevice.Id;
                        return s;
                    }
                    goto default;

                case PIRSensorModel:
                    PIRSensorModel? p = JsonSerializer.Deserialize<PIRSensorModel>(message);
                    if (p != null)
                    {
                        p.Name = foundDevice.Name;
                        p.Id = foundDevice.Id;
                        return p;
                    }
                    goto default;

                case IkeaBulb:
                    IkeaBulb? bulb = JsonSerializer.Deserialize<IkeaBulb>(message);
                    if (bulb != null)
                    {
                        bulb.Name = foundDevice.Name;
                        bulb.Id = foundDevice.Id;
                        return bulb;
                    }
                    goto default;

                case LedPanel:
                    LedPanel? panel = JsonSerializer.Deserialize<LedPanel>(message);
                    if (panel != null)
                    {
                        panel.Name = foundDevice.Name;
                        panel.Id = foundDevice.Id;
                        return panel;
                    }
                    goto default;
                case IkeaTryk:
                    IkeaTryk? tryk = JsonSerializer.Deserialize<IkeaTryk>(message);
                    if (tryk != null)
                    {
                        tryk.Name = foundDevice.Name;
                        tryk.Id = foundDevice.Id;
                        return tryk;
                    }
                    goto default;

                case FugaTryk:
                    FugaTryk? fugaTryk = JsonSerializer.Deserialize<FugaTryk>(message);
                    if (fugaTryk != null)
                    {
                        fugaTryk.Name = foundDevice.Name;
                        fugaTryk.Id = foundDevice.Id;
                        return fugaTryk;
                    }
                    goto default;
                default:
                    return new UnKnownType();
            }
        }
    }
}
