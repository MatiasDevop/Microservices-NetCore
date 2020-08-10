using CustomerApi.Data.Repository.v1;
using CustomerApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerApi.Service.v1.Query
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Customer>
    {
        private readonly IRepository<Customer> _repository;

        public GetCustomerByIdQueryHandler(IRepository<Customer> repository)
        {
            _repository = repository;
        }

        public async Task<Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAll().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }
    }
}
