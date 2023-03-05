namespace BlazorApp1.ZigbeeModels
{
    public class FugaTryk : BatteryPoweredDevice
    {
        public event EventHandler Button1Pressed;
        public event EventHandler Button2Pressed;
        public event EventHandler Button3Pressed;
        public event EventHandler Button4Pressed;
        public event EventHandler Button1LongPressed;
        public event EventHandler Button2LongPressed;
        public event EventHandler Button3LongPressed;
        public event EventHandler Button4LongPressed;
        public event EventHandler Button1Released;
        public event EventHandler Button2Released;
        public event EventHandler Button3Released;
        public event EventHandler Button4Released;
        public event EventHandler Button1ReleasedAfterLongPress;
        public event EventHandler Button2ReleasedAfterLongPress;
        public event EventHandler Button3ReleasedAfterLongPress;
        public event EventHandler Button4ReleasedAfterLongPress;

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
                    if (!longPress) Button1Pressed?.Invoke(this, null);
                    else Button1LongPressed?.Invoke(this, null);
                    break;

                case "release_1":
                    if (!longPress) Button1Released?.Invoke(this, null);
                    else Button1ReleasedAfterLongPress?.Invoke(this, null);
                    break;

                case "press_3":
                    if (!longPress) Button3Pressed?.Invoke(this, null);
                    else Button3LongPressed?.Invoke(this, null);
                    break;

                case "release_3":
                    if (!longPress) Button3Released?.Invoke(this, null);
                    else Button3ReleasedAfterLongPress?.Invoke(this, null);
                    break;
  
                case "press_2":
                    if (!longPress) Button2Pressed?.Invoke(this, null);
                    else Button2LongPressed?.Invoke(this, null);
                    break;
                case "release_2":
                    if (!longPress) Button2Released?.Invoke(this, null);
                    else Button2ReleasedAfterLongPress?.Invoke(this, null);
                    break;
                case "press_4":
                    if (!longPress) Button4Pressed?.Invoke(this, null);
                    else Button4LongPressed?.Invoke(this, null);
                    break;
                case "release_4":
                    if (!longPress) Button4Released?.Invoke(this, null);
                    else Button4ReleasedAfterLongPress?.Invoke(this, null);
                    break;
                default:
                    break;
            }
        }
    }
}

