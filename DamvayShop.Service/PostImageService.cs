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
    public interface IPostImageService
    {
        void Add(PostImage postImage);
        PostImage GetById(int id);
        IEnumerable<PostImage> getAllByPostId(string postId);
        void Delete(int id);
        void DeleteMultiByPostId(string postId);
        void SaveChange();

    }
    public class PostImageService : IPostImageService
    {
        IPostImageRepository _postImageRepository;
        IUnitOfWork _unitOfWork;

        public PostImageService(IPostImageRepository postImageRepository, IUnitOfWork unitOfWork)
        {
            this._postImageRepository = postImageRepository;
            this._unitOfWork = unitOfWork;
        }
        public void Add(PostImage postImage)
        {
            _postImageRepository.Add(postImage);
        }

        public void Delete(int id)
        {
            _postImageRepository.Delete(id);
        }

        public void DeleteMultiByPostId(string postId)
        {
            _postImageRepository.DeleteMulti(x => x.PostId == postId);
        }

        public IEnumerable<PostImage> getAllByPostId(string postId)
        {
          return  _postImageRepository.GetMulti(x => x.PostId == postId);
        }

        public PostImage GetById(int id)
        {
            return _postImageRepository.GetSingleById(id);
        }

        public void SaveChange()
        {
            _unitOfWork.Commit();
        }

      
    }
}
