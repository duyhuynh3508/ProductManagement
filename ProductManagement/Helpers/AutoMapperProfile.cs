using AutoMapper;
using ProductManagement;
using ProductManagement.Entities;
using ProductManagement.Model;
using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterModel, UserInfo>();
            CreateMap<CategoryModel, Category>();
            CreateMap<ProductModel, Product>();
        }
    }
}
