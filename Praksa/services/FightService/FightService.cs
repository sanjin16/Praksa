using Microsoft.EntityFrameworkCore;
using Praksa.Data;
using Praksa.Dtos.Fight;
using Praksa.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Praksa.services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;

        public FightService(DataContext context)
        {
            _context = context;
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

                int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
                damage -= new Random().Next(opponnent.Defense);

                if(damage > 0)
                    opponnent.HitPoints -= damage;


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

                if(skill == null)
                {
                    response.Success = false;
                    response.message = $"{attacker.Name} can not use that skill.";
                    return response;
                }

                int damage = skill.Damage + (new Random().Next(attacker.Intelligence));
                damage -= new Random().Next(opponnent.Defense);

                if (damage > 0)
                    opponnent.HitPoints -= damage;

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
    }
}
