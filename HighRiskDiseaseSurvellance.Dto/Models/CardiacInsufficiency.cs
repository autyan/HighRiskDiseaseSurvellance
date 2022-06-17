using System.Text.Json.Serialization;

namespace HighRiskDiseaseSurvellance.Dto.Models
{
    public class CardiacInsufficiency : ISurveillance
    {
        [JsonPropertyName("heavyActivity")]
        public HeavyActivity HeavyActivity { get; set; }

        [JsonPropertyName("hardActivity")]
        public HardActivity HardActivity { get; set; }

        [JsonPropertyName("normalActivity")]
        public NormalActivityCardiac NormalActivity { get; set; }

        [JsonPropertyName("casualActivity")]
        public CasualActivityCardiac CasualActivity { get; set; }

        public decimal Compute()
        {
            var basicScore = 0.0m;
            basicScore = CasualActivity.Compute();
            if(basicScore > 0) return basicScore;

            basicScore = NormalActivity.Compute();
            if (basicScore > 0) return basicScore;

            basicScore = HardActivity.Compute();
            if (basicScore > 0) return basicScore;

            basicScore = HeavyActivity.Compute();

            return basicScore;
        }
    }

    public class HeavyActivity
{
    /// <summary>
    /// 负重爬山
    /// </summary>
    [JsonPropertyName("mountainClimbingWithHeavy")]
    public bool MountainClimbingWithHeavy { get; set; }

    /// <summary>
    /// 打篮球
    /// </summary>
    [JsonPropertyName("basketBall")]
    public bool BasketBall { get; set; }

    /// <summary>
    /// 登山
    /// </summary>
    [JsonPropertyName("mountainClimbing")]
    public bool MountainClimbing { get; set; }

    /// <summary>
    /// 踢足球
    /// </summary>
    [JsonPropertyName("footBall")]
    public bool FootBall { get; set; }

    public decimal Compute()
    {
        var basicScore = 0;

        if (MountainClimbingWithHeavy)
        {
            basicScore += 10;
        }
        else
        {
            basicScore -= 5;
        }

        if (BasketBall)
        {
            basicScore += 10;
        }
        else
        {
            basicScore -= 5;
        }

        if (MountainClimbing)
        {
            basicScore += 10;
        }
        else
        {
            basicScore -= 5;
        }

        if (FootBall)
        {
            basicScore += 10;
        }
        else
        {
            basicScore -= 5;
        }

        basicScore += 20;

        return basicScore;
    }

}

public class HardActivity
{
    /// <summary>
    /// 负重行走
    /// </summary>
    [JsonPropertyName("walkWithLoad")]
    public bool WalkWithLoad { get; set; }

    /// <summary>
    /// 除草
    /// </summary>
    [JsonPropertyName("weeding")]
    public bool Weeding { get; set; }

    /// <summary>
    /// 骑自行车
    /// </summary>
    [JsonPropertyName("riding")]
    public bool Riding { get; set; }

    public decimal Compute()
    {
        var basicScore = 0;

        if (WalkWithLoad)
        {
            basicScore += 10;
        }
        else
        {
            basicScore -= 5;
        }

        if (Weeding)
        {
            basicScore += 10;
        }
        else
        {
            basicScore -= 5;
        }

        if (Riding)
        {
            basicScore += 10;
        }
        else
        {
            basicScore -= 5;
        }

        if (basicScore > 0) basicScore += 40;

        return basicScore;
    }
}

public class NormalActivityCardiac
{
    /// <summary>
    /// 在水面上行走
    /// </summary>
    [JsonPropertyName("walkOnWater")]
    public bool WalkOnWater { get; set; }

    /// <summary>
    /// 打扫卫生
    /// </summary>
    [JsonPropertyName("houseClean")]
    public bool HouseClean { get; set; }

    /// <summary>
    /// 看护小孩
    /// </summary>
    [JsonPropertyName("babySet")]
    public bool BabySet { get; set; }

    public decimal Compute()
    {
        var basicScore = 0;

        if (WalkOnWater)
        {
            basicScore += 10;
        }
        else
        {
            basicScore -= 5;
        }

        if (HouseClean)
        {
            basicScore += 10;
        }
        else
        {
            basicScore -= 5;
        }

        if (BabySet)
        {
            basicScore += 10;
        }
        else
        {
            basicScore -= 5;
        }

        if (basicScore > 0) basicScore += 70;

        return basicScore;
    }
}

public class CasualActivityCardiac
{
    /// <summary>
    /// 不能平躺
    /// </summary>
    [JsonPropertyName("layOff")]
    public bool LayOff { get; set; }

    /// <summary>
    /// 休息时有症状
    /// </summary>
    [JsonPropertyName("resting")]
    public bool Resting { get; set; }

    /// <summary>
    /// 血压 小于 90/60mmHg
    /// </summary>
    [JsonPropertyName("bloodPressurePoor")]
    public bool BloodPressurePoor { get; set; }

    /// <summary>
    /// 血压 90-120/60-70mmHg
    /// </summary>
    [JsonPropertyName("bloodPressureLow")]
    public bool BloodPressureLow { get; set; }

    /// <summary>
    /// 需要静脉给药
    /// </summary>
    [JsonPropertyName("intravenousInjection")]
    public bool IntravenousInjection { get; set; }

    public decimal Compute()
    {
        var basicScore = 0;

        if (Resting) basicScore              += 80;
        if (LayOff) basicScore               += 10;
        if (BloodPressurePoor) basicScore    += 20;
        if (BloodPressureLow) basicScore     += 10;
        if (IntravenousInjection) basicScore += 15;

        if (basicScore >= 80) basicScore += 75;

        return basicScore;
    }
}
}
