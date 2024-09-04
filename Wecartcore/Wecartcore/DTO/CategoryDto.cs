using Microsoft.AspNetCore.Http;

namespace Wecartcore.DTO
{
    public class CategoryDto
    {
        public string CategoryName { get; set; }
        public IFormFile CategoryImage { get; set; }
    }
}
