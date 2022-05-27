namespace Praksa.Dtos.Weapon
{
    public interface AddWeaponDto
    {
        public string Name { get; set; }
        public int damage { get; set; }
        public int CharacterId { get; set; }
    }
}
