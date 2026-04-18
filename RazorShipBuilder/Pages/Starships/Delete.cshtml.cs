using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorShipBuilder.Data;

namespace RazorShipBuilder.Pages.Starships
{
    public class DeleteModel(ApplicationDbContext context, ILogger<DeleteModel> logger) : PageModel
    {
        public Starship Starship { get; set; } = new Starship();

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

            return Page();
        }

        public async Task<IActionResult> OnPost(int? itemId)
        {
            try
            {
                var dbStarship = await context.Starships
                    .Include(s => s.MountPoints)
                    .FirstOrDefaultAsync(s => s.Id == itemId);
                if (dbStarship == null)
                {
                    return Page();
                }
                context.Starships.Remove(dbStarship);
                await context.SaveChangesAsync();

                // Return to index after update is done
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting a starship.");
            }

            // Show the page again to resolve errors
            var starship = await context.Starships
                .Include(s => s.MountPoints)
                .ThenInclude(mp => mp.Device)
                .FirstOrDefaultAsync(s => s.Id == itemId);
            if (starship != null)
            {
                Starship = starship;
            }
            return Page();
        }
    }
}
