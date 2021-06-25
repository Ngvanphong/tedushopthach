using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DamvayShop.Web.Models
{
    public class OrderViewModel
    {
  
        public int ID { get; set; }
 
        public string CustomerName { set; get; }
  
        public string CustomerAddress { set; get; }
   
        public string CustomerMobile { set; get; }
   
        public string CustomerEmail { set; get; }

        public string CustomerMessage { set; get; }
  
        public DateTime? CreateDate { get; set; }
  
        public string CreateBy { get; set; }
 
        public string PaymentMethod { get; set; }

        public string PaymentStatus { set; get; }
        public bool Status { get; set; }
        public string CustomerId { set; get; }

        public virtual ICollection<OrderDetailViewModel> OrderDetails { set; get; }

        public decimal? TotalPayment { set; get; }
    }
}