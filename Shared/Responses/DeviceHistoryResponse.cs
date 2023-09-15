namespace DeviceManagment.Shared.Responses
{
    public class DeviceHistoryResponse
    {
        public DateTime PurchasedDate { get; set; }
        public decimal PurchasedPrice { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
        public DateTime? UnassignedDate { get; set; }
        public decimal CurrentValuePrice { get; set; }
    }
}
