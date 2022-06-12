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

        public double Bmi => Weight / (Height * Height);

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
                    if ((Gender == 1 && Waistline >= 90) || (Gender == 0 && Waistline >= 85) || Bmi >= 28)
                        _basicRank += 1;
                    if (RiskFactors.Hyperhomocysteine) _basicRank += 1;
                }

                return _basicRank.Value;
            }
        }

        public decimal Compute()
        {
            var basicScore = 0;
            if (TargetOrganDamage.OrganDamageRank > 1)
            {
                if (Sbp >= 180 || Dbp >= 110)
                {
                    basicScore = 76;
                }
                else if(Sbp is >= 140 and <= 179 || Dbp is >= 90 and <= 109)
                {
                    basicScore = 51;
                }
            }

            if (ConcomitantClinicalDisorders.ConcomitantClinicalDisordersRank > 1
             && (Sbp >= 140 || Dbp >= 90))
            {
                basicScore = 76;
            }

            if (BasicRank >= 3)
            {
                if (basicScore < 76 && (Sbp >= 180 || Dbp >= 110)) basicScore = 76;

                if (basicScore < 51 && (Sbp is >= 160 and <= 179 || Dbp is >= 100 and <= 109)) basicScore = 51;

                if (basicScore < 51 && (Sbp is >= 140 and <= 159 || Dbp is >= 90 and <= 99)) basicScore = 51;
            }

            if (BasicRank is >= 1 and < 3)
            {
                if (basicScore < 76 && (Sbp >= 180 || Dbp >= 110)) basicScore = 76;

                if (basicScore < 26 && (Sbp is >= 160 and <= 179 || Dbp is >= 100 and <= 109)) basicScore = 26;

                if (basicScore < 26 && (Sbp is >= 140 and <= 159 || Dbp is >= 90 and <= 99)) basicScore = 26;
            }

            if (BasicRank == 0)
            {
                if (basicScore < 51 && (Sbp >= 180 || Dbp >= 110)) basicScore = 51;

                if (basicScore < 26 && (Sbp is >= 160 and <= 179 || Dbp is >= 100 and <= 109)) basicScore = 26;

                if (basicScore < 1 && (Sbp is >= 140 and <= 159 || Dbp is >= 90 and <= 99)) basicScore = 0;
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
        /// 糖尿病肾病
        /// </summary>
        [JsonPropertyName("diabeticNephropathy")]
        public bool DiabeticNephropathy { get; set; }

        /// <summary>
        /// 肾功能受损
        /// </summary>
        [JsonPropertyName("impairedRenalFunction")]
        public bool ImpairedRenalFunction { get; set; }

        /// <summary>
        /// 血肌酐升高 > 177ummol/L
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
        /// 重度高血压性视网膜病变
        /// </summary>
        [JsonPropertyName("severeHypertensiveRetinopathy")]
        public bool SevereHypertensiveRetinopathy { get; set; }

        /// <summary>
        /// 糖尿病
        /// </summary>
        [JsonPropertyName("diabetes")]
        public bool Diabetes { get; set; }

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
                    if (DiabeticNephropathy) _concomitantClinicalDisordersRank                             += 1;
                    if (ImpairedRenalFunction) _concomitantClinicalDisordersRank                           += 1;
                    if (ElevatedSerumCreatinine) _concomitantClinicalDisordersRank                         += 1;
                    if (AorticDissection) _concomitantClinicalDisordersRank                                += 1;
                    if (PeripheralVascularDisease) _concomitantClinicalDisordersRank                       += 1;
                    if (SevereHypertensiveRetinopathy) _concomitantClinicalDisordersRank                   += 1;
                    if (Diabetes) _concomitantClinicalDisordersRank                                        += 1;
                }

                return _concomitantClinicalDisordersRank.Value;
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
