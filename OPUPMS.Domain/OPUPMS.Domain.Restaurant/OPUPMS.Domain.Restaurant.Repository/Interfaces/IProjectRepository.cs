using System.Collections.Generic;
using OPUPMS.Domain.Restaurant.Model;
using OPUPMS.Domain.Restaurant.Model.Dtos;

namespace OPUPMS.Domain.Restaurant.Repository
{
    public interface IProjectRepository
    {
        int Create(ProjectCreateDTO req, out string msg);

        int Update(ProjectCreateDTO req, out string msg);

        ProjectCreateDTO GetModel(int id);

        List<ProjectListDTO> GetList(out int total, ProjectSearchDTO req);
        List<ProjectListDTO> GetList(int category);
        bool ExtendCreate(ProjectExtendCreateDTO req);
        bool DetailCreate(ProjectDetailCreateDTO req);
        List<ProjectDetailListDTO> GetDetailList(int category,bool isHasPackage=false);
        bool SpecificationSubmit(int cyxmId,List<R_ProjectDetail> list);
        List<ProjectExtendDTO> GetProjectExtends(int project);
        List<ProjectAndDetailListDTO> GetProjectAndDetailList(int category, bool isHasPackage = false);
        /// <summary>
        /// 获取菜品特殊要求分类列表
        /// </summary>
        /// <returns></returns>
        ProjectExtendSplitListDTO GetProjectExtendSplitList();
        bool ProjectClearSubmit(List<ProjectClearDTO> req);
        List<OrderProjectExtends> GetProjectExtendSplitListNew();
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        bool IsDelete(int id);
        List<OrderProjectExtends> GetProjectExtendSplitListByProjectId(int projectId);
        void CharcodeInit();

        bool IsEnable(List<int> ids, bool enable);
        ProjectImageDTO EditProjectImage(ProjectImageDTO req);
        List<ProjectImageDTO> GetProjectImages(int id, int cyxmTpSourceType);
        bool DeleteProjectImage(int piid);
        bool BathUpdateProjectImage(List<ProjectImageDTO> req);
        ProjectImageDTO GetProjectImageModel(int id);
        bool ProjectRecommendSubmit(List<ProjectRecomandDTO> req);
        List<PrinterProject> OrderDetailPrintTesting(List<OrderDetailDTO> req);
        bool GetOrderDetailIsTeset();
    }
}
