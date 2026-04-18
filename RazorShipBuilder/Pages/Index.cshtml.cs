using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorShipBuilder.Data;

namespace RazorShipBuilder.Pages
{
    public class IndexModel(ApplicationDbContext context) : PageModel
    {
        public Starship[] StarshipList = [];

        public void OnGet()
        {
            StarshipList = context.Starships.ToArray();
        }
    }
}
