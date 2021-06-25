using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Model.Models;

namespace DamvayShop.Data.Reponsitories
{
    public interface IProductQuantityRepository:IRepository<ProductQuantity>
    {

    }
   public class ProductQuantityRepository:RepositoryBase<ProductQuantity>,IProductQuantityRepository
    {
        public ProductQuantityRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }
    }
}
