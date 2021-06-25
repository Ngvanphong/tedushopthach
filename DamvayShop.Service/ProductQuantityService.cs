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
    public interface IProductQuantityService
    {
        void Add(ProductQuantity productQuantity);
        void Delete(int productId, int sizeId);
        void DeleteByProductID(int id);
        IEnumerable<ProductQuantity> GetAll(int productId, int? sizeId);
        ProductQuantity GetSingle(int productId, int sizeId);
        bool CheckExist(int productId, int sizeId);
        IEnumerable<Size> GetListSize();
        void AddSize(Size size);
        void DeleteSize(int id);
        Size GetSizeById(int id);
        void UpdateSize(Size size);
        void SaveChange();
        IEnumerable<Size> GetSizeByProductId(int productId);
        


    }
    public class ProductQuantityService : IProductQuantityService
    {
        private IProductQuantityRepository _productQuantityRepository;
        private ISizeRepository _sizeRepository;
        private IUnitOfWork _unitOfWork;
        public ProductQuantityService(IProductQuantityRepository productQuantiryRepository, ISizeRepository sizeRepository,IUnitOfWork unitOfWork)
        {
            this._productQuantityRepository = productQuantiryRepository;
            this._sizeRepository = sizeRepository;
            this._unitOfWork = unitOfWork;
        }
        public void Add(ProductQuantity productQuantity)
        {
            _productQuantityRepository.Add(productQuantity);

        }

        public bool CheckExist(int productId, int sizeId)
        {
            return _productQuantityRepository.CheckContains(x => x.ProductId == productId && x.SizeId == sizeId);

        }

        public void Delete(int productId, int sizeId)
        {
            _productQuantityRepository.DeleteMulti(x => x.ProductId == productId && x.SizeId == sizeId);
        }

        public void DeleteByProductID(int id)
        {
            _productQuantityRepository.DeleteMulti(x => x.ProductId == id);
        }

        public IEnumerable<ProductQuantity> GetAll(int productId, int? sizeId)
        {
            IEnumerable<ProductQuantity> listProductQuantity = _productQuantityRepository.GetMulti(x => x.ProductId == productId, new string[] { "Size" });
            if (sizeId.HasValue)
            {
                listProductQuantity = listProductQuantity.Where(x => x.SizeId == sizeId);
            }
            return listProductQuantity;
        }
        public ProductQuantity GetSingle(int productId, int sizeId)
        {
            ProductQuantity ProductQuantity = _productQuantityRepository.GetSingleByCondition(x => x.ProductId == productId && x.SizeId == sizeId);
            return ProductQuantity;
    
        }

        public IEnumerable<Size> GetListSize()
        {
           return _sizeRepository.GetAll();
        }
        public void AddSize(Size size)
        {
            _sizeRepository.Add(size);
        }
        public void DeleteSize(int sizeId)
        {
            _sizeRepository.DeleteMulti(x => x.ID == sizeId);
        }
       public Size GetSizeById(int id)
        {
          return  _sizeRepository.GetSingleById(id);
        }
        public void UpdateSize(Size size)
        {
            _sizeRepository.Update(size);
        }
        public void SaveChange()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<Size> GetSizeByProductId(int productId)
        {
            return _sizeRepository.GetSizeByProductId(productId);
        }

        
    }
}
