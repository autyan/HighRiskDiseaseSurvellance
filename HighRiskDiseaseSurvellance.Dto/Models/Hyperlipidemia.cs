﻿using System.Text.Json.Serialization;

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

        public decimal      Compute()
        {
            //先计算极高危疾病，如果存在则直接标记为高危
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
            if (Ldlc >= 4.9) basicScore += 5;

            //高危2
            if (Tc   >= 7.2) basicScore += 5;

            //高危3
            if (BasicDisease.Diabetes
             && Age > 40
             && Ldlc is >= 1.8 and <= 4.9
             && Tc is >= 3.1 and <= 7.2)
            {
                basicScore += 5;
            }

            //高危4
            if ((Tc is >= 3.1 and <= 4.1 || (Ldlc is >= 1.8 and <= 2.6 && BasicDisease.Hypertension))
             && RiskPoint >= 3)
            {
                basicScore += 5;
            }

            //高危5
            if ((Tc is >= 4.1 and < 5.2 || (Ldlc is >= 2.6 and < 3.4 && BasicDisease.Hypertension))
             && RiskPoint >= 2)
            {
                basicScore += 5;
            }

            //高危6
            if ((Tc is >= 5.2 and <= 7.2 || (Ldlc is >= 3.4 and < 4.9 && BasicDisease.Hypertension))
                && RiskPoint >= 2)
            {

                basicScore += 5;
            }

            if (basicScore > 0) basicScore += 50;
            return basicScore;
        }



        public decimal MediumRiskCompute()
        {
            var basicScore = 0.0m;

            //中危1
            if (((Tc is >= 3.1 and < 4.1) || (Ldlc is >= 1.8 and < 2.6 && BasicDisease.Hypertension))
             && RiskPoint >= 2)
            {
                basicScore += 5;
            }

            //中危2
            if (((Tc is >= 4.1 and < 5.2) || (Ldlc is >= 2.6 and < 3.4 && BasicDisease.Hypertension))
             && RiskPoint >= 1)
            {
                basicScore += 5;
            }

            //中危3
            if (((Tc is >= 5.2 and < 7.2) || (Ldlc is >= 3.4 and < 4.9 && BasicDisease.Hypertension))
             && RiskPoint >= 1)
            {
                basicScore += 5;
            }

            //中危4
            if (((Tc is >= 5.2 and < 7.2) || (Ldlc is >= 3.4 and < 4.9 && !BasicDisease.Hypertension))
             && RiskPoint >= 2)
            {
                basicScore += 5;
            }

            //中危5
            if (((Tc is >= 5.2 and < 7.2) || (Ldlc is >= 2.6 and < 3.4 && !BasicDisease.Hypertension))
             && RiskPoint >= 3)
            {
                basicScore += 5;
            }

            if (basicScore > 0) basicScore += 25;

            if (basicScore > 0 && Age < 55)
            {
                var mediumPoint = 0;
                if (Sbp >= 160 || Dbp >= 100)
                {
                    mediumPoint += 1;
                }

                if (Hdlc >= 5.2)
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
                    basicScore = 51;
                }
            }

            return basicScore;
        }

        public decimal LowRiskCompute()
        {
            var basicScore = 0.0m;

            //低危1
            if (((Tc is >= 3.1 and < 4.1) || (Ldlc is >= 1.8 and < 2.6 && BasicDisease.Hypertension))
             && RiskPoint <= 1)
            {
                basicScore += 4;
            }

            //低危2
            if (Tc is >= 3.1 and < 4.1 || (Ldlc is >= 1.8 and < 2.6 && !BasicDisease.Hypertension))
            {
                basicScore += 4;
            }

            //低危3
            if (((Tc is >= 4.1 and < 5.2) || (Ldlc is >= 2.6 and < 3.4 && !BasicDisease.Hypertension))
             && RiskPoint <= 2)
            {
                basicScore += 4;
            }

            //低危4
            if (((Tc is >= 4.1 and < 5.2) || (Ldlc is >= 2.6 and < 3.4 && BasicDisease.Hypertension))
             && RiskPoint == 0)
            {
                basicScore += 4;
            }

            //低危5
            if (((Tc is >= 5.2 and < 7.2) || (Ldlc is >= 3.4 and < 4.9 && !BasicDisease.Hypertension))
             && RiskPoint <= 1)
            {
                basicScore += 4;
            }

            //低危6
            if (((Tc is >= 5.2 and < 7.2) || (Ldlc is >= 3.4 and < 4.9 && BasicDisease.Hypertension))
             && RiskPoint == 0)
            {
                basicScore += 4;
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

        public double Bmi => Weight / (Height * Height);
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
        /// 急性冠状动脉综合征
        /// </summary>
        [JsonPropertyName("acuteCoronarySyndrome")]
        public bool AcuteCoronarySyndrome { get; set; }

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

        public decimal Compute()
        {
            var basicScore = 0.0m;

            if (AcuteCoronarySyndrome) basicScore         += 3;
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

            return basicScore;
        }
    }
}
