using Microsoft.AspNet.Identity.EntityFramework;

namespace WebKhoaLuan.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Email { get; set; }
        public bool ConfirmedEmail { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}