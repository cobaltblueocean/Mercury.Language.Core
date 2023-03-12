using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class IEnumeratorExtension
    {

        public static int Count<T>(this IEnumerator<T> obj)
        {
            return obj.ConvertToList().Count();
        }

        public static int Index<T>(this IEnumerator<T> enumerator)
        {
            var current = enumerator.Current;
            var list = enumerator.ToList();

            return list.IndexOf(current);
        }

        public static List<T> ToList<T>(this IEnumerator<T> enumerator)
        {
            var list = new List<T>();
            enumerator.Reset();

            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);
            }

            return list;
        }

        public static Boolean HasNext<T>(this IEnumerator<T> enumerator)
        {
            var list = enumerator.ToList();
            var index = enumerator.Index();

            return list.Count == 1 ? false : index < list.Count - 1 ? true : false;
        }

        //public static Boolean HasNext<T>(this IEnumerator<T> obj)
        //{
        //    return (obj.IndexOf(obj.Current) < obj.Count());
        //}

        public static T Next<T>(this IEnumerator<T> source)
        {
            source.MoveNext();
            return source.Current;
        }

        //public static T Next<T>(this IEnumerator<T> obj)
        //{
        //    if (obj.MoveNext())
        //    {
        //        return obj.Current;
        //    }
        //    else
        //    {
        //        return default;
        //    }
        //}

        public static int IndexOf<T>(this IEnumerator<T> obj, T value)
        {
            var enumerable = obj.ConvertToList();

            int index = enumerable.TakeWhile(x => x.Equals(value)).Count();
            return index == enumerable.Count() ? -1 : index;
        }
    }
}
