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
    public class PostCategoryRepositoryTest
    {
        private IPostCategoryRepository objRepository;
        private IUnitOfWork _unitOfWork;
        private IDbFactory _dbFactory;
        [TestInitialize]
        public void Initialize()
        {
            _dbFactory = new DbFactory();
            objRepository = new PostCategoryRepository(_dbFactory);
            _unitOfWork = new UnitOfWork(_dbFactory);
        }
        [TestMethod]
        public void PostCategoryRepository_Add()
        {

            PostCategory postCategory = new PostCategory();
            postCategory.Name = "Post Category 1";
            postCategory.Alias = "PostCategory1";
            postCategory.CreateDate = DateTime.Now;
            postCategory.Status = true;
            var result = objRepository.Add(postCategory);
            _unitOfWork.Commit();
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ID);
        }
        [TestMethod]
        public void PostCategoryRepositoryGetAll()
        {
            var result= objRepository.GetAll().ToList();
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

        }
    }
}
