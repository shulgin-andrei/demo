using System;
using System.Collections.Generic;

namespace demo.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? Article { get; set; }

    public string? Name { get; set; }

    public int? UnitId { get; set; }

    public double? Price { get; set; }

    public int? Supplierid { get; set; }

    public int? Manufacturerid { get; set; }

    public int? CategoryId { get; set; }

    public double? Discount { get; set; }

    public double? WarehouseCount { get; set; }

    public string? Description { get; set; }

    public string? ImagePath { get; set; }

    public virtual ProductCategory? Category { get; set; }

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual ICollection<OrdersProduct> OrdersProducts { get; set; } = new List<OrdersProduct>();

    public virtual Supplier? Supplier { get; set; }

    public virtual Unit? Unit { get; set; }

    public override string ToString()
    {
        return $"{Name}";
    }
}
