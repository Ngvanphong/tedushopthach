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
    public interface ITagService 
    {
        IEnumerable<Tag> GetAll();
        Tag GetDetail(string id);
        void Add(Tag tag);
        void Update(Tag tag);
        void Delete(string id);
        void DeleteMultiNotUse();
        IEnumerable<Tag> GetTagByProductId(int productId);
        void SaveChange();

    }
  public  class TagService:ITagService
    {
        private ITagRepository _tagRepository;
        private IUnitOfWork _unitOfWork;
        private IProductTagRepository _productTagRepository;
        private IPostTagRepository _postTagRepository;
        public TagService(ITagRepository tagRepository, IUnitOfWork unitOfWork, IProductTagRepository productTagRepository, IPostTagRepository postTagRepository)
        {
            this._tagRepository = tagRepository;
            this._unitOfWork = unitOfWork;
            this._productTagRepository = productTagRepository;
            this._postTagRepository = postTagRepository;
        }

        public void Add(Tag tag)
        {
            _tagRepository.Add(tag);
        }

        public void Delete(string id)
        {
            _tagRepository.DeleteMulti(x => x.ID == id);
        }

        public void DeleteMultiNotUse()
        {
            var listProductTag = _productTagRepository.GetAll().Select(x => x.TagID);
            var listPostTag = _postTagRepository.GetAll().Select(x=>x.TagID);
            var listTag = _tagRepository.GetAll().Select(x=>x.ID);
            var listTagUse = listProductTag.Union(listPostTag);
            var listTagNotUse = listTag.Except(listTagUse);
            foreach(var item in listTagNotUse)
            {
                _tagRepository.DeleteMulti(x => x.ID == item);
            }                    
        }

        public IEnumerable<Tag> GetAll()
        {
            return _tagRepository.GetAll().OrderByDescending(x=>x.Type);
        }

       
        public Tag GetDetail(string id)
        {
            return _tagRepository.GetSingleByCondition(x => x.ID == id);
        }

        public IEnumerable<Tag> GetTagByProductId(int productId)
        {
            return _tagRepository.GetTagByProductId(productId);
        }

        public void SaveChange()
        {
            _unitOfWork.Commit();
        }

        public void Update(Tag tag)
        {
            _tagRepository.Update(tag);
        }
    }
}
