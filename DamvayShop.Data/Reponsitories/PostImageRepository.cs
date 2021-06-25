using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Model.Models;

namespace DamvayShop.Data.Reponsitories
{
    public interface IPostImageRepository : IRepository<PostImage>
    {

    }
   public class PostImageRepository: RepositoryBase<PostImage>, IPostImageRepository
    {
        public PostImageRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }

    }
}
