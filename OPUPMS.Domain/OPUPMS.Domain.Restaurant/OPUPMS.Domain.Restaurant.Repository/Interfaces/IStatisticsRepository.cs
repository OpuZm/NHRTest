using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface IStatisticsRepository
    {
        List<ProducedStatisticsDTO> Produced(ProducedSearchDTO req);
        List<TurnDutyStatisticsGroupDto> GetTurnDuty(TurnDutySearchDTO req);
        List<ReportListDTO> GetReportList();
    }
}
