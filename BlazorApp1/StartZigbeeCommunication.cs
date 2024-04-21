using BlazorApp1.Database;
using BlazorApp1.ZigbeeModels;
using Dapper;
using MySql.Data.MySqlClient;

namespace BlazorApp1
{
    public static class StartZigbeeCommunication
    {
        public static DBHandler db = new("192.168.1.15", "zigbeedb", "JRA", "JRSQL");

        public static readonly MQTTClient client = new ();
        public static event EventHandler <TempAndHumiSensor>? AirSensorEvent;
        public static event EventHandler<PIRSensorModel>? PirSensorEvent;
        public static event EventHandler<RadiatorValve>? RadiatorValveEvent;
        public static event EventHandler<LedPanel>? LedPanelEvent;

        public static void StartZigbee(this WebApplication app)
        {           
            client.MessageRecieved += ZigbeeDataRecieved;
      //      StartZigbeeCommunication.client.Publish("zigbee2mqtt/bridge/config/devices", "/get");
        }
        private static void ZigbeeDataRecieved(object? sender, ZigbeeDevice zigbee)
        {
            if (zigbee == null) { return; }
            Console.WriteLine("Name: " + zigbee.Name);
            Console.WriteLine("ID: " + zigbee.Id);
            Console.WriteLine("Linkquality: " + zigbee.linkquality);

            zigbee.TimeStamp = DateTime.Now;

            if (zigbee is BridgeDevice) { }



            if (zigbee is BatteryPoweredDevice)
            {
                BatteryPoweredDevice battery = (BatteryPoweredDevice)zigbee;
                Console.WriteLine("Voltage: " + battery.voltage);
                Console.WriteLine("Battery %:" + battery.battery);
            }

                        switch (zigbee)
            {
                case Switch:
                    Switch sw = (Switch)zigbee;
                    Console.WriteLine("State: " + sw.state);
                    break;
                case PIRSensorModel:
                    PIRSensorModel pir = (PIRSensorModel)zigbee;
                    PIRSensorModel pirInList = (PIRSensorModel)Data.zigbeeDevices.Find(x => x.Name == pir.Name);
                    pirInList.occupancy = pir.occupancy;
                    pirInList.TimeStamp = DateTime.Now;
                    pirInList.battery = pir.battery;
                    pirInList.linkquality = pir.linkquality;
                    pirInList.voltage = pir.voltage;    
                    Console.WriteLine(pir.Name);
                    break;
                case IkeaTryk:
                    IkeaTryk tryk = (IkeaTryk)zigbee;
                    Console.WriteLine("Action: " + tryk.action);
                    break;

                case FugaTryk:
                    FugaTryk fugaTryk = (FugaTryk)zigbee;
                    FugaTryk fugaTrykInList = (FugaTryk)Data.zigbeeDevices.Find(x => x.Name == fugaTryk.Name);
                    fugaTrykInList.action = fugaTryk.action;
                    fugaTrykInList.battery = fugaTryk.battery;
                    fugaTrykInList.linkquality= fugaTryk.linkquality;
                    fugaTrykInList.voltage = fugaTryk.voltage;
                    Console.WriteLine("Action: " + fugaTryk.action);
                    fugaTrykInList.FugaTrykPressed();
                    break;

                case TempAndHumiSensor:
                    TempAndHumiSensor temp = (TempAndHumiSensor)zigbee;
                    db.InsertTempAndHumiSensor(temp);
                    TempAndHumiSensor tempAndHumiSensorInList = (TempAndHumiSensor)Data.zigbeeDevices.Find(x => x.Name == temp.Name);
                    tempAndHumiSensorInList.linkquality = temp.linkquality;
                    tempAndHumiSensorInList.temperature = temp.temperature;
                    tempAndHumiSensorInList.voltage = temp.voltage;
                    tempAndHumiSensorInList.battery = temp.battery;
                    tempAndHumiSensorInList.humidity = temp.humidity;
                    tempAndHumiSensorInList.TimeStamp=DateTime.Now;

                    AirSensorEvent?.Invoke(sender, tempAndHumiSensorInList);

                    break;
                case IkeaBulb:
               //     Data.pære = (IkeaBulb)zigbee;
                    break;

                case LedPanel:
                    LedPanel panel = (LedPanel)zigbee;

                    foreach (ZigbeeDevice zigbeeDevice in Data.zigbeeDevices)
                    {
                        if (zigbeeDevice.Id == panel.Id && zigbeeDevice is LedPanel)
                        {
                            LedPanel panel1 = (LedPanel)zigbeeDevice;
                            panel1.TimeStamp = panel.TimeStamp;
                            panel1.color_temp = panel.color_temp;
                            panel1.brightness = panel.brightness;
                            panel1.state = panel.state;
                            LedPanelEvent?.Invoke(sender, panel1);
                        }
                    }
                    break;
                case RadiatorValve:
                    RadiatorValve radiatorValve = (RadiatorValve)zigbee;
                    foreach (ZigbeeDevice zigbeeDevice in Data.zigbeeDevices)
                    {
                        if (zigbeeDevice.Id == radiatorValve.Id && zigbeeDevice is RadiatorValve)
                        {
                            RadiatorValve valve = (RadiatorValve)zigbeeDevice;
                            valve.TimeStamp = radiatorValve.TimeStamp;
                            valve.battery = radiatorValve.battery;
                            valve.child_lock = radiatorValve.child_lock;
                            valve.current_heating_setpoint = radiatorValve.current_heating_setpoint;
                            valve.linkquality = radiatorValve.linkquality;
                            valve.local_temperature = radiatorValve.local_temperature;
                            valve.position = radiatorValve.position;
                            valve.voltage = radiatorValve.voltage;
                            valve.window_detection = radiatorValve.window_detection;
                            valve.system_mode = radiatorValve.system_mode;
                            RadiatorValveEvent?.Invoke(sender, valve);
                        }
                    }
                    break;

            }
            Console.WriteLine();
        }
        //private static void PIRSensorChange(bool occupancy)
        //{
        //    //if (occupancy)
        //    //{
        //    //    client.Publish("zigbee2mqtt/0x94deb8fffe575196/set", "{\"state\": \"ON\"}");
        //    //}
        //    //else
        //    //{
        //    //    client.Publish("zigbee2mqtt/0x94deb8fffe575196/set", "{\"state\": \"OFF\"}");
        //    //}
        //}

        //private static void IKEATrykPressed(string action)
        //{
        //    switch (action)
        //    {
        //        case "on":
        //            client.Publish("zigbee2mqtt/0x94deb8fffe575196/set", "{\"state\": \"ON\"}");
        //            break;
        //        case "off":
        //            client.Publish("zigbee2mqtt/0x94deb8fffe575196/set", "{\"state\": \"OFF\"}");
        //            break;
        //        case "brightness_move_up":
        //            client.Publish("zigbee2mqtt/0x94deb8fffe575196/set", "{\"brightness_move\": 70}");
        //            break;
        //        case "brightness_move_down":
        //            client.Publish("zigbee2mqtt/0x94deb8fffe575196/set", "{\"brightness_move\": -70}");
        //            break;
        //        case "brightness_stop":
        //            client.Publish("zigbee2mqtt/0x94deb8fffe575196/set", "{\"brightness_move\": 0}");
        //            break;
        //        case "arrow_left_click":
        //            client.Publish("zigbee2mqtt/0x00124b002343dff3/set", "{\"state\": \"ON\"}");
        //            break;
        //        case "arrow_right_click":
        //            client.Publish("zigbee2mqtt/0x00124b002343dff3/set", "{\"state\": \"OFF\"}");
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
}
