using System;
using System.ComponentModel.DataAnnotations;
using HZSoft.Application.Code;
using System.ComponentModel.DataAnnotations.Schema;
using HZSoft.Util;
using System.Web;
using Newtonsoft.Json;

namespace HZSoft.Application.Entity.BaseManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创建人：佘赐雄
    /// 日 期：2015.11.02 14:27
    /// 描 述：机构管理
    /// </summary>
    [Table("Base_Organize")]
    public class OrganizeEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 机构主键
        /// </summary>	
        public string OrganizeId { get; set; }
        /// <summary>
        /// 机构分类
        /// </summary>		
        public int? Category { get; set; }
        /// <summary>
        /// 父级主键
        /// </summary>		
        public string ParentId { get; set; }
        /// <summary>
        /// 公司外文
        /// </summary>		
        public string EnCode { get; set; }
        /// <summary>
        /// 公司中文
        /// </summary>		
        public string ShortName { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>		
        public string FullName { get; set; }
        /// <summary>
        /// 公司性质
        /// </summary>		
        public string Nature { get; set; }
        /// <summary>
        /// 外线电话
        /// </summary>		
        public string OuterPhone { get; set; }
        /// <summary>
        /// 内线电话
        /// </summary>		
        public string InnerPhone { get; set; }
        /// <summary>
        /// 传真
        /// </summary>		
        public string Fax { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>		
        public string Postalcode { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>		
        public string Email { get; set; }
        /// <summary>
        /// 负责人主键
        /// </summary>		
        public string ManagerId { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>		
        public string Manager { get; set; }
        /// <summary>
        /// 省主键
        /// </summary>		
        public string ProvinceId { get; set; }
        /// <summary>
        /// 市主键
        /// </summary>		
        public string CityId { get; set; }
        /// <summary>
        /// 县/区主键
        /// </summary>		
        public string CountyId { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>		
        public string Address { get; set; }
        /// <summary>
        /// 公司官方
        /// </summary>		
        public string WebAddress { get; set; }
        /// <summary>
        /// 成立时间
        /// </summary>		
        public DateTime? FoundedTime { get; set; }
        /// <summary>
        /// 经营范围
        /// </summary>		
        public string BusinessScope { get; set; }
        /// <summary>
        /// 层
        /// </summary>		
        public int? Layer { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>		
        public int? SortCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>		
        public int? DeleteMark { get; set; }
        /// <summary>
        /// 有效标志
        /// </summary>		
        public int? EnabledMark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		
        public string Description { get; set; }
        /// <summary>
        /// 图1
        /// </summary>		
        public string Img1 { get; set; }
        /// <summary>
        /// 图2
        /// </summary>		
        public string Img2 { get; set; }
        /// <summary>
        /// 图3
        /// </summary>		
        public string Img3 { get; set; }
        /// <summary>
        /// 图4
        /// </summary>		
        public string Img4 { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>		
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>		
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>		
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>		
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>		
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>		
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 协议标识
        /// </summary>
        public int? AgreementMark { get; set; }
        /// <summary>
        /// 协议同意时间
        /// </summary>
        public DateTime? AgreementDate { get; set; }
        /// <summary>
        /// 顶级机构
        /// </summary>		
        public string TopOrganizeId { get; set; }
        /// <summary>
        /// 代售其它机构
        /// </summary>		
        public string OtherOrganizeId { get; set; }
        /// <summary>
        /// 是否是vip客户
        /// </summary>		
        public int? VipMark { get; set; }
        /// <summary>
        /// 平台管理员
        /// </summary>		
        public int? PTAdminMark { get; set; }
        /// <summary>
        /// 浏览
        /// </summary>		
        public int? SeeCount { get; set; }
        /// <summary>
        /// 最后浏览时间
        /// </summary>
        public DateTime? LastSeeDate { get; set; }
        /// <summary>
        /// 分享
        /// </summary>		
        public int? ShareCount { get; set; }
        /// <summary>
        /// 最后分享时间
        /// </summary>
        public DateTime? LastShareDate { get; set; }
        /// <summary>
        /// 加盟
        /// </summary>		
        public int? JoinCount { get; set; }
        /// <summary>
        /// 最后加盟时间
        /// </summary>
        public DateTime? LastJoinDate { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.OrganizeId = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.DeleteMark = 0;
            this.EnabledMark = 1;
            SeeCount = 0;
            ShareCount = 0;
            JoinCount = 0;
            if (ParentId == "wk207fa1a9-160c-4943-a89b-8fa4db0547ce" || ParentId == "b4a7e43a-0f58-4635-bb39-0185a0a130a7" || ParentId == "7e89a99c-8eae-44f6-873a-a434229f74bf")//文楷靓号0级
            {
                //this.Description = GetShortUrl(Config.GetValue("Domain") + "/WeChatManage/Liang/Main?organizeId=" + OrganizeId);
                this.Description = Config.GetValue("Domain") + "/WeChatManage/Liang/Main?organizeId=" + OrganizeId;
            }
            else
            {
                //this.Description = GetShortUrl(Config.GetValue("Domain") + "/WeChatManage/Liang/Index?organizeId=" + OrganizeId);
                this.Description = Config.GetValue("Domain") + "/WeChatManage/Liang/Index?organizeId=" + OrganizeId;
            }
        }

        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.OrganizeId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// TXT格式短网址API接口
        ///接口：http://suo.im/api.php?url=urlencode('要缩短的网址')
        ///例如：http://suo.im/api.php?url=http%3a%2f%2fwww.baidu.com
        ///返回：http://suo.im/baidu
        /// </summary>
        /// <param name="longUrl"></param>
        /// <returns></returns>
        public string GetShortUrl(string longUrl)
        {
            string result = HttpUtility.UrlEncode(longUrl);
            string shortUrl = HttpClientHelper.Get(@"http://suo.im/api.php?url="+ result);
            return shortUrl;
        }


        /// <summary>
        /// 协议状态
        /// </summary>
        /// <param name="keyValue"></param>
        public void AgreementModify(string keyValue)
        {
            this.OrganizeId = keyValue;
            this.AgreementDate = DateTime.Now;
        }


        /// <summary>
        /// 新浪转换短链接
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Convert_SINA_Short_Url(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return "";
            }
            //api地址
            var address = "http://api.t.sina.com.cn/short_url/shorten.json?source=2815391962";
            address += "&url_long=" + HttpUtility.UrlEncode(url);
            //http请求
            var json = HttpClientHelper.Get(address);
            //json转换
            var urls = JsonConvert.DeserializeObject<sina_short_url>(json);
            if (urls != null)
            {
                return urls.url_short;
            }
            return "";
        }
        #endregion
    }
}
//[{"url_short":"http://t.cn/EZ9UsTu","url_long":"http://hmk.lywenkai.com/WeChatManage/Liang/Main?organizeId=076b0487-fda4-41a3-a8dc-adc1cefd56ec","type":0}]
public class sina_short_url
{
    public string url_short { get; set; }

    public string url_long { get; set; }
    public int type { get; set; }
}