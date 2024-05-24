using RDStation.Interfaces;

namespace RDStation.Entities.Contacts.QueryEventsAssociatedContact.Response
{
    public class QueryEventsAssociatedContactResponse : BaseResponse, IResponse
    {

        public Events[] events { get; set; }
        
        public class Events
        {
            /// <summary>
            /// The type of event (CONVERSION or OPPORTUNITY)
            /// </summary>
            public string event_type { get; set; }

            /// <summary>
            /// Internal identifier of the RDSM
            /// </summary>
            public string event_family { get; set; }

            /// <summary>
            /// Event Identifier
            /// </summary>
            public string event_identifier { get; set; }

            /// <summary>
            /// Date/time the event was created, using the UTC format
            /// </summary>
            public DateTime event_timestamp { get; set; }

            /// <summary>
            /// Contact details
            /// </summary>
            public Payload payload { get; set; }
        }

        public class Payload
        {
            /// <summary>
            /// Contact name
            /// </summary>
            public string name { get; set; }

            /// <summary>
            /// Contact email
            /// </summary>
            public string email { get; set; }
        }

    }
}
