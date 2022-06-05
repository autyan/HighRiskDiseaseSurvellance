using System.ComponentModel;

namespace HighRiskDiseaseSurvellance.Dto
{
    public enum OrderPayStatus
    {
        [Description("未支付")]
        Unpaid,

        [Description("已关闭")]
        Closed,

        [Description("支付中")]
        Paying,

        [Description("支付完成")]
        Paid,

        [Description("已退款")]
        Refund
    }

    public enum SurveillanceRecordStatus
    {
        [Description("待支付")]
        Unpaid,

        [Description("已完成")]
        Finished
    }
}
