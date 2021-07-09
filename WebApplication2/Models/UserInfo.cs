using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Model
{
    public class UserInfo
    {
        public int UserId { get; set; }

        [MaxLength(200)]
        [Column(TypeName = "nvarchar(max)")]
        public string FirstName { get; set; }

        [MaxLength(200)]
        [Column(TypeName = "nvarchar(max)")]
        public string LastName { get; set; }

        [MaxLength(200)]
        [Column(TypeName = "nvarchar(max)")]
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        

    }
    public class Register
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
