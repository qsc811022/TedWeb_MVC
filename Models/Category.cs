using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TedWeb.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        [DisplayName("Disaply Order")]
        public int DisplayOrder{ get; set; }

    }
}
