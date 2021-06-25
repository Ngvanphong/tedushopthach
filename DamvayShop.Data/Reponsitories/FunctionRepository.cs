using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Model.Models;

namespace DamvayShop.Data.Reponsitories
{
    public interface IFunctionRepository : IRepository<Function>
    {
        List<Function> GetListFunctionWithPermission(string userId);
    }
    public class FunctionRepository:RepositoryBase<Function>,IFunctionRepository
    {
        public FunctionRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }

        public List<Function> GetListFunctionWithPermission(string userId)
        {
            var query = from fuc in DbContext.Functions
                        join per in DbContext.Permissions on fuc.ID equals per.FunctionId
                        join rol in DbContext.AppRoles on per.RoleId equals rol.Id
                        join urol in DbContext.UserRoles on rol.Id equals urol.RoleId
                        join user in DbContext.Users on urol.UserId equals user.Id
                        where user.Id == userId && (per.CanRead == true)
                        select fuc;
            var parentIds = query.Select(x => x.ParentId).Distinct();
            query = query.Union(DbContext.Functions.Where(f => parentIds.Contains(f.ID)));
            return query.ToList();
        }
    }
}
