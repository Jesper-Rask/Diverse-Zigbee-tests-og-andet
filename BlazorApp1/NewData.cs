


using BlazorApp1.ZigbeeModels;
using System.Timers;

namespace BlazorApp1
{
    public class NewData
    {
        private struct PublishString
        {
            public string device { get; set; }
            public string message { get; set; }
        }
        PublishString publishString = new();
        PublishString lastPublishString=new();
        public event EventHandler<TempAndHumiSensor> airSensorEvent;
        public event EventHandler<IkeaBulb> ikeaBulbEvent;
        public event EventHandler<PIRSensorModel> pirSensorEvent;
        public event EventHandler<RadiatorValve> radiatorValveEvent;
        public event EventHandler<LedPanel> ledPanelEvent;

        private System.Timers.Timer timer = new(200);

        public void ZigbeePublishMessage (string device, string message)
        {
            publishString.device = device;
            publishString.message = message;
            if (!timer.Enabled)
            {
                lastPublishString=publishString;
                StartZigbeeCommunication.client.Publish(device, message);
                timer.Enabled = true;
                timer.Stop();
                timer.Start();
            }
        }

        public static T Limit<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0) return min;
            if (value.CompareTo(max) > 0) return max;
            return value;
        }

        public NewData()
        {
            StartZigbeeCommunication.AirSensorEvent += AirSensorEvent;
            StartZigbeeCommunication.IkeaBulbEvent += IkeaBulbEvent;
            StartZigbeeCommunication.PirSensorEvent += PirSensorEvent;
            StartZigbeeCommunication.RadiatorValveEvent += RadiatorValveEvent;
            StartZigbeeCommunication.LedPanelEvent += LedPanelEvent;

            timer.Elapsed += DelayedZigbeePublishMessage;
        }

        private void RadiatorValveEvent(object? sender, RadiatorValve e)
        {
            radiatorValveEvent.Invoke(sender, e);
        }

        private void DelayedZigbeePublishMessage(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            if (lastPublishString.device != publishString.device || lastPublishString.message!=publishString.message)
            {
               StartZigbeeCommunication.client.Publish(publishString.device, publishString.message);
            }
        }

        private void LedPanelEvent(object sender, LedPanel e)
        {
            ledPanelEvent?.Invoke(sender, e);
        }

        private void PirSensorEvent(object sender, PIRSensorModel e)
        {
            pirSensorEvent?.Invoke(sender, e);
        }

        private void AirSensorEvent(object sender, TempAndHumiSensor e)
        {
            airSensorEvent?.Invoke(sender, e);
        }

        private void IkeaBulbEvent(object sender, IkeaBulb e)
        {
            ikeaBulbEvent?.Invoke(sender, e);
        }

    }
}
