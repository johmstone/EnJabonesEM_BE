using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class AppDirectory
    {
        [Key]
        public int AppID { get; set; }

        public string AppName { get; set; }

        public int Order { get; set; }

        public bool PrivateSite { get; set; }

        public string URL { get; set; }

        public string Description { get; set; }

        public bool ActiveFlag { get; set; }

        public string ActionType { get; set; }
    }
}
