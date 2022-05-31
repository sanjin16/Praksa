using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Praksa.Data;
using Praksa.Dtos.Fight;
using Praksa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Praksa.services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FightService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == request.AttackerId);

                var opponnent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == request.OpponnentId);

                int damage = DoWeaponAttack(attacker, opponnent);

                if (opponnent.HitPoints <= 0)
                    response.message = $"{opponnent.Name} has been defeated!";

                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    AttackerHP = attacker.HitPoints,
                    Opponnent = opponnent.Name,
                    OpponnentHP = opponnent.HitPoints,
                    Damage = damage
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.message = ex.Message;
            }
            return response;
        }

        private static int DoWeaponAttack(Character attacker, Character opponnent)
        {
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
            damage -= new Random().Next(opponnent.Defense);

            if (damage > 0)
                opponnent.HitPoints -= damage;
            return damage;
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == request.AttackerId);

                var opponnent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == request.OpponnentId);

                var skill = attacker.Skills.FirstOrDefault(s => s.Id == request.SkillId);

                if (skill == null)
                {
                    response.Success = false;
                    response.message = $"{attacker.Name} can not use that skill.";
                    return response;
                }

                int damage = DoSkillAttack(attacker, opponnent, skill);

                if (opponnent.HitPoints <= 0)
                    response.message = $"{opponnent.Name} has been defeated!";

                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    AttackerHP = attacker.HitPoints,
                    Opponnent = opponnent.Name,
                    OpponnentHP = opponnent.HitPoints,
                    Damage = damage
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.message = ex.Message;
            }
            return response;
        }

        private static int DoSkillAttack(Character attacker, Character opponnent, Skill skill)
        {
            int damage = skill.Damage + (new Random().Next(attacker.Intelligence));
            damage -= new Random().Next(opponnent.Defense);

            if (damage > 0)
                opponnent.HitPoints -= damage;
            return damage;
        }

        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request)
        {
            var response = new ServiceResponse<FightResultDto>()
            {
                Data = new FightResultDto()
            };
            try 
	        {
                var characters = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .Where(c => request.CharacterIds.Contains(c.Id)).ToListAsync();

                bool defeated = false;
                while (!defeated)
                {
                    foreach (var attacker in characters)
                    {
                        var opponnents = characters.Where(c => c.Id != attacker.Id).ToList();
                        var opponnent = opponnents[new Random().Next(opponnents.Count)];

                        int damage = 0;
                        string attackUsed = string.Empty;

                        bool useWeapon = new Random().Next(2) == 0;
                        if (useWeapon)
                        {
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, opponnent);
                        }
                        else
                        {
                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker, opponnent, skill);
                        }
                        response.Data.Log
                            .Add($"{attacker.Name} attacks {opponnent.Name} using {attackUsed} with {(damage > 0 ? damage : 0)} damage.");

                        if (opponnent.HitPoints <= 0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponnent.Defeats++;
                            response.Data.Log.Add($"{opponnent.Name} is defeated!");
                            response.Data.Log.Add($"{attacker.Name} wins with {attacker.HitPoints}HP left.");
                            break;
                        }
                    }
                }
                characters.ForEach(c =>
                {
                    c.Fights++;
                    c.HitPoints = 100;
                });
                await _context.SaveChangesAsync();
	        }
	        catch (Exception ex)
	        {
                response.Success = false;
                response.message = ex.Message;
	        }
            return response;
        }

        public async Task<ServiceResponse<List<HighScoreDto>>> GetHighScore()
        {
            var characters = await _context.Characters
                .Where(c => c.Fights > 0)
                .OrderByDescending(c => c.Victories)
                .ThenBy(c => c.Defeats)
                .ToListAsync();

            var response = new ServiceResponse<List<HighScoreDto>>
            {
                Data = characters.Select(c => _mapper.Map<HighScoreDto>(c)).ToList()
            };

            return response;
        }
    }
}
