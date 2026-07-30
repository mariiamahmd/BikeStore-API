using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class Stock
{
    public int? Quantity { get; set; }

    public int? StoreId { get; set; }

    public int? ProductId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Store? Store { get; set; }
}
