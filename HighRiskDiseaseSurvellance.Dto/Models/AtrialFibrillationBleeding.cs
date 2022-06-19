using System.Text.Json.Serialization;

namespace HighRiskDiseaseSurvellance.Dto.Models;

public class AtrialFibrillationBleeding : ISurveillance
{
    /// <summary>
    /// 基础疾病
    /// </summary>
    [JsonPropertyName("basicHealthy")]
    public BasicHealthy           BasicHealthy           { get; set; }
    
    /// <summary>
    /// 肾功能异常
    /// </summary>
    [JsonPropertyName("abnormalkidneyFunction")]
    public AbnormalKidneyFunction AbnormalKidneyFunction { get; set; }
    
    /// <summary>
    /// 肝功能异常
    /// </summary>
    [JsonPropertyName("abnormalLiverFunction")]
    public AbnormalLiverFunction  AbnormalLiverFunction  { get; set; }
    
    /// <summary>
    /// 出血
    /// </summary>
    [JsonPropertyName("bleeding")]
    public Bleeding               Bleeding               { get; set; }
    
    /// <summary>
    /// INR值不稳定
    /// </summary>
    [JsonPropertyName("inrUnstable")]
    public InrUnstable            InrUnstable            { get; set; }
    
    /// <summary>
    /// 药物
    /// </summary>
    [JsonPropertyName("drugs")]
    public Drugs                  Drugs                  { get; set; }
    public decimal Compute()
    {
        return BasicHealthy.Compute()
             + AbnormalKidneyFunction.Compute()
             + AbnormalLiverFunction.Compute()
             + Bleeding.Compute()
             + InrUnstable.Compute()
             + Drugs.Compute();
    }
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class AbnormalKidneyFunction
{
    /// <summary>
    /// 慢性透析
    /// </summary>
    [JsonPropertyName("chronicDialysis")]
    public bool ChronicDialysis  { get; set; }
    
    /// <summary>
    /// 肾移植
    /// </summary>
    [JsonPropertyName("kidneyTransplant")]
    public bool KidneyTransplant { get; set; }
    
    /// <summary>
    /// 血清肌酐大大于200umol/L
    /// </summary>
    [JsonPropertyName("serumCreatinine")]
    public bool SerumCreatinine  { get; set; }

    public decimal Compute()
    {
        var score                   = 0.0m;
        if (ChronicDialysis) score  += 1;
        if (KidneyTransplant) score += 1;
        if (SerumCreatinine) score  += 1;
        return score;
    }
}

public class AbnormalLiverFunction
{
    /// <summary>
    /// 慢性肝病
    /// </summary>
    [JsonPropertyName("chronicLiverDisease")]
    public bool ChronicLiverDisease { get; set; }
    
    /// <summary>
    /// 胆红素>2倍且谷草转氨酶/谷丙转氨酶/碱性磷酸酶>3倍
    /// </summary>
    [JsonPropertyName("bilirubin")]
    public bool Bilirubin           { get; set; }
    
    public decimal Compute()
    {
        var score                      = 0.0m;
        if (ChronicLiverDisease) score += 1;
        if (Bilirubin) score           += 1;
        return score;
    }
}

public class BasicHealthy
{
    /// <summary>
    /// 高血压>=160mmHg
    /// </summary>
    [JsonPropertyName("hypertension")]
    public bool Hypertension { get; set; }
    
    /// <summary>
    /// 卒中
    /// </summary>
    [JsonPropertyName("stroke")]
    public bool Stroke       { get; set; }
    
    /// <summary>
    /// 年龄大于65岁
    /// </summary>
    [JsonPropertyName("overAge")]
    public bool OverAge      { get; set; }
    
    public decimal Compute()
    {
        var score               = 0.0m;
        if (Hypertension) score += 1;
        if (Stroke) score       += 1;
        if (OverAge) score      += 1;
        return score;
    }
}

public class Bleeding
{
    /// <summary>
    /// 出血病史
    /// </summary>
    [JsonPropertyName("historyOfBleeding")]
    public bool HistoryOfBleeding    { get; set; }
    
    /// <summary>
    /// 出血体质
    /// </summary>
    [JsonPropertyName("bleedingConstitution")]
    public bool BleedingConstitution { get; set; }
    
    /// <summary>
    /// 贫血
    /// </summary>
    [JsonPropertyName("anemia")]
    public bool Anemia               { get; set; }
    
    public decimal Compute()
    {
        var score                       = 0.0m;
        if (HistoryOfBleeding) score    += 1;
        if (BleedingConstitution) score += 1;
        if (Anemia) score               += 1;
        return score;
    }
}

public class Drugs
{
    /// <summary>
    /// 抗血小板药物
    /// </summary>
    [JsonPropertyName("antiplateletDrugs")]
    public bool AntiplateletDrugs { get; set; }
    
    /// <summary>
    /// 非甾体类药物
    /// </summary>
    [JsonPropertyName("nonsteroidalDrugs")]
    public bool NonsteroidalDrugs { get; set; }
    
    /// <summary>
    /// 嗜酒
    /// </summary>
    [JsonPropertyName("alcoholism")]
    public bool Alcoholism        { get; set; }
    
    public decimal Compute()
    {
        var score                    = 0.0m;
        if (AntiplateletDrugs) score += 1;
        if (NonsteroidalDrugs) score += 1;
        if (Alcoholism) score        += 1;
        return score;
    }
}

public class InrUnstable
{
    /// <summary>
    /// 易变
    /// </summary>
    [JsonPropertyName("volatile")]
    public bool Volatile  { get; set; }
    
    /// <summary>
    /// 偏高
    /// </summary>
    [JsonPropertyName("high")]
    public bool High       { get; set; }
    
    /// <summary>
    /// 达不到治疗范围（小于60%)
    /// </summary>
    [JsonPropertyName("outOfRange")]
    public bool OutOfRange { get; set; }
    
    public decimal Compute()
    {
        var score             = 0.0m;
        if (Volatile) score   += 1;
        if (High) score       += 1;
        if (OutOfRange) score += 1;
        return score;
    }
}

