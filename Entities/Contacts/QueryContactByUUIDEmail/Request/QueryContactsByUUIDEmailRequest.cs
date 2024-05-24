using RDStation.Entities.Common.Extensions;
using RDStation.Interfaces;

namespace RDStation.Entities.Contacts.QueryContactsByUUIDEmail.Request
{
    public class QueryContactsByUUIDEmailRequest : BaseRequest, IRequest
    {
        public override string BaseUrl => "platform/contacts/{identifier}:{value}";

        /// <summary>
        /// Identifier (uuid/email) associated with each RD Station contact.
        /// </summary>
        public string identifier { get; set; }

        /// <summary>
        /// Identifier value.
        /// </summary>
        public string value { get; set; }

        public override IList<KeyValuePair<string, string>> GetQueryStringWithoutParameters()
        {
            var parameters = base.GetQueryStringWithoutParameters();
            parameters.Add("identifier", this.identifier);
            parameters.Add("value", this.value);
            return parameters;
        }
    }
}
