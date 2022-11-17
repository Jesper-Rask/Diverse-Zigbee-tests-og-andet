using BlazorApp1.ZigbeeModels;

namespace ZigbeeModels
{
    public static class Data
    {
        public static List<ZigbeeDevice> zigbeeDevices = new();
        public static List<BridgeDevice>? bridgeDevices=new();

        public static TempAndHumiSensor sensorKøkken = new();
        public static TempAndHumiSensor sensorKælderVærksted = new();
        public static TempAndHumiSensor sensorStue = new();
        public static TempAndHumiSensor sensorSoveVærelse = new();
        public static TempAndHumiSensor sensorVaskerum = new();
        public static TempAndHumiSensor sensorJuliesVærelse = new();
        public static TempAndHumiSensor sensorKontor = new();
        public static TempAndHumiSensor sensorSystue = new();
        public static TempAndHumiSensor sensorBad = new();
        public static TempAndHumiSensor sensorBadKælder = new();

        public static PIRSensorModel sensorPirSoveværelse = new();
        public static IkeaBulb pære = new();
        public static LedPanel panel1Køkken = new();
        public static LedPanel panel2Køkken = new();
        public static LedPanel panel3Køkken = new();
        public static LedPanel panel1Stue = new();
        public static LedPanel panel2Stue = new();
        public static LedPanel panel3Stue = new();
        public static LedPanel panel4Stue = new();
        public static LedPanel panel5Stue = new();

       // public static int slider;
    }
}
