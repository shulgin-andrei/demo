using System;
using System.Collections.Generic;

namespace demo.Models;

public partial class OrdersProduct
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Count { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }

    public override string ToString()
    {
        return base.ToString();
    }
}
