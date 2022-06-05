using HighRiskDiseaseSurvellance.Infrastructure;

namespace HighRiskDiseaseSurvellance.Domain.Models
{
    public class OfficeUser : AppAggregateRoot
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }

        protected OfficeUser(string name, string passwordHash, string salt)
        {
            Id           = ObjectId.GenerateNewId().ToString();
            Name         = name;
            PasswordHash = passwordHash;
            Salt         = salt;
        }
    }
}
