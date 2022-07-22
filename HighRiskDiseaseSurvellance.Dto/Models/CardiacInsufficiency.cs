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
            var basicScore = CasualActivity.Compute();
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

    /// <summary>
    /// 以上皆无
    /// </summary>
    [JsonPropertyName("none")]
    public bool None { get; set; }

    public decimal Compute()
    {
        var heavyActivatyRatre                            = 0;
        if (MountainClimbingWithHeavy) heavyActivatyRatre += 1;
        if (BasketBall) heavyActivatyRatre                += 1;
        if (MountainClimbing) heavyActivatyRatre          += 1;
        if (FootBall) heavyActivatyRatre                  += 1;

        var basicScore = 0;

        basicScore = heavyActivatyRatre switch
                     {
                         1 => 6,
                         2 => 12,
                         3 => 18,
                         4 => 24,
                         _ => basicScore
                     };

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

    /// <summary>
    /// 以上皆无
    /// </summary>
    [JsonPropertyName("none")]
    public bool None { get; set; }

    public decimal Compute()
    {
        var hardActivityRate               = 0;
        if (WalkWithLoad) hardActivityRate += 1;
        if (Weeding) hardActivityRate      += 1;
        if (Riding) hardActivityRate       += 1;

        var basicScore = 0;

        basicScore = hardActivityRate switch
                     {
                         1 => 33,
                         2 => 41,
                         3 => 49,
                         _ => basicScore
                     };

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

    /// <summary>
    /// 以上皆无
    /// </summary>
    [JsonPropertyName("none")]
    public bool None { get; set; }

    public decimal Compute()
    {
        var normalActivityRate = 0;

        if (WalkOnWater) normalActivityRate += 1;
        if (HouseClean) normalActivityRate  += 1;
        if (BabySet) normalActivityRate     += 1;
        var basicScore                      = 0.0m;

        basicScore = normalActivityRate switch
                     {
                         1 => 58,
                         2 => 66,
                         3 => 74,
                         _ => basicScore
                     };

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
    /// 休息时有症状且血压（BP）&lt;90/60 mmHg）
    /// </summary>
    [JsonPropertyName("restingWithBp")]
    public bool RestingWithBp { get; set; }

    /// <summary>
    /// 休息时有症状且收缩压(90-120)/舒张压（60-70） mmHg）
    /// </summary>
    [JsonPropertyName("restingWithDbpOrSbp")]
    public bool RestingWithDbpOrSbp { get; set; }

    /// <summary>
    /// 休息时有症状且需要静脉用药
    /// </summary>
    [JsonPropertyName("restingWithDrug")]
    public bool RestingWithDrug { get; set; }

    /// <summary>
    /// 以上皆无
    /// </summary>
    [JsonPropertyName("none")]
    public bool None { get; set; }

    public decimal Compute()
    {
        var basicScore = 0;

        if (LayOff) basicScore              = 80;
        if (Resting) basicScore             = 90;
        if (RestingWithDbpOrSbp) basicScore = 90;
        if (RestingWithDrug) basicScore     = 90;
        if (RestingWithBp) basicScore       = 95;

        return basicScore;
    }
}
}
