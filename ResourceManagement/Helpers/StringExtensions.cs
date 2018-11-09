using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public class MyResource : IDisposable
    {
        // Pointer to an external unmanaged resource. 
        private IntPtr handle;
        // Other managed resource this class uses. 
        private Component component = new Component();
        // Track whether Dispose has been called. 
        private bool disposed = false;

        // The class constructor. 
        public MyResource(IntPtr handle)
        {
            this.handle = handle;
        }

        // Implement IDisposable. 
        // Do not make this method virtual. 
        // A derived class should not be able to override this method. 
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method. 
            // Therefore, you should call GC.SupressFinalize to 
            // take this object off the finalization queue 
            // and prevent finalization code for this object 
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios. 
        // If disposing equals true, the method has been called directly 
        // or indirectly by a user's code. Managed and unmanaged resources 
        // can be disposed. 
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed. 
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing)
                {
                    // Dispose managed resources.
                    component.Dispose();
                }

                // Call the appropriate methods to clean up 
                // unmanaged resources here. 
                // If disposing is false, 
                // only the following code is executed.
                CloseHandle(handle);
                handle = IntPtr.Zero;

                // Note disposing has been done.
                disposed = true;

            }
        }

        // Use interop to call the method necessary 
        // to clean up the unmanaged resource.
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

        // Use C# destructor syntax for finalization code. 
        // This destructor will run only if the Dispose method 
        // does not get called. 
        // It gives your base class the opportunity to finalize. 
        // Do not provide destructors in types derived from this class.
        ~MyResource()
        {
            // Do not re-create Dispose clean-up code here. 
            // Calling Dispose(false) is optimal in terms of 
            // readability and maintainability.
            Dispose(false);
        }
    }
}
