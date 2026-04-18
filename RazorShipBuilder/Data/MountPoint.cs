using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorShipBuilder.Data
{
    public class MountPoint
    {
        [Key]
        public int Id { get; set; }
        public int StarshipId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [ForeignKey("Device")]
        public int? MountedDeviceId { get; set; }
        public FacingDirection Direction { get; set; } = FacingDirection.Forward;
        public bool IsTurret { get; set; } // False means it can only fire in the facing direction

        public MountDevice? Device { get; set; }
    }
}
