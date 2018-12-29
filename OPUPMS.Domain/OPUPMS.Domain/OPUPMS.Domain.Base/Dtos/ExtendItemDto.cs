using OPUPMS.Domain.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Domain.Base.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class ExtendItemDto
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int TypeId { get; set; }

        public string TypeName { get; set; }

        public int CompanyId { get; set; }

        public string ItemValue { get; set; }

        public int Sort { get; set; }

    }
}
