namespace DeviceManagment.Shared
{
    public class DeviceType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<Ticket> Ticktes { get; set; }
        public DeviceType()
        {
            Devices = new HashSet<Device>();
            Ticktes = new HashSet<Ticket>();
        }
    }
}
