using System.ComponentModel.DataAnnotations;

namespace ChineseSaleApi.Dto
{
    public class CategoryDto
    {
            public int Id { get; set; } 
            
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }
    }

    

}
