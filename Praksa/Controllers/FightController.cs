using Microsoft.AspNetCore.Mvc;
using Praksa.Dtos.Fight;
using Praksa.Models;
using Praksa.services.FightService;
using System.Threading.Tasks;

namespace Praksa.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class FightController : ControllerBase
    {
        private readonly IFightService _fightService;

        public FightController(IFightService fightService)
        {
            _fightService = fightService;
        }

        [HttpPost("Weapon")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack(WeaponAttackDto request)
        {
            return Ok(await _fightService.WeaponAttack(request));
        }

        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> SkillAttack(SkillAttackDto request)
        {
            return Ok(await _fightService.SkillAttack(request));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<FightResultDto>>> Fight(FightRequestDto request)
        {
            return Ok(await _fightService.Fight(request));
        }
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<HighScoreDto>>> GetHighScore()
        {
            return Ok(await _fightService.GetHighScore());
        }
    }
}
