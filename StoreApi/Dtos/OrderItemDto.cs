namespace StoreApi.Dtos
{
        public class OrderItemDto
        {
            public int ItemId { get; set; }
            public int? ProductId { get; set; }
            public int? Quantity { get; set; }
            public int? ListPrice { get; set; }
            public int? Discount { get; set; }
            public int? OrderId { get; set; }
        }
    
}
