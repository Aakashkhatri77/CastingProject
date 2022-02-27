using static CastingProject.Models.EnumField;

namespace CastingProject.Filter
{
    public class GeneralFilter
    {
        public Gender? Gender { get; set; }
        public SkinColor? SkinColor  { get; set; }
        public string Height { get; set; }
        public string Ethnicity { get; set; }
        public string searchText { get; set; }
        public string Category { get; set; }

    }
}
