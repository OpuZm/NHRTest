/***********************************************************************
 * Module:  CytcMx.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class CytcMx
 ***********************************************************************/

// <summary>
// ²ÍÒûÌ×²ÍÃ÷Ï¸
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// ²ÍÒûÌ×²ÍÃ÷Ï¸
    public class R_PackageDetail
    {


        ///<summary>
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// ²ÍÒûÌ×²Í
        ///</summary>
        public int R_Package_Id { get; set; }


        ///<summary>
        /// ²ÍÒûÏîÄ¿Ã÷Ï¸
        ///</summary>
        public int R_ProjectDetail_Id { get; set; }


        ///<summary>
        /// ÊıÁ¿
        ///</summary>
        public decimal Num { get; set; }

        public bool IsDelete { get; set; }

    }
}