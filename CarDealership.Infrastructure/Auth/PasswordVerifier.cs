﻿using CarDealership.Application.Auth;

namespace CarDealership.Infrastructure.Auth
{
    public class PasswordVerifier : IPasswordVerifier
    {
        public bool Verify(string password, string hashPassword) =>
            BCrypt.Net.BCrypt.EnhancedVerify(password, hashPassword);
    }
}
