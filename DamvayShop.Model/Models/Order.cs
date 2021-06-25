using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamvayShop.Model.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [MaxLength(256)]
        public string CustomerName { set; get; }
        [Required]
        [MaxLength(256)]
        public string CustomerAddress { set; get; }
        [Required]
        [MaxLength(256)]
        public string CustomerMobile { set; get; }

        [MaxLength(256)]
        public string CustomerEmail { set; get; }
        public string CustomerMessage { set; get; }
        [Required]
        public DateTime? CreateDate { get; set; }
        [MaxLength(256)]
        public string CreateBy { get; set; }
        [MaxLength(256)]
        public string PaymentMethod { get; set; }
        [MaxLength(256)]
        public string PaymentStatus { set; get; }
        public bool Status { get; set; }
        [StringLength(128)]
        [Column(TypeName = "nvarchar")]
        public string CustomerId { set; get; }
        public virtual ICollection<OrderDetail> OrderDetails { set; get; }


        public decimal? TotalPayment { set; get; }

    }
}
