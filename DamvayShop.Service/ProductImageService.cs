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
    public interface IProductImageService
    {
        void Add(ProductImage productImage);
        void Update(ProductImage productImage);
        void Delete(int id);
        ProductImage GetByID(int id);
        List<ProductImage> GetProductImageByProdutID(int id);
        IEnumerable<ProductImage> GetAll(int productId);

        void Save();
    }
    public class ProductImageService : IProductImageService
    {
        private IProductImageRepository _productImageRepository;
        private IUnitOfWork _unitOfWork;
        public ProductImageService(IProductImageRepository productImageRepository,IUnitOfWork unitOfWork)
        {
            this._productImageRepository = productImageRepository;
            this._unitOfWork = unitOfWork;
        }
        public void Add(ProductImage productImage)
        {
            this._productImageRepository.Add(productImage);
        }
        public void Update(ProductImage productImage)
        {
            this._productImageRepository.Update(productImage);
        }

        public void Delete(int id)
        {
            this._productImageRepository.Delete(id);
        }

        public IEnumerable<ProductImage> GetAll(int productId)
        {
            return _productImageRepository.GetMulti(x => x.ProductId == productId);
        }

        public ProductImage GetByID(int id)
        {
          return  _productImageRepository.GetSingleById(id);
        }

        public List<ProductImage> GetProductImageByProdutID(int id)
        {
            return _productImageRepository.GetMulti(x => x.ProductId == id).ToList();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
