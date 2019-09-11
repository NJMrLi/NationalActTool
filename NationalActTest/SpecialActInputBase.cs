using System;
using System.Runtime.Serialization;

namespace NationalActTest
{
    /// <summary>
    /// 活动Api输入基类
    /// </summary>
    [DataContract]
    [Serializable]
    public class SpecialActInputBase
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public long UserId { get; set; }
        /// <summary>
        /// 时间戳精确到秒（10位）
        /// </summary>
        [DataMember]
        public long TimeStamp { get; set; }
        /// <summary>
        /// MD5加密后的字符串
        /// </summary>
        [DataMember]
        public string CheckCode { get; set; }
        /// <summary>
        /// 客户端类型 1.PC  2.移动
        /// </summary>
        [DataMember]
        public AwardClientType ClientType { get; set; }
        /// <summary>
        /// 移动端平台 1.安卓 2.IOS
        /// </summary>
        [DataMember]
        public int Os { get; set; }
        /// <summary>
        /// 客户端可以不填写，由服务端自己获取
        /// </summary>
        [DataMember]
        public string Ip { get; set; } = string.Empty;

        /// <summary> 用户名称 </summary>
        [DataMember]
        public string UserName { get; set; }

    }
}
