using AutoMapper;
using CustomerApi.Domain.Entities;
using CustomerApi.Models.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Infrastructure.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCustomerModel, Customer>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<UpdateCustomerModel, Customer>();
        }
    }
}
