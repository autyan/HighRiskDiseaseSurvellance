using System;
using System.Text.Json.Serialization;

namespace HighRiskDiseaseSurvellance.Dto.Models
{
    public class Hyperlipidemia : ISurveillance
    {
        /// <summary>
        /// 年龄
        /// </summary>
        [JsonPropertyName("age")]
        public int Age { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [JsonPropertyName("gender")]
        public int Gender { get; set; }

        /// <summary>
        /// 身高
        /// </summary>
        [JsonPropertyName("height")]
        public double Height { get; set; }

        /// <summary>
        /// 体重
        /// </summary>
        [JsonPropertyName("weight")]
        public double Weight { get; set; }

        /// <summary>
        /// 是否抽烟
        /// </summary>
        [JsonPropertyName("smoke")]
        public bool Smoke { get; set; }

        /// <summary>
        /// 低密度脂蛋白
        /// </summary>
        [JsonPropertyName("ldlc")]
        public double Ldlc { get; set; }

        /// <summary>
        /// 高密度脂蛋白
        /// </summary>
        [JsonPropertyName("hdlc")]
        public double Hdlc { get; set; }

        /// <summary>
        /// 总胆固醇
        /// </summary>
        [JsonPropertyName("tc")]
        public double Tc { get; set; }

        /// <summary>
        /// 收缩压
        /// </summary>
        [JsonPropertyName("sbp")]
        public double Sbp { get; set; }

        /// <summary>
        /// 舒张压
        /// </summary>
        [JsonPropertyName("dbp")]
        public double Dbp { get; set; }

        /// <summary>
        /// 基础疾病
        /// </summary>
        [JsonPropertyName("basicDisease")]
        public BasicDisease BasicDisease { get; set; }

        public double UnHdlc => Tc - Hdlc;

        public decimal      Compute()
        {
            //先计算极高危疾病，如果存在则直接标记为极高危
            var basicScore = BasicDisease.Compute();
            if (basicScore > 0) return basicScore;

            basicScore = HighRiskCompute();
            if (basicScore > 0) return basicScore;

            basicScore = MediumRiskCompute();
            if(basicScore > 0) return basicScore;

            basicScore = LowRiskCompute();

            return basicScore;
        }

        public decimal HighRiskCompute()
        {
            var basicScore = 0.0m;

            //高危1
            if (Ldlc >= 4.9)
            {
                basicScore = this.TryUpdateScore(55, basicScore);
            }

            //高危2
            if (Tc >= 7.2)
            {
                basicScore = this.TryUpdateScore(55, basicScore);
            }

            //高危3
            if (Ldlc is >= 1.8 and < 4.9 || Tc is >= 3.1 and < 7.2)
            {
                if (BasicDisease.Diabetes && Age >= 40)
                {
                    basicScore = this.TryUpdateScore(60, basicScore);
                }
            }

            //高危4
            if ((Tc is >= 3.1 and <= 4.1 || (Ldlc is >= 1.8 and <= 2.6 && BasicDisease.Hypertension)))
            {
                basicScore = this.TryUpdateScore(RiskPoint >= 3 ? 65 : 0, basicScore);
            }

            //高危5
            if ((Tc is >= 4.1 and < 5.2 || (Ldlc is >= 2.6 and < 3.4 && BasicDisease.Hypertension)))
            {
                basicScore = RiskPoint switch
                             {
                                 2 => this.TryUpdateScore(65, basicScore),
                                 3 => this.TryUpdateScore(70, basicScore),
                                 _ => basicScore
                             };
            }

            //高危6
            if ((Tc is >= 5.2 and < 7.2 || (Ldlc is >= 3.4 and < 4.9 && BasicDisease.Hypertension)))
            {

                basicScore = RiskPoint switch
                             {
                                 2 => this.TryUpdateScore(70, basicScore),
                                 3 => this.TryUpdateScore(74, basicScore),
                                 _ => basicScore
                             };
            }

            return basicScore;
        }



        public decimal MediumRiskCompute()
        {
            var basicScore = 0.0m;

            //中危1
            if (((Tc is >= 3.1 and < 4.1) || (Ldlc is >= 1.8 and < 2.6 && BasicDisease.Hypertension)))
            {
                basicScore = this.TryUpdateScore(RiskPoint == 2 ? 30 : 0, basicScore);
            }

            //中危2
            if (((Tc is >= 4.1 and < 5.2) || (Ldlc is >= 2.6 and < 3.4 && BasicDisease.Hypertension)))
            {
                basicScore = this.TryUpdateScore(RiskPoint == 1 ? 35 : 0, basicScore);
            }

            //中危3
            if (((Tc is >= 5.2 and < 7.2) || (Ldlc is >= 3.4 and < 4.9 && BasicDisease.Hypertension)))
            {
                basicScore = this.TryUpdateScore(RiskPoint == 1 ? 45 : 0, basicScore);
            }

            //中危4
            if (((Tc is >= 5.2 and < 7.2) || (Ldlc is >= 3.4 and < 4.9 && !BasicDisease.Hypertension)))
            {
                basicScore = RiskPoint switch
                             {
                                 2 => this.TryUpdateScore(45, basicScore),
                                 3 => this.TryUpdateScore(48, basicScore),
                                 _ => basicScore
                             };
            }

            //中危5
            if (((Tc is >= 5.2 and < 7.2) || (Ldlc is >= 2.6 and < 3.4 && !BasicDisease.Hypertension)))
            {
                basicScore = this.TryUpdateScore(RiskPoint == 3 ? 40 : 0, basicScore);
            }

            if (basicScore >= 25 && Age < 55)
            {
                var mediumPoint = 0;
                if (Sbp >= 160 || Dbp >= 100)
                {
                    mediumPoint += 1;
                }

                if (UnHdlc >= 5.2)
                {
                    mediumPoint += 1;
                }

                if (Hdlc < 1)
                {
                    mediumPoint += 1;
                }

                if (Bmi >= 28)
                {
                    mediumPoint += 1;
                }

                if (Smoke)
                {
                    mediumPoint += 1;
                }

                if (mediumPoint >= 2)
                {
                    basicScore = this.TryUpdateScore(55, basicScore);
                }
            }

            return basicScore;
        }

        public decimal LowRiskCompute()
        {
            var basicScore = 0.0m;

            //低危1
            if (((Tc is >= 3.1 and < 4.1) || (Ldlc is >= 1.8 and < 2.6 && BasicDisease.Hypertension)))
            {
                basicScore = RiskPoint switch
                             {
                                 0 => this.TryUpdateScore(5, basicScore),
                                 1 => this.TryUpdateScore(10, basicScore),
                                 _ => basicScore
                             };
            }

            //低危2
            if (Tc is >= 3.1 and < 4.1 || (Ldlc is >= 1.8 and < 2.6 && !BasicDisease.Hypertension))
            {
                basicScore = RiskPoint switch
                             {
                                 0 => this.TryUpdateScore(3, basicScore),
                                 1 => this.TryUpdateScore(5, basicScore),
                                 2 => this.TryUpdateScore(8, basicScore),
                                 3 => this.TryUpdateScore(10, basicScore),
                                 _ => basicScore
                             };
            }

            //低危3
            if (((Tc is >= 4.1 and < 5.2) || (Ldlc is >= 2.6 and < 3.4 && !BasicDisease.Hypertension)))
            {
                basicScore = RiskPoint switch
                             {
                                 0 => this.TryUpdateScore(8, basicScore),
                                 1 => this.TryUpdateScore(10, basicScore),
                                 2 => this.TryUpdateScore(15, basicScore),
                                 _ => basicScore
                             };
            }

            //低危4
            if (((Tc is >= 4.1 and < 5.2) || (Ldlc is >= 2.6 and < 3.4 && BasicDisease.Hypertension)))
            {
                basicScore = this.TryUpdateScore(RiskPoint == 0 ? 20 : 0, basicScore);
            }

            //低危5
            if (((Tc is >= 5.2 and < 7.2) || (Ldlc is >= 3.4 and < 4.9 && !BasicDisease.Hypertension)))
            {
                basicScore = RiskPoint switch
                             {
                                 0 => this.TryUpdateScore(15, basicScore),
                                 1 => this.TryUpdateScore(24, basicScore),
                                 _ => basicScore
                             };
            }

            //低危6
            if (((Tc is >= 5.2 and < 7.2) || (Ldlc is >= 3.4 and < 4.9 && BasicDisease.Hypertension)))
            {
                basicScore = this.TryUpdateScore(RiskPoint == 0 ? 24 : 0, basicScore);
            }

            return basicScore;
        }

        [JsonIgnore]
        public bool HighRiskMale => Gender == 0 && Age >= 45;

        [JsonIgnore]
        public bool HighRiskFemale => Gender == 1 && Age >= 55;

        [JsonIgnore]
        public bool HighRiskAge => HighRiskMale || HighRiskFemale;

        [JsonIgnore]
        private int? _riskPoint;

        public int RiskPoint
        {
            get
            {
                if (_riskPoint == null)
                {
                    _riskPoint = 0;
                    if (HighRiskAge)
                    {
                        _riskPoint += 1;
                    }

                    if (Smoke) _riskPoint += 1;

                    if (Hdlc < 1) _riskPoint += 1;
                }

                return _riskPoint.Value;
            }
        }

        public double Bmi => Weight / Math.Pow(Height / 100.0, 2.0);
    }

    public class BasicDisease
    {
        /// <summary>
        /// 高血压
        /// </summary>
        [JsonPropertyName("hypertension")]
        public bool Hypertension { get; set; }

        /// <summary>
        /// 糖尿病
        /// </summary>
        [JsonPropertyName("diabetes")]
        public bool Diabetes { get; set; }

        /// <summary>
        /// 急性心肌梗死
        /// </summary>
        [JsonPropertyName("acuteMyocardialInfarction")]
        public bool AcuteMyocardialInfarction { get; set; }

        /// <summary>
        /// 不稳定性心绞痛
        /// </summary>
        [JsonPropertyName("unstableAngina")]
        public bool UnstableAngina { get; set; }

        /// <summary>
        /// 稳定性冠心病
        /// </summary>
        [JsonPropertyName("stableCoronaryHeartDisease")]
        public bool StableCoronaryHeartDisease { get; set; }

        /// <summary>
        /// 血运重建术后
        /// </summary>
        [JsonPropertyName("afterRevascularization")]
        public bool AfterRevascularization { get; set; }

        /// <summary>
        /// 缺血性心肌病
        /// </summary>
        [JsonPropertyName("ischemicCardiomyopathy")]
        public bool IschemicCardiomyopathy { get; set; }

        /// <summary>
        /// 缺血性卒中
        /// </summary>
        [JsonPropertyName("ischemicStroke")]
        public bool IschemicStroke { get; set; }

        /// <summary>
        /// 短暂性脑缺血发作
        /// </summary>
        [JsonPropertyName("transientIschemicAttack")]
        public bool TransientIschemicAttack { get; set; }

        /// <summary>
        /// 颈动脉狭窄
        /// </summary>
        [JsonPropertyName("carotidArteryStenosis")]
        public bool CarotidArteryStenosis { get; set; }

        /// <summary>
        /// 肾动脉狭窄
        /// </summary>
        [JsonPropertyName("renalArteryStenosis")]
        public bool RenalArteryStenosis { get; set; }

        /// <summary>
        /// 四肢动脉狭窄
        /// </summary>
        [JsonPropertyName("arterialStenosisInExtremities")]
        public bool ArterialStenosisInExtremities { get; set; }

        /// <summary>
        /// 腹主动脉瘤
        /// </summary>
        [JsonPropertyName("abdominalAorticAneurysm")]
        public bool AbdominalAorticAneurysm       { get; set; }

        /// <summary>
        /// 以上皆无
        /// </summary>
        [JsonPropertyName("none")]
        public bool None { get; set; }

        public decimal Compute()
        {
            var basicScore = 0.0m;

            if (AcuteMyocardialInfarction) basicScore     += 3;
            if (UnstableAngina) basicScore                += 3;
            if (StableCoronaryHeartDisease) basicScore    += 3;
            if (AfterRevascularization) basicScore        += 3;
            if (IschemicCardiomyopathy) basicScore        += 3;
            if (IschemicStroke) basicScore                += 3;
            if (TransientIschemicAttack) basicScore       += 3;
            if (CarotidArteryStenosis) basicScore         += 3;
            if (RenalArteryStenosis) basicScore           += 3;
            if (ArterialStenosisInExtremities) basicScore += 3;
            if (AbdominalAorticAneurysm) basicScore       += 3;

            if (basicScore > 0) basicScore += 75;

            if (basicScore > 100) basicScore = 100;

            return basicScore;
        }
    }
}
