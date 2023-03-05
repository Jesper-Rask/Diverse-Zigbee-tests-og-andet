


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
        public event EventHandler<PIRSensorModel> pirSensorEvent;
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

        public NewData()
        {
            StartZigbeeCommunication.AirSensorEvent += AirSensorEvent;
            StartZigbeeCommunication.PirSensorEvent += PirSensorEvent;
            timer.Elapsed += DelayedZigbeePublishMessage;
        }

        private void DelayedZigbeePublishMessage(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            if (lastPublishString.device != publishString.device || lastPublishString.message!=publishString.message)
            {
               StartZigbeeCommunication.client.Publish(publishString.device, publishString.message);
            }
        }

        private void PirSensorEvent(object sender, PIRSensorModel e)
        {
            pirSensorEvent?.Invoke(sender, e);
        }

        private void AirSensorEvent(object sender, TempAndHumiSensor e)
        {
            airSensorEvent?.Invoke(sender, e);
        }

    }
}
