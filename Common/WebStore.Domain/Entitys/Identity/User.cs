using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.Entitys.Identity
{
    public class User : IdentityUser
    {
        public const string Administrator = "Admin";
        public const string AdmPass = "Adm1@";
    }
}
