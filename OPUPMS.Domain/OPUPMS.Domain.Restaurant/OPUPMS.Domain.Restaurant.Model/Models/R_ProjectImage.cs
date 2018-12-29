/***********************************************************************
 * Module:  CyxmTp.cs
 * Author:  ZMAdministrator
 * Purpose: Definition of the Class CyxmTp
 ***********************************************************************/

// <summary>
// 餐饮项目图片
// </summary>

using System;

namespace OPUPMS.Domain.Restaurant.Model
{
    /// 餐饮项目图片
    public class R_ProjectImage
    {


        ///<summary>
        ///</summary>
        public int Id { get; set; }


        ///<summary>
        /// 来源类别(1.餐饮项目;2.餐饮套餐)
        ///</summary>
        public int CyxmTpSourceType { get; set; }


        ///<summary>
        /// 图片路径
        ///</summary>
        public string Url { get; set; }


        ///<summary>
        /// 来源ID
        ///</summary>
        public int Source_Id { get; set; }


        ///<summary>
        /// 是否封面
        ///</summary>
        public bool IsCover { get; set; }

        public int Sorted { get; set; }
    }
}