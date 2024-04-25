using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodingWiki_Model.Models;

public class BookAuthorMap
{
    [ForeignKey("Book")]
    public int BookId { get; set; }
    
    [ForeignKey("Author")]
    public int AuthorId { get; set; }
    
    public Book Book { get; set; }
    
    public Author Author { get; set; }
}