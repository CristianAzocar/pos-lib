using PosLib.Printing.NPI;
using PosLib.Printing.StarIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosLib.Printing
{
    /// <summary>
    /// This class is used to request printer instances.
    /// </summary>
    public static class PrinterManager
    {
        private static PrinterBase printer = null;

        /// <summary>
        /// Returns a printer instance.
        /// </summary>
        /// <param name="printerName">The name of the printer.</param>
        /// <returns>The printer instance.</returns>
        public static PrinterBase GetPrinter(string printerName)
        {
            if (string.IsNullOrWhiteSpace(printerName))
            {
                throw new ArgumentException("The name of the printer cannot be null, empty nor contain only white-spaces.", nameof(printerName));
            }

            if (printer != null)
            {
                if (printer.PrinterName == printerName)
                {
                    return printer;
                }
            }
            
            if (printerName.Contains("NII"))
            {
                printer = new NiiPrinter();
                printer.PrinterName = printerName;
                printer.PrinterType = PrinterType.Nii;
            }
            else if (printerName.Contains("TUP900"))
            {
                printer = new StarIOPrinter();
                printer.PrinterName = printerName;
                printer.PrinterType = PrinterType.TUP900;
            }
            else
            {
                throw new InvalidOperationException("The printer type could not be guessed. Specify it manually.");
            }

            return printer;
        }

        /// <summary>
        /// Returns a printer instance using the specified printer type.
        /// </summary>
        /// <param name="printerName">The name of the printer.</param>
        /// <param name="printerType">The type of the printer.</param>
        /// <returns>The printer instance.</returns>
        public static PrinterBase GetPrinter(string printerName, PrinterType printerType)
        {
            if (string.IsNullOrWhiteSpace(printerName))
            {
                throw new ArgumentException("The name of the printer cannot be null, empty nor contain only white-spaces.", nameof(printerName));
            }

            if (printer != null)
            {
                if (printer.PrinterName == printerName && printer.PrinterType == printerType)
                {
                    return printer;
                }
            }

            if (printerType == PrinterType.Nii)
            {
                printer = new NiiPrinter();
                printer.PrinterName = printerName;
                printer.PrinterType = printerType;
            }
            else if (printerType == PrinterType.TUP900)
            {
                printer = new StarIOPrinter();
                printer.PrinterName = printerName;
                printer.PrinterType = printerType;
            }
            else
            {
                throw new InvalidOperationException("The printer type is not supported.");
            }

            return printer;
        }
    }
}
