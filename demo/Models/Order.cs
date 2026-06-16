using System;
using System.Collections.Generic;

namespace demo.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? Number { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public int? PickupPointId { get; set; }

    public int? UserId { get; set; }

    public int? Code { get; set; }

    public int? StatusId { get; set; }

    public virtual ICollection<OrdersProduct> OrdersProducts { get; set; } = new List<OrdersProduct>();

    public virtual PickupPoint? PickupPoint { get; set; }

    public virtual OrderStatus? Status { get; set; }

    public virtual User? User { get; set; }

    public override string ToString()
    {
        return  $"{Number}";
    }
}
