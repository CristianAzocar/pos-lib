using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosLib.Printing
{
    /// <summary>
    /// Represents the status of the printer.
    /// </summary>
    public class PrinterStatus
    {
        /// <summary>
        /// Gets a value that indicates whether the paper is near end.
        /// </summary>
        public bool ReceiptPaperNearEmpty { get; internal set; }
        /// <summary>
        /// Gets a value that indicates whether the cover is open.
        /// </summary>
        public bool CoverOpen { get; internal set; }
        /// <summary>
        /// Gets a value that indicates whether the receipt printer unit is out of paper.
        /// </summary>
        public bool ReceiptPaperEmpty { get; internal set; }
        /// <summary>
        /// Gets a value that indicates whether the printer is overhated.
        /// </summary>
        public bool OverTemp { get; internal set; }
        /// <summary>
        /// Gets a value that indicates whether the cutter is jammed or otherwise in error.
        /// </summary>
        public bool CutterError { get; internal set; }
        /// <summary>
        /// Gets a value that indicates whether the printer is offline.
        /// </summary>
        public bool IsOffline { get; internal set; }
    }
}
