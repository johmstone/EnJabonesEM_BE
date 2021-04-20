using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Property
    {
        [Key]
        public int PropertyID { get; set; }

        public string PropertyName { get; set; }

        public string ActionType { get; set; }
    }
}
