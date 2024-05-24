using System.ComponentModel.DataAnnotations;

namespace ChatApplication.Models
{
    public class Message
    {
        [Key]
        public int id { get; set; }
        public int senderid { get; set; }
        public string sendername { get; set; }
        public int receiverid { get; set; }
        public string receivername { get; set; }
        public DateTime datetime { get; set; }
        public string content { get; set; }
    }
}
