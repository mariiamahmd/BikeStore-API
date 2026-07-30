namespace StoreApi.Dtos
{
    public class StoreDto
    {
        public int StoreId { get; set; }
        public string? StoreName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int? ZipCode { get; set; }
    }
}