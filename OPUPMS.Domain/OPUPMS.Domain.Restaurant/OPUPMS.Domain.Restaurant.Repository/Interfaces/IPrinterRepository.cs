using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface IPrinterRepository
    {
        bool Create(PrinterDTO req);

        bool Update(PrinterDTO req);

        PrinterDTO GetModel(int id);

        List<PrinterDTO> GetList(PrinterSearchDTO req, out int total);
        List<PrinterDTO> GetList();
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        bool IsDelete(int id);
        int GetPrintModel();
        bool GetNightTrial();

    }
}
