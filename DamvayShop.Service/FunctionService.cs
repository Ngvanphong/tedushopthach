using System;
using System.Collections.Generic;
using System.Linq;
using DamvayShop.Data.Inframestructure;
using DamvayShop.Data.Reponsitories;
using DamvayShop.Model.Models;

namespace DamvayShop.Service
{
    public interface IFunctionService
    {
        Function Create(Function function);

        IEnumerable<Function> GetAll(string filter);

        IEnumerable<Function> GetAllWithPermission(string userId);

        IEnumerable<Function> GetAllWithParentID(string parentId);

        Function Get(string id);

        void Update(Function function);

        void Delete(string id);

        void SaveChange();

        bool CheckExistedId(string id);
    }

    public class FunctionService : IFunctionService
    {
        private IFunctionRepository _functionRepository;
        private IUnitOfWork _unitOfWork;

        public FunctionService(IFunctionRepository functionRepository, IUnitOfWork unitOfWork)
        {
            this._functionRepository = functionRepository;
            this._unitOfWork = unitOfWork;
        }

        public bool CheckExistedId(string id)
        {
            return _functionRepository.CheckContains(x => x.ID == id);
        }

        public Function Create(Function function)
        {
            return _functionRepository.Add(function);
        }

        public void Delete(string id)
        {
            var fuction = _functionRepository.GetSingleByCondition(x => x.ID == id);
            _functionRepository.Delete(fuction);
        }

        public Function Get(string id)
        {
            return _functionRepository.GetSingleByCondition(x => x.ID == id);
        }

        public IEnumerable<Function> GetAll(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                IEnumerable<Function> query = _functionRepository.GetMulti(x => x.Status).OrderBy(x => x.ParentId);
                return query;
            }
            else
            {
                IEnumerable<Function> query = _functionRepository.GetMulti(x => x.Name.Contains(filter) && x.Status).OrderBy(x => x.ParentId);
                return query;
            }
        }

        public IEnumerable<Function> GetAllWithParentID(string parentId)
        {
            return _functionRepository.GetMulti(x => x.ParentId == parentId);
        }

        public IEnumerable<Function> GetAllWithPermission(string userId)
        {
            var query = _functionRepository.GetListFunctionWithPermission(userId);
            return query.OrderBy(x => x.ParentId);
        }

        public void SaveChange()
        {
            _unitOfWork.Commit();
        }

        public void Update(Function function)
        {
            _functionRepository.Update(function);
        }
    }
}