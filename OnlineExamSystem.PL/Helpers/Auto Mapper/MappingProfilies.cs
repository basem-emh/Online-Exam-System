using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OnlineExamSystem.PL.ViewModels.Identity;

namespace OnlineExamSystem.PL.Helpers.Auto_Mapper
{
    public class MappingProfilies : Profile
    {
        public MappingProfilies()
        {
            CreateMap<IdentityRole, RoleViewModel>()
                .ForMember(d=>d.RoleName,o=>o.MapFrom(s=>s.Name))
                .ReverseMap();
        }
    }
}
