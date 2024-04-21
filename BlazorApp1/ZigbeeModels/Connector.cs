using static ZigbeeModels.ZigbeeDevice;

namespace BlazorApp1.ZigbeeModels
{
    public class Connector
    {
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public EventNames EventName { get; set; }
        public Commands Command { get; set; }
        public int Value { get; set; }

        private ZigbeeDevice? senderDevice;
        private ZigbeeDevice? receiverDevice;
        public Connector(string senderName, EventNames eventName, string receiverName, Commands command, int value)
        {
            SenderName = senderName;
            ReceiverName = receiverName;
            EventName = eventName;
            Command = command;
            Value = value;

            senderDevice = FindDeviceFromName(senderName);
            if (senderDevice == null) return;
            receiverDevice = FindDeviceFromName(receiverName);
            if (receiverDevice == null) return;

            switch (senderDevice)
            {
                case FugaTryk: AddFugaTrykSubscribtion((FugaTryk)senderDevice); break;
                case TonTimer: AddTonTimerSubscribtion((TonTimer)senderDevice); break;
                case PIRSensorModel: AddPirSensorSubscribtion((PIRSensorModel)senderDevice); break;
                case TempAndHumiSensor: AddTempAndHumiSensorSubscribtion((TempAndHumiSensor)senderDevice); break;
                case WeeklySchedule: AddWeeklyScheduleSubscription((WeeklySchedule)senderDevice); break;
            }
        }

        private void AddWeeklyScheduleSubscription(WeeklySchedule senderDevice)
        {
            switch (EventName)
            {
                case EventNames.WeeklyScheduleOn: senderDevice.CalenderActive += ButtonPressed; break;
                case EventNames.WeeklyScheduleOff: senderDevice.CalenderInactive += ButtonPressed; break;
            }
        }

        private void AddTempAndHumiSensorSubscribtion(TempAndHumiSensor senderDevice)
        {
            switch (EventName)
            {
                case EventNames.HumidityAboveHigh: senderDevice.HumidityAboveHigh += ButtonPressed; break;
                case EventNames.HumidityBelowHigh: senderDevice.HumidityBelowHigh += ButtonPressed; break;
                case EventNames.HumidityAboveLow: senderDevice.HumidityAboveLow += ButtonPressed; break;
                case EventNames.HumidityBelowLow: senderDevice.HumidityBelowLow += ButtonPressed; break;
                case EventNames.HumidityChanged: senderDevice.HumidityChanged += ButtonPressed; break;
                case EventNames.TemperatureAboveHigh: senderDevice.TemperatureAboveHigh += ButtonPressed; break;
                case EventNames.TemperatureBelowHigh: senderDevice.TemperatureBelowHigh += ButtonPressed; break;
                case EventNames.TemperatureAboveLow: senderDevice.TemperatureAboveLow += ButtonPressed; break;
                case EventNames.TemperatureBelowLow: senderDevice.TemperatureBelowLow += ButtonPressed; break;
                case EventNames.TemperatureChanged: senderDevice.TemperatureChanged += ButtonPressed; break;
            }
        }

        private void AddPirSensorSubscribtion(PIRSensorModel senderDevice)
        {
            switch (EventName)
            {
                case EventNames.MotionDetected: senderDevice.MotionDetected+= ButtonPressed; break;
                case EventNames.MotionNotDetected: senderDevice.MotionNotDetected += ButtonPressed; break;
            }
        }

        public void RemoveConnection()
        {
            switch (senderDevice)
            {
                default: break;
                case FugaTryk: RemoveFugaTrykSubscribtion((FugaTryk)senderDevice); break;
                case TonTimer: RemoveTonTimerSubscribtion((TonTimer)senderDevice); break;
                case PIRSensorModel: RemovePirSensorSubscribtion((PIRSensorModel)senderDevice); break;
                case TempAndHumiSensor: RemoveTempAndHumiSensorSubscription((TempAndHumiSensor)senderDevice); break;
                case WeeklySchedule: RemoveWeeklyScheduleSubscription((WeeklySchedule)senderDevice); break;

            }
            Data.connectors.Remove(this);
        }

        private void RemoveWeeklyScheduleSubscription(WeeklySchedule senderDevice)
        {
            switch (EventName)
            {
                case EventNames.WeeklyScheduleOn: senderDevice.CalenderActive -= ButtonPressed; break;
                case EventNames.WeeklyScheduleOff: senderDevice.CalenderInactive -= ButtonPressed; break;
            }
        }

        private void RemoveTempAndHumiSensorSubscription(TempAndHumiSensor senderDevice)
        {
               switch (EventName)
            {
                case EventNames.HumidityAboveHigh: senderDevice.HumidityAboveHigh -= ButtonPressed; break;
                case EventNames.HumidityBelowHigh: senderDevice.HumidityBelowHigh -= ButtonPressed; break;
                case EventNames.HumidityAboveLow: senderDevice.HumidityAboveLow -= ButtonPressed; break;
                case EventNames.HumidityBelowLow: senderDevice.HumidityBelowLow -= ButtonPressed; break;
                case EventNames.HumidityChanged: senderDevice.HumidityChanged -= ButtonPressed; break;
                case EventNames.TemperatureAboveHigh: senderDevice.TemperatureAboveHigh -= ButtonPressed; break;
                case EventNames.TemperatureBelowHigh: senderDevice.TemperatureBelowHigh -= ButtonPressed; break;
                case EventNames.TemperatureAboveLow:  senderDevice.TemperatureAboveLow -= ButtonPressed; break;
                case EventNames.TemperatureBelowLow:  senderDevice.TemperatureBelowLow -= ButtonPressed; break;
                case EventNames.TemperatureChanged:   senderDevice.TemperatureChanged -= ButtonPressed; break;
            }
        }

        private void RemovePirSensorSubscribtion(PIRSensorModel senderDevice)
        {
            switch (EventName)
            {
                case EventNames.MotionDetected: senderDevice.MotionDetected -= ButtonPressed; break;
                case EventNames.MotionNotDetected: senderDevice.MotionNotDetected -= ButtonPressed; break;
            }

        }

        private void RemoveTonTimerSubscribtion(TonTimer senderDevice)
        {
            senderDevice.TimerElapsed-= ButtonPressed;
        }

        private void AddTonTimerSubscribtion(TonTimer sender)
        {
            sender.TimerElapsed += ButtonPressed;
        }
private void AddFugaTrykSubscribtion(FugaTryk sender)
{
    switch (EventName)
    {
                case ZigbeeDevice.EventNames.Button1Pressed: sender.Button1Pressed += ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button2Pressed: sender.Button2Pressed += ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button3Pressed: sender.Button3Pressed += ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button4Pressed: sender.Button4Pressed += ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button1LongPressed: sender.Button1LongPressed += ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button2LongPressed: sender.Button2LongPressed += ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button3LongPressed: sender.Button3LongPressed += ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button4LongPressed: sender.Button4LongPressed += ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button1Released: sender.Button1Released += ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button2Released: sender.Button2Released += ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button3Released: sender.Button3Released += ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button4Released: sender.Button4Released += ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button1ReleasedAfterLongPress: sender.Button1ReleasedAfterLongPress += ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button2ReleasedAfterLongPress: sender.Button2ReleasedAfterLongPress += ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button3ReleasedAfterLongPress: sender.Button3ReleasedAfterLongPress += ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button4ReleasedAfterLongPress: sender.Button4ReleasedAfterLongPress += ButtonPressed; break;
            }
        }

        private void RemoveFugaTrykSubscribtion(FugaTryk sender)
        {
            switch (EventName)
            {
                case ZigbeeDevice.EventNames.Button1Pressed: sender.Button1Pressed -= ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button2Pressed: sender.Button2Pressed -= ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button3Pressed: sender.Button3Pressed -= ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button4Pressed: sender.Button4Pressed -= ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button1LongPressed: sender.Button1LongPressed -= ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button2LongPressed: sender.Button2LongPressed -= ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button3LongPressed: sender.Button3LongPressed -= ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button4LongPressed: sender.Button4LongPressed -= ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button1Released: sender.Button1Released -= ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button2Released: sender.Button2Released -= ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button3Released: sender.Button3Released -= ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button4Released: sender.Button4Released -= ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button1ReleasedAfterLongPress: sender.Button1ReleasedAfterLongPress -= ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button2ReleasedAfterLongPress: sender.Button2ReleasedAfterLongPress -= ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button3ReleasedAfterLongPress: sender.Button3ReleasedAfterLongPress -= ButtonPressed; break;
                case ZigbeeDevice.EventNames.Button4ReleasedAfterLongPress: sender.Button4ReleasedAfterLongPress -= ButtonPressed; break;

            }
        }
        private ZigbeeDevice FindDeviceFromName(string name)
{
            ZigbeeDevice device = Data.zigbeeDevices.Find(x => x.Name == name);
            return device;
}


private void ButtonPressed(object? sender, int value)
{
    if (value != -1)
    {
         Value = value;
    }
    switch (receiverDevice)
    {
        case LedPanel: LedPanel ledPanel = (LedPanel)receiverDevice; ledPanel.Send(Command, Value); break;
        case IkeaBulb: IkeaBulb ikeaBulb = (IkeaBulb)receiverDevice; ikeaBulb.Send(Command, Value); break;
        case TonTimer: TonTimer tonTimer = (TonTimer)receiverDevice; tonTimer.Send(Command, Value); break;
        case Switch: Switch sw = (Switch)receiverDevice; sw.Send(Command, Value); break;
            }
        }
    }
}
