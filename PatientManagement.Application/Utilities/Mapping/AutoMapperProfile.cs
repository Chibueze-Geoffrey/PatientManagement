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
            // Map between PatientDto and Patient
            CreateMap<PatientDto, Patient>().ReverseMap();

            // Map between Patient and PatientResponse
            CreateMap<Patient, PatientResponse>().ReverseMap();

            // Map between Patient and PatientUpdateResponseDto
            CreateMap<Patient, PatientUpdateResponseDto>().ReverseMap();

            // Map between PatientUpdateDto and Patient
            CreateMap<PatientUpdateDto, Patient>().ReverseMap();
        }
    }
}