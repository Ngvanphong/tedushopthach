using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Data.Reponsitories;
using DamvayShop.Model.Models;

namespace DamvayShop.Service
{
    public interface IOrderService
    {
        Order Create(Order order);

        IEnumerable<Order> GetList(string startDate, string endDate, string customerName, string paymentStatus,
            int pageIndex, int pageSize, out int totalRow);

        Order GetDetail(int orderId);

        OrderDetail CreateDetail(OrderDetail order);

        void DeleteDetail(int productId, int orderId, int sizeId);
        void DeleteOrder(int id);

        void UpdateStatus(int orderId);

        IEnumerable<OrderDetail> GetOrderDetails(int orderId);

        void Save();
    }

    public class OrderService : IOrderService
    {
        private IOrderRepository _orderRepository;
        private IOrderDetailRepository _orderDetailRepository;
        private IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork, IOrderDetailRepository orderDetailRepository, IOrderRepository orderRepository)
        {
            this._unitOfWork = unitOfWork;
            this._orderDetailRepository = orderDetailRepository;
            this._orderRepository = orderRepository;
        }

        public Order Create(Order order)
        {
            return _orderRepository.Add(order);
        }

        public OrderDetail CreateDetail(OrderDetail order)
        {
            return _orderDetailRepository.Add(order);
        }

        public void DeleteDetail(int productId, int orderId, int sizeId)
        {
            OrderDetail orderDetail = _orderDetailRepository.GetSingleByCondition(x => x.ProductID == productId && x.OrderID == orderId && x.SizeId == sizeId);
            _orderDetailRepository.Delete(orderDetail);
        }

        public void DeleteOrder(int id)
        {
            _orderRepository.Delete(id);
        }

        public Order GetDetail(int orderId)
        {
            return _orderRepository.GetSingleByCondition(x => x.ID == orderId, new string[] { "OrderDetails" });
        }

        public IEnumerable<Order> GetList(string startDate, string endDate, string customerName, string paymentStatus, int pageIndex, int pageSize, out int totalRow)
        {
            IEnumerable<Order> query = _orderRepository.GetAll();
            if (!string.IsNullOrEmpty(startDate))
            {
                DateTime dateStart = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.CreateDate >= dateStart);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                DateTime dateEnd = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.CreateDate <= dateEnd);
            }
            if (!string.IsNullOrEmpty(customerName))
            {
                query = query.Where(x => x.CustomerName.Contains(customerName));
            }
            if (!string.IsNullOrEmpty(paymentStatus))
            {
                query = query.Where(x => x.PaymentStatus == paymentStatus);
            }
            totalRow = query.Count();
            return query.OrderByDescending(x => x.CreateDate).Skip((pageIndex-1) * pageSize).Take(pageSize);
        }

        public IEnumerable<OrderDetail> GetOrderDetails(int orderId)
        {
            return _orderDetailRepository.GetMulti(x => x.OrderID == orderId, new string[] { "Order", "Size", "Product" }).ToList();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateStatus(int orderId)
        {
            Order orderDb = _orderRepository.GetSingleById(orderId);
            orderDb.Status = true;
            _orderRepository.Add(orderDb);
        }
    }
}