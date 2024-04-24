using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodingWiki_Model.Models;

public class Book
{
    [Key]
    public int BookId { get; set; }
    
    public string Title { get; set; }
    
    public decimal Price { get; set; }
    
    [MaxLength(20)]
    [Required]
    public string ISBN { get; set; }
    
    [NotMapped]
    public int PriceRange { get; set; }
}