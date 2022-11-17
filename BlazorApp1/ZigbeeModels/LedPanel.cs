using BlazorApp1.MqttComm;

namespace BlazorApp1.ZigbeeModels
{
    public class LedPanel:ZigbeeDevice
    {
        private int _brightness=0;
        private int _colorTemp=0;
        private bool _state = false;
        public event EventHandler? SomethingChanged;
        public int Brightness {
            get { return _brightness; }
            set { SetBrightness(value); SomethingChanged?.Invoke(this, EventArgs.Empty); }
        }
        public int ColorTemp
        {
            get { return _colorTemp; }
            set { _colorTemp = value; SetColorTemp(value); }
        }

        public bool State
        {
            get { return _state; }
            set
            {
                _state = value;
                Toggle();
            }
        }
        public ZigbeeMessage SetBrightness(int brightness)
        {
            return new ZigbeeMessage("zigbee2mqtt/" + Id, "/set{\"brightness\": \"" + brightness.ToString() + "\"}");
        }
        public void SetColorTemp(int colorTemp)
        {
          //  newData.ZigbeePublishMessage("zigbee2mqtt/" + Id + "/set", "{\"color_temp\": \"" + colorTemp.ToString() + "\"}");
    //        SomethingChanged?.Invoke(this, EventArgs.Empty);
        }
        public void TurnOn()
        {
        //    newData.ZigbeePublishMessage("zigbee2mqtt/" + Id + "/set", "{\"state\": \"ON\"}");
     //       SomethingChanged?.Invoke(this, EventArgs.Empty);
        }

        public void TurnOff()
        {
         //   newData.ZigbeePublishMessage("zigbee2mqtt/" + Id + "/set", "{\"state\": \"OFF\"}");
    //        SomethingChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Toggle()
        {
            if (!State)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }
        }
    }
}
