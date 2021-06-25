using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamvayShop.Model.Abstract
{
    public abstract class TableCommon : ISeotable, IAuditable, ISwitchtable
    {
        [MaxLength(250)]
        public string MetaKeyword { get; set; }
        [MaxLength(250)]
        public string MetaDiscription { get; set; }
        public DateTime? CreateDate { get; set; }
        [MaxLength(250)]
        public string CreateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [MaxLength(250)]
        public string UpdatedBy { get; set; }
        public bool Status { get; set; }
    }
}
