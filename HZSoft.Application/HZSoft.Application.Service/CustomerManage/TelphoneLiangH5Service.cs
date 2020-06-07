using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Entity.SystemManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace HZSoft.Application.Service.CustomerManage
{
    /// <summary>
    /// �� �� 6.1
    /// 
    /// �� ������������Ա
    /// �� �ڣ�2020-06-07 14:11
    /// �� ����ͷ�����ſ�
    /// </summary>
    public class TelphoneLiangH5Service : RepositoryFactory<TelphoneLiangH5Entity>, TelphoneLiangH5IService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<TelphoneLiangH5Entity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TelphoneLiangH5Entity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TelphoneLiangH5Entity GetEntity(int? keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(int? keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(int? keyValue, TelphoneLiangH5Entity entity)
        {
            if (keyValue!=0)
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="dtSource">ʵ�����</param>
        /// <returns></returns>
        public string BatchAddEntity(DataTable dtSource)
        {
            int rowsCount = dtSource.Rows.Count;
            //�жϺ�������
            //string greaterMsg = IsGreater(rowsCount);
            //if (!string.IsNullOrEmpty(greaterMsg))
            //{
            //    return greaterMsg;
            //}

            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();

            int columns = dtSource.Columns.Count;
            string cf = "";
            for (int i = 0; i < rowsCount; i++)
            {
                try
                {
                    string telphone = dtSource.Rows[i][0].ToString();
                    if (telphone.Length == 11)
                    {
                        var liang_Data = db.FindEntity<TelphoneLiangEntity>(t => t.Telphone == telphone && t.DeleteMark != 1);//ɾ�����Ŀ����ٴε���
                        if (liang_Data != null)
                        {
                            cf += telphone + ",";
                        }

                        //����ǰ7λȷ�����к���Ӫ��
                        string Number7 = telphone.Substring(0, 7);
                        string City = "", CityId = "", Operator = "";
                        var TelphoneData = db.FindEntity<TelphoneDataEntity>(t => t.Number7 == Number7);
                        if (TelphoneData != null)
                        {
                            if (string.IsNullOrEmpty(TelphoneData.City))
                            {
                                return "�Ŷγ���Ϊ�գ�" + Number7;
                            }
                            else
                            {
                                City = TelphoneData.City.Replace("��", "");
                                CityId = TelphoneData.CityId;
                                Operator = TelphoneData.Operate;
                            }
                        }
                        else
                        {
                            return "�Ŷβ����ڣ�" + Number7;
                        }

                        //�۸�
                        if (string.IsNullOrEmpty(dtSource.Rows[i][1].ToString()))
                        {
                            return telphone + "�۸�Ϊ��";
                        }
                        decimal Price = Convert.ToDecimal(dtSource.Rows[i][1].ToString());
                        //�ɱ���
                        if (string.IsNullOrEmpty(dtSource.Rows[i][2].ToString()))
                        {
                            return telphone + "�ɱ���Ϊ��";
                        }
                        decimal MinPrice = Convert.ToDecimal(dtSource.Rows[i][2].ToString());
                        //����
                        decimal ChaPrice = Price - MinPrice;

                        //�����
                        decimal CheckPrice = Convert.ToDecimal(dtSource.Rows[i][3].ToString());

                        //���
                        string itemName = dtSource.Rows[i][4].ToString();
                        if (string.IsNullOrEmpty(itemName))
                        {
                            return telphone + "���Ϊ��";
                        }
                        string itemNCode = "";
                        var DataItemDetail = db.FindEntity<DataItemDetailEntity>(t => t.ItemName == itemName);
                        if (DataItemDetail != null)
                        {
                            itemNCode = DataItemDetail.ItemValue;
                        }
                        else
                        {
                            return "���Ͳ����ڣ�" + itemName + ",���������ֵ���ά�������͡�";
                        }

                        //�ײ�
                        string Package = dtSource.Rows[i][5].ToString();
                        //״̬
                        string existStr = dtSource.Rows[i][6].ToString();

                        if (string.IsNullOrEmpty(existStr))
                        {
                            return telphone + "�ֿ�/����/��ɱ״̬Ϊ��";
                        }
                        if (existStr != "��ɱ" && existStr != "�ֿ�" && existStr != "Ԥ��")
                        {
                            return telphone + "�ֿ�/����/��ɱ״̬��д����";
                        }
                        int existMark = 0;
                        if (existStr == "��ɱ")
                        {
                            existMark = 2;
                        }
                        else if (existStr == "�ֿ�")
                        {
                            existMark = 1;
                        }
                        else
                        {
                            existMark = 0;
                        }

                        //��ֹ�����ƹ�۴��� ������ʾ���޷��ҵ��� 7
                        decimal? MaxPrice = null;
                        if (columns == 8)
                        {
                            //�ƹ��
                            string maxPriceStr = dtSource.Rows[i][7].ToString();
                            //�����ǰ�еĵ�Ԫ�񱨴�Ҳ��ת���ʹ���
                            if (!string.IsNullOrEmpty(maxPriceStr))
                            {
                                MaxPrice = Convert.ToDecimal(maxPriceStr);
                            }
                        }


                        //�������
                        TelphoneLiangEntity entity = new TelphoneLiangEntity()
                        {
                            Telphone = telphone,
                            Price = Price,
                            MaxPrice = MaxPrice,
                            MinPrice = MinPrice,
                            ChaPrice = ChaPrice,
                            CheckPrice = CheckPrice,
                            City = City,
                            CityId = CityId,
                            Operator = Operator,
                            Grade = itemNCode,
                            Package = Package,
                            ExistMark = existMark,
                            SellMark = 0,
                            DeleteMark = 0,
                            OrganizeId = OperatorProvider.Provider.Current().CompanyId,
                        };
                        entity.Create();
                        db.Insert(entity);
                    }

                }
                catch (Exception ex)
                {
                    LogHelper.AddLog(ex.Message);
                    return ex.Message;
                }

            }
            db.Commit();
            if (cf != "")
            {
                LogHelper.AddLog("�����ظ�������룺" + cf);
                return "�����ظ�������룺" + cf;
            }
            else
            {
                return "����ɹ�";
            }

        }
        #endregion
    }
}
