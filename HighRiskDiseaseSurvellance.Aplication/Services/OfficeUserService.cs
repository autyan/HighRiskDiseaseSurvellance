using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HighRiskDiseaseSurvellance.Dto;
using HighRiskDiseaseSurvellance.Dto.Models;
using HighRiskDiseaseSurvellance.Dto.Requests;
using HighRiskDiseaseSurvellance.Infrastructure;
using HighRiskDiseaseSurvellance.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HighRiskDiseaseSurvellance.Aplication.Services
{
    public class OfficeUserService : BaseService
    {
        public OfficeUserService(SurveillanceContext        dbContext, 
                                 ILogger<OfficeUserService> logger) : base(dbContext, logger)
        {

        }

        public async Task<PasswordSignIn> PasswordSignInAsync(PasswordSignInRequest request)
        {
            var officeUser = await DbContext.OfficeUsers.FirstOrDefaultAsync(u => u.Name == request.UserName);
            if (officeUser == null)
            {
                throw new DomainException(ErrorCode.OfficeUserNotFound);
            }

            var       hashedPassword = string.Concat(request.Password, officeUser.Salt);
            using var sha256         = SHA256.Create();
            var       hash           = sha256.ComputeHash(Encoding.UTF8.GetBytes(hashedPassword));
            var       storeHash      = Convert.ToBase64String(hash);

            if (officeUser.PasswordHash != storeHash)
            {
                throw new DomainException(ErrorCode.OfficeUserAuthFailed);
            }

            return new PasswordSignIn
                   {
                       Id   = officeUser.Id,
                       Name = officeUser.Name
                   };
        }

    }
}
