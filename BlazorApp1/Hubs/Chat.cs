using BlazorApp1.ZigbeeModels;
using Microsoft.AspNetCore.SignalR;
using System.Timers;

namespace BlazorApp1.Hubs
{
    public class Chat : Hub
    {
        private System.Timers.Timer timer = new(200);
        private int number;
        private int lastNumber;
        public Chat()
        {
            timer.Elapsed += DelayedMessage;
        }

        private void DelayedMessage(object? sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            if (number!=lastNumber)
            {
                Clients.All.SendAsync("RecieveInt", number);
            }
        }

        public async Task SendAirSensorMessage(TempAndHumiSensor airSensor)
        {
            await Clients.All.SendAsync("RecieveAirSensor", airSensor);
        }
        public async Task SendPirSensorMessage(PIRSensorModel pirSensor)
        {
            await Clients.All.SendAsync("RecievePirSensor", pirSensor);
        }

        public async Task SendRadiatorValveMessage(RadiatorValve valve)
        {
            await Clients.All.SendAsync("ReceiveRadiatorValve", valve);
        }
        public async Task SendIntMessage(int number)
        {
            this.number = number;
            if (!timer.Enabled)
            {
                lastNumber = number;
                await Clients.All.SendAsync("RecieveInt", number);
                timer.Enabled = true;
                timer.Stop();
                timer.Start();
            }

        }

        public async Task SendLedPanelMessage(LedPanel zigbee)
        {
            await Clients.All.SendAsync("ReceiveLedPanel", zigbee);
        }

        public async Task NewVisitor(int visitorsCount)
        {
            await Clients.All.SendAsync("UpdateVisitorsCount", visitorsCount);
        }

    }
}

