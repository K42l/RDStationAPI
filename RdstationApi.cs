using RDStation.Entities.Authentication.ObtainTokens.Request;
using RDStation.Entities.Authentication.ObtainTokens.Response;
using RDStation.Entities.Authentication.RenewAccessToken.Request;
using RDStation.Entities.Authentication.RenewAccessToken.Response;
using RDStation.Entities.Contacts.CreateContact.Request;
using RDStation.Entities.Contacts.CreateContact.Response;
using RDStation.Entities.Contacts.CreateTagForLeadViaUUIDEmail.Request;
using RDStation.Entities.Contacts.CreateTagForLeadViaUUIDEmail.Response;
using RDStation.Entities.Contacts.DeleteContactByUUIDEmail.Request;
using RDStation.Entities.Contacts.DeleteContactByUUIDEmail.Response;
using RDStation.Entities.Contacts.QueryContactsByUUIDEmail.Request;
using RDStation.Entities.Contacts.QueryContactsByUUIDEmail.Response;
using RDStation.Entities.Contacts.QueryEventsAssociatedContact.Request;
using RDStation.Entities.Contacts.QueryEventsAssociatedContact.Response;
using RDStation.Entities.Contacts.UpdateContactViaUUIDEmail.Request;
using RDStation.Entities.Contacts.UpdateContactViaUUIDEmail.Response;
using RDStation;

namespace RDStationAPI
{
    public class RdstationApi
    {
        /// <summary>
        /// Get access_token and refresh_roken
        /// </summary>
        public static HttpEngine<ObtainTokensRequest, ObtainTokensResponse> ObtainTokensRequest => HttpEngine<ObtainTokensRequest, ObtainTokensResponse>.instance;

        /// <summary>
        /// After generating the tokens, you must use refresh_token to update the access_token At every 24 hours; upon receipt of access_token.
        /// </summary>
        public static HttpEngine<RenewAccessTokenRequest, RenewAccessTokenResponse> RenewAccessToken => HttpEngine<RenewAccessTokenRequest, RenewAccessTokenResponse>.instance;

        /// <summary>
        /// It creates a new contact in the Lead Base. If the contact already exists, the following error will occur: EMAIL_ALREADY_IN_USE.
        /// There is no record of a Conversion Event in the contact history.
        /// The tags sent in the request are added to the contact.
        /// </summary>
        public static HttpEngine<CreateContactRequest, CreateContactResponse> CreateContact => HttpEngine<CreateContactRequest, CreateContactResponse>.instance;

        /// <summary>
        /// It only updates an existing contact in the Lead Base. If the contact doesn't exist, the following error will occur: RESOURCE_NOT_FOUND.
        /// There is no record of a Conversion Event in the lead history.
        /// The tags submitted are assembled in the Lead Profile.That is, they are added in addition to the existing tags.
        /// </summary>
        public static HttpEngine<CreateTagForLeadViaUUIDEmailRequest, CreateTagForLeadViaUUIDEmailResponse> CreateTagForLeadViaUUIDEmail => HttpEngine<CreateTagForLeadViaUUIDEmailRequest, CreateTagForLeadViaUUIDEmailResponse>.instance;

        /// <summary>
        /// Delete a specific contact.
        /// </summary>
        public static HttpEngine<DeleteContactByUUIDEmailRequest, DeleteContactByUUIDEmailResponse> DeleteContactByUUIDEmail => HttpEngine<DeleteContactByUUIDEmailRequest, DeleteContactByUUIDEmailResponse>.instance;

        /// <summary>
        /// Returns data for a specific contact.
        /// </summary>
        public static HttpEngine<QueryContactsByUUIDEmailRequest, QueryContactsByUUIDEmailResponse> QueryContactsByUUIDEmailR => HttpEngine<QueryContactsByUUIDEmailRequest, QueryContactsByUUIDEmailResponse>.instance;

        /// <summary>
        /// Returns a list of events associated with the contact.
        /// </summary>
        public static HttpEngine<QueryEventsAssociatedContactRequest, QueryEventsAssociatedContactResponse> QueryEventsAssociatedContact => HttpEngine<QueryEventsAssociatedContactRequest, QueryEventsAssociatedContactResponse>.instance;

        /// <summary>
        /// Creates or updates an existing contact in the Lead Base.
        /// There is no record of a Conversion Event in the contact history.
        /// If the contact already exists, the tags inserted in the contact are replaced by the new tags sent via the request.
        /// </summary>
        public static HttpEngine<UpdateContactViaUUIDEmailRequest, UpdateContactViaUUIDEmailResponse> UpdateContactViaUUIDEmail => HttpEngine<UpdateContactViaUUIDEmailRequest, UpdateContactViaUUIDEmailResponse>.instance;
    }
}
