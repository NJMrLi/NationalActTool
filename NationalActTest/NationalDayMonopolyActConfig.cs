using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace NationalActTest
{
    /// <summary>
    /// 大富翁活动配置
    /// </summary>
    [DataContract]
    [Serializable]
    public class NationalDayMonopolyActConfig
    {        
        [DataMember]
        public int MasterActId { get; set; }
        /// <summary>
        /// 子活动ID   
        /// </summary>
        [DataMember]
        public int SubActId { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        [DataMember]
        public int ExpireHours { get; set; }
        /// <summary>
        /// PC免费GUID
        /// </summary>
        [DataMember]
        public string PcFreeGuid { get; set; }
        /// <summary>
        /// PC付费GUID
        /// </summary>
        [DataMember]
        public string PcPayGuid { get; set; }
        /// <summary>
        /// PC加倍奖池
        /// </summary>
        [DataMember]
        public string PcFreeDoubleGuid { get; set; }
        /// <summary>
        /// PC付费加倍奖池
        /// </summary>
        [DataMember]
        public string PcPayDoubleGuid { get; set; }
        /// <summary>
        /// 移动免费GUID
        /// </summary>
        [DataMember]
        public string MobileFreeGuid { get; set; }
        /// <summary>
        /// 移动付费
        /// </summary>
        [DataMember]
        public string MobilePayGuid { get; set; }
        /// <summary>
        /// 移动双倍
        /// </summary>
        [DataMember]
        public string MobileFreeDoubleGuid { get; set; }
        /// <summary>
        /// 移动双倍
        /// </summary>
        [DataMember]
        public string MobilePayDoubleGuid { get; set; }
        /// <summary>
        /// PC返利奖池
        /// </summary>
        [DataMember]
        public string PcRebateGuid { get; set; }
        /// <summary>
        /// 移动返利奖池
        /// </summary>
        [DataMember]
        public string MobileRebateGuid { get; set; }       
        /// <summary>
        /// 多抽次数
        /// </summary>
        [DataMember]
        public int MutiCount { get; set; }
        /// <summary>
        /// 返利常数(运营需求的一个常量)
        /// </summary>
        [DataMember]
        public int RebateConstMin { get; set; }
        /// <summary>
        /// 返利常数(运营需求的一个常量)
        /// </summary>
        [DataMember]
        public int RebateConstMax { get; set; }
        /// <summary>
        /// 返利次数
        /// </summary>
        [DataMember]
        public int RebateCount { get; set; }
        /// <summary>
        /// 单次消耗银两数量
        /// </summary>
        [DataMember]
        public int SingleCostSliver { get; set; }
        /// <summary>
        /// 多次消耗银两数量
        /// </summary>
        [DataMember]
        public int MutiCostSliver { get; set; }
        /// <summary>
        /// 免费奖池具体内容
        /// </summary>
        [DataMember]
        public List<AwardConfigItem> FreeAwardConfig { get; set; } = new List<AwardConfigItem>();
        /// <summary>
        /// 付费奖池内容
        /// </summary>
        [DataMember]
        public List<AwardConfigItem> PayAwardConfig { get; set; } = new List<AwardConfigItem>();
        /// <summary>
        /// 双倍奖池内容
        /// </summary>
        [DataMember]
        public List<AwardConfigItem> DoubleAwardConfig { get; set; } = new List<AwardConfigItem>();
        /// <summary>
        /// 付费双倍奖池
        /// </summary>
        [DataMember]
        public List<AwardConfigItem> DoublePayAwardConfig { get; set; } = new List<AwardConfigItem>();

    }

    /// <summary>
    /// 大富翁奖池配置详细
    /// </summary>
    [DataContract]
    [Serializable]
    public class AwardConfigItem
    {
        /// <summary>
        /// 奖励Id
        /// </summary>
        [DataMember]
        public int AwardIndex { get; set; }

        /// <summary>
        /// 概率整数值
        /// </summary>
        [DataMember]
        public int Procent { get; set; }
        /// <summary>            
        /// 奖励的客户端类型
        /// </summary>
        [DataMember]
        public AwardClientType Client { get; set; }
        /// <summary>
        /// 奖励的类型
        /// </summary>
        [DataMember]
        public ItemTypeEnum ItemType { get; set; }
        /// <summary>
        /// 奖励的Code 
        /// </summary>
        [DataMember]
        public string ItemCode { get; set; }
        /// <summary>
        /// 奖励的数量
        /// </summary>
        [DataMember]
        public int ItemCount { get; set; }
        /// <summary>
        /// 特殊物品的唯一标识
        /// </summary>
        [DataMember]
        public int ItemId { get; set; }

        [DataMember]
        public string ItemName { get; set; }

        [DataMember]
        public int UnitId { get; set; }
    }


}
