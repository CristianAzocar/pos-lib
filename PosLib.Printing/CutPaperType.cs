using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosLib.Printing
{
    /// <summary>
    /// Specifies the types of cut of the paper.
    /// </summary>
    public enum CutPaperType
    {
        /// <summary>
        /// The printer executes full cutting of the paper.
        /// </summary>
        Full,
        /// <summary>
        /// The printer partially cuts the paper.
        /// </summary>
        Partial
    }
}
