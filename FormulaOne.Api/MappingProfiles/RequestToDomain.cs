using AutoMapper;
using FormulaOne.Entities.DbSet;
using FormulaOne.Entities.DTOs;

namespace FormulaOne.Api.MappingProfiles;

public class RequestToDomain : Profile
{
    public RequestToDomain()
    {
        CreateMap<CreateDriverRequest, Driver>();
        CreateMap<UpdateDriverRequest, Driver>();
    }
}