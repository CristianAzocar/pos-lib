using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosLib.Printing
{
    /// <summary>
    /// Contains the event data for the printer status change.
    /// </summary>
    public class PrinterStatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the status of the printer.
        /// </summary>
        public PrinterStatus PrinterStatus { get; internal set; }
        /// <summary>
        /// Gets the exception associated with the error returned by the printer.
        /// </summary>
        public Exception Exception { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterStatusChangedEventArgs"/> class.
        /// </summary>
        public PrinterStatusChangedEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterStatusChangedEventArgs"/> class with a given printer status.
        /// </summary>
        /// <param name="printerStatus">The status of the printer.</param>
        public PrinterStatusChangedEventArgs(PrinterStatus printerStatus)
        {
            PrinterStatus = printerStatus;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterStatusChangedEventArgs"/> class with a given exception.
        /// </summary>
        /// <param name="exception">The exception thrown by the printer.</param>
        public PrinterStatusChangedEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
