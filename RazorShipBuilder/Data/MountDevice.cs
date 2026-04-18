using System.ComponentModel.DataAnnotations;

namespace RazorShipBuilder.Data
{
    public class MountDevice
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public bool RequiresTurret { get; set; }
        public bool IsContinualUse { get; set; }
        public int ReloadDuration { get; set; } // Seconds after use before the device can be used again
        public int PowerDraw { get; set; } // Per shot (or per second for continual use)
        public int TractorPower { get; set; }
        public int EnergyDamage { get; set; }
        public int PhysicalDamage { get; set; }
        public int EnergyShield { get; set; }
        public int PhysicalShield { get; set; }
    }
}
