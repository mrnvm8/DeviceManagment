using System.Text.Json.Serialization;

namespace DeviceManagment.Shared.Responses
{
    public class DeviceTypeResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Total { get; set; }
    }
}
