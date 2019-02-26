using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosLib.Printing
{
    /// <summary>
    /// Specifies the type of printer that an instance of the <see cref="PrinterBase"/> class represents.
    /// </summary>
    public enum PrinterType
    {
        /// <summary>
        /// The printer is a NII model.
        /// </summary>
        Nii,
        /// <summary>
        /// The printer is a TUP900 model.
        /// </summary>
        TUP900
    }
}
