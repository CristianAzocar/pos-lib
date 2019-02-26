using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosLib.Printing
{
    /// <summary>
    /// Represents the base class for classes that implement the logic for printers.
    /// </summary>
    public abstract class PrinterBase
    {
        /// <summary>
        /// Gets or sets the name of the printer.
        /// </summary>
        public string PrinterName { get; set; }
        /// <summary>
        /// Gets or sets the amount of time, in milliseconds, to wait for the communication requests to finish. The default value is 10000.
        /// </summary>
        public int Timeout { get; set; } = 10000;
        /// <summary>
        /// Gets or sets the type of the printer.
        /// </summary>
        public PrinterType PrinterType { get; set; }
        
        /// <summary>
        /// Returns the status of the printer.
        /// </summary>
        /// <returns>An object that contains the status of the printer.</returns>
        public abstract PrinterStatus GetStatus();
        /// <summary>
        /// Prints the specified value.
        /// </summary>
        /// <param name="value">The value to print.</param>
        public abstract void Print(string value);
        /// <summary>
        /// Prints the specified value, followed by a new line.
        /// </summary>
        /// <param name="value">The value to print.</param>
        public abstract void PrintLine(string value);
        /// <summary>
        /// Starts pooling the printer status.
        /// </summary>
        public abstract void StartPoolingStatus();
        /// <summary>
        /// Feeds a specified number of lines.
        /// </summary>
        /// <param name="linesToFeed">The number of lines to feed.</param>
        public abstract void FeedLines(byte linesToFeed);
        /// <summary>
        /// Cuts the paper.
        /// </summary>
        /// <param name="cutPaperType">One of the <see cref="CutPaperType"/> values.</param>
        public abstract void CutPaper(CutPaperType cutPaperType);
        
        /// <summary>
        /// The event that is raised whenever the status of the printer changes.
        /// </summary>
        public event EventHandler<PrinterStatusChangedEventArgs> PrinterStatusChanged;

        /// <summary>
        /// Handles the <see cref="PrinterStatusChanged"/> event.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected virtual void OnPrinterStatusChanged(PrinterStatusChangedEventArgs e)
        {
            EventHandler<PrinterStatusChangedEventArgs> handler = PrinterStatusChanged;
            handler?.Invoke(this, e);
        }
    }
}
