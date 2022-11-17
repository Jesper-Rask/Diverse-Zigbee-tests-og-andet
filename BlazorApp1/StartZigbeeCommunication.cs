using BlazorApp1.Database;
using BlazorApp1.ZigbeeModels;
using Dapper;
using MySql.Data.MySqlClient;

namespace BlazorApp1
{
    public static class StartZigbeeCommunication
    {
        public static DBHandler db = new("192.168.1.4", "zigbeedb", "Jesper", "JRSQL");

        public static readonly MQTTClient client = new ();
        public static event EventHandler <TempAndHumiSensor>? AirSensorEvent;
        public static event EventHandler<PIRSensorModel>? PirSensorEvent;
        public static void StartZigbee(this WebApplication app)
        {
            
            client.MessageRecieved += ZigbeeDataRecieved;
        }
        private static void ZigbeeDataRecieved(object? sender, ZigbeeDevice zigbee)
        {
            if (zigbee == null) { return; }
            Console.WriteLine("Name: " + zigbee.Name);
            Console.WriteLine("ID: " + zigbee.Id);
            Console.WriteLine("Linkquality: " + zigbee.linkquality);

            zigbee.TimeStamp = DateTime.Now;

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
                    Data.sensorPirSoveværelse = pir;
                    PirSensorEvent?.Invoke(sender, pir);
                    PIRSensorChange(pir.occupancy);
                    break;
                case IkeaTryk:
                    IkeaTryk tryk = (IkeaTryk)zigbee;
                    Console.WriteLine("Action: " + tryk.action);
                    IKEATrykPressed(tryk.action);
                    break;

                case FugaTryk:
                    FugaTryk fugaTryk = (FugaTryk)zigbee;
                    Console.WriteLine("Action: " + fugaTryk.action);
                    FugaTrykPressed(fugaTryk.Name, fugaTryk.action);
                    break;

                case TempAndHumiSensor:
                    TempAndHumiSensor temp = (TempAndHumiSensor)zigbee;
                    db.InsertTempAndHumiSensor(temp);
                    switch (temp.Name)
                    {
                        case "Sensor Soveværelse": Data.sensorSoveVærelse = temp; break;
                        case "Sensor Stue": Data.sensorStue = temp; break;
                        case "Sensor Vaskerum": Data.sensorVaskerum = temp; break;
                        case "Sensor Julies Værelse": Data.sensorJuliesVærelse = temp; break;
                        case "Sensor Kælder Værksted": Data.sensorKælderVærksted = temp; break;
                        case "Sensor Køkken": Data.sensorKøkken = temp; break;
                        case "Sensor Kontor": Data.sensorKontor = temp; break;
                        case "Sensor Systue": Data.sensorSystue = temp; break;
                        case "Sensor Bad": Data.sensorBad = temp; break;
                        case "Sensor Kælder Bad": Data.sensorBadKælder = temp; break;
                    }


                    AirSensorEvent?.Invoke(sender, temp);
                    break;
                case IkeaBulb:
                    Data.pære = (IkeaBulb)zigbee;
                    break;

                case LedPanel:
                    LedPanel panel = (LedPanel)zigbee;

                    foreach (ZigbeeDevice zigbeeDevice in Data.zigbeeDevices)
                    {
                        if (zigbeeDevice.Id == panel.Id && zigbeeDevice is LedPanel)
                        {
                            LedPanel panel1 = (LedPanel)zigbeeDevice;
                            panel1.Brightness = panel.Brightness;
                            panel1.ColorTemp = panel.ColorTemp;
                            panel.State=panel.State;
                        }
                    }
                    break;

                }
            Console.WriteLine();
        }
        private static void PIRSensorChange(bool occupancy)
        {
            if (occupancy)
            {
                client.Publish("zigbee2mqtt/0x94deb8fffe575196/set", "{\"state\": \"ON\"}");
            }
            else
            {
                client.Publish("zigbee2mqtt/0x94deb8fffe575196/set", "{\"state\": \"OFF\"}");
            }
        }

        private static void IKEATrykPressed(string action)
        {
            switch (action)
            {
                case "on":
                    client.Publish("zigbee2mqtt/0x94deb8fffe575196/set", "{\"state\": \"ON\"}");
                    break;
                case "off":
                    client.Publish("zigbee2mqtt/0x94deb8fffe575196/set", "{\"state\": \"OFF\"}");
                    break;
                case "brightness_move_up":
                    client.Publish("zigbee2mqtt/0x94deb8fffe575196/set", "{\"brightness_move\": 70}");
                    break;
                case "brightness_move_down":
                    client.Publish("zigbee2mqtt/0x94deb8fffe575196/set", "{\"brightness_move\": -70}");
                    break;
                case "brightness_stop":
                    client.Publish("zigbee2mqtt/0x94deb8fffe575196/set", "{\"brightness_move\": 0}");
                    break;
                case "arrow_left_click":
                    client.Publish("zigbee2mqtt/0x00124b002343dff3/set", "{\"state\": \"ON\"}");
                    break;
                case "arrow_right_click":
                    client.Publish("zigbee2mqtt/0x00124b002343dff3/set", "{\"state\": \"OFF\"}");
                    break;
                default:
                    break;
            }
        }

        private static void FugaTrykPressed(string id, string action)
        {
            switch (action)
            {
                case "press_1":
                    client.Publish("zigbee2mqtt/0x086bd7fffe2b0647/set", "{\"state\": \"ON\"}");
                    client.Publish("zigbee2mqtt/0x84ba20fffe34518e/set", "{\"state\": \"ON\"}");
                    client.Publish("zigbee2mqtt/0x84ba20fffe344eea/set", "{\"state\": \"ON\"}");
                    client.Publish("zigbee2mqtt/0x84ba20fffe3451b0/set", "{\"state\": \"ON\"}");

                    break;
                case "release_1":
                    //client.Publish("zigbee2mqtt/0x94deb8fffe575196/set", "{\"state\": \"OFF\"}");
                    break;
                case "press_3":
                    client.Publish("zigbee2mqtt/0x086bd7fffe2b0647/set", "{\"state\": \"OFF\"}");
                    client.Publish("zigbee2mqtt/0x84ba20fffe34518e/set", "{\"state\": \"OFF\"}");
                    client.Publish("zigbee2mqtt/0x84ba20fffe344eea/set", "{\"state\": \"OFF\"}");
                    client.Publish("zigbee2mqtt/0x84ba20fffe3451b0/set", "{\"state\": \"OFF\"}");
                    break;
                case "release_3":
                    //client.Publish("zigbee2mqtt/0x94deb8fffe575196/set", "{\"brightness_move\": -70}");
                    break;
                case "press_2":
                    client.Publish("zigbee2mqtt/0x086bd7fffe2b0647/set", "{\"brightness_move\": 20}");
                    client.Publish("zigbee2mqtt/0x84ba20fffe34518e/set", "{\"brightness_move\": 20}");
                    client.Publish("zigbee2mqtt/0x84ba20fffe344eea/set", "{\"brightness_move\": 20}");
                    client.Publish("zigbee2mqtt/0x84ba20fffe3451b0/set", "{\"brightness_move\": 20}");

                    break;
                case "release_2":
                    client.Publish("zigbee2mqtt/0x086bd7fffe2b0647/set", "{\"brightness_move\": 0}");
                    client.Publish("zigbee2mqtt/0x84ba20fffe34518e/set", "{\"brightness_move\": 0}");
                    client.Publish("zigbee2mqtt/0x84ba20fffe344eea/set", "{\"brightness_move\": 0}");
                    client.Publish("zigbee2mqtt/0x84ba20fffe3451b0/set", "{\"brightness_move\": 0}");
                    //client.Publish("zigbee2mqtt/0x00124b002343dff3/set", "{\"state\": \"ON\"}");
                    break;
                case "press_4":
                    client.Publish("zigbee2mqtt/0x086bd7fffe2b0647/set", "{\"brightness_move\": -20}");
                    client.Publish("zigbee2mqtt/0x84ba20fffe34518e/set", "{\"brightness_move\": -20}");
                    client.Publish("zigbee2mqtt/0x84ba20fffe344eea/set", "{\"brightness_move\": -20}");
                    client.Publish("zigbee2mqtt/0x84ba20fffe3451b0/set", "{\"brightness_move\": -20}");
                    //client.Publish("zigbee2mqtt/0x00124b002343dff3/set", "{\"state\": \"OFF\"}");
                    break;
                case "release_4":
                    client.Publish("zigbee2mqtt/0x086bd7fffe2b0647/set", "{\"brightness_move\": 0}");
                    client.Publish("zigbee2mqtt/0x84ba20fffe34518e/set", "{\"brightness_move\": 0}");
                    client.Publish("zigbee2mqtt/0x84ba20fffe344eea/set", "{\"brightness_move\": 0}");
                    client.Publish("zigbee2mqtt/0x84ba20fffe3451b0/set", "{\"brightness_move\": 0}");

                    //client.Publish("zigbee2mqtt/0x086bd7fffe2b0647/set", "{\"state\": \"OFF\"}");
                    break;
                default:
                    break;
            }
        }
    }
}
