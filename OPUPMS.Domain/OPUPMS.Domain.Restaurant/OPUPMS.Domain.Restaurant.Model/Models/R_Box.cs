/***********************************************************************
 * Module:  Cybx.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class Cybx
 ***********************************************************************/

// <summary>
// ²ÍÒû°üÏá
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// ²ÍÒû°üÏá
    public class R_Box
    {


        ///<summary>
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// Ãû³Æ
        ///</summary>
        public string Name { get; set; }


        ///<summary>
        /// ÃèÊö
        ///</summary>
        public string Description { get; set; }


        ///<summary>
        /// ËùÊô²ÍÌüÇøÓò
        ///</summary>
        public int R_Area_Id { get; set; }

        /// <summary>
        /// ËùÊô²ÍÌü
        /// </summary>
        public int R_Restaurant_Id { get; set; }
        public bool IsDelete { get; set; }

    }
}