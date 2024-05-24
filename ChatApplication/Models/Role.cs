using System.ComponentModel.DataAnnotations;

namespace ChatApplication.Models
{
    public class Role
    {
        [Key]
        public int id { get; set; }
        public string rolename { get; set; }
    }
}
