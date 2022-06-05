using System;
using System.Text.Json.Serialization;
using aspnetapp.JsonConverters;
using HighRiskDiseaseSurvellance.Dto.Models;

namespace aspnetapp.TokenProvider
{
    public class AuthToken
    {
        public string TokenType { get; set; }

        public string AccessToken { get; set; }

        [JsonConverter(typeof(DateTimeToRFC2822Converter))]
        public DateTime ExpiresAt { get; set; }

        public AppUserBaseInfo UserInfo { get; set; }
    }
}
