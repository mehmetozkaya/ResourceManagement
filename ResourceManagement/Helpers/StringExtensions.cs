using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
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

        public static bool ExContains(this string fullText, string value)
        {
            return ExIndexOf(fullText, value) > -1;
        }

        public static bool ExEquals(this string text, string textToCompare)
        {
            return text.Equals(textToCompare, StringComparison.OrdinalIgnoreCase);
        }

        public static bool ExHasAllEquals(this string text, params string[] textArgs)
        {
            for (int index = 0; index < textArgs.Length; index++)
                if (ExEquals(text, textArgs[index]) == false) return false;
            return true;
        }

        public static bool ExHasEquals(this string text, params string[] textArgs)
        {
            for (int index = 0; index < textArgs.Length; index++)
                if (ExEquals(text, textArgs[index])) return true;
            return false;
        }

        public static bool ExHasNoEquals(this string text, params string[] textArgs)
        {
            return ExHasEquals(text, textArgs) == false;
        }

        public static bool ExHasNotAllEquals(this string text, params string[] textArgs)
        {
            for (int index = 0; index < textArgs.Length; index++)
                if (ExEquals(text, textArgs[index])) return false;
            return true;
        }
      
        public static int ExIndexOf(this string fullText, string value)
        {
            return fullText.IndexOf(value, StringComparison.OrdinalIgnoreCase);
        }

        public static bool ExNotEquals(this string text, string textToCompare)
        {
            return ExEquals(text, textToCompare) == false;
        }
    }

    public class SimpleCleanup : IDisposable
    {
        // some fields that require cleanup
        private SafeHandle handle;
        private bool disposed = false; // to detect redundant calls

        public SimpleCleanup()
        {
            this.handle = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                    if (handle != null)
                    {
                        handle.Dispose();
                    }
                }

                // Dispose unmanaged managed resources.

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
