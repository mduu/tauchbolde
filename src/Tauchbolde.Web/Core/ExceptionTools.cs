using System;
using System.Text;

namespace Tauchbolde.Web.Core
{
    public static class ExceptionTools
    {
        /// <summary>
        /// Unwinds the exception message on all inner exceptions.
        /// </summary>
        /// <returns>All the messages joined together.</returns>
        /// <param name="exception">Exception to start unwinding the exception messages.</param>
        public static string UnwindMessage(this Exception exception)
        {
            var messageBuilder = new StringBuilder();

            var ex = exception;
            var isFirst = true;
            while (ex != null)
            {
                if (isFirst)
                {
                    messageBuilder.Append(" ");
                    isFirst = false;
                }

                messageBuilder.Append(ex.Message);

                ex = ex.InnerException;
            }

            return messageBuilder.ToString();
        }
    }
}