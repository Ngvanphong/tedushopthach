using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Data.Reponsitories;
using DamvayShop.Model.Models;

namespace DamvayShop.UnitTest.RepositoryTest
{
    [TestClass]
    public class ProductRepositoryTest
    {
        private IDbFactory dbFactory;
        private IUnitOfWork uniOfWork;
        private IProductRepository objRepository;
        [TestInitialize]
        public void Initialize()
        {
            dbFactory = new DbFactory();
            uniOfWork = new UnitOfWork(dbFactory);
            objRepository = new ProductRepository(dbFactory);

        }
        [TestMethod]
        public void ProductRepository_Add()
        {
            Product product = new Product();
            product.Name = "Product test";
            product.Alias = "Product-tag";
            product.CategoryID = 6;
            product.CreateDate = DateTime.Now;
            product.Status = true;
           var result= objRepository.Add(product);
            uniOfWork.Commit();
            Assert.AreEqual(8, result.ID);
        }
    }
}
