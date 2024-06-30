namespace ZigbeeModels
{
    public class IkeaBulb : ZigbeeDevice
    {



        public int brightness { get; set; }

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

        public int Brightness
        {
            get { return brightness; }
            set
            {
                brightness = value;
                SetBrightness(value);
            }
        }


        #region On/Off

        private bool booleanState = false;
        public string state { get; set; }               
        public bool State
        {
            get { return state == "ON"? true : false; }
            set
            {
                if (value != booleanState)
                {
                    booleanState = value;
                    if (booleanState) TurnOn();
                    else TurnOff();
                }
            }
        }
        private void TurnOn()
        {
            StartZigbeeCommunication.client.Publish("zigbee2mqtt/" + Id + "/set", "{\"state\": \"ON\"}");
            booleanState = true;
            state = "ON";
        }
        private void TurnOff()
        {
            StartZigbeeCommunication.client.Publish("zigbee2mqtt/" + Id + "/set", "{\"state\": \"OFF\"}");
            booleanState = false;
            state = "OFF";
        }
        private void Toggle()
        {
            if (booleanState) TurnOff(); else TurnOn();
        }
        #endregion

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
