using AutoMapper;
using Praksa.Dtos.Character;
using Praksa.Dtos.Fight;
using Praksa.Dtos.Skill;
using Praksa.Dtos.Weapon;
using Praksa.Models;

namespace Praksa
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<Weapon, GetWeaponDto>();
            CreateMap<Skill, GetSkillDto>();
            CreateMap<Character, HighScoreDto>();
        }
    }
}
