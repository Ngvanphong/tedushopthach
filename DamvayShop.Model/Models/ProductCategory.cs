using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DamvayShop.Model.Abstract;

namespace DamvayShop.Model.Models
{
    [Table("ProductCategories")]
    public class ProductCategory:TableCommon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string Alias { get; set; }
        public int? ParentID { get; set; }
        [MaxLength(256)]
        public string Description { set; get; }
        public int? DisplayOrder { get; set; }
        [MaxLength(256)]
        public string Image { get; set; }
        public bool? HomeFlag { set; get; }
        public int? HomeOrder { set; get; }
       

    }
}
