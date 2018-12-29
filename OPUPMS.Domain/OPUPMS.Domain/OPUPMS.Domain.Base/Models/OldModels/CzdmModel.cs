using Dapper.FastCrud;
using OPUPMS.Infrastructure.Common;
using Smooth.IoC.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace OPUPMS.Domain.Base.Models
{
    /// <summary>
    /// 操作代码 实体类
    /// </summary>
    [Table("SUsers")]
    public class CzdmModel : Entity<int>
    {
        //static CzdmModel()
        //{
        //    OrmConfiguration.GetDefaultEntityMapping<CzdmModel>()
        //        .SetProperty(entity => entity.Id,
        //            prop =>
        //            {
        //                prop.SetDatabaseColumnName("Czdmdm00");
        //            });
        //}

        /// <summary>
        /// 操作代码 主键列
        /// </summary>        
        private int _id;
        private string _usercode;
        private string _username;
        private string _userpwd;
        private string _cardno;
        private DateTime _birthdate;
        private string _gender;
        private int _status;
        private string _email;
        private string _phone;
        private int? _creator;
        private DateTime _createdtime;
        private int _logincount;
        private bool _isdeleted;
        private DateTime _updatetime;
        private string _remark;
        private int? _usertype;
        private int? _groupid;
        private bool _istechnicalassistance;
        private int? _restaurantauthority;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserCode
        {
            set { _usercode = value; }
            get { return _usercode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserPwd
        {
            set { _userpwd = value; }
            get { return _userpwd; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CardNo
        {
            set { _cardno = value; }
            get { return _cardno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime BirthDate
        {
            set { _birthdate = value; }
            get { return _birthdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Gender
        {
            set { _gender = value; }
            get { return _gender; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Creator
        {
            set { _creator = value; }
            get { return _creator; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedTime
        {
            set { _createdtime = value; }
            get { return _createdtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int LoginCount
        {
            set { _logincount = value; }
            get { return _logincount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDeleted
        {
            set { _isdeleted = value; }
            get { return _isdeleted; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateTime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? UserType
        {
            set { _usertype = value; }
            get { return _usertype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? GroupId
        {
            set { _groupid = value; }
            get { return _groupid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsTechnicalAssistance
        {
            set { _istechnicalassistance = value; }
            get { return _istechnicalassistance; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? RestaurantAuthority
        {
            set { _restaurantauthority = value; }
            get { return _restaurantauthority; }
        }
    }

    public class UserRestaurant
    {
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
    }
}
