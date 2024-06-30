using BlazorApp1.ZigbeeModels;
using System.Text;
using System.Text.Json;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using static BlazorApp1.ZigbeeModels.FugaTryk;
using static ZigbeeModels.ZigbeeDevice;

namespace MqttComm
{
    public sealed class MQTTClient
    {
        MqttClient? mqttClient;
        public event EventHandler<ZigbeeDevice>? MessageRecieved;
        public MQTTClient()
        {
            ZigbeeDevicesToList();
            CreateConnectors();
            Connect(new MQTTConnectionModel());
        }

        private void CreateConnectors()
        {
            Data.connectors.Add(new Connector("Fuga Tryk Køkken", EventNames.Button1Pressed, "Lampe 1 Køkken", Commands.Turn_On, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Køkken", EventNames.Button1Pressed, "Lampe 2 Køkken", Commands.Turn_On, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Køkken", EventNames.Button1LongPressed, "Lampe 1 Køkken", Commands.BrightnessMove, 50));
            Data.connectors.Add(new Connector("Fuga Tryk Køkken", EventNames.Button1LongPressed, "Lampe 2 Køkken", Commands.BrightnessMove, 50));
            Data.connectors.Add(new Connector("Fuga Tryk Køkken", EventNames.Button1ReleasedAfterLongPress, "Lampe 1 Køkken", Commands.BrightnessMove, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Køkken", EventNames.Button1ReleasedAfterLongPress, "Lampe 2 Køkken", Commands.BrightnessMove, 0));

            Data.connectors.Add(new Connector("Fuga Tryk Køkken", EventNames.Button3LongPressed, "Lampe 1 Køkken", Commands.BrightnessMove, -50));
            Data.connectors.Add(new Connector("Fuga Tryk Køkken", EventNames.Button3LongPressed, "Lampe 2 Køkken", Commands.BrightnessMove, -50));
            Data.connectors.Add(new Connector("Fuga Tryk Køkken", EventNames.Button3ReleasedAfterLongPress, "Lampe 1 Køkken", Commands.BrightnessMove, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Køkken", EventNames.Button3ReleasedAfterLongPress, "Lampe 2 Køkken", Commands.BrightnessMove, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Køkken", EventNames.Button3Released, "Lampe 1 Køkken", Commands.Turn_Off, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Køkken", EventNames.Button3Released, "Lampe 2 Køkken", Commands.Turn_Off, 0));

            Data.connectors.Add(new Connector("Fuga Tryk Køkken", EventNames.Button2Pressed, "Lampe 3 Køkken", Commands.Turn_On, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Køkken", EventNames.Button2LongPressed, "Lampe 3 Køkken", Commands.BrightnessMove, 50));
            Data.connectors.Add(new Connector("Fuga Tryk Køkken", EventNames.Button2ReleasedAfterLongPress, "Lampe 3 Køkken", Commands.BrightnessMove, 0));

            Data.connectors.Add(new Connector("Fuga Tryk Køkken", EventNames.Button4LongPressed, "Lampe 3 Køkken", Commands.BrightnessMove, -50));
            Data.connectors.Add(new Connector("Fuga Tryk Køkken", EventNames.Button4ReleasedAfterLongPress, "Lampe 3 Køkken", Commands.BrightnessMove, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Køkken", EventNames.Button4Released, "Lampe 3 Køkken", Commands.Turn_Off, 0));

            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button1Pressed, "Lampe 1 Stue", Commands.Turn_On, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button1Pressed, "Lampe 2 Stue", Commands.Turn_On, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button1Pressed, "Lampe 3 Stue", Commands.Turn_On, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button1LongPressed, "Lampe 1 Stue", Commands.BrightnessMove, 50));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button1LongPressed, "Lampe 2 Stue", Commands.BrightnessMove, 50));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button1LongPressed, "Lampe 3 Stue", Commands.BrightnessMove, 50));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button1ReleasedAfterLongPress, "Lampe 1 Stue", Commands.BrightnessMove, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button1ReleasedAfterLongPress, "Lampe 2 Stue", Commands.BrightnessMove, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button1ReleasedAfterLongPress, "Lampe 3 Stue", Commands.BrightnessMove, 0));

            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button3LongPressed, "Lampe 1 Stue", Commands.BrightnessMove, -50));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button3LongPressed, "Lampe 2 Stue", Commands.BrightnessMove, -50));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button3LongPressed, "Lampe 3 Stue", Commands.BrightnessMove, -50));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button3ReleasedAfterLongPress, "Lampe 1 Stue", Commands.BrightnessMove, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button3ReleasedAfterLongPress, "Lampe 2 Stue", Commands.BrightnessMove, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button3ReleasedAfterLongPress, "Lampe 3 Stue", Commands.BrightnessMove, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button3Released, "Lampe 1 Stue", Commands.Turn_Off, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button3Released, "Lampe 2 Stue", Commands.Turn_Off, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button3Released, "Lampe 3 Stue", Commands.Turn_Off, 0));

            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button2Pressed, "Lampe 4 Stue", Commands.Turn_On, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button2Pressed, "Lampe 5 Stue", Commands.Turn_On, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button2LongPressed, "Lampe 4 Stue", Commands.BrightnessMove, 50));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button2LongPressed, "Lampe 5 Stue", Commands.BrightnessMove, 50));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button2ReleasedAfterLongPress, "Lampe 4 Stue", Commands.BrightnessMove, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button2ReleasedAfterLongPress, "Lampe 5 Stue", Commands.BrightnessMove, 0));

            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button4LongPressed, "Lampe 4 Stue", Commands.BrightnessMove, -50));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button4LongPressed, "Lampe 5 Stue", Commands.BrightnessMove, -50));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button4ReleasedAfterLongPress, "Lampe 4 Stue", Commands.BrightnessMove, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button4ReleasedAfterLongPress, "Lampe 5 Stue", Commands.BrightnessMove, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button4Released, "Lampe 4 Stue", Commands.Turn_Off, 0));
            Data.connectors.Add(new Connector("Fuga Tryk Stue", EventNames.Button4Released, "Lampe 5 Stue", Commands.Turn_Off, 0));

            Data.connectors.Add(new Connector("Sensor Kælder Bad", EventNames.HumidityAboveHigh, "Ventilator Kælder Bad", Commands.Turn_On, 0));
            Data.connectors.Add(new Connector("Sensor Kælder Bad", EventNames.HumidityBelowLow, "Ventilator Kælder Bad", Commands.Turn_Off, 0));

            Data.connectors.Add(new Connector("PIR Sensor Carport", EventNames.MotionDetected, "IKEA Pære Carport", Commands.Turn_On, 0));
            Data.connectors.Add(new Connector("PIR Sensor Carport", EventNames.MotionDetected, "Timer 1", Commands.StartTimer, 300));
            Data.connectors.Add(new Connector("Timer 1", EventNames.TimerElapsed, "IKEA Pære Carport", Commands.Turn_Off, 0));
        }

        private static void ZigbeeDevicesToList()
        {
            Data.zigbeeDevices.Add(new RadiatorValve { Name = "Radiatorventil Kontor", Id = "0x003c84fffecbb586" });
            Data.zigbeeDevices.Add(new RadiatorValve { Name = "Radiatorventil Systue", Id = "0x003c84fffecefe59" });
            Data.zigbeeDevices.Add(new RadiatorValve { Name = "Radiatorventil Kælder Entre", Id = "0x003c84fffecbb580" });
            Data.zigbeeDevices.Add(new RadiatorValve { Name = "Radiatorventil Kælder Vaskerum", Id = "0x94deb8fffe2e72f7" });
            Data.zigbeeDevices.Add(new RadiatorValve { Name = "Radiatorventil Kælder Bad", Id = "0x003c84fffec4f600" });
            Data.zigbeeDevices.Add(new RadiatorValve { Name = "Radiatorventil Kælder Julies Værelse", Id = "0x003c84fffec4f30d" });
            Data.zigbeeDevices.Add(new RadiatorValve { Name = "Radiatorventil Kælder Julies Tøjværelse", Id = "0x003c84fffec52cb2" });
            Data.zigbeeDevices.Add(new TempAndHumiSensor { Name = "Sensor Køkken", Id = "0x00124b0025034367" });
            Data.zigbeeDevices.Add(new TempAndHumiSensor { Name = "Sensor Kælder Værksted", Id = "0x00124b0025030228" });
            Data.zigbeeDevices.Add(new TempAndHumiSensor { Name = "Sensor Vaskerum", Id = "0x00124b00250338cf" });
            Data.zigbeeDevices.Add(new TempAndHumiSensor { Name = "Sensor Julies Værelse", Id = "0x00124b00251ef0ec" });
            Data.zigbeeDevices.Add(new TempAndHumiSensor { Name = "Sensor Soveværelse", Id = "0x00124b0025046109" });
            Data.zigbeeDevices.Add(new TempAndHumiSensor { Name = "Sensor Stue", Id = "0x00124b00250335b1" });
            Data.zigbeeDevices.Add(new TempAndHumiSensor { Name = "Sensor Kontor", Id = "0x00124b00251d4787"});
            Data.zigbeeDevices.Add(new TempAndHumiSensor { Name = "Sensor Systue", Id = "0x00124b00251515b1" });
            Data.zigbeeDevices.Add(new TempAndHumiSensor { Name = "Sensor Bad", Id = "0x00124b00251e034d" });
            Data.zigbeeDevices.Add(new TempAndHumiSensor { Name = "Sensor Kælder Bad", Id = "0x00124b0025212f83" });
            Data.zigbeeDevices.Add(new Switch { Name = "Relæ", Id = "0x00124b002343dff3" });
            Data.zigbeeDevices.Add(new PIRSensorModel { Name = "PIR Sensor Entre", Id = "0x00124b002508d2ce" });
            Data.zigbeeDevices.Add(new IkeaBulb { Name = "IKEA Pære Carport", Id = "0x94deb8fffe575196" });
            Data.zigbeeDevices.Add(new FugaTryk { Name = "Test Tryk", Id = "0x000000000171819b" });
            Data.zigbeeDevices.Add(new IkeaTryk { Name = "IKEA Tryk", Id = "0x540f57fffe3c6bee" });
            Data.zigbeeDevices.Add(new FugaTryk { Name = "Fuga Tryk Køkken", Id = "0x000000000171191b" });
            Data.zigbeeDevices.Add(new FugaTryk { Name = "Fuga Tryk Stue", Id = "0x000000000171838e" });
            Data.zigbeeDevices.Add(new IkeaBulb { Name = "IKEA Pære Stue", Id = "0x086bd7fffe2b0647" });
            Data.zigbeeDevices.Add(new LedPanel { Name = "Lampe 1 Køkken", Id = "0x84ba20fffe34518e" });
            Data.zigbeeDevices.Add(new LedPanel { Name = "Lampe 2 Køkken", Id = "0x84ba20fffe3451b0" });
            Data.zigbeeDevices.Add(new LedPanel { Name = "Lampe 3 Køkken", Id = "0x84ba20fffe344eea" });
            Data.zigbeeDevices.Add(new LedPanel { Name = "Lampe 1 Stue", Id = "0x84ba20fffe3450bf" });
            Data.zigbeeDevices.Add(new LedPanel { Name = "Lampe 2 Stue", Id = "0x84ba20fffe345d07" });
            Data.zigbeeDevices.Add(new LedPanel { Name = "Lampe 3 Stue", Id = "0x84ba20fffe345d90" });
            Data.zigbeeDevices.Add(new LedPanel { Name = "Lampe 4 Stue", Id = "0x84ba20fffe345d77" });
            Data.zigbeeDevices.Add(new LedPanel { Name = "Lampe 5 Stue", Id = "0x84ba20fffe344f74" });
            Data.zigbeeDevices.Add(new LedPanel { Name = "Lampe Entre",             Id = "0x84ba20fffe344f08" });
            Data.zigbeeDevices.Add(new Switch   { Name = "Ventilator Kælder Bad",  Id = "0x9035eafffeef70ca" });
            Data.zigbeeDevices.Add(new PIRSensorModel { Name = "PIR Sensor Carport", Id = "0x00124b002508d3b8" });
            Data.zigbeeDevices.Add(new TonTimer { Name = "Timer 1", Id = "" });
            Data.zigbeeDevices.Add(new TonTimer { Name = "Timer 2", Id = "" });
            Data.zigbeeDevices.Add(new TonTimer { Name = "Timer 3", Id = "" });
            Data.zigbeeDevices.Add(new TonTimer { Name = "Timer 4", Id = "" });
            Data.zigbeeDevices.Add(new TonTimer { Name = "Timer 5", Id = "" });
            Data.zigbeeDevices.Add(new TonTimer { Name = "Timer 6", Id = "" });

            WeeklySchedule schedule = new();

            TimeOnly time1 = new TimeOnly(15, 21);
            TimeOnly time2 = new TimeOnly(15, 22);
            TimeOnly time3 = new TimeOnly(15, 23);
            TimeOnly time4 = new TimeOnly(15, 24);

            schedule.AddScheduleItem(DayOfWeek.Sunday, time1, true, 1);
            schedule.AddScheduleItem(DayOfWeek.Sunday, time2, true, 250);
            schedule.AddScheduleItem(DayOfWeek.Sunday, time3, true, 1);
            schedule.AddScheduleItem(DayOfWeek.Sunday, time4, false, 1);
            schedule.Name = "Ugeskema 1";
            schedule.Id = "";


            Data.zigbeeDevices.Add(schedule);

        }
        public void Publish(string topic, string message)
        {
            Console.WriteLine(topic + message);
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
                mqttClient.MqttMsgPublished += MqttMsgPublished;
                byte[] QOS = new byte[Data.zigbeeDevices.Count()];
                Array.Fill(QOS, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE);



                mqttClient.Subscribe(Data.zigbeeDevices.Select(x => "zigbee2mqtt/" + x.Id).ToArray(), QOS);
                mqttClient.Subscribe(new string[] {
                    "zigbee2mqtt/bridge/config/devices",
                    "zigbee2mqtt/bridge/devices"
                }, new byte[] { 0, 0});

                mqttClient.Connect(model.Client);
            });
        }

        private void MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
           
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

                foreach (BridgeDevice device in Data.bridgeDevices)
                {
                    foreach (ZigbeeDevice dev in Data.zigbeeDevices)
                    {
                        if (device.ieee_address == dev.Id)
                        {
                            device.friendly_name = dev.Name;
                            break;
                        }
                    }
                }
            }

            foreach (ZigbeeDevice device in Data.zigbeeDevices)
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

                case RadiatorValve:
                    RadiatorValve? radiatorValve = JsonSerializer.Deserialize<RadiatorValve>(message);
                    if (radiatorValve != null)
                    {
                        radiatorValve.Name = foundDevice.Name;
                        radiatorValve.Id = foundDevice.Id;
                        return radiatorValve;
                    }
                    goto default;
                default:
                    return new UnKnownType();
            }
        }
    }
}
