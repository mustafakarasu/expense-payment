using AutoMapper;
using Business.Services;
using DataAccess.Repositories.Abstractions;

namespace Business.Managers
{
    public class EmployeeManager : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
        private readonly IRepositoryService _repository;

        public EmployeeManager(IMapper mapper, IAuthenticationService authenticationService, IRepositoryService repository)
        {
            _mapper = mapper;
            _authenticationService = authenticationService;
            _repository = repository;
        }
    }
}
