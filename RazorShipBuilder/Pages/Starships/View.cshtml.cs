using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorShipBuilder.Data;

namespace RazorShipBuilder.Pages.Starships
{
    public class ViewModel(ApplicationDbContext context) : PageModel
    {
        public Starship Starship { get; set; } = new Starship();

        public async Task<ActionResult> OnGet(int? itemId)
        {
            if (itemId == null)
            {
                return RedirectToPage("/Index");
            }
            var starship = await context.Starships
                .Include(s => s.MountPoints)
                .ThenInclude(mp => mp.Device)
                .FirstOrDefaultAsync(s => s.Id == itemId);
            if (starship == null)
            {
                return RedirectToPage("/Index");
            }
            Starship = starship;
            return Page();
        }
    }
}
