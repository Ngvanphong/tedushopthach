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
    public interface IFooterService
    {
        Footer GetAll();
        void Add(Footer footer);
        void Update(Footer footer);
        void SaveChange();
    }
    public class FooterService:IFooterService
    {
        private IFooterRepository _footerRepository;
        private IUnitOfWork _unitOfWork;
        public FooterService(IFooterRepository footerRepository, IUnitOfWork unitOfWork)
        {
            this._footerRepository = footerRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Add(Footer footer)
        {
            _footerRepository.Add(footer);
        }

        public Footer GetAll()
        {
           return _footerRepository.GetAll().SingleOrDefault();
        }

        public void SaveChange()
        {
            _unitOfWork.Commit();
        }

        public void Update(Footer footer)
        {
            _footerRepository.Update(footer);
        }
    }
}
