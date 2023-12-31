﻿using AutoMapper;
using MagicVillaApi.Models;
using MagicVillaApi.Models.Dtos;

namespace MagicVillaApi.Data
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDto>().ReverseMap();
            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaEditDto>().ReverseMap();
        }
    }
}
