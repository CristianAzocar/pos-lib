using PosLib.Printing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosLib.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            PrinterBase printer;

            try
            {
                printer = PrinterManager.GetPrinter("Star TUP900 Presenter (TUP992)");

                // Use the following lines to manually specify the printer type
                // printer = PrinterManager.GetPrinter("Star TUP900 Presenter (TUP992)", PrinterType.TUP900);
                // printer = PrinterManager.GetPrinter("NII ExD NP-K205", PrinterType.Nii);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("The printer type could not be guessed, please specify it manually.");
                Console.ReadKey();
                return;
            }

            var printerStatus = printer.GetStatus();
            WritePrinterStatus(printerStatus);

            // To get notified whenever the status of the printer changes, subscribe to the PrinterStatusChanged event.
            // printer.PrinterStatusChanged += Printer_PrinterStatusChanged;
            // printer.StartPoolingStatus();

            // Prints a text, feeds 5 lines, and then cuts the paper
            printer.Print("Hello world");
            printer.FeedLines(5);
            printer.CutPaper(CutPaperType.Full);

            Console.ReadKey();
        }

        private static void Printer_PrinterStatusChanged(object sender, PrinterStatusChangedEventArgs e)
        {
            Console.WriteLine("The status of the printer has changed");

            if (e.Exception != null)
            {
                Console.WriteLine("An error has occured: {0}", e.Exception.Message);
            }
            else
            {
                WritePrinterStatus(e.PrinterStatus);
            }
        }

        private static void WritePrinterStatus(PrinterStatus printerStatus)
        {
            Console.WriteLine("Offline: {0}", printerStatus.IsOffline);
            Console.WriteLine("Receipt paper near empty: {0}", printerStatus.ReceiptPaperNearEmpty);
            Console.WriteLine("Receipt paper empty: {0}", printerStatus.ReceiptPaperEmpty);
            Console.WriteLine("Cover open: {0}", printerStatus.CoverOpen);
            Console.WriteLine("Over temp: {0}", printerStatus.OverTemp);
            Console.WriteLine("Cutter error: {0}", printerStatus.CutterError);
        }
    }
}
