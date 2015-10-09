using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshFruit.Common.BaseCommon
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="seqA"></param>
        /// <param name="seqB"></param>
        /// <returns></returns>
        public static IEnumerable<T> Combine<T>(this IEnumerable<T> seqA, IEnumerable<T> seqB)
        {

            List<T> list = seqA as List<T>;
            using (var iteratorB = seqB.GetEnumerator())
            {
                bool hasValue;
                hasValue = iteratorB.MoveNext();
                while (hasValue)
                {
                    list.Add(iteratorB.Current);
                    yield return iteratorB.Current;
                }
            }
            //return list;
        }


        /// <summary>
        /// //
        /// 摘要:
        ///     确定序列是否包含任何元素。
        ///
        /// 参数:
        ///   source:
        ///     无需检查是否为空的 System.Collections.Generic.IEnumerable<T>。
        ///
        /// 类型参数:
        ///   TSource:
        ///     source 中的元素的类型。
        ///
        /// 返回结果:
        ///     如果源序列包含任何元素，则为 true；否则为 false。
        ///
        /// 异常:
        ///   System.ArgumentNullException:
        ///     source 为 null。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNotNullEmpty<TSource>(this IEnumerable<TSource> source)
        {
            bool flag = true;
            if (source == null)
                flag = false;
            else if (!source.Any())
                flag = false;
            return flag;
        }
    }
}
