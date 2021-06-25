using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DamvayShop.Model.Abstract;

namespace DamvayShop.Model.Models
{
    [Table("Products")]
    public class Product: TableCommon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { set; get; }
        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string Alias { set; get; }
        [Required]
        public int CategoryID { set; get; }
        [ForeignKey("CategoryID")]
        public virtual ProductCategory ProductCategory { set; get; }
        public int? DisplayOrder { set; get; }

        [MaxLength(256)]
        public string ThumbnailImage { set; get; }

        public Decimal Price { get; set; }
        
        public Decimal? PromotionPrice { set; get; }
        public int? Warranty { set; get; }
        [MaxLength(500)]
        public string Description { set; get; }
        public string Content { set; get; }
        public bool? HomeFlag { set; get; }
        public bool? HotFlag { get; set; }
        public int? ViewCount { set; get; }

        public virtual ICollection<ProductTag> ProductTags { set; get; }
        [MaxLength(256)]
        public string Tags { set; get; }

       
    }
}
