using System.Collections.Generic;
using System.Linq;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Model.Models;

namespace DamvayShop.Data.Reponsitories
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetAllByTag(string tag, int pageIndex, int pageSize, out int totalRow);
    }

    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Product> GetAllByTag(string tag, int pageIndex, int pageSize, out int totalRow)
        {
            var query = from p in DbContext.Products
                        join
                        pt in DbContext.ProductTags
                        on p.ID equals pt.ProductID
                        where p.Status && pt.TagID == tag
                        orderby p.UpdatedDate descending                       
                        select p;
            totalRow = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            
        }
    }
}