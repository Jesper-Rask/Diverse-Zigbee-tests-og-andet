namespace ZigbeeModels
{
    public class IkeaBulb : ZigbeeDevice
    {
        private bool state = false;
        public void Send(Commands cmd, int value = 0)
        {
            switch (cmd)
            {
                case Commands.Turn_On: TurnOn(); break;
                case Commands.Turn_Off: TurnOff(); break;
                case Commands.Toggle: Toggle(); break;
                case Commands.SetBrightness: SetBrightness(Limit(1, value, 253)); break;
                case Commands.BrightnessMove: BrightnessMove(value); break;
                case Commands.SetColorTemp: SetColorTemp(value); break;
                case Commands.ColorTempMove: ColorTempMove(value); break;
                default: break;
            }
        }
        private void TurnOn()
        {
            StartZigbeeCommunication.client.Publish("zigbee2mqtt/" + Id + "/set", "{\"state\": \"ON\"}");
            state = true;
            //    SomethingChanged?.Invoke(this, EventArgs.Empty);
        }
        private void TurnOff()
        {
            StartZigbeeCommunication.client.Publish("zigbee2mqtt/" + Id + "/set", "{\"state\": \"OFF\"}");
            state = false;
            // SomethingChanged?.Invoke(this, EventArgs.Empty);
        }
        private void Toggle()
        {
            if (state) TurnOff(); else TurnOn();
        }

        private void SetBrightness(int brightness)
        {
            StartZigbeeCommunication.client.Publish("zigbee2mqtt/" + Id + "/set", "{\"brightness\": \"" + brightness.ToString() + "\"}");
        }
        public void BrightnessMove(int moveValue)
        {
            StartZigbeeCommunication.client.Publish("zigbee2mqtt/" + Id + "/set", "{\"brightness_move\": " + moveValue + "}");
        }
        private void SetColorTemp(int colorTemp)
        {
            StartZigbeeCommunication.client.Publish("zigbee2mqtt/" + Id + "/set", "{\"color_temp\": \"" + colorTemp.ToString() + "\"}");
        }
        private void ColorTempMove(int moveValue)
        {
            StartZigbeeCommunication.client.Publish("zigbee2mqtt/" + Id + "/set", "{\"color_temp_move\": " + moveValue + "}");
        }
    }
}
