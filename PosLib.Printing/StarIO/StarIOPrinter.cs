using KellermanSoftware.CompareNetObjects;
using StarMicronics.StarIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PosLib.Printing.StarIO
{
    /// <summary>
    /// Represents a StarIO printer.
    /// </summary>
    public sealed class StarIOPrinter : PrinterBase
    {
        private readonly CompareLogic compareLogic;
        private readonly Timer timer;
        private PrinterStatus lastStatus;
        private Exception lastException;        

        /// <summary>
        /// Initializes a new instance of the <see cref="StarIOPrinter"/> class.
        /// </summary>
        public StarIOPrinter()
        {
            timer = new Timer() { AutoReset = false };
            timer.Interval = 2000;
            timer.Elapsed += Timer_Elapsed;
        }

        /// <summary>
        /// Prints the specified value.
        /// </summary>
        /// <param name="value">The value to print.</param>
        public override void Print(string value)
        {
            IPort port = null;

            try
            {
                port = Factory.I.GetPort(PrinterName, string.Empty, Timeout);
                var isOnline = port.GetOnlineStatus();

                if (!isOnline)
                {
                    throw new PrinterException("The printer is offline.");
                }

                byte[] dataByteArray = Encoding.GetEncoding("Windows-1252").GetBytes(value);
                uint amountWritten = 0;
                uint amountWrittenKeep = 0;
                while (dataByteArray.Length > amountWritten)
                {
                    amountWrittenKeep = amountWritten;
                    amountWritten += port.WritePort(dataByteArray, amountWritten, (uint)dataByteArray.Length - amountWritten);
                    if (amountWrittenKeep == amountWritten)
                    {
                        throw new PrinterException("Can't send data.");
                    }
                }

                if (amountWritten != dataByteArray.Length)
                {
                    throw new PrinterException("All data was not sent.");
                }
            }
            finally
            {
                if (port != null)
                {
                    Factory.I.ReleasePort(port);
                }
            }
        }

        /// <summary>
        /// Prints the specified value, followed by a new line.
        /// </summary>
        /// <param name="value">The value to print.</param>
        public override void PrintLine(string value)
        {
            Print(value);
            Print(Environment.NewLine);
        }

        /// <summary>
        /// Returns the status of the printer.
        /// </summary>
        /// <returns>An object that contains the status of the printer.</returns>
        public override PrinterStatus GetStatus()
        {
            IPort port = null;
            var printerStatus = new PrinterStatus();

            try
            {
                port = Factory.I.GetPort(PrinterName, string.Empty, Timeout);
                var status = port.GetParsedStatus();

                if (status == null)
                {
                    throw new PrinterException("The printer status is null.");
                }

                printerStatus.ReceiptPaperNearEmpty = status.ReceiptPaperNearEmptyInner || status.ReceiptPaperNearEmptyOuter;
                printerStatus.CoverOpen = status.CoverOpen;
                printerStatus.ReceiptPaperEmpty = status.ReceiptPaperEmpty;
                printerStatus.OverTemp = status.OverTemp;
                printerStatus.CutterError = status.CutterError;
                printerStatus.IsOffline = status.Offline;

                return printerStatus;
            }
            finally
            {
                if (port != null)
                {
                    Factory.I.ReleasePort(port);
                }
            }
        }

        /// <summary>
        /// Starts pooling the printer status.
        /// </summary>
        public override void StartPoolingStatus()
        {
            timer.Enabled = true;
        }
        
        /// <summary>
        /// Feeds a specified number of lines.
        /// </summary>
        /// <param name="linesToFeed">The number of lines to feed.</param>
        public override void FeedLines(byte linesToFeed)
        {
            Print("\x1B\x61" + (char)linesToFeed);
        }

        /// <summary>
        /// Cuts the paper.
        /// </summary>
        /// <param name="cutPaperType">One of the <see cref="CutPaperType"/> values.</param>
        public override void CutPaper(CutPaperType cutPaperType)
        {
            if (cutPaperType == CutPaperType.Full)
            {
                Print("\x1B\x64\x00");
            }
            else if (cutPaperType == CutPaperType.Partial)
            {
                Print("\x1B\x64\x01");
            }
            else
            {
                throw new InvalidOperationException($"The value {cutPaperType} is not supported on this printer.");
            }
        }

        /// <summary>
        /// Handles the <see cref="Timer.Elapsed"/> event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The data of the event.</param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                var status = GetStatus();
                var comparisonResult = compareLogic.Compare(status, lastStatus);

                lastException = null;
                lastStatus = status;

                if (!comparisonResult.AreEqual)
                {
                    OnPrinterStatusChanged(new PrinterStatusChangedEventArgs(lastStatus));
                }
            }
            catch (Exception ex)
            {
                var comparisonResult = compareLogic.Compare(ex, lastException);
                lastException = ex;

                if (!comparisonResult.AreEqual)
                {
                    OnPrinterStatusChanged(new PrinterStatusChangedEventArgs(lastException));
                }
            }
            finally
            {
                timer.Enabled = true;
            }
        }
    }
}
