﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace TheBackend.Models
{
    public partial class User
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
    }
}
