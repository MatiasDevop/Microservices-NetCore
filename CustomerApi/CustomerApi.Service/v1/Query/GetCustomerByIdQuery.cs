using CustomerApi.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApi.Service.v1.Query
{
    public class GetCustomerByIdQuery : IRequest<Customer>
    {
        public Guid Id { get; set; }
    }
}
