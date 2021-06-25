using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamvayShop.Model.Models
{
    [Table("OrderUserAnnoucements")]
   public class OrderUserAnnoucement
    {
        [Key]
        [Column(Order = 1)]
        public int OrderId { get; set; }

        [Key]
        [Column(Order = 2)]
        public string UserId { get; set; }

        public bool HasRead { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
