using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPUPMS.Domain.Restaurant.Services.Interfaces;
using OPUPMS.Infrastructure.Common;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Repository;
using SqlSugar;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using System.Reflection;
using System.IO;
using OPUPMS.Infrastructure.Common.Operator;

namespace OPUPMS.Domain.Restaurant.Services
{
    public class PrintService : SqlSugarService, IPrintService
    {
        readonly IOrderPayRecordRepository _orderPayRecordRepository;
        readonly ICheckOutRepository _checkOutRepository;
        readonly ICheckOutService _checkOutService;
        readonly ITableRepository _tableRepository;

        public PrintService(IOrderPayRecordRepository orderPayRecordRepository, 
            ICheckOutRepository checkOutRepository,
            ICheckOutService checkOutService,
            ITableRepository tableRepository)
        {
            _orderPayRecordRepository = orderPayRecordRepository;
            _checkOutRepository = checkOutRepository;
            _checkOutService = checkOutService;
            _tableRepository = tableRepository;
        }

        public bool CheckedOut(WholeOrPartialCheckoutDto req, bool isLocked)
        {
            bool result = true;
            #region 验证数据
            //_checkOutService.VerifyOrderInfo(req);
            //var checkOutOrderDTO = _checkOutService.GetCheckOutOrderDTO(req.OrderId, req.TableIds);
            //_checkOutService.VerifyAndCalcDetailInfo(req, checkOutOrderDTO);
            #endregion

            var userInfo = OperatorProvider.Provider.GetCurrent();
            var tableList = _tableRepository.GetlistByIds(req.TableIds);    //获取台号列表
            var model = new CheckOutPrint() {
                OrderType = isLocked ? "结账单" : "预结单",
                RestaurantName = req.RestaurantName??string.Empty,
                CheckOutUser = userInfo.UserName,
                OpenUser = req.OperUserName??string.Empty,
                ReserveUser = string.Empty,
                PrintDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                BillDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                Market = string.Empty,
                PersonNum = "0",
                OrderNo = "",
                TableName = string.Join(",",tableList.Select(p => p.Name).ToArray()),
                ListOrderDetailDTO=req.ListOrderDetailDTO,
                ConAmount=req.ConAmount.ToString(),
                GiveAmount=req.GiveAmount.ToString(),
                DiscountRateNow=req.DiscountRateNow.ToString(),
                DiscountAmount=req.DiscountAmount.ToString(),
                OriginalAmount=req.OriginalAmount.ToString(),
                ContactPerson=req.ContactPerson
            };
            Type type = typeof(CheckOutPrint);
            Type typeDetail = typeof(OrderDetailDTO);

            #region 打印内容
            PrintInvoice print = new PrintInvoice();
            var tcpClient = print.GetMyPrint();
            var stream = print.BuildStream();
            var inputStream = print.GetStream(tcpClient,stream);
            string txt = string.Empty;
            int left = 0;
            string key = string.Empty;
            foreach (var item in print.root.Element("Page").Elements())
            {
                if (item.Name== "Text")
                {
                    txt = item.Value;
                    left = Convert.ToInt32(item.Attribute("Left").Value);
                    if (txt.Contains("{{") && txt.Contains("}}"))
                    {
                        key = ReplaceTemplate(txt);
                        txt = txt.Replace("{{" + key + "}}", type.GetProperty(key).GetValue(model).ToString());
                    }
                    print.PrintText(inputStream, txt, left);
                }
                else if (item.Name== "Loop")
                {
                    
                    string lopValues = item.Attribute("Values").Value;
                    if (lopValues== "Detials")
                    {
                        if (model.ListOrderDetailDTO.Any())
                        {
                            foreach (var detail in model.ListOrderDetailDTO)
                            {
                                foreach (var loopItem in item.Elements("Text"))
                                {
                                    txt = loopItem.Value;
                                    left = Convert.ToInt32(item.Attribute("Left"));
                                    if (txt.Contains("{{") && txt.Contains("}}"))
                                    {
                                        key = ReplaceTemplate(txt);
                                        txt = txt.Replace("{{" + key + "}}", typeDetail.GetProperty(key).GetValue(detail).ToString());
                                    }
                                    print.PrintText(inputStream, txt, left);
                                }
                            }
                        }
                    }
                }
            }
            //inputStream.Close();
            print.PrintText(inputStream, print.root.ToString(), 0);
            print.QieZhi(inputStream);
            print.DiposeStreamClient(tcpClient, inputStream);

            #endregion
            return result;
        }

        public bool CheckedOutBill(CheckOutBillDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                var tableCount = db.Queryable<R_OrderTable>()
                    .Where(p => req.OrderTableIds.Contains(p.R_Table_Id) && p.R_Order_Id == req.OrderId && p.IsCheckOut==false).Count();
                if (tableCount==req.OrderTableIds.Count())
                {
                    if (req.IsLocked)
                    {
                        result = db.Update<R_OrderTable>(new { IsLock = true }, 
                            p => p.R_Order_Id == req.OrderId && req.OrderTableIds.Contains(p.R_Table_Id));
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
                return result;
            }
        }

        public bool Unlock(CheckOutBillDTO req)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                var tableCount = db.Queryable<R_OrderTable>()
                    .Where(p => req.OrderTableIds.Contains(p.R_Table_Id) && p.R_Order_Id == req.OrderId && p.IsCheckOut == false).Count();
                if (tableCount == req.OrderTableIds.Count())
                {
                    result = db.Update<R_OrderTable>(new { IsLock = false },
    p => p.R_Order_Id == req.OrderId && req.OrderTableIds.Contains(p.R_Table_Id));
                }
                else
                {
                    result = false;
                }
                return result;
            }
        }

        private string ReplaceTemplate(string template)
        {
            string result = template;
            int beginIndex = template.IndexOf("{{");
            int endIndex = template.LastIndexOf("}}");
            result = template.Substring(beginIndex + 2, endIndex - beginIndex - 2);
            return result;
        }
    }
}
