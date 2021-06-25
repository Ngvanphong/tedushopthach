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
    public interface ISupportOnlineService
    {
        SupportOnline Get();
        void Add(SupportOnline supportOnline);
        void Update(SupportOnline supportOnline);
        void SaveChange();
    }
    public class SupportOnlineService : ISupportOnlineService
    {
        ISupportOnlineRepository _supportOnlineRepository;
        IUnitOfWork _unitOfWork;
        public SupportOnlineService(ISupportOnlineRepository supportOnlineRepository,IUnitOfWork unitOfWork)
        {
            this._supportOnlineRepository = supportOnlineRepository;
            this._unitOfWork = unitOfWork;
        }
        public void Add(SupportOnline supportOnline)
        {
            _supportOnlineRepository.Add(supportOnline);
        }

        public SupportOnline Get()
        {
             return  _supportOnlineRepository.GetSingleByCondition(x => x.ID !=null);
        }

        public void SaveChange()
        {
            _unitOfWork.Commit();
        }

        public void Update(SupportOnline supportOnline)
        {
            _supportOnlineRepository.Update(supportOnline);
        }
    }
}
