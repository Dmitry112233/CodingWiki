using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodingWiki_Model.Models;

public class BookDetail
{
    [Key]
    public int BookDetailId { get; set; }
    
    [Required]
    public int NumberOfChapters { get; set; }
    
    public int NumberOfPages { get; set; }
    
    public string Weight { get; set; }
    
    [ForeignKey("Book")]
    public int BookId { get; set; }
    
    public Book Book { get; set; }
}