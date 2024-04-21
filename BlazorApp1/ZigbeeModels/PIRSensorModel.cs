namespace ZigbeeModels
{
    public class PIRSensorModel : BatteryPoweredDevice
    {

        public event EventHandler<int> MotionDetected;
        public event EventHandler<int> MotionNotDetected;

        private bool _occupancy;
        public bool occupancy
        {
            get { return _occupancy;  }
            set { _occupancy = value;

                if (value) MotionDetected?.Invoke(this, -1);
                else MotionNotDetected?.Invoke(this, -1); 
            }
        }
    }
}