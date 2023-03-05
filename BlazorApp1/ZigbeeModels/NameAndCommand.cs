using static ZigbeeModels.ZigbeeDevice;

namespace BlazorApp1.ZigbeeModels
{
    public class NameAndCommand
    {
        public string Name { get; set; }
        public Commands Cmd { get; set; }
        public EventNames Output { get; set; }
        public int Value { get; set; }
    }
}
