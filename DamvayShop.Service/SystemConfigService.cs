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
    public interface ISystemConfigService
    {
        IEnumerable<SystemConfig> GetAll();
        SystemConfig Detail(int id);
        string GetByCode(string code);
        void Delete(int id);
        void Update(SystemConfig systemConfig);
        void Add(SystemConfig systemConfig);
        void SaveChange();
    }
    public class SystemConfigService : ISystemConfigService
    {
        private ISystemConfigRepository _systemConfigRepository;
        private IUnitOfWork _unitOfWork;
        public SystemConfigService(ISystemConfigRepository systemConfigRepository,IUnitOfWork unitOfWork)
        {
            this._systemConfigRepository = systemConfigRepository;
            this._unitOfWork = unitOfWork;
        }
        public void Add(SystemConfig systemConfig)
        {
            _systemConfigRepository.Add(systemConfig);
        }

        public void Delete(int id)
        {
            _systemConfigRepository.Delete(id);
        }

        public SystemConfig Detail(int id)
        {
            return _systemConfigRepository.GetSingleById(id);
        }

        public IEnumerable<SystemConfig> GetAll()
        {
            return _systemConfigRepository.GetAll();
        }

        public string GetByCode(string code)
        {
           return _systemConfigRepository.GetSingleByCondition(x => x.Code == code).ValueString;
        }

        public void SaveChange()
        {
            _unitOfWork.Commit();
        }

        public void Update(SystemConfig systemConfig)
        {
            _systemConfigRepository.Update(systemConfig);
        }
    }
}
