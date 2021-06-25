using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Model.Models;

namespace DamvayShop.Data.Reponsitories
{
    public interface IOrderUserAnnoucementRepository : IRepository<OrderUserAnnoucement>
    {
        List<Order> GetAllUnread(string userId);
    }
   public class OrderUserAnnoucementRepository:RepositoryBase<OrderUserAnnoucement>, IOrderUserAnnoucementRepository
    {
        public OrderUserAnnoucementRepository(IDbFactory dbFactory):base(dbFactory)
        {
           

        }
        public List<Order> GetAllUnread(string userId)
        {
            List<int> ListOrderinAnnoucement = DbContext.OrderUserAnnoucements.Where(x => x.UserId == userId).Select(x => x.OrderId).ToList();
            List<int> ListOrder = DbContext.Orders.Select(x => x.ID).ToList();

            List<int> ListOrdreCanRead = ListOrder.Except(ListOrderinAnnoucement).ToList();

            List<Order> query = new List<Order>
            {
            };
           foreach(var item in ListOrdreCanRead)
            {
                Order order = DbContext.Orders.Find(item);
                 query.Add(order);         
            }
            return query;

        }

    }
}

                       