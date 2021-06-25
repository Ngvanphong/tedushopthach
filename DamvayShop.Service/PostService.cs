using System.Collections.Generic;
using System.Linq;
using DamvayShop.Common;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Data.Reponsitories;
using DamvayShop.Model.Models;

namespace DamvayShop.Service
{
    public interface IPostService
    {
        void Add(Post post);

        void Update(Post post);

        void Delete(string id);

        IEnumerable<Post> GetAll();
  
        IEnumerable<Post> GetAllPaging(int page, int pageSize, out int totalRow);

        IEnumerable<Post> GetByCategoryPaging(int id,int page, int pageSize, out int totalRow);

        Post GetById(string id);

        IEnumerable<Post> GetAllByTagPaging(string tag, int page, int pageSize, out int totalRow);

        void SaveChanges();
    }

    public class PostService : IPostService
    {
        private IPostRepository _postRepository;
        private IUnitOfWork _unitOfWork;
        private ITagRepository _tagReponsitory;
        private IPostTagRepository _postTagRepository;
        public PostService(IPostRepository postRepository, IUnitOfWork unitOfWork, ITagRepository tagReponsitory, IPostTagRepository postTagRepository)
        {
            this._postRepository = postRepository;
            this._unitOfWork = unitOfWork;
            this._tagReponsitory = tagReponsitory;
            this._postTagRepository = postTagRepository;
        }

        public void Add(Post post)
        {
            Post query = _postRepository.Add(post);
            _unitOfWork.Commit();
            if (!string.IsNullOrEmpty(post.Tags))
            {
                string[] listTag = post.Tags.Split(',');
                for (int i = 0; i < listTag.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(listTag[i]);
                    if (_tagReponsitory.Count(x => x.ID == tagId) == 0)
                    {
                        Tag tag = new Tag()
                        {
                            ID = tagId,
                            Name = listTag[i],
                            Type = CommonConstant.PostTag,
                        };
                        _tagReponsitory.Add(tag);
                    }
                    PostTag postTag = new PostTag()
                    {
                        PostID = query.ID,
                        TagID = tagId,
                    };
                    _postTagRepository.Add(postTag);
                }
            }
           
        }

        public void Delete(string id)
        {

            _postRepository.DeleteMulti(x => x.ID == id);
        }

        public IEnumerable<Post> GetAll()
        {
            return this._postRepository.GetAll(new string[] { "PostCategory" });
        }

        public IEnumerable<Post> GetAllByTagPaging(string tag, int page, int pageSize, out int totalRow)
        {
            return this._postRepository.GetAllByTag(tag, page, pageSize, out totalRow);
        }

        public IEnumerable<Post> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return this._postRepository.GetMultiPaging(x => x.Status, out totalRow, page, pageSize);
        }

        public IEnumerable<Post> GetByCategoryPaging(int id, int page, int pageSize, out int totalRow)
        {
            IEnumerable<Post> query = _postRepository.GetMulti(x => x.Status == true && x.CategoryID == id).OrderByDescending(x => x.UpdatedDate);
            totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            return query;
        }


        public Post GetById(string id)
        {
            return this._postRepository.GetSingleByCondition(x => x.ID == id,new string[] { "PostCategory" });
        }

        public void SaveChanges()
        {
            this._unitOfWork.Commit();
        }

        public void Update(Post post)
        {
           this._postRepository.Update(post);
            _unitOfWork.Commit();
            if (!string.IsNullOrEmpty(post.Tags))
            {
                string[] listTag = post.Tags.Split(',');
                for (int i = 0; i < listTag.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(listTag[i]);
                    if (_tagReponsitory.Count(x => x.ID == tagId) == 0)
                    {
                        Tag tag = new Tag()
                        {
                            ID = tagId,
                            Name = listTag[i],
                            Type = CommonConstant.PostTag,
                        };
                        _tagReponsitory.Add(tag);
                    }
                    int coutPostTag = _postTagRepository.GetMulti(x => (x.PostID == post.ID && x.TagID == tagId)).Count();
                    if (coutPostTag == 0)
                    {
                        PostTag postTag = new PostTag()
                        {
                            PostID = post.ID,
                            TagID = tagId,
                        };
                        _postTagRepository.Add(postTag);
                    }
                }
            }

        }
    }
}