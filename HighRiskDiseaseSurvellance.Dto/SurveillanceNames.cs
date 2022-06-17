using System.Collections.Generic;

namespace HighRiskDiseaseSurvellance.Dto
{
    public static class SurveillanceNames
    {
        private static readonly Dictionary<string, string> SurveillanceTypeNames = new();

        public const string Hyperlipidemia       = nameof(Hyperlipidemia);
        public const string Hypertension         = nameof(Hypertension);
        public const string AnginaPectoris       = nameof(AnginaPectoris);
        public const string CardiacInsufficiency = nameof(CardiacInsufficiency);

        static SurveillanceNames()
        {
            SurveillanceTypeNames.Add(Hyperlipidemia, "高血脂风险评估");
            SurveillanceTypeNames.Add(Hypertension, "高血压风险评估");
            SurveillanceTypeNames.Add(AnginaPectoris, "心绞痛严重分级");
            SurveillanceTypeNames.Add(CardiacInsufficiency, "心功能严重分级");
        }

        public static string GetSurveillanceTypeName(string type)
        {
            if (!SurveillanceTypeNames.ContainsKey(type)) return "未知健康监测类型";

            return SurveillanceTypeNames[type];
        }
    }
}
