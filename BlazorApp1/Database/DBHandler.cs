using Dapper;
using MySql.Data.MySqlClient;

namespace BlazorApp1.Database
{
    public class DBHandler
    {
        private string _serverAddress;
        private string _databaseName;
        private string _user;
        private string _password;
        public DBHandler(string srvAddress, string DBName, string user, string password)
        {
            _serverAddress = srvAddress;
            _databaseName = DBName;
            _user = user;
            _password = password;
        }
        public void InsertTempAndHumiSensor(TempAndHumiSensor zigbee)
        {
            string temp = zigbee.temperature.ToString().Replace(',', '.');
            string humi = zigbee.humidity.ToString().Replace(',', '.');
            string time = zigbee.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss");
            string volt = zigbee.voltage.ToString().Replace(',', '.');
            string battPerc = zigbee.battery.ToString().Replace(',', '.');
            string query = $"insert into tempandhumisensor (Name, Temperature, Humidity, Timestamp, BatteryVoltage, BatteryPercent)" +
                $" values ('{zigbee.Name}', {temp}, {humi}, '{time}', {volt}, {battPerc})";

            using (var connection = new MySqlConnection($"Server={_serverAddress};Database={_databaseName};Uid={_user};Pwd={_password};"))
            {
                connection.Execute(query);
            }
        }
        public async Task<List<TempAndHumiSensor>> SelectData(string tableName, string name, DateTime from, DateTime to)
        {
            string sFrom = from.ToString("yyyy-MM-dd HH:mm:ss");
            string sTo = to.ToString("yyyy-MM-dd HH:mm:ss");
            string query = $"select Temperature, Humidity, Timestamp from {tableName} where " +
                $"Timestamp > '{sFrom}' and " +
                $"Timestamp < '{sTo}' and " +
                $"Name='{name}'";
            List<TempAndHumiSensor> output = new();

            await using (var connection = new MySqlConnection($"Server={_serverAddress};Database={_databaseName};Uid={_user};Pwd={_password};"))
            {
                var result = connection.Query<(double, double, DateTime)>(query).ToList();
                foreach (var item in result)
                {
                    TempAndHumiSensor temp = new();
                    temp.Name = name;
                    temp.temperature = item.Item1;
                    temp.humidity = item.Item2;
                    temp.TimeStamp = item.Item3;
                    output.Add(temp);
                }
                return output;
            }
        }
        public void InsertDevice(ZigbeeDevice device)
        {

        }
        public List<ZigbeeDevice> GetDevices()
        {
            return new List<ZigbeeDevice>();
        }
    }
}
