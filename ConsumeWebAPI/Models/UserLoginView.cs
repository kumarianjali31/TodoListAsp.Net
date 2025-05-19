using System.ComponentModel.DataAnnotations;

namespace ConsumeWebAPI.Models
{
    public class UserLoginView
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
