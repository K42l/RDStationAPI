using RDStation.Entities.Common.Extensions;
using RDStation.Interfaces;

namespace RDStation.Entities.Contacts.QueryEventsAssociatedContact.Request
{
    public class QueryEventsAssociatedContactRequest : BaseRequest, IRequest
    {
        public override string BaseUrl => "platform/contacts/{uuid}/events";

        /// <summary>
        /// The unique uuid associated with each RD Station contact.
        /// </summary>
        public string uuid { get; set; }

        /// <summary>
        /// The type of event you want to consult (CONVERSION or OPPORTUNITY)
        /// </summary>
        public string event_type { get; set; }

        public override IList<KeyValuePair<string, string>> GetQueryStringWithoutParameters()
        {
            var parameters = base.GetQueryStringWithoutParameters();
            parameters.Add("uuid", this.uuid);
            return parameters;
        }

        public override IList<KeyValuePair<string, string>> GetQueryStringParameters()
        {
            var parameters = base.GetQueryStringWithoutParameters();
            if (!string.IsNullOrEmpty(this.event_type.ToString()))
                parameters.Add("event_type", this.event_type.ToString());
            
            return parameters;
        }
    }
}
