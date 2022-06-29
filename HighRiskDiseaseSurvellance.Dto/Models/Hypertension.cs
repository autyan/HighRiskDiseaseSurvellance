using System;
using System.Dynamic;
using System.Text.Json.Serialization;

namespace HighRiskDiseaseSurvellance.Dto.Models
{
    public class Hypertension: ISurveillance
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
        /// 腰围
        /// </summary>
        [JsonPropertyName("waistline")]
        public double Waistline { get; set; }

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
        /// 风险因素
        /// </summary>
        [JsonPropertyName("riskFactors")]
        public RiskFactors RiskFactors { get; set; }

        /// <summary>
        /// 伴随临床疾患
        /// </summary>
        [JsonPropertyName("concomitantClinicalDisorders")]
        public ConcomitantClinicalDisorders ConcomitantClinicalDisorders { get; set; }

        /// <summary>
        /// 靶器官损害
        /// </summary>
        [JsonPropertyName("targetOrganDamage")]
        public TargetOrganDamage TargetOrganDamage { get; set; }

        public double Bmi => Weight / Math.Pow(Height / 100.0, 2.0);

        private int? _basicRank = null;

        public int BasicRank
        {
            get
            {
                if (_basicRank == null)
                {
                    _basicRank = 0;
                    if ((Gender == 0 && Age > 55) || (Gender == 1 && Age > 65)) _basicRank                       += 1;
                    if (Smoke) _basicRank                                                                        += 1;
                    if (RiskFactors.ImpairedSugarAdhesion || RiskFactors.AbnormalFastingBloodGlucose) _basicRank += 1;
                    if (Tc >= 5.7                         || Ldlc > 3.3 || Hdlc < 1) _basicRank                  += 1;
                    if (RiskFactors.FamilyHistoryOfEarlyonsetCardiovascularDisease) _basicRank                   += 1;
                    if ((Gender == 0 && Waistline >= 90) || (Gender == 1 && Waistline >= 85) || Bmi >= 28)
                        _basicRank += 1;
                    if (RiskFactors.Hyperhomocysteine) _basicRank += 1;
                }

                return _basicRank.Value;
            }
        }

        public decimal Compute()
        {
            var basicScore = 0m;
            if (TargetOrganDamage.OrganDamageRank > 0)
            {
                if (Sbp is >= 130 and <= 139 || Dbp is >= 85 and <= 89)
                {
                    basicScore = TargetOrganDamage.OrganDamageRank switch
                                 {
                                     1 => this.TryUpdateScore(30, basicScore),
                                     2 => this.TryUpdateScore(40, basicScore),
                                     3 => this.TryUpdateScore(55, basicScore),
                                     _ => basicScore
                                 };
                }

                if (Sbp is >= 140 and <= 159 || Dbp is >= 90 and <= 99)
                {
                    basicScore = TargetOrganDamage.OrganDamageRank switch
                                 {
                                     1 => this.TryUpdateScore(55, basicScore),
                                     2 => this.TryUpdateScore(60, basicScore),
                                     3 => this.TryUpdateScore(70, basicScore),
                                     _ => basicScore
                                 };
                }


                if (Sbp is >= 160 and <= 179 || Dbp is >= 100 and < 109)
                {
                    basicScore = TargetOrganDamage.OrganDamageRank switch
                                 {
                                     1 => this.TryUpdateScore(60, basicScore),
                                     2 => this.TryUpdateScore(68, basicScore),
                                     3 => this.TryUpdateScore(72, basicScore),
                                     _ => basicScore
                                 };
                }
                if (Sbp >= 180 || Dbp >= 110)
                {
                    basicScore = TargetOrganDamage.OrganDamageRank switch
                                 {
                                     1 => this.TryUpdateScore(80, basicScore),
                                     2 => this.TryUpdateScore(90, basicScore),
                                     3 => this.TryUpdateScore(95, basicScore),
                                     _ => basicScore
                                 };
                }
            }

            if (Sbp is >= 130 and <= 139 || Dbp is >= 85 and <= 89)
            {
                basicScore = ConcomitantClinicalDisorders.ConcomitantClinicalDisordersRank switch
                             {
                                 0 => basicScore,
                                 1 => this.TryUpdateScore(76, basicScore),
                                 _ => this.TryUpdateScore(85, basicScore)
                             };
            }

            if (Sbp is >= 140 and <= 159 || Dbp is >= 90 and <= 99)
            {
                basicScore = ConcomitantClinicalDisorders.ConcomitantClinicalDisordersRank switch
                             {
                                 0 => basicScore,
                                 1 => this.TryUpdateScore(85, basicScore),
                                 _ => this.TryUpdateScore(90, basicScore)
                             };
            }

            if (Sbp is >= 160 and <= 179 || Dbp is >= 100 and <= 109)
            {
                basicScore = ConcomitantClinicalDisorders.ConcomitantClinicalDisordersRank switch
                             {
                                 0 => basicScore,
                                 1 => this.TryUpdateScore(90, basicScore),
                                 _ => this.TryUpdateScore(95, basicScore)
                             };
            }

            if (Sbp >= 180 || Dbp >= 110)
            {
                basicScore = ConcomitantClinicalDisorders.ConcomitantClinicalDisordersRank switch
                             {
                                 0 => basicScore,
                                 1 => this.TryUpdateScore(95, basicScore),
                                 _ => this.TryUpdateScore(98, basicScore)
                             };
            }

            if (BasicRank >= 3)
            {
                if (Sbp is >= 130 and <= 139 || Dbp is >= 85 and <= 99)
                {
                    basicScore = BasicRank switch
                                 {
                                     3 => this.TryUpdateScore(30, basicScore),
                                     4 => this.TryUpdateScore(35, basicScore),
                                     5 => this.TryUpdateScore(45, basicScore),
                                     6 => this.TryUpdateScore(55, basicScore),
                                     7 => this.TryUpdateScore(65, basicScore),
                                     _ => basicScore
                                 };
                }

                if (Sbp is >= 140 and <= 159 || Dbp is >= 90 and <= 99)
                {
                    basicScore = BasicRank switch
                                 {
                                     3 => this.TryUpdateScore(53, basicScore),
                                     4 => this.TryUpdateScore(60, basicScore),
                                     5 => this.TryUpdateScore(65, basicScore),
                                     6 => this.TryUpdateScore(70, basicScore),
                                     7 => this.TryUpdateScore(73, basicScore),
                                     _ => basicScore
                                 };
                }

                if (Sbp is >= 160 and <= 179 || Dbp is >= 100 and <= 109)
                {
                    basicScore = BasicRank switch
                                 {
                                     3 => this.TryUpdateScore(55, basicScore),
                                     4 => this.TryUpdateScore(65, basicScore),
                                     5 => this.TryUpdateScore(68, basicScore),
                                     6 => this.TryUpdateScore(72, basicScore),
                                     7 => this.TryUpdateScore(74, basicScore),
                                     _ => basicScore
                                 };
                }

                if (Sbp >= 180 || Dbp >= 100)
                {
                    basicScore = BasicRank switch
                                 {
                                     3 => this.TryUpdateScore(82, basicScore),
                                     4 => this.TryUpdateScore(84, basicScore),
                                     5 => this.TryUpdateScore(88, basicScore),
                                     6 => this.TryUpdateScore(93, basicScore),
                                     7 => this.TryUpdateScore(98, basicScore),
                                     _ => basicScore
                                 };
                }
            }

            if (BasicRank is >= 1 and < 3)
            {
                if (Sbp is >= 130 and <= 139 || Dbp is >= 85 and <= 99)
                {
                    basicScore = BasicRank switch
                                 {
                                     1 => this.TryUpdateScore(10, basicScore),
                                     2 => this.TryUpdateScore(15, basicScore),
                                     _ => basicScore
                                 };
                }

                if (Sbp is >= 140 and <= 159 || Dbp is >= 90 and <= 99)
                {
                    basicScore = BasicRank switch
                                 {
                                     1 => this.TryUpdateScore(35, basicScore),
                                     2 => this.TryUpdateScore(45, basicScore),
                                     _ => basicScore
                                 };
                }

                if (Sbp is >= 160 and <= 179 || Dbp is >= 100 and <= 109)
                {
                    basicScore = BasicRank switch
                                 {
                                     1 => this.TryUpdateScore(45, basicScore),
                                     2 => this.TryUpdateScore(55, basicScore),
                                     _ => basicScore
                                 };
                }

                if (Sbp >= 180 || Dbp >= 110)
                {
                    basicScore = BasicRank switch
                                 {
                                     1 => this.TryUpdateScore(76, basicScore),
                                     2 => this.TryUpdateScore(80, basicScore),
                                     _ => basicScore
                                 };
                }
            }

            if (BasicRank == 0)
            {
                if (Sbp is >= 140 and <= 159 || Dbp is >= 90 and <= 99)
                {
                    basicScore = this.TryUpdateScore(20, basicScore);
                }

                if (Sbp is >= 160 and <= 179 || Dbp is >= 100 and <= 109)
                {
                    basicScore = this.TryUpdateScore(40, basicScore);
                }

                if (Sbp >= 180 || Dbp >= 110)
                {
                    basicScore = this.TryUpdateScore(60, basicScore);
                }
            }

            if (ConcomitantClinicalDisorders.Diabetes)
            {
                if (ConcomitantClinicalDisorders.DiabetesDisordersRank == 0)
                {
                    if (Sbp is >= 130 and <= 139 || Dbp is >= 85 and <= 99)
                    {
                        basicScore = this.TryUpdateScore(50, basicScore);
                    }

                    if (Sbp is >= 140 and <= 159 || Dbp is >= 90 and <= 99)
                    {
                        basicScore = this.TryUpdateScore(60, basicScore);
                    }

                    if (Sbp is >= 160 and <= 179 || Dbp is >= 100 and <= 109)
                    {
                        basicScore = this.TryUpdateScore(70, basicScore);
                    }

                    if (Sbp >= 180 || Dbp >= 110)
                    {
                        basicScore = this.TryUpdateScore(80, basicScore);
                    }
                }

                if (ConcomitantClinicalDisorders.DiabetesDisordersRank > 0)
                {
                    if (Sbp is >= 130 and <= 139 || Dbp is >= 85 and <= 99)
                    {
                        basicScore = this.TryUpdateScore(80, basicScore);
                    }

                    if (Sbp is >= 140 and <= 159 || Dbp is >= 90 and <= 99)
                    {
                        basicScore = this.TryUpdateScore(90, basicScore);
                    }

                    if (Sbp is >= 160 and <= 179 || Dbp is >= 100 and <= 109)
                    {
                        basicScore = this.TryUpdateScore(95, basicScore);
                    }

                    if (Sbp >= 180 || Dbp >= 110)
                    {
                        basicScore = this.TryUpdateScore(98, basicScore);
                    }
                }
            }

            return basicScore;
        }
    }

    public class RiskFactors
    {
        /// <summary>
        /// 糖附量受损（2小时血糖7.8-11.0 mmol/L）
        /// </summary>
        [JsonPropertyName("impairedSugarAdhesion")]
        public bool ImpairedSugarAdhesion { get; set; }

        /// <summary>
        /// 空腹血糖异常（6.1-6.9 mmol/L）
        /// </summary>
        [JsonPropertyName("abnormalFastingBloodGlucose")]
        public bool AbnormalFastingBloodGlucose { get; set; }

        /// <summary>
        /// 早发心血管病家族史一级亲属发病年龄小于50岁
        /// </summary>
        [JsonPropertyName("familyHistoryOfEarlyonsetCardiovascularDisease")]
        public bool FamilyHistoryOfEarlyonsetCardiovascularDisease { get; set; }

        /// <summary>
        /// 高同型半胱氨酸 > 10mmol/L
        /// </summary>
        [JsonPropertyName("hyperhomocysteine")]
        public bool Hyperhomocysteine { get; set; }

        /// <summary>
        /// 以上皆无
        /// </summary>
        [JsonPropertyName("none")]
        public bool None { get; set; }
    }

    public class ConcomitantClinicalDisorders
    {
        /// <summary>
        /// 脑出血
        /// </summary>
        [JsonPropertyName("cerebralHaemorrhage")]
        public bool CerebralHaemorrhage { get; set; }

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
        /// 心肌梗死史
        /// </summary>
        [JsonPropertyName("historyOfMyocardialInfarction")]
        public bool HistoryOfMyocardialInfarction { get; set; }

        /// <summary>
        /// 心绞痛
        /// </summary>
        [JsonPropertyName("angina")]
        public bool Angina { get; set; }

        /// <summary>
        /// 冠脉血运重建史
        /// </summary>
        [JsonPropertyName("historyOfCoronaryBloodCirculationReconstruction")]
        public bool HistoryOfCoronaryBloodCirculationReconstruction { get; set; }

        /// <summary>
        /// 充血性心力衰竭
        /// </summary>
        [JsonPropertyName("congestiveHeartFailure")]
        public bool CongestiveHeartFailure { get; set; }

        /// <summary>
        /// 心房颤动
        /// </summary>
        [JsonPropertyName("atrialFibrillation")]
        public bool AtrialFibrillation { get; set; }

        /// <summary>
        /// 糖尿病肾病
        /// </summary>
        [JsonPropertyName("diabeticNephropathy")]
        public bool DiabeticNephropathy { get; set; }

        /// <summary>
        /// 血肌酐（Cr）升高（男性>133umol/L ,女性>124umol/L)
        /// </summary>
        [JsonPropertyName("elevatedSerumCreatinine")]
        public bool ElevatedSerumCreatinine { get; set; }

        /// <summary>
        /// 主动脉夹层
        /// </summary>
        [JsonPropertyName("aorticDissection")]
        public bool AorticDissection { get; set; }

        /// <summary>
        /// 外周血管疾病
        /// </summary>
        [JsonPropertyName("peripheralVascularDisease")]
        public bool PeripheralVascularDisease { get; set; }

        /// <summary>
        /// 视网膜出血、渗出或水肿
        /// </summary>
        [JsonPropertyName("severeHypertensiveRetinopathy")]
        public bool SevereHypertensiveRetinopathy { get; set; }

        /// <summary>
        /// 空腹血糖≥7.0mmol/L
        /// </summary>
        [JsonPropertyName("fastingBloodGlucose")]
        public bool FastingBloodGlucose { get; set; }

        /// <summary>
        /// 餐后2小时血糖≥11.1mmol/L
        /// </summary>
        [JsonPropertyName("bloodGlucose2HoursAfterAMeal")]
        public bool BloodGlucose2HoursAfterAMeal { get; set; }

        /// <summary>
        /// 糖化血红蛋白≥6.5%
        /// </summary>
        [JsonPropertyName("glycosylatedHemoglobin")]
        public bool GlycosylatedHemoglobin { get; set; }

        /// <summary>
        /// 糖尿病
        /// </summary>
        [JsonPropertyName("diabetes")]
        public bool Diabetes { get; set; }

        /// <summary>
        /// 以上皆无
        /// </summary>
        [JsonPropertyName("none")]
        public bool None { get; set; }

        private int? _concomitantClinicalDisordersRank = null;

        public int ConcomitantClinicalDisordersRank
        {
            get
            {
                if (_concomitantClinicalDisordersRank == null)
                {
                    _concomitantClinicalDisordersRank = 0;
                    if (CerebralHaemorrhage) _concomitantClinicalDisordersRank                             += 1;
                    if (IschemicStroke) _concomitantClinicalDisordersRank                                  += 1;
                    if (TransientIschemicAttack) _concomitantClinicalDisordersRank                         += 1;
                    if (HistoryOfMyocardialInfarction) _concomitantClinicalDisordersRank                   += 1;
                    if (Angina) _concomitantClinicalDisordersRank                                          += 1;
                    if (HistoryOfCoronaryBloodCirculationReconstruction) _concomitantClinicalDisordersRank += 1;
                    if (CongestiveHeartFailure) _concomitantClinicalDisordersRank                          += 1;
                    if (AtrialFibrillation) _concomitantClinicalDisordersRank                              += 1;
                    if (DiabeticNephropathy) _concomitantClinicalDisordersRank                             += 1;
                    if (ElevatedSerumCreatinine) _concomitantClinicalDisordersRank                         += 1;
                    if (AorticDissection) _concomitantClinicalDisordersRank                                += 1;
                    if (PeripheralVascularDisease) _concomitantClinicalDisordersRank                       += 1;
                    if (SevereHypertensiveRetinopathy) _concomitantClinicalDisordersRank                   += 1;
                    if (FastingBloodGlucose) _concomitantClinicalDisordersRank                             += 1;
                    if (BloodGlucose2HoursAfterAMeal) _concomitantClinicalDisordersRank                    += 1;
                    if (GlycosylatedHemoglobin) _concomitantClinicalDisordersRank                          += 1;
                }

                return _concomitantClinicalDisordersRank.Value;
            }
        }

        private int? _diabetesDisordersRank = null;

        public int DiabetesDisordersRank {
            get
            {
                if (_diabetesDisordersRank == null)
                {
                    _diabetesDisordersRank = 0;
                    if (CerebralHaemorrhage) _diabetesDisordersRank                             += 1;
                    if (IschemicStroke) _diabetesDisordersRank                                  += 1;
                    if (TransientIschemicAttack) _diabetesDisordersRank                         += 1;
                    if (HistoryOfMyocardialInfarction) _diabetesDisordersRank                   += 1;
                    if (Angina) _diabetesDisordersRank                                          += 1;
                    if (HistoryOfCoronaryBloodCirculationReconstruction) _diabetesDisordersRank += 1;
                    if (CongestiveHeartFailure) _diabetesDisordersRank                          += 1;
                    if (AtrialFibrillation) _diabetesDisordersRank                              += 1;
                    if (DiabeticNephropathy) _diabetesDisordersRank                             += 1;
                    if (ElevatedSerumCreatinine) _diabetesDisordersRank                         += 1;
                    if (AorticDissection) _diabetesDisordersRank                                += 1;
                    if (PeripheralVascularDisease) _diabetesDisordersRank                       += 1;
                    if (SevereHypertensiveRetinopathy) _diabetesDisordersRank                   += 1;
                }

                return _diabetesDisordersRank.Value;
            }
        }
    }

    public class TargetOrganDamage
    {
        /// <summary>
        /// 左心室肥厚（心电图或超声心电图）
        /// </summary>
        [JsonPropertyName("leftVentricularHypertrophy")]
        public bool LeftVentricularHypertrophy { get; set; }

        /// <summary>
        /// 劲动脉超声或X线证实有动脉粥样斑块（颈、髂、股或主动脉）
        /// </summary>
        [JsonPropertyName("atheroscleroticPlaques")]
        public bool AtheroscleroticPlaques { get; set; }

        /// <summary>
        /// 视网膜动脉局灶或广泛狭窄
        /// </summary>
        [JsonPropertyName("retinalArteryFocalPointOrExtensiveStenosis")]
        public bool RetinalArteryFocalPointOrExtensiveStenosis { get; set; }

        /// <summary>
        /// 以上皆无
        /// </summary>
        [JsonPropertyName("none")]
        public bool None { get; set; }


        private int? _organDamageRank = null;

        public int OrganDamageRank
        {
            get
            {
                if (_organDamageRank == null)
                {
                    _organDamageRank = 0;
                    if (LeftVentricularHypertrophy) _organDamageRank                 += 1;
                    if (AtheroscleroticPlaques) _organDamageRank                     += 1;
                    if (RetinalArteryFocalPointOrExtensiveStenosis) _organDamageRank += 1;
                }

                return _organDamageRank.Value;
            }
        }
    }
}
