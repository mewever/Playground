using Microsoft.AspNetCore.Identity;
using RazorShipBuilder.Data;

namespace RazorShipBuilder
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Add mount devices to the database if they aren't there yet
            if (!context.MountDevices.Any())
            {
                context.MountDevices.Add(new MountDevice()
                {
                    Name = "Laser Cannon 1",
                    RequiresTurret = false,
                    IsContinualUse = false,
                    ReloadDuration = 2,
                    PowerDraw = 30,
                    TractorPower = 0,
                    EnergyDamage = 10,
                    PhysicalDamage = 0,
                    EnergyShield = 0,
                    PhysicalShield = 0,
                });

                context.MountDevices.Add(new MountDevice()
                {
                    Name = "Laser Cannon 2",
                    RequiresTurret = false,
                    IsContinualUse = false,
                    ReloadDuration = 2,
                    PowerDraw = 35,
                    TractorPower = 0,
                    EnergyDamage = 20,
                    PhysicalDamage = 0,
                    EnergyShield = 0,
                    PhysicalShield = 0,
                });

                context.MountDevices.Add(new MountDevice()
                {
                    Name = "Laser Cannon 3",
                    RequiresTurret = false,
                    IsContinualUse = false,
                    ReloadDuration = 1,
                    PowerDraw = 40,
                    TractorPower = 0,
                    EnergyDamage = 20,
                    PhysicalDamage = 0,
                    EnergyShield = 0,
                    PhysicalShield = 0,
                });

                context.MountDevices.Add(new MountDevice()
                {
                    Name = "Rail Gun",
                    RequiresTurret = false,
                    IsContinualUse = false,
                    ReloadDuration = 4,
                    PowerDraw = 30,
                    TractorPower = 0,
                    EnergyDamage = 0,
                    PhysicalDamage = 20,
                    EnergyShield = 0,
                    PhysicalShield = 0,
                });

                context.MountDevices.Add(new MountDevice()
                {
                    Name = "Torpedo Launcher",
                    RequiresTurret = false,
                    IsContinualUse = false,
                    ReloadDuration = 5,
                    PowerDraw = 10,
                    TractorPower = 0,
                    EnergyDamage = 15,
                    PhysicalDamage = 15,
                    EnergyShield = 0,
                    PhysicalShield = 0,
                });

                context.MountDevices.Add(new MountDevice()
                {
                    Name = "Tractor Beam",
                    RequiresTurret = true,
                    IsContinualUse = true,
                    ReloadDuration = 0,
                    PowerDraw = 20,
                    TractorPower = 10,
                    EnergyDamage = 0,
                    PhysicalDamage = 0,
                    EnergyShield = 0,
                    PhysicalShield = 0,
                });

                context.MountDevices.Add(new MountDevice()
                {
                    Name = "Shield 1",
                    RequiresTurret = false,
                    IsContinualUse = true,
                    ReloadDuration = 0,
                    PowerDraw = 40,
                    TractorPower = 0,
                    EnergyDamage = 0,
                    PhysicalDamage = 0,
                    EnergyShield = 20,
                    PhysicalShield = 0,
                });

                context.MountDevices.Add(new MountDevice()
                {
                    Name = "Shield 2",
                    RequiresTurret = false,
                    IsContinualUse = true,
                    ReloadDuration = 0,
                    PowerDraw = 50,
                    TractorPower = 0,
                    EnergyDamage = 0,
                    PhysicalDamage = 0,
                    EnergyShield = 30,
                    PhysicalShield = 0,
                });

                context.MountDevices.Add(new MountDevice()
                {
                    Name = "Advanced Shield",
                    RequiresTurret = false,
                    IsContinualUse = true,
                    ReloadDuration = 0,
                    PowerDraw = 60,
                    TractorPower = 0,
                    EnergyDamage = 0,
                    PhysicalDamage = 0,
                    EnergyShield = 30,
                    PhysicalShield = 20,
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
