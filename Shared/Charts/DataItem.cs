namespace DeviceManagment.Shared.Charts
{
    public class DataItem
    {
        public string Year { get; set; } = string.Empty;
        public List<ChartValues> Items { get; set; } = new List<ChartValues>();
    }
}
