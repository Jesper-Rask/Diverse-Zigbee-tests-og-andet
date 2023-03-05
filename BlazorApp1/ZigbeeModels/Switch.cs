namespace ZigbeeModels
{
    public class Switch : ZigbeeDevice
    {
        public string state { get; set; } = "";

        public void Send(Commands cmd, int value = 0)
        {
            switch (cmd)
            {
                case Commands.Turn_On: TurnOn(); break;
                case Commands.Turn_Off: TurnOff(); break;
                default: break;
            }
        }

        private void TurnOn()
        {
            StartZigbeeCommunication.client.Publish("zigbee2mqtt/" + Id + "/set", "{\"state\": \"ON\"}");
        }
        private void TurnOff()
        {
            StartZigbeeCommunication.client.Publish("zigbee2mqtt/" + Id + "/set", "{\"state\": \"OFF\"}");
        }
    }
}
