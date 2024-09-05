namespace Wecartcore.DTO
{
    public class CategoryWithProductCountDTO
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public int ProductCount { get; set; } // Add ProductCount
    }
}
