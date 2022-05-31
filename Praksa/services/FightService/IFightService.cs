using Praksa.Dtos.Fight;
using Praksa.Models;
using System.Threading.Tasks;

namespace Praksa.services.FightService
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);
        Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request);
    }
}
