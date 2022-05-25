using AutoMapper;
using Praksa.Dtos.Character;
using Praksa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Praksa.services.CharacterService
{
    public class CharacterService : ICharacterService
        {
            private static List<Character> characters = new List<Character> {
           new Character(),
           new Character { Id = 1, Name = "Sam"}
        };
        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
            {
                var ServiceResponse = new ServiceResponse<List<GetCharacterDto>>();
                Character character = _mapper.Map<Character>(newCharacter);
                character.Id = characters.Max(c => c.Id)+1;
                characters.Add(character);
                ServiceResponse.Data = characters.Select(c=> _mapper.Map<GetCharacterDto>(c)).ToList();
                return ServiceResponse;
            }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var ServiceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                Character character = characters.First(c => c.Id == id);

                characters.Remove(character);

                ServiceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch (Exception ex)
            {
                ServiceResponse.Success = false;
                ServiceResponse.message = ex.Message;
            }
            return ServiceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
            {
                var ServiceResponse = new ServiceResponse<List<GetCharacterDto>>();
                ServiceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
                return ServiceResponse;
            }

            public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
            {
                var ServiceResponse = new ServiceResponse<GetCharacterDto>();
                ServiceResponse.Data = _mapper.Map<GetCharacterDto>( characters.FirstOrDefault(c => c.Id == id));
                return ServiceResponse;
            }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var ServiceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                Character character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);

                character.Name = updatedCharacter.Name;
                character.Strength = updatedCharacter.Strength;
                character.Defense = updatedCharacter.Defense;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Class = updatedCharacter.Class;

                ServiceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            } 
            catch(Exception ex)
            {
                ServiceResponse.Success = false;
                ServiceResponse.message = ex.Message;
            }
            return ServiceResponse;
         }
    }
}
