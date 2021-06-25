using System.Collections.Generic;

namespace DamvayShop.Web.Models
{
    public class SavePermissionRequest
    {
        public string FunctionId { set; get; }

        public IList<PermissionViewModel> Permissions { get; set; }
    }
}