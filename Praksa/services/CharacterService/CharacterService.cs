using Praksa.Dtos.Character;
using Praksa.Models;
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

            public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
            {
                var ServiceResponse = new ServiceResponse<List<GetCharacterDto>>();
                characters.Add(newCharacter);
                ServiceResponse.Data = characters;
                return ServiceResponse;
            }

            public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
            {
                var ServiceResponse = new ServiceResponse<List<GetCharacterDto>>();
                ServiceResponse.Data = characters;
                return ServiceResponse;
            }

            public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
            {
                var ServiceResponse = new ServiceResponse<GetCharacterDto>();
                ServiceResponse.Data = characters.FirstOrDefault(c => c.Id == id);
                return ServiceResponse;
            }
    }
}
