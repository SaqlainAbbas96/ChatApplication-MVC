using System.ComponentModel.DataAnnotations;

namespace ChatApplication.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string email { get; set; }
        public string username{ get; set; }
        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }
    }
}
