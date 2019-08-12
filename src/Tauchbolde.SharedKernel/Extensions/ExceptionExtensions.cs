using System;
using System.Text;

namespace Tauchbolde.SharedKernel.Extensions
{
    public static class ExceptionExtensions
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
                if (!isFirst)
                {
                    messageBuilder.Append(" ");
                }

                messageBuilder.Append(ex.Message);

                ex = ex.InnerException;
                isFirst = false;
            }

            return messageBuilder.ToString();
        }

    }
}