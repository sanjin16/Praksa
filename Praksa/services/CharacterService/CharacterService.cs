using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Praksa.Data;
using Praksa.Dtos.Character;
using Praksa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Praksa.services.CharacterService
{
    public class CharacterService : ICharacterService
        {
        
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
            {
                var ServiceResponse = new ServiceResponse<List<GetCharacterDto>>();
                Character character = _mapper.Map<Character>(newCharacter);
                character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

                _context.Characters.Add(character);
                await _context.SaveChangesAsync();
                ServiceResponse.Data = await _context.Characters
                .Where(c => c.User.Id == GetUserId())
                .Select(c=> _mapper.Map<GetCharacterDto>(c)).ToListAsync();
                return ServiceResponse;
            }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var ServiceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());

                if (character != null)
                {
                    _context.Characters.Remove(character);

                    await _context.SaveChangesAsync();

                    ServiceResponse.Data = _context.Characters
                        .Where(_c => _c.User.Id == GetUserId())
                        .Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
                } else
                {
                    ServiceResponse.Success = false;
                    ServiceResponse.message = "Character not found.";
                }
                return ServiceResponse;
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
                var dbCharacters = await _context.Characters.Where(c => c.User.Id == GetUserId()).ToListAsync();
                ServiceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
                return ServiceResponse;
            }

            public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
            {
                var ServiceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
                ServiceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
                return ServiceResponse;
            }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var ServiceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                Character character = await _context.Characters
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
                if (character.User.Id == GetUserId())
                {

                    character.Name = updatedCharacter.Name;
                    character.Strength = updatedCharacter.Strength;
                    character.Defense = updatedCharacter.Defense;
                    character.HitPoints = updatedCharacter.HitPoints;
                    character.Intelligence = updatedCharacter.Intelligence;
                    character.Class = updatedCharacter.Class;

                    await _context.SaveChangesAsync();

                    ServiceResponse.Data = _mapper.Map<GetCharacterDto>(character);
                } else
                {
                    ServiceResponse.Success = false;
                    ServiceResponse.message = "Character not found.";
                }
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
