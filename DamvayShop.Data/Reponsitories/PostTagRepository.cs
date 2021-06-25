using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Model.Models;

namespace DamvayShop.Data.Reponsitories
{
    public interface IPostTagRepository: IRepository<PostTag>
    {

    }
    public class PostTagRepository:RepositoryBase<PostTag>,IPostTagRepository
    {
        public PostTagRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }
    }
}
