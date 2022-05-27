using Praksa.Dtos.Character;
using Praksa.Dtos.Weapon;
using Praksa.Models;
using System.Threading.Tasks;

namespace Praksa.services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
    }
}
