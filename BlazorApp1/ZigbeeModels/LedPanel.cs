using BlazorApp1.MqttComm;
using Microsoft.AspNetCore.Components;

namespace BlazorApp1.ZigbeeModels
{
    public class LedPanel:ZigbeeDevice
    {
        private int _brightness;
        public int brightness { get { return _brightness; } set { _brightness= value; _brightnessProp = value; Brightness = value; } }
        public string color_mode { get; set; }
        private int _color_temp;
        public int color_temp { get { return _color_temp; } set { _color_temp=value; _colorTemp = value; ColorTemp = value; } }
        public int linkquality { get; set; }
        private string _stateString;
        public string state
        {
            get { return _stateString; }
            set {
                _stateString = value;
                if (value == "ON") ;// _state = true; State = true;
                if (value == "OFF") ;// _state = false; State = false;
            }
        }
        private int _brightnessProp;
        private int _colorTemp;
        private bool _state;
        public event EventHandler? SomethingChanged;
        public int Brightness {
            get { return _brightnessProp; }
            set {
                if (value != _brightnessProp)
                {
                    _brightnessProp = value; SetBrightness(_brightnessProp);
                }
            }
        }

        public int ColorTemp
        {
            get { return _colorTemp; }
            set
            {
                if (value != _colorTemp)
                {
                    _colorTemp = value;
                    Console.WriteLine(_colorTemp);
                    SetColorTemp(_colorTemp);
                }
            }
        }
        public bool State
        {
            get { return _state; }
            set
            {
                if (value != _state)
                {
                    _state = value;
                    if (_state) { TurnOn(); } else { TurnOff(); }
                
                }
            }
        }
        public void Send(Commands cmd, int value = 0)
        {
            switch (cmd)
            {
                case Commands.Turn_On: TurnOn(); break;
                case Commands.Turn_Off: TurnOff(); break;
                case Commands.Toggle: State =! State; break;
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
            //    SomethingChanged?.Invoke(this, EventArgs.Empty);
        }
        private void TurnOff()
        {
            StartZigbeeCommunication.client.Publish("zigbee2mqtt/" + Id + "/set", "{\"state\": \"OFF\"}");
            // SomethingChanged?.Invoke(this, EventArgs.Empty);
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
