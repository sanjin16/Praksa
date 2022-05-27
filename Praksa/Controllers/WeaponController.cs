using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Praksa.Dtos.Character;
using Praksa.Dtos.Weapon;
using Praksa.Models;
using Praksa.services.WeaponService;
using System.Threading.Tasks;

namespace Praksa.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[Controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService _weaponService;

        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddWeapon(AddWeaponDto newWeapon)
        {
            return Ok(await _weaponService.AddWeapon(newWeapon));
        }
    }
}
