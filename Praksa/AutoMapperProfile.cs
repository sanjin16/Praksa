using AutoMapper;
using Praksa.Dtos.Character;
using Praksa.Models;

namespace Praksa
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
        }
    }
}
