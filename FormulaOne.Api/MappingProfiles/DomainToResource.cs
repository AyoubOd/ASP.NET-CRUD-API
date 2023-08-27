using AutoMapper;
using FormulaOne.Entities.DbSet;
using FormulaOne.Entities.Resources;

namespace FormulaOne.Api.MappingProfiles;

public class DomainToResource : Profile
{
    public DomainToResource()
    {
        CreateMap<Driver, DriverResource>()
            .ForMember(
                dest => dest.firstName,
                opt => opt.MapFrom(src => src.FirstName))
            .ForMember(
                dest => dest.lastName,
                opt => opt.MapFrom(src => src.LastName))
            .ForMember(
                dest => dest.driverNumber,
                opt => opt.MapFrom(src => src.DriverNumber))
            .ForMember(
                dest => dest.dateOfBirth,
                opt => opt.MapFrom(src => src.DateOfBirth)
            );
    }
}