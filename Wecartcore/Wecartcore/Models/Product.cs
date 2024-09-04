using System;
using System.Collections.Generic;

namespace Wecartcore.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public double Price { get; set; }

    public double? OldPrice { get; set; }

    public string? ImageUrl { get; set; }

    public double? Rating { get; set; }

    public int? CategoryId { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
