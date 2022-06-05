using System;

namespace HighRiskDiseaseSurvellance.Infrastructure
{
    public class DomainException : Exception
    {
        public object ErrorCode { get; protected set; }

        public DomainException(object errorCode, string message = null, Exception innerException = null)
            :base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
