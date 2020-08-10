using CustomerApi.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApi.Service.v1.Command
{
    public class CreateCustomerCommand : IRequest<Customer>
    {
        public Customer Customer { get; set; }
    }
}
