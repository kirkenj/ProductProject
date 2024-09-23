﻿namespace CustomGateway.Models.JWT
{
    public class JwtSettings
    {
        public string Key { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public double DurationInMinutes { get; set; }
        public string SecurityAlgorithm { get; set; } = null!;
    }
}
