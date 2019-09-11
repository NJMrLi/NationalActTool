using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalActTest
{
    /// <summary>
    /// 用户银子状态
    /// </summary>
    [Serializable]
    public class GetUserSliverInfo
    {
        /// <summary>
        /// 用户密码状态
        /// </summary>
        public bool UserPwdStatus { get; set; }
        /// <summary>
        /// 用户剩余消耗金额
        /// </summary>
        public long TodayCostRemain { get; set; }
        /// <summary>
        /// 用户银子数
        /// </summary>
        public long UserSliverCount { get; set; }

    }
}
