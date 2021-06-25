using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Data.Reponsitories;
using DamvayShop.Model.Models;
using DamvayShop.Service;

namespace DamvayShop.UnitTest.ServiceTest
{
    [TestClass]
    public class PostServiceTest
    {
        //private Mock<IPostRepository> _mockRepository;
        //private Mock<IUnitOfWork> _mockUnitOfWork;
        //private IPostService _postService;
        //private List<Post> _listPost;
        //private List<PostCategory> _listPostCategory;

        //[TestInitialize]
        //public void Initialize()
        //{
        //    _mockRepository = new Mock<IPostRepository>();
        //    _mockUnitOfWork = new Mock<IUnitOfWork>();
        //    _postService = new PostService(_mockRepository.Object, _mockUnitOfWork.Object);
        //    _listPost = new List<Post>()
        //    {
        //        new Post(){ID=1,Name="Post 1",Alias="post1",Status=true,CreateDate=DateTime.Now,CategoryID=1, },
        //        new Post(){ID=2, Name="Post 2", Alias="post2", Status=true,CreateDate=DateTime.Now,CategoryID=1},
        //        new Post(){ID=3, Name="Post 3", Alias="post3",Status=true,CreateDate=DateTime.Now,CategoryID=1},
        //    };
        //    _listPostCategory = new List<PostCategory>()
        //    {
        //        new PostCategory(){ID=1,Name="Post Category 1", Alias="postcategory1", Status=true, CreateDate=DateTime.Now},
        //    };
        //}

        //[TestMethod]
        //public void PostService_GetAll()
        //{
        //    _mockRepository.Setup(m => m.GetAll(null)).Returns(_listPost);
        //    var result = _postService.GetAll() as List<Post>;
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(3, result.Count);
        //}

        //[TestMethod]
        //public void PostService_Create()
        //{
        //    Post post = new Post();
        //    post.Name = "Post 4";
        //    post.Alias = "post4";
        //    post.Status = true;
        //    _mockRepository.Setup(m => m.Add(post)).Returns((Post p) =>
        //    {
        //        p.ID = 7;
        //        return p;
        //    });
        //    var result = _postService.Add(post);
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(7, result);
        //}
    }
}