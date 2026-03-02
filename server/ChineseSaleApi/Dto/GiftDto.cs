using ChineseSaleApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ChineseSaleApi.Dto
{
public class GiftDto

{

public  int Id { get; set; }

public required string Name { get; set; }

public required int TicketCost { get; set; }

public string? Description { get; set; }

public string? PictureUrl { get; set; }

public CategoryDto? Category { get; set; }

public required DonorDto Donor { get; set; }



}




    public class AddGiftDto
    {
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "TicketCost must be greater than 0")]
        public required int TicketCost { get; set; }
        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "DonorId is required")]
        public int DonorId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "CategoryId is required")]
        public  int CategoryId { get; set; }
    }
     
    public class SearchGiftDto
    {
        public string? Name { get; set; }
        public string? DonorName { get; set; }
        public int? NumOfTickets { get; set; }
    }


    public class  UserGiftDto
    {
        public int Id { get; set; } 
        public required string Name { get; set; }
        public required int TicketCost { get; set; }
        public string? Description { get; set; }
        public string? PictureUrl { get; set; }
        public string? Category { get; set; }
        public string? DonorName { get; set; }
        public string? WinnerUser { get; set; }

    }
}
