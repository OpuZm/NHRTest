using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPUPMS.Web.Hotel.Models.Dtos
{
    public class MenuJsonDto
    {
        public int id { get; set; }
        public string title { get; set; }
        public int menuvalue { get; set; }
        public string icon { get; set; }
        public bool spread { get; set; }
        public string href { get; set; }
        [JsonIgnore]
        public List<MenuJsonDto> submenus { get; set; }
        public int parentId { get; set; }
    }
}