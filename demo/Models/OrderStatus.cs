using System;
using System.Collections.Generic;

namespace demo.Models;

public partial class OrderStatus
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public override string ToString()
    {
        return $"{Name}";
    }
}
