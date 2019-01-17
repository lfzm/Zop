#region Copyright (C) 2017 AL系列开源项目

/***************************************************************************
*　　	文件功能描述：Web上下文 Request,Response,Url 扩展类
*
*　　	创建人： 阿凌
*       创建人Email：513845034@qq.com
*       
*****************************************************************************/

#endregion

using Microsoft.AspNetCore.Http;
using System;
using System.Web;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// HttpContext 扩展类
    /// </summary>
    public static class HttpContextExtention
    {
        /// <summary>
        /// 判断是否为Ajax提交
        /// </summary>
        /// <returns></returns>
        public static bool IsAjax(this HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
        /// <summary>
        /// 获取客户端地址IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP(this HttpContext context)
        {
            string ip = context.Connection.RemoteIpAddress.ToString();
            if (ip.IndexOf("localhost") > -1 || ip.IndexOf("::1") > -1)
            {
                ip = "127.0.0.1";
                return ip;
            }
            else
                return ip;
        }


        /// <summary>
        /// 获取游客终端信息
        /// </summary>
        public static VisitorTerminal GetVisitorTerminal(this HttpContext context)
        {
            string userAgent = context.Request.Headers["User-Agent"];
            VisitorTerminal VT = new VisitorTerminal();
            VT.Time = DateTime.Now;

            if (string.IsNullOrWhiteSpace(userAgent))
                userAgent = "";
            VT.UserAgent = userAgent;

            string str2 = userAgent.ToLower();
            bool flag1 = str2.Contains("ipad");
            bool flag2 = str2.Contains("iphone os");
            bool flag3 = str2.Contains("midp");
            bool flag4 = str2.Contains("rv:1.2.3.4");
            bool flag5 = flag4 ? flag4 : str2.Contains("ucweb");
            bool flag6 = str2.Contains("android");
            bool flag7 = str2.Contains("windows ce");
            bool flag8 = str2.Contains("windows mobile");
            bool flag9 = str2.Contains("micromessenger");
            bool flag10 = str2.Contains("windows phone ");
            bool flag11 = str2.Contains("appwebview(ios)");
            bool flag12 = str2.Contains("aliapp");
            VT.Terminal = EnumVisitorTerminal.PC;
            if (flag1 || flag2 || (flag3 || flag5) || (flag6 || flag7 || (flag8 || flag10)))
                VT.Terminal = EnumVisitorTerminal.Moblie;
            if (flag1 || flag2)
            {
                VT.OperaSystem = EnumVisitorOperaSystem.IOS;
                VT.Terminal = EnumVisitorTerminal.Moblie;
                if (flag1)
                    VT.Terminal = EnumVisitorTerminal.PAD;
                if (flag11)
                    VT.Terminal = EnumVisitorTerminal.IOS;
            }
            if (flag6)
            {
                VT.OperaSystem = EnumVisitorOperaSystem.Android;
                VT.Terminal = EnumVisitorTerminal.Moblie;
            }

            if (flag9)
                VT.Terminal = EnumVisitorTerminal.WeiXin;
            if (flag12)
                VT.Terminal = EnumVisitorTerminal.Alipay;

            //判断是否为手机端
            if (VT.Terminal == EnumVisitorTerminal.Moblie ||
                VT.Terminal == EnumVisitorTerminal.PAD ||
                VT.Terminal == EnumVisitorTerminal.WeiXin ||
                VT.Terminal == EnumVisitorTerminal.IOS ||
                VT.Terminal == EnumVisitorTerminal.Alipay)
                VT.IsMobileTerminal = true;

            //获取客户端IP
            VT.ClientIP = context.GetIP();
            //获取访问地址
            VT.Uri = context.Request.Host.ToString();
            return VT;
        }



    }


    /// <summary>
    /// 终端类型
    /// </summary>
    public enum EnumVisitorTerminal
    {
        /// <summary>
        /// PC端
        /// </summary>
        PC = 0,
        /// <summary>
        /// 移动端
        /// </summary>
        Moblie = 1,
        /// <summary>
        /// 平板电脑
        /// </summary>
        PAD = 2,
        /// <summary>
        /// 微信
        /// </summary>
        WeiXin = 3,
        /// <summary>
        /// IOS
        /// </summary>
        IOS = 4,
        /// <summary>
        /// Android
        /// </summary>
        Android = 5,
        /// <summary>
        /// Alipay
        /// </summary>
        Alipay = 6,
        Other = 0x63

    }
    /// <summary>
    /// 终端系统类型
    /// </summary>
    public enum EnumVisitorOperaSystem
    {
        /// <summary>
        /// Windows
        /// </summary>
        Windows = 0,
        /// <summary>
        /// Android
        /// </summary>
        Android = 1,
        /// <summary>
        /// IOS
        /// </summary>
        IOS = 2,
        /// <summary>
        /// Linux
        /// </summary>
        Linux = 3,
        /// <summary>
        /// UNIX
        /// </summary>
        UNIX = 4,
        /// <summary>
        /// MacOS
        /// </summary>
        MacOS = 5,
        /// <summary>
        /// WindowsPhone
        /// </summary>
        WindowsPhone = 6,
        /// <summary>
        /// WindowsCE
        /// </summary>
        WindowsCE = 7,
        /// <summary>
        /// WindowsMobile
        /// </summary>
        WindowsMobile = 8,

        /// <summary>
        /// 其他
        /// </summary>
        Other = 0x63




    }

    /// <summary>
    /// 访问终端信息
    /// </summary>
    public class VisitorTerminal
    {
        /// <summary>
        /// 是否手机端
        /// </summary>
        public bool IsMobileTerminal { get; set; }
        /// <summary>
        /// 控制器
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// 请问地址
        /// </summary>
        public string Uri { get; set; }
        /// <summary>
        /// 终端系统类型
        /// </summary>
        public EnumVisitorOperaSystem OperaSystem { get; set; }
        /// <summary>
        /// 终端类型
        /// </summary>
        public EnumVisitorTerminal Terminal { get; set; }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIP { get; set; }
        /// <summary>
        /// HTTP 请求的 User-Agent 标头的值
        /// </summary>
        public string UserAgent { get; set; }
        /// <summary>
        /// 访问时间
        /// </summary>
        public DateTime Time { get; set; }
    }
}
