using Microsoft.Extensions.Primitives;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorShipBuilder.Data
{
    public class Starship
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(50)]
        [DisplayName("Model")]
        public string ModelName { get; set; } = string.Empty;
        [MaxLength(50)]
        [DisplayName("Generator")]
        public string GeneratorName { get; set; } = string.Empty;
        [DisplayName("Power Generation")]
        public int PowerGeneration { get; set; }
        [DisplayName("Maximum Power")]
        public int MaximumPower { get; set; }
        public int Cost { get; set; }

        public List<MountPoint> MountPoints { get; set; } = new List<MountPoint>();
    }
}
