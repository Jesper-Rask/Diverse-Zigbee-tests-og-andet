namespace ZigbeeModels
{
    public class ZigbeeDevice
    {
        public virtual string Name { get; set; } = "";
        public string Id { get; set; } = "";
        public double linkquality { get; set; }
        public DateTime TimeStamp { get; set; } = new DateTime();

        public Commands Cmd { get; set; }
        public Commands EventName { get; set; }


        public enum Commands
        {
            Turn_On,
            Turn_Off,
            Toggle,
            SetBrightness,
            BrightnessMove,
            SetColorTemp,
            ColorTempMove,
            StartTimer,
            StopTimer,
            SetSetpoint
        }

        public enum EventNames
        {
            Button1Pressed,
            Button2Pressed,
            Button3Pressed,
            Button4Pressed,
            Button1LongPressed,
            Button2LongPressed,
            Button3LongPressed,
            Button4LongPressed,
            Button1Released,
            Button2Released,
            Button3Released,
            Button4Released,
            Button1ReleasedAfterLongPress,
            Button2ReleasedAfterLongPress,
            Button3ReleasedAfterLongPress,
            Button4ReleasedAfterLongPress,
            TimerElapsed,
            MotionDetected,
            MotionNotDetected,
            TemperatureChanged,
            TemperatureAboveHigh,
            TemperatureBelowHigh,
            TemperatureAboveLow,
            TemperatureBelowLow,
            HumidityChanged,
            HumidityAboveHigh,
            HumidityBelowHigh,
            HumidityAboveLow,
            HumidityBelowLow,
            WeeklyScheduleOn,
            WeeklyScheduleOff,
            SetpointChanged
        }


        public int Limit(int min, int value, int max)
        {
            return (int)Math.Max(min, Math.Min(max, value));
        }
    }
}