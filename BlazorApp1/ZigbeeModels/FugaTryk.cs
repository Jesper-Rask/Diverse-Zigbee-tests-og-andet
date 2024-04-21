namespace BlazorApp1.ZigbeeModels
{
    public class FugaTryk : BatteryPoweredDevice
    {
        public event EventHandler<int> Button1Pressed;
        public event EventHandler<int> Button2Pressed;
        public event EventHandler<int> Button3Pressed;
        public event EventHandler<int> Button4Pressed;
        public event EventHandler<int> Button1LongPressed;
        public event EventHandler<int> Button2LongPressed;
        public event EventHandler<int> Button3LongPressed;
        public event EventHandler<int> Button4LongPressed;
        public event EventHandler<int> Button1Released;
        public event EventHandler<int> Button2Released;
        public event EventHandler<int> Button3Released;
        public event EventHandler<int> Button4Released;
        public event EventHandler<int> Button1ReleasedAfterLongPress;
        public event EventHandler<int> Button2ReleasedAfterLongPress;
        public event EventHandler<int> Button3ReleasedAfterLongPress;
        public event EventHandler<int> Button4ReleasedAfterLongPress;

        private System.Timers.Timer timer = new(500);
        private bool longPress=false;
        public string action { get; set; } = "";
        private int command;
 
        public struct Output
        {
            public string OutputName;
            public List<NameAndCommand> Out;
        }

        public List<Output> Bindings;
 
        public FugaTryk()
        {
            timer.AutoReset = false;
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            longPress = true;         
            FugaTrykPressed();
        }

        public void FugaTrykPressed()
        {
            if (action.Contains("press"))
            {
                    timer.Enabled = true;
                    timer.Start();
            }

            FugaTrykPublish();
            
            if (action.Contains("release"))
             {
                timer.Stop();
                timer.Enabled = false;
                longPress = false;
             }
        }

        private void FugaTrykPublish()
        {
            switch (action)
            {
                case "press_1":
                    if (!longPress) Button1Pressed?.Invoke(this, -1);
                    else Button1LongPressed?.Invoke(this, -1);
                    break;

                case "release_1":
                    if (!longPress) Button1Released?.Invoke(this, -1);
                    else Button1ReleasedAfterLongPress?.Invoke(this, -1);
                    break;

                case "press_3":
                    if (!longPress) Button3Pressed?.Invoke(this, -1);
                    else Button3LongPressed?.Invoke(this, -1);
                    break;

                case "release_3":
                    if (!longPress) Button3Released?.Invoke(this, -1);
                    else Button3ReleasedAfterLongPress?.Invoke(this, -1);
                    break;
  
                case "press_2":
                    if (!longPress) Button2Pressed?.Invoke(this, -1);
                    else Button2LongPressed?.Invoke(this, -1);
                    break;
                case "release_2":
                    if (!longPress) Button2Released?.Invoke(this, -1);
                    else Button2ReleasedAfterLongPress?.Invoke(this, -1);
                    break;
                case "press_4":
                    if (!longPress) Button4Pressed?.Invoke(this, -1);
                    else Button4LongPressed?.Invoke(this, -1);
                    break;
                case "release_4":
                    if (!longPress) Button4Released?.Invoke(this, -1);
                    else Button4ReleasedAfterLongPress?.Invoke(this, -1);
                    break;
                default:
                    break;
            }
        }
    }
}

