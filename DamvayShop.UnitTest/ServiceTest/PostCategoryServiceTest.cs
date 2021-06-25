using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Data.Reponsitories;
using DamvayShop.Model.Models;
using DamvayShop.Service;

namespace DamvayShop.UnitTest.ServiceTest
{
    [TestClass]
    public class PostCategoryServiceTest
    {
        private Mock<IPostCategoryRepository> _mockRepository;
        private Mock<IUnitOfWork> _uniOfWork;
        private IPostCategoryService _postCategoryService;
        private List<PostCategory> _listPostCategory;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IPostCategoryRepository>();
            _uniOfWork = new Mock<IUnitOfWork>();
            _postCategoryService = new PostCategoryService(_mockRepository.Object, _uniOfWork.Object);
            _listPostCategory = new List<PostCategory>()
            {
                new PostCategory(){ID=1,Name="P1",Alias="p1",Status=true},
                new PostCategory(){ID=2,Name="P2",Alias="p2",Status=true},
                new PostCategory(){ID=3,Name="P3",Alias="p3",Status=true},
            };
        }

        [TestMethod]
        public void PostCategoryService_GetAll()
        {
            //setup method
            _mockRepository.Setup(m => m.GetAll(null)).Returns(_listPostCategory);
            //call method
            var result = _postCategoryService.GetAll() as List<PostCategory>;
            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void PostCategoryService_Create()
        {
            PostCategory postCategory = new PostCategory();
            postCategory.Name = "Post1";
            postCategory.Alias = "post1";
            postCategory.Status = true;

            _mockRepository.Setup(m => m.Add(postCategory)).Returns((PostCategory p) =>
            {
                p.ID = 1;
                return p;
            });
                

            var retult = _postCategoryService.Add(postCategory);
            Assert.IsNotNull(retult);
            Assert.AreEqual(1, retult);

        }
    }
}