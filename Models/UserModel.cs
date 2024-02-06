using System.ComponentModel.DataAnnotations;

namespace kolisale.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Display(Name = "username")]
        public string UserName { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
    }

    public class Logentry
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime When { get; set; }
    }
}
