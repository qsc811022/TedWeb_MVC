using System.ComponentModel.DataAnnotations;

namespace TedWeb.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public string DisplayOrder{ get; set; }

    }
}
