using RDStation.Entities.Common.Extensions;

namespace RDStation.Entities.Contacts.UpdateContactViaUUIDEmail.Request
{
    public class UpdateContactViaUUIDEmailRequest : BaseContactRequest
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
