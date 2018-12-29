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
    public class BaseDto
    {
        public int Key { get; set; }

        public string Text { get; set; }

        public bool Selected { get; set; }

        public int Sort { get; set; }

        public string Value { get; set; }
    }
}
