using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPUPMS.Model.Common;
//using OPUPMS.Model.PMSEntity;
using OPUPMS.Business.Common;
//using OPUPMS.Business.PMSService;
using OPUPMS.Model.Common;
using System.Diagnostics;
//using OPUPMS.UI.Web.ConsoleTest.ViewModels;

namespace OPUPMS.Web.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var entity = new DeployEntity();
            //entity.AppId = 100;
            //entity.DeployCode = "100";
            //entity.DeployPackage = "100";
            //entity.DeployContent = "100";
            //entity.UploadUserId = 100;
            //entity.UploadTime = DateTime.Now;
            //entity.DeployType = 100;

            ////var obj = new ModelCommon();

            //var service = new BDeployService();
            ////插入
            ////service.Insert(obj);
            //service.Insert(entity);

            //////查询所有
            //////var allList = service.GetAll<DeployEntity>();
            //var allList = service.GetAll();

            ////多条件查询
            //var pgMain = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };

            //var pga = new PredicateGroup() { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            //pga.Predicates.Add(Predicates.Field<DeployEntity>(f => f.DeployCode, Operator.Eq, "100"));
            //pga.Predicates.Add(Predicates.Field<DeployEntity>(f => f.ID, Operator.Ge, 47));
            //pga.Predicates.Add(Predicates.Field<DeployEntity>(f => f.ID, Operator.Le, 48));
            //pgMain.Predicates.Add(pga);

            //var pgb = new PredicateGroup() { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            //pgb.Predicates.Add(Predicates.Field<DeployEntity>(f => f.DeployCode, Operator.Eq, "10000"));
            //pgMain.Predicates.Add(pgb);

            ////var specialList = service.GetList<DeployEntity>(pgMain).ToList();
            ////var specialList = service.GetList<DeployEntity>(pgMain).ToList();

            ////分页查询
            //long allRowsCount = 0;
            ////var pageList = service.GetPageList<DeployEntity>(1, 2, out allRowsCount);
            
            //Stopwatch sw = new Stopwatch();
            Console.WriteLine(string.Format("代码执行开始，时间：{0} ", DateTime.Now));
            Console.WriteLine("判断数字 536870913 是否包含值 ");
            var v = Console.ReadLine();
            //sw.Start();
            //BCzdmService service = new BCzdmService();
            //List<CzdmEntity> list = new List<CzdmEntity>();
            //CzdmEntity obj = null;
            //for (int i = 0; i < 100; i++)
            //{
            //    service = new BCzdmService();
            //    list = service.GetAll();
            //    obj = service.LoadByKey();
            //}
            ////var list = service.GetAll();
            //sw.Stop();
            //Console.WriteLine(string.Format("共有记录：{0}条，执行100次完成共用时间：{1} 毫秒", list.Count, sw.Elapsed.Milliseconds));
            bool flag = Program.ValidPermission(536870913, int.Parse(v));
            if (flag)
                Console.WriteLine(string.Format("数字 {0} 二进制包含在值 536870913", v));
            else
                Console.WriteLine(string.Format("数字 {0} 二进制未包含在值 536870913", v));

            Console.ReadLine();
        }

        public static bool ValidPermission(int userPermissionValue, int validateValue)
        {
            //var sourceValue = Convert.ToByte(userPermissionValue);
            //var targetValue = Convert.ToByte(validateValue);
            var result = (userPermissionValue & validateValue);
            return result == validateValue;
        }
    }
}
