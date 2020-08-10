using CustomerApi.Data.Repository.v1;
using CustomerApi.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerApi.Service.v1.Command
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Customer>
    {
        private readonly IRepository<Customer> _repository;
        public CreateCustomerCommandHandler(IRepository<Customer> repository)
        {
            _repository = repository;
        }
        public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            return await _repository.AddAsync(request.Customer);
        }
    }
}
