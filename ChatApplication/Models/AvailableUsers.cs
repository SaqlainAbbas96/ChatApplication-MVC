using System.ComponentModel.DataAnnotations;

namespace ChatApplication.Models
{
    public class AvailableUsers
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public string connectionid { get; set; }
     }
}
