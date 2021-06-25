using System.Collections.Generic;
using System.Linq;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Data.Reponsitories;
using DamvayShop.Model.Models;

namespace DamvayShop.Service
{
    public interface IPostCategoryService
    {
        int Add(PostCategory postCategory);

        void Delete(int id);

        void Update(PostCategory postCategory);

        IEnumerable<PostCategory> GetAll();

        PostCategory GetDetail (int id);

        IEnumerable<PostCategory> GetAllByParentID(int parentID);

        PostCategory GetByID(int id);

        void SaveChanges();
    }

    public class PostCategoryService : IPostCategoryService
    {
        private IPostCategoryRepository _postCategoryRepository;
        private IUnitOfWork _unitOfWork;

        public PostCategoryService(IPostCategoryRepository postCategoryRepository, IUnitOfWork unitOfWork)
        {
            this._postCategoryRepository = postCategoryRepository;
            this._unitOfWork = unitOfWork;
        }

        public int Add(PostCategory postCategory)
        {
            var query = this._postCategoryRepository.Add(postCategory);
            return query.ID;
        }

        public void Delete(int id)
        {
            this._postCategoryRepository.Delete(id);
        }

        public IEnumerable<PostCategory> GetAll()
        {
            return this._postCategoryRepository.GetAll().OrderBy(x=>x.ParentID);
        }

        public IEnumerable<PostCategory> GetAllByParentID(int parentID)
        {
            return this._postCategoryRepository.GetMulti(x => x.ParentID == parentID && x.Status);
        }

        public PostCategory GetByID(int id)
        {
            return this._postCategoryRepository.GetSingleById(id);
        }

        public PostCategory GetDetail(int id)
        {
           return _postCategoryRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            this._unitOfWork.Commit();
        }

        public void Update(PostCategory postCategory)
        {
            this._postCategoryRepository.Update(postCategory);
        }
    }
}