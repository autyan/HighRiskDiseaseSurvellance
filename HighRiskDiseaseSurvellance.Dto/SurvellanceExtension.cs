using HighRiskDiseaseSurvellance.Dto.Models;

namespace HighRiskDiseaseSurvellance.Dto
{
    public static class SurvellanceExtension
    {
        public static decimal TryUpdateScore(this ISurveillance surveillance, decimal targetScore, decimal currentScore)
        {
            var finalScore = currentScore;
            if (targetScore >= currentScore)
            {
                finalScore = currentScore;
            }

            return finalScore;
        }
    }
}
