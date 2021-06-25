using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamvayShop.Model.Models
{
    [Table("SupportOnlines")]
  public  class SupportOnline
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }      
        [MaxLength(256)]
        public string Skype { set; get; }
        [MaxLength(256)]
        public string Email { set; get; }
        [MaxLength(256)]
        public string Mobile { set; get; }
        [MaxLength(256)]     
        public string Facebook { set; get; }
        [Required]
        public string Status { get; set; }
    }
}
