using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManagement.Helpers
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public static bool ContainsOther(this string source, string toCheck, StringComparison comp)
        {
            if (string.IsNullOrEmpty(toCheck) || string.IsNullOrEmpty(source))
                return true;

            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static bool Contains<T>(IEnumerator<T> sourceEnumerator, IEnumerator<T> sequenceEnumerator, IEqualityComparer<T> equalityComparer)
        {
            if (equalityComparer == null)
            {
                equalityComparer = EqualityComparer<T>.Default;
            }

            while (sequenceEnumerator.MoveNext())
            {
                if (sourceEnumerator.MoveNext())
                {
                    if (!equalityComparer.Equals(
                        sourceEnumerator.Current,
                        sequenceEnumerator.Current))
                    {
                        sequenceEnumerator.Reset();
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}
