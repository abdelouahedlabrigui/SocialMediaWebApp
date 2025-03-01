using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaWebApp.Data.Credentials
{
    public class UserModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? HashedPassword { get; set; }
    }
}