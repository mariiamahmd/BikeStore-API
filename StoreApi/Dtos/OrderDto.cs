namespace StoreApi.Dtos
{
    public class OrderDto
    {
        public string? OrderStatus { get; set; }

        public int? StaffId { get; set; }

        public DateOnly? OrderDate { get; set; }

        public DateOnly? RequiredDate { get; set; }

        public DateOnly? ShippedDate { get; set; }

        public int? CustomerId { get; set; }

        public int? StoreId { get; set; }

    }
}
