using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string? OrderStatus { get; set; }

    public int? StaffId { get; set; }

    public DateOnly? OrderDate { get; set; }

    public DateOnly? RequiredDate { get; set; }

    public DateOnly? ShippedDate { get; set; }

    public int? CustomerId { get; set; }

    public int? StoreId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual OrderItem? OrderItem { get; set; }

    public virtual Staff? Staff { get; set; }

    public virtual Store? Store { get; set; }
}
