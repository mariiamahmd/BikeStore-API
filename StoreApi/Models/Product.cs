using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public int? BrandId { get; set; }

    public int? ModelYear { get; set; }

    public int? ListPrice { get; set; }

    public int? CategoryId { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Category? Category { get; set; }
}
