using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosLib.Printing
{
    /// <summary>
    /// The exception that is thrown when an error occurs with the printer.
    /// </summary>
    public class PrinterException : Exception
    {
        /// <summary>
        /// Gets the error code that is associated with this exception.
        /// </summary>
        public int ErrorCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterException"/> class.
        /// </summary>
        public PrinterException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterException"/> class using a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PrinterException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterException"/> class with a specified error code.
        /// </summary>
        /// <param name="errorCode">The error code that is associated with this exception.</param>
        /// <param name="message">The message that describes the error.</param>
        public PrinterException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
