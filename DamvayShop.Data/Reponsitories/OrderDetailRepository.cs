using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Model.Models;

namespace DamvayShop.Data.Reponsitories
{
    public interface IOrderDetailRepository: IRepository<OrderDetail>
    {

    }
   public class OrderDetailRepository:RepositoryBase<OrderDetail>,IOrderDetailRepository
    {
        public OrderDetailRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }
    }
}
