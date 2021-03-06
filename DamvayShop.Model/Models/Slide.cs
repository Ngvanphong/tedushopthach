using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamvayShop.Model.Models
{
    [Table("Slides")]
  public  class Slide
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        [Required]
        [MaxLength(50)]
        public string Name { set; get; }
        [MaxLength(256)]
        public string Description { set; get; }
        [Required]
        [MaxLength(256)]
        public string URL { get; set; }
        [MaxLength(256)]
        public string Image { set; get; }
        public int? DisplayOrder { set; get; }
        [Required]
        public bool Status { set; get; }

        public string Content { set; get; }

    }
}
