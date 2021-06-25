using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Data.Reponsitories;
using DamvayShop.Model.Models;

namespace DamvayShop.Service
{
    public interface IOrderUserAnnoucementService
    {
        void Add(OrderUserAnnoucement annoucement);
        OrderUserAnnoucement GetById(int OrderId, string UserId);
        IEnumerable<Order> ListAllUnread(string userId, int pageIndex, int pageSize, out int totalRow);
        IEnumerable<OrderUserAnnoucement> GetAll();
        IEnumerable<OrderUserAnnoucement> GetAllByHasRead();
        void Delete(OrderUserAnnoucement annoucement);
        void SaveChange();
    }
    public class OrderUserAnnoucementService : IOrderUserAnnoucementService
    {
        private IOrderUserAnnoucementRepository _oderUserRepository;
        private IUnitOfWork _unitOfWork;
        public OrderUserAnnoucementService(IOrderUserAnnoucementRepository orderUserRepository,IUnitOfWork unitOfWork)
        {
            this._oderUserRepository = orderUserRepository;
            this._unitOfWork = unitOfWork;
        }
        public void Add(OrderUserAnnoucement annoucement)
        {
            _oderUserRepository.Add(annoucement);
        }

      

        public void Delete(OrderUserAnnoucement annoucement)
        {
            _oderUserRepository.Delete(annoucement);
        }

        public IEnumerable<OrderUserAnnoucement> GetAll()
        {
           return _oderUserRepository.GetAll();
        }

        public IEnumerable<OrderUserAnnoucement> GetAllByHasRead()
        {
            return _oderUserRepository.GetMulti(x => x.HasRead == false);
        }

        public OrderUserAnnoucement GetById(int OrderId, string UserId)
        {
            return _oderUserRepository.GetSingleByCondition(x => x.OrderId == OrderId && x.UserId == UserId);
        }


        public IEnumerable<Order> ListAllUnread(string userId, int pageIndex, int pageSize, out int totalRow)
        {
            var query = _oderUserRepository.GetAllUnread(userId);
            totalRow = query.Count();
            return query.OrderByDescending(x => x.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public void SaveChange()
        {
            _unitOfWork.Commit();
        }
    }
}
