using System.Collections.Generic;
using System.Linq;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Model.Models;

namespace DamvayShop.Data.Reponsitories
{
    public interface IPostRepository : IRepository<Post>
    {
        IEnumerable<Post> GetAllByTag(string tag, int page, int pageSize, out int totalRow);
    }

    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Post> GetAllByTag(string tag, int page, int pageSize, out int totalRow)
        {
            var query = from p in DbContext.Posts
                        join pt in DbContext.PostTags
                        on p.ID equals pt.PostID
                        where pt.TagID == tag && p.Status
                        orderby p.UpdatedDate descending
                        select p;
            totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            return query;
        }
    }
}