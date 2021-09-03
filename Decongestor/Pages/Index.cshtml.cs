using Decongestor.Business;
using Decongestor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Decongestor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IDecongestorBusiness _business;

        public VehicleViewModel[] Vehicles { get; set; }

        [BindProperty]
        public VehicleViewModel VehicleModel { get; set; }

        public IndexModel(IDecongestorBusiness business)
        {
            _business = business;
        }

        public IActionResult OnGet()
        {
            Vehicles = _business.GetAllVehicles();
            return Page();
        }

        public IActionResult OnPost()
        {
            _business.ApplyCharge(VehicleModel.Id);
            return RedirectToPage("/index");
        }
    }
}
