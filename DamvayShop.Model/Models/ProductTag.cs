using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DamvayShop.Model.Models
{
    [Table("ProductTags")]
    public class ProductTag
    {
        [Key]
        [Column(Order=1)]
        public int ProductID { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

        [Key]
        [Column(Order=2)]
        public string TagID { get; set; }

        [ForeignKey("TagID")]
        public virtual Tag Tag { get; set; }
    }
}