using System.Text.Json.Serialization;

namespace HighRiskDiseaseSurvellance.Dto.Models;

public class AtrialFibrillationIschemia : ISurveillance
{
    /// <summary>
    /// 充血性心力衰竭、左心功能障碍
    /// </summary>
    [JsonPropertyName("congestiveHeartFailure")]
    public bool CongestiveHeartFailure { get; set; }
    
    /// <summary>
    /// 高血压
    /// </summary>
    [JsonPropertyName("hypertension")]
    public bool Hypertension { get; set; }
        
    /// <summary>
    /// 年龄>75岁
    /// </summary>
    [JsonPropertyName("overAge")]
    public bool OverAge { get; set; }
        
    /// <summary>
    /// 糖尿病
    /// </summary>
    [JsonPropertyName("diabetes")]
    public bool Diabetes { get; set; }
        
    /// <summary>
    /// 既往卒中或血栓病史
    /// </summary>
    [JsonPropertyName("historyOfPreviousStrokeOrThrombosis")]
    public bool HistoryOfPreviousStrokeOrThrombosis { get; set; }
    
    /// <summary>
    /// 血管疾病
    /// </summary>
    [JsonPropertyName("vascularDisease")]
    public bool VascularDisease { get; set; }
        
    /// <summary>
    /// 年龄65-74
    /// </summary>
    [JsonPropertyName("ageAmount")]
    public bool AgeAmount { get; set; }
        
    /// <summary>
    /// 女性
    /// </summary>
    [JsonPropertyName("female")]
    public bool Female { get; set; }
    
    public decimal Compute()
    {
        var score = 0.0m;

        if (CongestiveHeartFailure) score              += 1;
        if (Hypertension) score                        += 1;
        if (OverAge) score                             += 2;
        if (Diabetes) score                            += 1;
        if (HistoryOfPreviousStrokeOrThrombosis) score += 2;
        if (VascularDisease) score                     += 1;
        if (AgeAmount) score                           += 1;
        if (Female) score                              += 1;

        return score;
    }
}