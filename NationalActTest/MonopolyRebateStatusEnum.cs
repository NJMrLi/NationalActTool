using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalActTest
{
    public enum MonopolyRebateStatusEnum
    {
        未提现 = 0,
        提现成功 = 1,
        提现失败 = 2
    }

    public enum AwardClientType
    {
        不限 = 0,
        PC = 1,
        移动 = 2
    }

    public enum ItemTypeEnum
    {
        系统物品 = 0,
        特殊物品 = 1
    }
}
