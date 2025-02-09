using AutoMapper;
using PatientManagement.Common.Dtos.PatientDtos;
using PatientManagement.Common.Dtos.PatientDtos.Request;
using PatientManagement.Common.Dtos.PatientDtos.Response;
using PatientManagement.Domain.Models;

namespace PatientManagement.Application.Utilities.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PatientDto, Patient>().ReverseMap();
            CreateMap<Patient, PatientResponse>().ReverseMap();
            CreateMap<Patient, PatientUpdateResponseDto>().ReverseMap();
            CreateMap<PatientUpdateDto, Patient>().ReverseMap();

        }
    }
}
