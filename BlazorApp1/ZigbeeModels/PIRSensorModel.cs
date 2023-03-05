namespace ZigbeeModels
{
    public class PIRSensorModel : BatteryPoweredDevice
    {

        public event EventHandler MotionDetected;
        public event EventHandler MotionNotDetected;

        private bool _occupancy;
        public bool occupancy
        {
            get { return _occupancy;  }
            set { _occupancy = value;

                if (value) MotionDetected?.Invoke(this, EventArgs.Empty);
                else MotionNotDetected?.Invoke(this, EventArgs.Empty); 
            }
        }
    }
}