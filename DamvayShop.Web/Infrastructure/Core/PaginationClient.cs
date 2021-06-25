using System.Collections.Generic;
using System.Linq;

namespace DamvayShop.Web.Infrastructure.Core
{
    public class PaginationClient<T>
    {
        public int PageIndex { set; get; }
        public int PageSize { get; set; }
        public int TotalRows { set; get; }
        public IEnumerable<T> Items { set; get; }
        public int TotalPage { set; get; }
        public int PageDisplay { set; get; }

        public int Count
        {
            get
            {
                return Items.Count();
            }
        }
    }
}