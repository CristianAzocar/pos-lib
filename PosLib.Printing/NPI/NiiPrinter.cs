using KellermanSoftware.CompareNetObjects;
using NiiPrinterCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PosLib.Printing.NPI
{
    /// <summary>
    /// Represents a NPI printer.
    /// </summary>
    public sealed class NiiPrinter : PrinterBase
    {
        private readonly NIIClassLib niiPrinter;
        private readonly CompareLogic compareLogic;
        private readonly Timer timer;
        private long jobId;
        private PrinterStatus lastStatus;
        private Exception lastException;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NiiPrinter"/> class.
        /// </summary>
        public NiiPrinter()
        {
            niiPrinter = new NIIClassLib();
            compareLogic = new CompareLogic();

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
            int statusCode;
            string aux = "";
            
            foreach (char c in value)
            {
                aux += string.Format("{0:X2}", Convert.ToUInt32(c));
            }

            statusCode = niiPrinter.NiiStartDoc(PrinterName, out jobId);
            EnsureSuccessStatusCode(statusCode);

            statusCode = niiPrinter.NiiPrint(PrinterName, aux, aux.Length, out jobId);
            EnsureSuccessStatusCode(statusCode);

            statusCode = niiPrinter.NiiEndDoc(PrinterName);
            EnsureSuccessStatusCode(statusCode);
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
            int resultCode;
            long statusCode;
            var printerStatus = new PrinterStatus();

            resultCode = niiPrinter.NiiGetStatus(PrinterName, out statusCode);
            if (resultCode != 0)
            {
                lastException = new NiiStatusException(resultCode);
                throw lastException;
            }

            statusCode = statusCode & 0x1F;
            printerStatus.ReceiptPaperNearEmpty = (statusCode & 0x01) > 0;
            printerStatus.CoverOpen = (statusCode & 0x02) > 0;
            printerStatus.ReceiptPaperEmpty = (statusCode & 0x04) > 0;
            printerStatus.OverTemp = (statusCode & 0x08) > 0;
            printerStatus.CutterError = (statusCode & 0x10) > 0;
            printerStatus.IsOffline = (statusCode & 0x80) > 0;

            if (lastStatus == null)
            {
                lastStatus = printerStatus;
            }

            return printerStatus;
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
            if (linesToFeed < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(linesToFeed), "The number of lines to feed must be greater than zero.");
            }

            Print("\x1B\x64" + (char)linesToFeed);
        }

        /// <summary>
        /// Cuts the paper.
        /// </summary>
        /// <param name="cutPaperType">One of the <see cref="CutPaperType"/> values.</param>
        public override void CutPaper(CutPaperType cutPaperType)
        {
            if (cutPaperType == CutPaperType.Full)
            {
                Print("\x1B\x69");
            }
            else
            {
                throw new InvalidOperationException($"The value {cutPaperType} is not supported on this printer.");
            }
        }
        
        /// <summary>
        /// Throws an exception if the status code is not zero.
        /// </summary>
        /// <param name="statusCode">The status code to verify.</param>
        private void EnsureSuccessStatusCode(int statusCode)
        {
            if (statusCode != 0)
            {
                throw new NiiException(statusCode);
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