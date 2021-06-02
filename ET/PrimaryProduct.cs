using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class PrimaryProduct
    {
        [Key]
        public int PrimaryProductID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Technique { get; set; }

        public string PhotoURL { get; set; }

        public string BrochureURL { get; set; }

        public bool ActiveFlag { get; set; }

        public bool VisibleFlag { get; set; }

        public string ActionType { get; set; }
    }
}
