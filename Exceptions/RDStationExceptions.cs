namespace RDStation.Exceptions
{
    /// <summary>
    /// Exception that is thrown when RDStation returns a status code 
    /// </summary>
    public class RDStationExceptions : Exception
    {
        /// <summary>
        /// Constructor, accepting a error message and a optional status.
        /// </summary>
        /// <param name="message">The error message.</param>
        public RDStationExceptions(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor, accepting a error message and a optional status.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public RDStationExceptions(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
