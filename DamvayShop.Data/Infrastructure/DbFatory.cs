using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamvayShop.Data.Inframestructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private DamvayShopDbContext dbContext;

        public DamvayShopDbContext Init()
        {
            return dbContext ?? (dbContext = new DamvayShopDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
