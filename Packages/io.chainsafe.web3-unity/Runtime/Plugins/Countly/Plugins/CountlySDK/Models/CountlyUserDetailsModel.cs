using System.Collections.Generic;
using Newtonsoft.Json;

namespace Plugins.CountlySDK.Models
{
    public class CountlyUserDetailsModel
    {
        [JsonProperty("name")]
        internal string Name { get; set; }
        [JsonProperty("username")]
        internal string Username { get; set; }
        [JsonProperty("email")]
        internal string Email { get; set; }
        [JsonProperty("organization")]
        internal string Organization { get; set; }
        [JsonProperty("phone")]
        internal string Phone { get; set; }

        //Web URL to picture
        //"https://pbs.twimg.com/profile_images/1442562237/012_n_400x400.jpg",
        [JsonProperty("picture")]
        internal string PictureUrl { get; set; }

        [JsonProperty("gender")]
        internal string Gender { get; set; }
        [JsonProperty("byear")]
        internal string BirthYear { get; set; }

        [JsonProperty("custom")]
        //dots (.) and dollar signs ($) in key names will be stripped out.
        internal IDictionary<string, object> Custom { get; set; }

        /// <summary>
        /// Initializes a new instance of User Model with the specified params
        /// </summary>
        /// <param name="name"></param>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="organization"></param>
        /// <param name="phone"></param>
        /// <param name="pictureUrl"></param>
        /// <param name="gender"></param>
        /// <param name="birthYear"></param>
        /// <param name="customData"></param>
        public CountlyUserDetailsModel(string name, string username, string email, string organization, string phone,
                                    string pictureUrl, string gender, string birthYear, IDictionary<string, object> customData)
        {
            Name = name;
            Username = username;
            Email = email;
            Organization = organization;
            Phone = phone;
            PictureUrl = pictureUrl;
            Gender = gender;
            BirthYear = birthYear;
            if (customData != null) {
                Custom = customData as Dictionary<string, object>;
            }
        }

        /// <summary>
        /// This constructor is used to initialize custom user details only.
        /// </summary>
        /// <param name="customData"></param>
        public CountlyUserDetailsModel(IDictionary<string, object> customData)
        {
            Custom = customData as Dictionary<string, object>;
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Username)}: {Username}, {nameof(Email)}: {Email}, {nameof(Organization)}: {Organization}, {nameof(Phone)}: {Phone}, {nameof(PictureUrl)}: {PictureUrl}, {nameof(Gender)}: {Gender}, {nameof(BirthYear)}: {BirthYear}, {nameof(Custom)}: {Custom}";
        }
    }
}
