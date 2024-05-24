using RDStation.Interfaces;

namespace RDStation.Entities.Contacts.QueryContactsByUUIDEmail.Response
{
    public class QueryContactsByUUIDEmailResponse : BaseContactResponse
    {
        /// <summary>
        /// Unique contact UUID generated in RD Station for control and management
        /// </summary>
        public string uuid { get; set; }

        /// <summary>
        /// Contact name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Contact email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// Contact job position
        /// </summary>
        public string job_title { get; set; }

        /// <summary>
        /// Birthday Date
        /// </summary>
        public string birthdate { get; set; }

        /// <summary>
        /// Notes about the contact
        /// </summary>
        public string bio { get; set; }

        /// <summary>
        /// Contact website
        /// </summary>
        public string website { get; set; }

        /// <summary>
        /// Contact phone number
        /// </summary>
        public string personal_phone { get; set; }

        /// <summary>
        /// Contact phone number
        /// </summary>
        public string mobile_phone { get; set; }

        /// <summary>
        /// Contact city
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// Contact state
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// Contact country
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// Contact Twitter
        /// </summary>
        public string twitter { get; set; }

        /// <summary>
        /// Contact Facebook
        /// </summary>
        public string facebook { get; set; }

        /// <summary>
        /// Contact LinkedIn
        /// </summary>
        public string linkedin { get; set; }

        /// <summary>
        /// Contact tags
        /// </summary>
        public string[] tags { get; set; }

        /// <summary>
        /// Extra emails from the contact
        /// </summary>
        public string[] extra_emails { get; set; }

        /// <summary>
        /// Legal basis for communication with the contact
        /// </summary>
        public Legal_Bases[] legal_bases { get; set; }

        /// <summary>
        /// List of hyperlinks
        /// </summary>
        public Link[] links { get; set; }

        public class Legal_Bases
        {
            public string category { get; set; }
            public string type { get; set; }
            public string status { get; set; }
        }

        public class Link
        {
            public string rel { get; set; }
            public string href { get; set; }
            public string media { get; set; }
            public string type { get; set; }
        }
    }
}
