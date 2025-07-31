﻿namespace WebAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
        public Guid SessionId { get; set; }

    }
}
