using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamvayShop.Model.Models
{
    [Table("Erros")]
   public class Error
    {
        [Key]
        public int ID { set; get; }
        public string Message { get; set; }
        public string StackTrace { set; get; }
        public DateTime CreateDate { get; set; }

    }
}
