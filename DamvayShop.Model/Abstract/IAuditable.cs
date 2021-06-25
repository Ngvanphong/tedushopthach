using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamvayShop.Model.Abstract
{
    interface IAuditable
    {
       
        DateTime? CreateDate { set; get; }
        String CreateBy { get; set; }
        DateTime? UpdatedDate { set; get; }
        String UpdatedBy { set; get; }

    }
}
