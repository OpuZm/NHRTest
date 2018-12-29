using System.Collections.Generic;
using System.Linq;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;
using OPUPMS.Domain.Restaurant.Repository.IocManagerMoudles;
using SqlSugar;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public class PrinterRepository : SqlSugarService, IPrinterRepository
    {
        public PrinterRepository()
        {

        }

        public bool Create(PrinterDTO printer)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                Printer model = ConvertToModel(printer);
                model.RealStatus = PrintStatus.正常;
                model.RealStatusRemark = "";

                if (db.Insert(model) == null)
                {
                    result = false;
                }

                return result;
            }
        }

        public List<PrinterDTO> GetList(PrinterSearchDTO req, out int total)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                //string order = "Id desc";
                List<PrinterDTO> list = new List<PrinterDTO>();
                var models = db.Queryable<Printer>().Where(p => p.IsDelete == false).ToList();
                list = ConvertToInfoList(models);
                total = list.Count;

                return list.Skip(req.offset).Take(req.limit).ToList();
            }
        }

        public List<PrinterDTO> GetList()
        {
            using (var db = new SqlSugarClient(Connection))
            {
                List<PrinterDTO> list = new List<PrinterDTO>();

                var models = db.Queryable<Printer>().ToList();
                list = ConvertToInfoList(models);
                return list;
            }
        }

        public PrinterDTO GetModel(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                PrinterDTO result = null;
                if (id == 0)
                    return result;

                var data = db.Queryable<Printer>().InSingle(id);

                result = ConvertToInfo(data);
                return result;
            }
        }

        public bool Update(PrinterDTO dto)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = false;
                result = db.Update<Printer>(
                    new
                    {
                        Name = dto.Name,
                        Code = dto.Code,
                        Remark = dto.Remark,
                        PcName = dto.PcName,
                        PrintPort = dto.PrintPort,
                        IpAddress = dto.IpAddress,
                        IsDelete = dto.IsDelete
                    }, x => x.Id == dto.Id);
                return result;
            }
        }

        private Printer ConvertToModel(PrinterDTO info)
        {
            if (info == null)
                return null;
            return AutoMapper.Mapper.Map<Printer>(info);
        }

        private PrinterDTO ConvertToInfo(Printer model)
        {
            if (model == null)
                return null;
            return AutoMapper.Mapper.Map<PrinterDTO>(model);
        }

        private List<PrinterDTO> ConvertToInfoList(List<Printer> sourceList)
        {
            List<PrinterDTO> list = new List<PrinterDTO>();


            foreach (var item in sourceList)
            {
                list.Add(ConvertToInfo(item));
            }

            return list;
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool IsDelete(int id)
        {
            using (var db = new SqlSugarClient(Connection))
            {
                bool result = true;
                result = db.Update<Printer>(new { IsDelete = true }, x => x.Id == id);
                return result;
            }
        }

        public int GetPrintModel()
        {
            return PrintModel;
        }

        public bool GetNightTrial()
        {
            return NightTrial;
        }
    }
}