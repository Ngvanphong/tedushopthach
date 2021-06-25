using System;

using System.Linq;
using System.Web;

namespace DamvayShop.Web.Models
{
  
    public class PostImageViewModel
    {


        public int ID { get; set; }

        public string PostId { get; set; }

      
        public string Path { get; set; }

        public string Caption { get; set; }

    }
}