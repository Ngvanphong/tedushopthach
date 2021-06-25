using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Model.Models;

namespace DamvayShop.Data.Reponsitories
{
    public interface IProductImageRepository : IRepository<ProductImage>
    {
       
    }
   public class ProductImageRepository:RepositoryBase<ProductImage>,IProductImageRepository
    {
        public ProductImageRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }
    }
}
