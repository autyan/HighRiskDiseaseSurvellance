using System;
using System.Text.Json.Serialization;

namespace HighRiskDiseaseSurvellance.Dto.Models
{
    public class Atherosclerosis : ISurveillance
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
        /// 糖尿病
        /// </summary>
        [JsonPropertyName("diabetes")]
        public bool Diabetes { get; set; }

        /// <summary>
        /// 体重指数（kg/m2，BMI=体重（kg）/身高的平方（m2）
        /// </summary>
        public double Bmi => Weight / Math.Pow(Height / 100.0, 2.0);

        public decimal Compute()
        {
            var score = 0.0m;
            if (Gender == 0)
            {
                score = Age switch
                        {
                            >= 35 and <= 39 => 0,
                            >= 40 and <= 44 => 1,
                            >= 45 and <= 49 => 2,
                            >= 50 and <=54  => 3,
                            >= 55 and <= 59 => 4,
                            >= 60 => 4 + (Age - 60) / 5,
                            _               => score
                        };
                score = Sbp switch
                        {
                            < 120            => score - 2,
                            > 120 and <= 129 => score,
                            > 130 and <= 139 => score + 1,
                            > 140 and <= 159 => score + 2,
                            > 160 and <= 179 => score + 5,
                            >= 180           => score + 8,
                            _                => score
                        };

                if (Diabetes)
                {
                    score += 1;
                }

                if (Smoke)
                {
                    score += 2;
                }

                if (Tc >= 5.2)
                {
                    score += 1;
                }

                score = Bmi switch
                        {
                            > 24 and <= 27.9 => score + 1,
                            >= 28            => score + 2,
                            _                => score
                        };

            }
            else
            {
                score = Age switch
                        {
                            >= 35 and <= 39 => 0,
                            >= 40 and <= 44 => 1,
                            >= 45 and <= 49 => 2,
                            >= 50 and <=54  => 3,
                            >= 55 and <= 59 => 4,
                            >= 60           => 4 + (Age - 60) / 5,
                            _               => score
                        };
                score = Sbp switch
                        {
                            < 120            => score - 2,
                            > 120 and <= 129 => score,
                            > 130 and <= 139 => score + 1,
                            > 140 and <= 159 => score + 2,
                            > 160 and <= 179 => score + 3,
                            >= 180           => score + 4,
                            _                => score
                        };

                if (Diabetes)
                {
                    score += 2;
                }

                if (Smoke)
                {
                    score += 1;
                }

                if (Tc >= 5.2)
                {
                    score += 1;
                }

                score = Bmi switch
                        {
                            > 24 and <= 27.9 => score + 1,
                            >= 28            => score + 2,
                            _                => score
                        };
            }

            return score;
        }
    }
}
