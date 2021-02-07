﻿

using jwtLib.JWTAuth.Interfaces;
using System.Collections.Generic;
using BO.Models.MenuApp.DAL.Domain;

namespace BO.Models.DAL.Domain
{
    public class User: IJWTUser
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string RefreshTokenHash { get; set; }

        public List<Article> Articles { get; set; }

        public User()
        {
            Articles = new List<Article>();
        }
    }
}