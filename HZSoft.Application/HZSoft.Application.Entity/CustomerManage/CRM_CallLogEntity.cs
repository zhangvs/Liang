using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-11-04 16:22
    /// 描 述：话单
    /// </summary>
    [Serializable]
    public class CRM_CallLogEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 话单id
        /// </summary>
        /// <returns></returns>
        public string Id { get; set; }
        /// <summary>
        /// 企业Id
        /// </summary>
        /// <returns></returns>
        public string EnterpriseId { get; set; }
        /// <summary>
        /// 坐席
        /// </summary>
        /// <returns></returns>
        public string WorkerUserName { get; set; }
        /// <summary>
        /// 坐席名称
        /// </summary>
        /// <returns></returns>
        public string WorkerName { get; set; }
        /// <summary>
        /// 坐席组
        /// </summary>
        /// <returns></returns>
        public string WorkerGroupId { get; set; }
        /// <summary>
        /// 坐席号码
        /// </summary>
        /// <returns></returns>
        public string WorkerPhoneNumber { get; set; }
        /// <summary>
        /// 话单上传时间
        /// </summary>
        /// <returns></returns>
        public DateTime? UploadTime { get; set; }
        /// <summary>
        /// 坐席IP
        /// </summary>
        /// <returns></returns>
        public string WorkerIP { get; set; }
        /// <summary>
        /// 呼叫类型
        /// </summary>
        /// <returns></returns>
        public int? CallType { get; set; }
        /// <summary>
        /// 客户号码
        /// </summary>
        /// <returns></returns>
        public string CallNumber { get; set; }
        /// <summary>
        /// 呼叫时间
        /// </summary>
        /// <returns></returns>
        public DateTime? CallTime { get; set; }
        /// <summary>
        /// 振铃时间
        /// </summary>
        /// <returns></returns>
        public DateTime? RingTime { get; set; }
        /// <summary>
        /// 接通时间
        /// </summary>
        /// <returns></returns>
        public DateTime? ConnectedTime { get; set; }
        /// <summary>
        /// 通话结束时间
        /// </summary>
        /// <returns></returns>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 0 失败，未接通，1 成功，接通
        /// </summary>
        /// <returns></returns>
        public int? CallResult { get; set; }
        /// <summary>
        /// 是否有录音
        /// </summary>
        /// <returns></returns>
        public bool? IsRecordingFile { get; set; }
        /// <summary>
        /// 录音文件名
        /// </summary>
        /// <returns></returns>
        public string RecordingFile { get; set; }
        /// <summary>
        /// 本地文件名（上传前）
        /// </summary>
        /// <returns></returns>
        public string LocalFileName { get; set; }
        /// <summary>
        /// 录音文件大小K
        /// </summary>
        /// <returns></returns>
        public int? RecordingFileSize { get; set; }
        /// <summary>
        /// 录音格式
        /// </summary>
        /// <returns></returns>
        public int? RecordingType { get; set; }
        /// <summary>
        /// 坐席挂机，客户挂机，其他 
        /// 未使用保留
        /// </summary>
        /// <returns></returns>
        public string HangReson { get; set; }
        /// <summary>
        /// 通话时长
        /// </summary>
        /// <returns></returns>
        public int? CallDuration { get; set; }
        /// <summary>
        /// 录音是否已上传
        /// </summary>
        /// <returns></returns>
        public bool? IsRecordingFileUploaded { get; set; }
        /// <summary>
        /// 录音上传时间
        /// </summary>
        /// <returns></returns>
        public DateTime? RecordingUploadTime { get; set; }
        /// <summary>
        /// 话单是否已上传
        /// </summary>
        /// <returns></returns>
        public bool? IsUploaded { get; set; }
        /// <summary>
        /// 是否话单和录音都已上传完毕
        /// </summary>
        /// <returns></returns>
        public bool? IsDone { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = Guid.NewGuid().ToString();
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
                                            }
        #endregion
    }
}