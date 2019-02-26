# POS Lib

The POS Lib is a .NET library that provides the main functions to work with point of sales (POS) printers, using the DLLs provided for each manufacturer.
Currently the only supported printers are:

- Nippon NP-K205 / NP-K305
- StarIO TUP900

## Features

- Print text.
- Get the status of the printer.
- Feed lines.
- Cut the paper.

## Getting Started

These instructions will get you a copy of the project and running on your local machine.

### Installing

Clone this repository, open `PosLib.sln` with Visual Studio 2015 or superior and compile it.

### Prerequisites

- .NET Framework 4.5

### Dependencies

- [KellermanSoftware Compare-NET-Objects](https://github.com/GregFinzer/Compare-Net-Objects)
- NiiPrinterCLib.dll
- StarIO.dll

## Usage

After you clone this repository and compile it, go to `PosLib.Printing/bin/Debug` or `PosLib.Printing/bin/Release`, depending if you compiled it under Debug or Release mode.  
There you will find the file `PosLib.Printing.dll` which you have to reference in your own project.  

After you reference the DLL file in your project, you must first create a printer instance:

```cs
using PosLib.Printing;

// Create a new printer instance using the name of the printer.
// The library will try to guess the type of the printer.
var printer = PrinterManager.GetPrinter("Star TUP900 Presenter (TUP992)");

// If the library can't guess the type of the printer or guess it wrong,
// specify it manually.
var printer = PrinterManager.GetPrinter("Star TUP900 Presenter (TUP992)", PrinterType.TUP900);
```

After you instantiate the printer, you are ready to work with the printer:

```cs
// Print a simple text
printer.Print("Hello world");

// Feed 5 lines
printer.FeedLines(5);

// Fully cut the paper
printer.CutPaper(CutPaperType.Full);

// Partially cut the paper
printer.CutPaper(CutPaperType.Partial);
```

You can get the status of the printer like this:

```cs
// Get the status of the printer
var printerStatus = printer.GetStatus();

if (printerStatus.IsOffline)
{
    // The printer is offline, do something...
}
```

Or if you prefer, you can get notified when the status of the printer changes subscribing to the `PrinterStatusChanged` event.

```cs
// Subscribe to the event
printer.PrinterStatusChanged += Printer_PrinterStatusChanged;

// Start checking for changes in the status of the printer
printer.StartPoolingStatus();

// This handler will be called whenever the status of the printer changes
private static void Printer_PrinterStatusChanged(object sender, PrinterStatusChangedEventArgs e)
{
    if (e.Exception != null)
    {
        Console.WriteLine("An error has occured: {0}", e.Exception.Message);
    }
    else
    {
        Console.WriteLine("Offline: {0}", printerStatus.IsOffline);
        Console.WriteLine("Receipt paper near empty: {0}", printerStatus.ReceiptPaperNearEmpty);
        Console.WriteLine("Receipt paper empty: {0}", printerStatus.ReceiptPaperEmpty);
        Console.WriteLine("Cover open: {0}", printerStatus.CoverOpen);
        Console.WriteLine("Over temp: {0}", printerStatus.OverTemp);
        Console.WriteLine("Cutter error: {0}", printerStatus.CutterError);
    }
}
```

## TO-DO list

There are many improvements and functionalities to add to the library, such as:

- Check if the printer exists.
- Print on bold and italic.
- Print on differents font size.
- Print images.
- Print barcodes, QR, etc.

## Authors

- Cristian Azócar - [CristianAzocar](https://github.com/CristianAzocar)