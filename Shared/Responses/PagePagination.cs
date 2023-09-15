namespace DeviceManagment.Shared.Responses
{
    public class PagePagination<T>
    {
        public List<T> Pagination { get; set; } = new List<T>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
