namespace Wecartcore.DTO
{
    public class ProductDTO
    {
        public string? ProductName { get; set; }
        public IFormFile? ProductImage { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }

        public int CategoryId { get; set; }
    }
}
