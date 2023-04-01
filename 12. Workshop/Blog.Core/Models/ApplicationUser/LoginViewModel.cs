using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Blog.Core.Models.User
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("Username")]
        public string UserName { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
