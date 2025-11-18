using System.ComponentModel.DataAnnotations;

namespace ToolCaptureAppClean.Models
{
    public class BrandResult
    {
        public int Id { get; set; }

        public int ToolTestId { get; set; }
        public ToolTest? ToolTest { get; set; }

        [Required]
        public string BrandName { get; set; } = "";

        // Unified fields (we’ll show/hide in UI depending on Operation)
        [Display(Name = "Vc (m/min)")]
        public decimal? Vc { get; set; }

        [Display(Name = "Ae (mm)")]
        public decimal? Ae { get; set; }

        [Display(Name = "Ap (mm)")]
        public decimal? Ap { get; set; }

        [Display(Name = "Fz (mm/tooth)")]
        public decimal? Fz { get; set; }

        [Display(Name = "Flutes")]
        public int? Flutes { get; set; }

        [Display(Name = "Result Notes")]
        public string? ResultNotes { get; set; }
    }
}
