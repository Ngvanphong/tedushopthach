using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Model.Models;

namespace DamvayShop.Data.Reponsitories
{
    public interface ISupportOnlineRepository: IRepository<SupportOnline>
    {

    }
   public class SupportOnlineRepository:RepositoryBase<SupportOnline>,ISupportOnlineRepository
    {
        public SupportOnlineRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }
    }
}
