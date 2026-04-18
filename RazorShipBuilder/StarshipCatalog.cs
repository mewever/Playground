using Microsoft.AspNetCore.Components.Web;
using RazorShipBuilder.Data;

namespace RazorShipBuilder
{
    public static class StarshipCatalog
    {
        public static ModelOption[] Models = [
            new ModelOption()
            {
                Name = "Star Hopper",
                GeneratorOptions = [
                    new PowerGeneratorOption() {
                        Name = "Standard",
                        PowerGeneration = 100,
                        MaximumPower = 300,
                        Cost = 100,
                    },
                    new PowerGeneratorOption() {
                        Name = "Heavy Duty",
                        PowerGeneration = 100,
                        MaximumPower = 500,
                        Cost = 200,
                    },
                    new PowerGeneratorOption() {
                        Name = "Dual Cycle",
                        PowerGeneration = 150,
                        MaximumPower = 450,
                        Cost = 200
                    }
                ],
                MountPoints = [
                    new ModelMountPoint() {
                        Name = "Fixed Port",
                        FacingDirection = FacingDirection.Forward,
                        IsTurret = false,
                    },
                    new ModelMountPoint() {
                        Name = "Fixed Starboard",
                        FacingDirection = FacingDirection.Forward,
                        IsTurret = false,
                    },
                    new ModelMountPoint() {
                        Name = "Shield Mount",
                        FacingDirection = FacingDirection.Top,
                        IsTurret = false,
                    }
                    ],
                HullCost = 300
            },

            new ModelOption()
            {
                Name = "Hunter",
                GeneratorOptions = [
                    new PowerGeneratorOption() {
                        Name = "Introductory",
                        PowerGeneration = 100,
                        MaximumPower = 500,
                        Cost = 200,
                    },
                    new PowerGeneratorOption() {
                        Name = "Seasoned",
                        PowerGeneration = 150,
                        MaximumPower = 500,
                        Cost = 300,
                    },
                    new PowerGeneratorOption() {
                        Name = "Veteran",
                        PowerGeneration = 150,
                        MaximumPower = 700,
                        Cost = 400
                    }
                ],
                MountPoints = [
                    new ModelMountPoint() {
                        Name = "Fixed Port Outer",
                        FacingDirection = FacingDirection.Forward,
                        IsTurret = false,
                    },
                    new ModelMountPoint() {
                        Name = "Fixed Port Inner",
                        FacingDirection = FacingDirection.Forward,
                        IsTurret = false,
                    },
                    new ModelMountPoint() {
                        Name = "Fixed Starboard Inner",
                        FacingDirection = FacingDirection.Forward,
                        IsTurret = false,
                    },
                    new ModelMountPoint() {
                        Name = "Fixed Starboard Outer",
                        FacingDirection = FacingDirection.Forward,
                        IsTurret = false,
                    },
                    new ModelMountPoint() {
                        Name = "Rear Turret",
                        FacingDirection = FacingDirection.Rear,
                        IsTurret = true,
                    },
                    new ModelMountPoint() {
                        Name = "Shield Mount",
                        FacingDirection = FacingDirection.Top,
                        IsTurret = false,
                    }
                    ],
                HullCost = 500
            },


            new ModelOption()
            {
                Name = "Merchant",
                GeneratorOptions = [
                    new PowerGeneratorOption() {
                        Name = "Silver",
                        PowerGeneration = 100,
                        MaximumPower = 400,
                        Cost = 200,
                    },
                    new PowerGeneratorOption() {
                        Name = "Gold",
                        PowerGeneration = 100,
                        MaximumPower = 500,
                        Cost = 300,
                    },
                    new PowerGeneratorOption() {
                        Name = "Platinum",
                        PowerGeneration = 150,
                        MaximumPower = 600,
                        Cost = 400
                    }
                ],
                MountPoints = [
                    new ModelMountPoint() {
                        Name = "Upper Turret",
                        FacingDirection = FacingDirection.Top,
                        IsTurret = true,
                    },
                    new ModelMountPoint() {
                        Name = "Lower Turret",
                        FacingDirection = FacingDirection.Bottom,
                        IsTurret = true,
                    },
                    new ModelMountPoint() {
                        Name = "Tractor Mount",
                        FacingDirection = FacingDirection.Rear,
                        IsTurret = true,
                    },
                    new ModelMountPoint() {
                        Name = "Shield Mount",
                        FacingDirection = FacingDirection.Top,
                        IsTurret = false,
                    }
                    ],
                HullCost = 500
            },
            ];
    }

    public class ModelOption
    {
        public string Name { get; set; } = string.Empty;
        public PowerGeneratorOption[] GeneratorOptions { get; set; } = [];
        public ModelMountPoint[] MountPoints { get; set; } = [];
        public int HullCost { get; set; }
    }

    public class ModelMountPoint
    {
        public string Name { get; set; } = string.Empty;
        public FacingDirection FacingDirection { get; set; } = FacingDirection.Forward;
        public bool IsTurret { get; set; } = false;
    }

    public class PowerGeneratorOption
    {
        public string Name { get; set; } = string.Empty;
        public int PowerGeneration { get; set; } // Units per second
        public int MaximumPower { get; set; } // The maximum number of units that can be stored
        public int Cost { get; set; }
    }
}
