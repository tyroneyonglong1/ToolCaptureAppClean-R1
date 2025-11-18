using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToolCaptureAppClean.Models
{
    public class ToolTest
    {
        public int Id { get; set; }

        [Required, Display(Name = "Test Tool ID")]
        public string ToolId { get; set; } = "";

        [Display(Name = "Test Date")]
        public DateTime TestDate { get; set; } = DateTime.Today;

        public string? Operator { get; set; }
        public string? Material { get; set; }

        // Choice later in UI: Milling / Drilling / Turning
        public string? Operation { get; set; }

        public string? Customer { get; set; }

        public string? Coolant { get; set; }
        public string? Notes { get; set; }

        // semicolon-separated paths
        public string? PhotoPath { get; set; }

        public List<BrandResult> BrandResults { get; set; } = new();
    }
}
