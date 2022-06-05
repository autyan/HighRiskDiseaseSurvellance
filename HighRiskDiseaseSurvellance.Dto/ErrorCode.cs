using System.ComponentModel;

namespace HighRiskDiseaseSurvellance.Dto
{
    public enum ErrorCode
    {
        [Description("微信用户登录失败，请重新尝试")]
        WeChatUserLoginFailed = 10001,

        [Description("用户不存在")]
        UserNotFound,

        [Description("管理员用户未找到")]
        OfficeUserNotFound,

        [Description("用户名或密码错误")]
        OfficeUserAuthFailed
    }
}
