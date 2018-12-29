using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUPMS.Infrastructure.Common
{
    /// <summary>
    /// 扩展AutoMapper 自动转换
    /// 源对象与目标对象需满足AutoMapper 的自动转换条件，或配置好自定义源对象与目标对象的属性映射
    /// </summary>
    /// <typeparam name="TSource">转换源对象</typeparam>
    /// <typeparam name="TTarget">转换目标对象</typeparam>
    public static class AutoMapperExtend<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        /// <summary>
        /// 转换成一个指定目标对象
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="target">源对象</param>
        /// <returns></returns>
        public static TTarget ConvertToObj(TSource source, TTarget target)
        {
            if (source == null)
                return null;

            target = AutoMapper.Mapper.Map<TSource, TTarget>(source);
            return target;
        }

        /// <summary>
        /// 转换成指定目标对象列表
        /// </summary>
        /// <param name="sourceList">源对象集合</param>
        /// <param name="targetList">目标对象列表</param>
        /// <returns></returns>
        public static List<TTarget> ConvertToList(IEnumerable<TSource> sourceList, List<TTarget> targetList)
        {
            if (sourceList == null)
                return null;

            targetList = AutoMapper.Mapper.Map<IEnumerable<TSource>, List<TTarget>>(sourceList);
            return targetList;
        }
    }
}
