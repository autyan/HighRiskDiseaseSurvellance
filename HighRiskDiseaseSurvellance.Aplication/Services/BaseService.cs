using HighRiskDiseaseSurvellance.Persistence;
using Microsoft.Extensions.Logging;

namespace HighRiskDiseaseSurvellance.Aplication.Services
{
    public abstract class BaseService
    {
        protected SurveillanceContext DbContext { get; private set; }
        protected ILogger Logger { get; private set; }

        protected BaseService(SurveillanceContext dbContext, ILogger logger)
        {
            DbContext = dbContext;
            Logger    = logger;
        }
    }
}
