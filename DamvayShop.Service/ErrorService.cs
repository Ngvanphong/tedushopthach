using DamvayShop.Data.Inframestructure;
using DamvayShop.Data.Reponsitories;
using DamvayShop.Model.Models;

namespace DamvayShop.Service
{
    public interface IErrorService
    {
        Error Create(Error error);

        void SaveChanges();
    }

    public class ErrorService : IErrorService
    {
        private IErrorRepository _errorRepository;
        private IUnitOfWork _unitOfWork;

        public ErrorService(IErrorRepository errorRepository, IUnitOfWork unitOfWork)
        {
            this._errorRepository = errorRepository;
            this._unitOfWork = unitOfWork;
        }

        public Error Create(Model.Models.Error error)
        {
            return this._errorRepository.Add(error);
        }

        public void SaveChanges()
        {
            this._unitOfWork.Commit();
        }
    }
}