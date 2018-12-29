using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    public class R_AutoAdditionProject
    {
        ///<summary>
        ///</summary>
        public int Id { get; set; }
        public int R_AutoAddition_Id { get; set; }
        public int R_ProjectDetail_Id { get; set; }
        public AutoAdditionType AutoAdditionType { get; set; }
    }
}
