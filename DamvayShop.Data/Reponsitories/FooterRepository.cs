using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Model.Models;

namespace DamvayShop.Data.Reponsitories
{
    public interface IFooterRepository: IRepository<Footer>
    {

    }
   public class FooterRepository:RepositoryBase<Footer>,IFooterRepository
    {
        public FooterRepository(IDbFactory dbFactory):base(dbFactory)
        {
                
        }
    }
}
