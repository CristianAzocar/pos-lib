﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosLib.Printing.NPI
{
    /// <summary>
    /// The exception that is thrown when a status error occurs.
    /// </summary>
    public sealed class NiiStatusException : PrinterException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NiiStatusException"/> class with a specified error code.
        /// </summary>
        /// <param name="errorCode">The error code that is associated with this exception.</param>
        public NiiStatusException(int errorCode) : base(errorCode, GetErrorDescription(errorCode))
        {
        }

        /// <summary>
        /// Returns a description for a given error code.
        /// </summary>
        /// <param name="errorCode">The error code of the exception.</param>
        /// <returns>A string that contains the description.</returns>
        private static string GetErrorDescription(int errorCode)
        {
            switch (errorCode)
            {
                case -3:
                    return "Printer open error";
                case -5:
                    return "Offline";
                case -6:
                    return "Invalid argument error";
                case -9:
                    return "Printer information acquisition failure";
                case -11:
                    return "Status information acquisition failure";
                case -12:
                    return "Status information open error";
                case -31:
                    return "Resource shortage";
                case -102:
                    return "Socket error";
                case -110:
                    return "Host unknown";
                case -105:
                    return "Connection error";
                case -106:
                    return "Transmission error";
                case -107:
                    return "Transmission error (Timeout)";
                case -108:
                    return "Receipt error";
                case -109:
                    return "Receipt error (Timeout)";
                case -111:
                    return "Communication error";
                case 1:
                    return "Status information open error (recovered)";
                default:
                    return null;
            }
        }
    }
}
