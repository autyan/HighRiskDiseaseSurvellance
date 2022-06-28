using System.Text.Json.Serialization;

namespace HighRiskDiseaseSurvellance.Dto.Models;

public class AnginaPectoris : ISurveillance
{
    [JsonPropertyName("basicActivity")]
    public BasicActivity BasicActivity { get; set; }

    [JsonPropertyName("commonActivity")]
    public CommonActivity CommonActivity { get; set; }

    [JsonPropertyName("normalActivity")]
    public NormalActivity NormalActivity { get; set; }

    [JsonPropertyName("casualActivity")]
    public CasualActivity CasualActivity { get; set; }

    public decimal Compute()
    {
        var basicScore = 0.0m;
        basicScore = CasualActivity.Compute();
        if(basicScore > 0) return basicScore;

        basicScore = NormalActivity.Compute();
        if (basicScore > 0) return basicScore;

        basicScore = CommonActivity.Compute();
        if (basicScore > 0) return basicScore;

        basicScore = BasicActivity.Compute();

        return basicScore;
    }
}

public class BasicActivity
{
    /// <summary>
    /// 正常行走（一般速度）
    /// </summary>
    [JsonPropertyName("walk")]
    public bool Walk { get; set; }

    /// <summary>
    /// 正常上楼
    /// </summary>
    [JsonPropertyName("climbStairs")]
    public bool ClimbStairs { get; set; }

    /// <summary>
    /// 以上皆无
    /// </summary>
    [JsonPropertyName("none")]
    public bool None { get; set; }

    public decimal Compute()
    {
        var basicScore = 0;

        if (Walk) basicScore          += 10;
        if (ClimbStairs) basicScore   += 10;

        return basicScore;
    }

}

public class CommonActivity
{
    /// <summary>
    /// 快步行走或上楼、登高
    /// </summary>
    [JsonPropertyName("briskWalk")]
    public bool BriskWalk { get; set; }

    /// <summary>
    /// 饭后行走或上楼
    /// </summary>
    [JsonPropertyName("walkAfterMeal")]
    public bool WalkAfterMeal { get; set; }

    /// <summary>
    /// 寒冷或风中行走
    /// </summary>
    [JsonPropertyName("walkInWind")]
    public bool WalkInWind { get; set; }

    /// <summary>
    /// 一般速度平地步行200m以上
    /// </summary>
    [JsonPropertyName("walkOverRange")]
    public bool WalkOverRange { get; set; }

    /// <summary>
    /// 登一层以上的楼梯
    /// </summary>
    [JsonPropertyName("goUpOneStairs")]
    public bool GoUpOneStairs { get; set; }

    /// <summary>
    /// 以上皆无
    /// </summary>
    [JsonPropertyName("none")]
    public bool None { get; set; }

    public decimal Compute()
    {
        var basicScore = 0;

        if (BriskWalk) basicScore     += 5;
        if (WalkAfterMeal) basicScore += 5;
        if (WalkInWind) basicScore    += 5;
        if (WalkOverRange) basicScore += 5;
        if (GoUpOneStairs) basicScore += 5;

        if (basicScore > 0) basicScore += 25;

        return basicScore;
    }
}

public class NormalActivity
{
    /// <summary>
    /// 一般速度平地步行100~200m
    /// </summary>
    [JsonPropertyName("walkOverRange")]
    public bool WalkOverRange { get; set; }

    /// <summary>
    /// 登一层楼梯
    /// </summary>
    [JsonPropertyName("goUpOneStairs")]
    public bool GoUpOneStairs { get; set; }

    /// <summary>
    /// 水平面上走动
    /// </summary>
    [JsonPropertyName("walkOnGround")]
    public bool WalkOnGround { get; set; }

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
        var basicScore = 0;

        if (WalkOverRange) basicScore += 5;
        if (GoUpOneStairs) basicScore += 5;
        if (WalkOnGround) basicScore  += 5;
        if (HouseClean) basicScore    += 5;
        if (BabySet) basicScore       += 5;

        if (basicScore > 0) basicScore += 50;

        return basicScore;
    }
}

public class CasualActivity
{
    /// <summary>
    /// 开会
    /// </summary>
    [JsonPropertyName("metting")]
    public bool Metting { get; set; }

    /// <summary>
    /// 开车
    /// </summary>
    [JsonPropertyName("driving")]
    public bool Driving { get; set; }

    /// <summary>
    /// 打字
    /// </summary>
    [JsonPropertyName("typing")]
    public bool Typing { get; set; }

    /// <summary>
    /// 听音乐
    /// </summary>
    [JsonPropertyName("listenMusic")]
    public bool ListenMusic { get; set; }

    /// <summary>
    /// 绘画
    /// </summary>
    [JsonPropertyName("painting")]
    public bool Painting { get; set; }

    /// <summary>
    /// 以上皆无
    /// </summary>
    [JsonPropertyName("none")]
    public bool None { get; set; }

    public decimal Compute()
    {
        var basicScore = 0;

        if (Metting) basicScore     += 5;
        if (Driving) basicScore     += 5;
        if (Typing) basicScore      += 5;
        if (ListenMusic) basicScore += 5;
        if (Painting) basicScore    += 5;

        if (basicScore > 0) basicScore += 75;

        return basicScore;
    }
}