namespace BlazorApp1.ZigbeeModels
{
    public class RadiatorValve : BatteryPoweredDevice
    {
        public event EventHandler<float> SetpointChanged;
        public string child_lock { get; set; }
        public string window_detection { get; set; }
        public string valve_detection { get; set; }
        public float position { get; set; }
        public float current_heating_setpoint
        {
            get { return _heating_setpoint; }
            set { _heating_setpoint = value;}
        }
        private float _heating_setpoint;
        public float RadiatorSP
        {
            get { return _heating_setpoint; }
            set { SetTemperatureSetpoint((int)value); _heating_setpoint = value; }
        }

        public float local_temperature { get; set; }
        public string system_mode { get; set; }
        public void Send(Commands cmd, int value)
        {
            switch (cmd)
            {
                case Commands.SetSetpoint: SetTemperatureSetpoint(Limit(5, value, 30)); break;
            }
        }

        public void SetTemperatureSetpoint(int setpoint)
        {
            StartZigbeeCommunication.client.Publish("zigbee2mqtt/" + Id + "/set", "{\"current_heating_setpoint\":  \"" + setpoint.ToString() + "\"}");
        }

    }
}
