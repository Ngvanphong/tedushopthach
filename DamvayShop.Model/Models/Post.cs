using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DamvayShop.Model.Abstract;

namespace DamvayShop.Model.Models
{
    [Table("Posts")]
    public class Post : TableCommon
    {
        [Key]
        public string ID { set; get; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string Alias { set; get; }

        [Required]
        public int CategoryID { set; get; }

        [ForeignKey("CategoryID")]
        public virtual PostCategory PostCategory { get; set; }

        public int? DisplayOrder { get; set; }

        [MaxLength(256)]
        public string Description { set; get; }
        [MaxLength(256)]
        public string Image { get; set; }

        public string Content { set; get; }
        public bool? HomeFlag { get; set; }
        public int? ViewCount { get; set; }
      
        public virtual ICollection<PostTag> PostTag { get; set; }

        [MaxLength(256)]
        public string Tags { set; get; }
    }
}