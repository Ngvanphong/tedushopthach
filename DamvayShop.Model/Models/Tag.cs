using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamvayShop.Model.Models
{
    [Table("Tags")]
    public class Tag
    {
        [Key]
        [MaxLength(50)]
        public string ID { get; set; }
        [Required]
        [MaxLength(256)]
        public string Name { set; get; }
        [MaxLength(256)]
        public string Type { set; get; }
        public virtual ICollection<ProductTag> ProductTag { get; set; }
        public virtual ICollection<PostTag> PostTag { get; set; }


    }
}
