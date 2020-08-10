using CustomerApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApi.Messaging.Send.Sender.v1
{
    public interface ICustomerUpdateSender
    {
        void SendCustomer(Customer customer);
    }
}
