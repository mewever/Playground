using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorShipBuilder.Data;
using System.Text;

namespace RazorShipBuilder.Pages.Starships
{
    public class EditModel(ApplicationDbContext context, ILogger<EditModel> logger) : PageModel
    {
        public Starship Starship { get; set; } = new Starship();
        public List<PowerGeneratorOption> GeneratorOptions { get; set; } = new();
        public List<MountDevice> DeviceOptions { get; set; } = new();
        public string DeviceOptionsJs { get; set; } = string.Empty;

        public async Task<IActionResult> OnGet(int? itemId)
        {
            var starship = await context.Starships
                .Include(s => s.MountPoints)
                .ThenInclude(mp => mp.Device)
                .FirstOrDefaultAsync(s => s.Id == itemId);
            if (starship == null)
            {
                // Redirect to the index if the starship cannot be found
                return RedirectToPage("/Index");
            }
            Starship = starship;

            LoadOptions(Starship.ModelName);
            return Page();
        }

        public async Task<IActionResult> OnPost(int? itemId)
        {
            var updatedStarship = await context.Starships.FindAsync(itemId);
            if (updatedStarship == null)
            {
                return Page();
            }
            if (await TryUpdateModelAsync(
                updatedStarship,
                "Starship",
                s => s.Name,
                s => s.GeneratorName,
                s => s.MountPoints
                ))
            {
                LoadOptions(updatedStarship.ModelName);

                // Default to the first generator in the list
                var model = StarshipCatalog.Models.FirstOrDefault(m => m.Name == updatedStarship.ModelName);
                var generatorOption = GeneratorOptions.FirstOrDefault(g => g.Name == updatedStarship.GeneratorName);
                updatedStarship.PowerGeneration = generatorOption?.PowerGeneration ?? 0;
                updatedStarship.MaximumPower = generatorOption?.MaximumPower ?? 0;
                updatedStarship.Cost = (model?.HullCost + generatorOption?.Cost) ?? 0;

                context.Starships.Update(updatedStarship);
                try
                {
                    await context.SaveChangesAsync();
                    // Return to index after update is done
                    return RedirectToPage("/Index");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while updating a starship.");
                }
            }

            // Show the page again to resolve errors
            return Page();
        }

        private void LoadOptions(string modelName)
        {
            var model = StarshipCatalog.Models.FirstOrDefault(m => m.Name == modelName);
            GeneratorOptions.Clear();
            foreach (var generator in model?.GeneratorOptions ?? [])
            {
                GeneratorOptions.Add(new GeneratorCardOption(generator));
            }

            DeviceOptions = context.MountDevices.ToList();
            StringBuilder sb = new();
            sb.Append(
                "const deviceOptions = \n" +
                "[\n");
            foreach(var option in DeviceOptions)
            {
                sb.Append(
                    "option = {\n" +
                    $"id: {option.Id},\n" +
                    $"name: '{option.Name}',\n" +
                    $"requiresTurret: {(option.RequiresTurret ? "true" : "false")},\n" +
                    $"isContinualUse: {(option.IsContinualUse ? "true" : "false")},\n" +
                    $"reloadDuration: {option.ReloadDuration},\n" +
                    $"powerDraw: {option.PowerDraw},\n" +
                    $"tractorPower: {option.TractorPower},\n" +
                    $"energyDamage: {option.EnergyDamage},\n" +
                    $"physicalDamage: {option.PhysicalDamage},\n" +
                    $"energyShield: {option.EnergyShield},\n" +
                    $"physicalShield: {option.PhysicalShield},\n" +
                    "},\n");
            }
            sb.Append("];");
            DeviceOptionsJs = sb.ToString();
        }
    }

    public class GeneratorCardOption : PowerGeneratorOption
    {
        public bool IsSelected { get; set; } = false;

        public GeneratorCardOption(PowerGeneratorOption source)
        {
            this.Name = source.Name;
            this.PowerGeneration = source.PowerGeneration;
            this.MaximumPower = source.MaximumPower;
            this.Cost = source.Cost;
            this.IsSelected = false;
        }
    }
}
