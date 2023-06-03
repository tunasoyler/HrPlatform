using AutoMapper;
using Bilgeadam.HrPlatform.Entities.DTOs;
using Bilgeadam.HrPlatform.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilgeadam.HrPlatform.BLL.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser,SiteManagerDTO>().ReverseMap();
            CreateMap<AppUser,SiteManagerUpdateDTO>().ReverseMap();
            CreateMap<SiteManagerDTO,SiteManagerUpdateDTO>().ReverseMap();//Mapper yapmıyor
            CreateMap<Company, CompanyDto>().ReverseMap();
            CreateMap<Company, CompanyUpdateDtos>().ReverseMap();
            CreateMap<AppUser, CompanyManagerDto>().ReverseMap();
            CreateMap<AppUser, CompanyManagerUpdateDto>().ReverseMap();
            CreateMap<Advance, AdvanceDTO>();
            CreateMap<AdvanceDTO, Advance>();
        }
    }
}
