using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamvayShop.Model.Models
{
    [Table("PostTags")]
  public  class PostTag
    {
        [Key]
        [Column(Order=1)]
        public string PostID { get; set; }
        [ForeignKey("PostID")]
        public virtual Post Post { get; set; }
        [Key]
        [Column(Order = 2)]
        public string TagID { get; set; }
        [ForeignKey("TagID")]
        public virtual Tag Tag { set; get; }
    }
}
