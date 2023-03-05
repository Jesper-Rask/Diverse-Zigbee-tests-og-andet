using static ZigbeeModels.ZigbeeDevice;

namespace BlazorApp1.ZigbeeModels
{
    public class TonTimer : ZigbeeDevice
    {
        public event EventHandler TimerElapsed;
        private System.Timers.Timer timer = new();
        public void Send(Commands cmd, int value = 0)
        {
            switch (cmd)
            {
                case Commands.StartTimer: StartTimer(value); break;
                case Commands.StopTimer: StopTimer(); break;
                default: break;
            }
        }


        private void StopTimer()
        {
            timer.Stop();
        }

        private void StartTimer(int seconds)
        {
            timer.Interval=seconds*1000;
            timer.AutoReset = false;
         
            timer.Start();
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            TimerElapsed?.Invoke(sender, e);
        }
    }
}
