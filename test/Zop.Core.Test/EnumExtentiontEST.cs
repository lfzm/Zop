using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Zop.Core.Test
{
    public class EnumExtentionTest
    {
        [Fact]
        public void GetDespTest()
        {
            var res = BankType.Alipay.GetDescription();
        }
        [Fact]
        public void ToEnumDirsTest()
        {
            var res = BankType.Alipay.GetDescriptionList();
        }
        /// <summary>
        /// 银行卡类型
        /// </summary>
        public enum BankType
        {
            /// <summary>
            /// 银行卡
            /// </summary>
            [Description("银行卡")]
            BankCard = 1,
            /// <summary>
            /// 支付宝
            /// </summary>
            [Description("支付宝")]
            Alipay = 2,
            /// <summary>
            /// 微信
            /// </summary>
            [Description("微信")]
            Weixin = 3
        }
    }
}
