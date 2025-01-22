//-----------------------------------------------------------------------
// <copyright file="AzureFunctionsAssertionFailedException.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Azure.Functions
{
    /// <summary>
    /// Occurs when an Azure Functions assertions has been failed.
    /// </summary>
    public class AzureFunctionsAssertionFailedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureFunctionsAssertionFailedException"/> class.
        /// </summary>
        public AzureFunctionsAssertionFailedException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureFunctionsAssertionFailedException"/> class
        /// with the specified <paramref name="message"/>.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        public AzureFunctionsAssertionFailedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureFunctionsAssertionFailedException"/> class
        /// with the specified <paramref name="message"/> and the <paramref name="innerException"/>.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        /// <param name="innerException">Inner exception related to the <see cref="AzureFunctionsAssertionFailedException"/> to create.</param>
        public AzureFunctionsAssertionFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
