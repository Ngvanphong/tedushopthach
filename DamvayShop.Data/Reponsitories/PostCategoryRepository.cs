using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Model.Models;

namespace DamvayShop.Data.Reponsitories
{
    public interface IPostCategoryRepository: IRepository<PostCategory>
    {
        

    }
    public class PostCategoryRepository:RepositoryBase<PostCategory>,IPostCategoryRepository
    {
        public PostCategoryRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }

       
    }
}
