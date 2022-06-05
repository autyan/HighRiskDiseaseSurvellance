using System.ComponentModel.DataAnnotations;

namespace aspnetapp.Models
{
    public class LoginViewModel
    {
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name = "登陆密码")]
        public string Password { get; set; }
    }
}
