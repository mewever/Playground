using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorShipBuilder.Data;

namespace RazorShipBuilder.Pages.Starships
{

    public class AddModel(ApplicationDbContext context, ILogger<AddModel> logger) : PageModel
    {
        public Starship Starship { get; set; } = new Starship();
        public List<ModelCardOption> ModelOptions { get; set; } = new();

        public void OnGet()
        {
            Starship = new Starship();

            ModelOptions.Clear();
            foreach (var model in StarshipCatalog.Models)
            {
                ModelOptions.Add(new ModelCardOption(model));
            }
        }

        public async Task<IActionResult> OnPost()
        {
            var newStarship = new Starship();
            if (await TryUpdateModelAsync(
                newStarship,
                "Starship",
                s => s.Name,
                s => s.ModelName))
            {
                // Default to the first generator in the list
                var model = StarshipCatalog.Models.FirstOrDefault(m => m.Name == newStarship.ModelName);
                var generatorOption = model?.GeneratorOptions.FirstOrDefault();
                newStarship.GeneratorName = generatorOption?.Name ?? "";
                newStarship.PowerGeneration = generatorOption?.PowerGeneration ?? 0;
                newStarship.MaximumPower = generatorOption?.MaximumPower ?? 0;
                newStarship.Cost = (model?.HullCost + generatorOption?.Cost) ?? 0;

                // Set the mount points based on the model
                List<MountPoint> mountPoints = new();
                foreach (var mp in model?.MountPoints ?? [])
                {
                    mountPoints.Add(new MountPoint()
                    {
                        Name = mp.Name,
                        Direction = mp.FacingDirection,
                        IsTurret = mp.IsTurret,
                        MountedDeviceId = null
                    });
                }
                newStarship.MountPoints = mountPoints;

                context.Starships.Add(newStarship);
                try
                {
                    await context.SaveChangesAsync();
                    // Go to edit page after add is done
                    return RedirectToPage("/Starships/Edit", new { itemId = newStarship.Id });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while adding a starship");
                }
            }
            // Show the page again to resolve errors
            ModelOptions.Clear();
            foreach (var model in StarshipCatalog.Models)
            {
                ModelOptions.Add(new ModelCardOption(model));
            }
            return Page();
        }
    }

    public class ModelCardOption: ModelOption
    {
        public bool IsSelected { get; set; } = false;

        public ModelCardOption(ModelOption source)
        {
            this.Name = source.Name;
            this.GeneratorOptions = source.GeneratorOptions;
            this.MountPoints = source.MountPoints;
            this.HullCost = source.HullCost;
            this.IsSelected = false;
        }
    }
}
