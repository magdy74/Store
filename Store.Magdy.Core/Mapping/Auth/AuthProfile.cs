﻿using AutoMapper;
using Store.Magdy.Core.Dtos.Auth;
using Store.Magdy.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Magdy.Core.Mapping.Auth
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<Address, AddressDto>()
                .ForMember(d => d.FirstName, options => options.MapFrom(d => d.FName))
                .ForMember(d => d.LastName, options => options.MapFrom(d => d.LName))
                .ReverseMap();
        }
    }
}
