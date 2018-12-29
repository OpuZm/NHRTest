using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Repository;
using Smooth.IoC.UnitOfWork;

namespace OPUPMS.Domain.Restaurant.Services
{
    public class CyxmService : ICyxmService
    {
        readonly IDbFactory _dbFactory;
        readonly ICyxmRepository _cyxmRepository;

        public CyxmService(IDbFactory dbFactory, ICyxmRepository cyxmRepository)
        {
            _dbFactory = dbFactory;
            _cyxmRepository = cyxmRepository;
        }

        public bool Create(R_Project req)
        {
            return _cyxmRepository.Create(req);
        }

        public R_Project GetModel(int id)
        {
            return _cyxmRepository.GetModel(id);
        }

        public List<R_Project> GetList()
        {
            return _cyxmRepository.GetList();
        }
    }
}
