namespace RDStation.Interfaces
{
    public interface IRequest
    {
        /// <summary>
        /// Returns the Uri for the request.
        /// </summary>
        /// <returns>The <see cref="Uri"/>.</returns>
        Uri GetUri();

        /// <summary>
        /// Get the query string collection of aggregated from all parameters added to the request.
        /// </summary>
        /// <returns>The <see cref="IList{KeyValuePair}"/> collection.</returns>
        IList<KeyValuePair<string, string>> GetQueryStringParameters();

        /// <summary>
        /// Get the query string collection of aggregated from all parameters added to the request.
        /// </summary>
        /// <returns>The <see cref="IList{KeyValuePair}"/> collection.</returns>
        IList<KeyValuePair<string, string>> GetQueryStringWithoutParameters();

        object DataJsonPost { get; set; }
    }
}
