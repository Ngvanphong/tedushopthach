using System.Collections.Generic;
using System.Linq;
using DamvayShop.Common;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Data.Reponsitories;
using DamvayShop.Model.Models;

namespace DamvayShop.Service
{
    public interface IProductService
    {
        Product Add(Product product);

        void Update(Product product);

        void Delete(int id);
        IEnumerable<string> GetProductName(string productName);

        IEnumerable<Product> GetAll();

        IEnumerable<Product> GetAll(int? categoryId, string keyword);
        IEnumerable<Product> GetAll(int? categoryId, string hotPromotion, string keyword);

        IEnumerable<Product> GetAllPaging(int page, int pageSize, out int totalRow);

        Product GetById(int id);
        IEnumerable<Product> GetHotProduct();
        IEnumerable<Product> GetAllHotProduct(int page, int pageSize, out int totalRow);
        IEnumerable<Product> GetPromotionProduct();
        IEnumerable<Product> GetAllPromotionProduct(int page, int pageSize, out int totalRow);

        IEnumerable<Product> GetAllByTagPaging(string tag, int page, int pageSize, out int totalRow);

        IEnumerable<Product> GetAllByCategoryPaging(int CategoryId, int page, int pageSize, string sort, out int totalRow);

        IEnumerable<Product> GetAllByNamePaging(string Name, int page, int pageSize, string sort, out int totalRow);

    

        IEnumerable<Product> GetProductRelate(int CategoryId);



        void SaveChanges();
    }

    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;
        private IUnitOfWork _unitOfWork;
        private ITagRepository _tagReponsitory;
        private IProductTagRepository _productTagRepository;

        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, ITagRepository tagRepository, IProductTagRepository productTagRepository)
        {
            this._productRepository = productRepository;
            this._unitOfWork = unitOfWork;
            this._tagReponsitory = tagRepository;
            this._productTagRepository = productTagRepository;
        }

        public Product Add(Product product)
        {
            Product query = _productRepository.Add(product);
            _unitOfWork.Commit();
            if (!string.IsNullOrEmpty(product.Tags))
            {
                string[] listTag = product.Tags.Split(',');
                for (int i = 0; i < listTag.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(listTag[i]);
                    if (_tagReponsitory.Count(x => x.ID == tagId) == 0)
                    {
                        Tag tag = new Tag()
                        {
                            ID = tagId,
                            Name = listTag[i],
                            Type = CommonConstant.ProductTag,
                        };
                        _tagReponsitory.Add(tag);
                    }
                    ProductTag productTag = new ProductTag()
                    {
                        ProductID = query.ID,
                        TagID = tagId,
                    };
                    _productTagRepository.Add(productTag);
                }
            }
            return query;
        }

        public void Delete(int id)
        {
            _productRepository.Delete(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _productRepository.GetAll(new string[] { "ProductCategory","ProductTags"});
        }

        public IEnumerable<Product> GetAll(int? categoryId, string keyword)
        {
            var query = _productRepository.GetAll(new string[] { "ProductCategory", "ProductTags" });
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => (x.Name.Contains(keyword) || x.Alias.Contains(keyword) && x.Status));
            };
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryID == categoryId);
            };
            return query;
        }

        public IEnumerable<Product> GetAll(int? categoryId, string hotPromotion, string keyword)
        {
            var query = _productRepository.GetAll(new string[] { "ProductCategory", "ProductTags" });
  
            if (!string.IsNullOrEmpty(keyword))
            {
                int id = 0;
                bool flagId = int.TryParse(keyword, out id);
                if (flagId == true)
                {
                    query = query.Where(x => x.ID==id);
                }
                else
                {
                    query = query.Where(x => x.Name.Contains(keyword));
                }
                              
            };
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryID == categoryId.Value);
            };
            if (!string.IsNullOrEmpty(hotPromotion))
            {
                if(hotPromotion=="Hot")
                        query = query.Where(x =>x.HotFlag==true && x.Status);
                else
                {
                    query = query.Where(x => x.PromotionPrice.HasValue&&x.Status);
                }
            };
            return query;
        }

        public IEnumerable<Product> GetAllByCategoryPaging(int CategoryId, int page, int pageSize, string sort, out int totalRow)
        {
            IEnumerable<Product> query = _productRepository.GetMulti(x => x.Status == true && x.CategoryID == CategoryId);
            switch (sort)
            {
                case "price":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "promotion":
                    query = query.Where(x => x.PromotionPrice.HasValue).OrderBy(x=>x.PromotionPrice);
                    break;
                default:
                    query = query.OrderByDescending(x => x.UpdatedDate);
                    break;
            }
            totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            return query;
        }

        public IEnumerable<Product> GetAllByNamePaging(string Name, int page, int pageSize, string sort, out int totalRow)
        {
            IEnumerable<Product> query = _productRepository.GetMulti(x => x.Status == true && x.Name.Contains(Name));
            switch (sort)
            {
                case "price":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "promotion":
                    query = query.Where(x => x.PromotionPrice.HasValue).OrderBy(x => x.PromotionPrice);
                    break;
                default:
                    query = query.OrderByDescending(x => x.UpdatedDate);
                    break;
            }
            totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            return query;
        }

        public IEnumerable<Product> GetAllByTagPaging(string tag, int page, int pageSize, out int totalRow)
        {
            return _productRepository.GetAllByTag(tag, page, pageSize, out totalRow);
        }

        public IEnumerable<Product> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return _productRepository.GetMultiPaging(x => x.Status, out totalRow, page, pageSize);
        }

        public Product GetById(int id)
        {
            return _productRepository.GetSingleByCondition(x=>x.ID==id, new string[] { "ProductCategory" });
        }

        public IEnumerable<Product> GetHotProduct()
        {
            IEnumerable<Product> listHotProduct = _productRepository.GetMulti(x => x.Status == true && x.HotFlag == true).OrderByDescending(x => x.UpdatedDate).Take(7);
                
            return listHotProduct;
        }

        public IEnumerable<Product> GetAllHotProduct(int page, int pageSize, out int totalRow)
        {
            IEnumerable<Product> listHotProduct = _productRepository.GetMulti(x => x.Status == true && x.HotFlag == true).OrderByDescending(x => x.UpdatedDate);
            totalRow = listHotProduct.Count();
            listHotProduct = listHotProduct.Skip((page - 1) * pageSize).Take(pageSize);

            return listHotProduct;
        }

        public IEnumerable<Product> GetAllPromotionProduct(int page, int pageSize, out int totalRow)
        {
            IEnumerable<Product> listPromotionProduct = _productRepository.GetMulti(x => x.Status == true && x.PromotionPrice.HasValue).OrderByDescending(x => x.UpdatedDate);
            totalRow = listPromotionProduct.Count();
            listPromotionProduct = listPromotionProduct.Skip((page - 1) * pageSize).Take(pageSize);

            return listPromotionProduct;
        }

        public IEnumerable<string> GetProductName(string productName)
        {
           return _productRepository.GetMulti(x => x.Status && x.Name.Contains(productName)).Select(x => x.Name);
        }

        public IEnumerable<Product> GetProductRelate(int CategoryId)
        {
            return _productRepository.GetMulti(x => x.Status == true && x.CategoryID == CategoryId).OrderBy(x => x.UpdatedDate).Take(8);
        }

        public IEnumerable<Product> GetPromotionProduct()
        {
            IEnumerable<Product> listProduct = _productRepository.GetMulti(x => x.Status == true && x.PromotionPrice.HasValue).OrderByDescending(x => x.CreateDate).Take(7);
            return listProduct;
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Product product)
        {
            _productRepository.Update(product);
            _unitOfWork.Commit();
            if (!string.IsNullOrEmpty(product.Tags))
            {
                string[] listTags = product.Tags.Split(',');
                foreach (var item in listTags)
                {
                    var tagId = StringHelper.ToUnsignString(item);
                    if (_tagReponsitory.Count(x => x.ID == tagId) == 0)
                    {
                        Tag tag = new Tag()
                        {
                            ID = tagId,
                            Name = item,
                            Type = CommonConstant.ProductTag,
                        };
                        _tagReponsitory.Add(tag);
                    }
                    int coutProductTag = _productTagRepository.GetMulti(x => (x.ProductID == product.ID && x.TagID == tagId)).Count();
                    if (coutProductTag == 0)
                    {
                        ProductTag productTag = new ProductTag()
                        {
                            ProductID = product.ID,
                            TagID = tagId,
                        };
                        _productTagRepository.Add(productTag);
                    }
                }
            }
        }
    }
}