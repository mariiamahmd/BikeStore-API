using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Active { get; set; }

    public int? ManagerId { get; set; }

    public int? StoreId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Store? Store { get; set; }
}
