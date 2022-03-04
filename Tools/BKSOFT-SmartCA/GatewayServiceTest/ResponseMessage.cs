using System;
using System.Collections.Generic;

namespace GatewayServiceTest
{
    /// <summary>
    /// access_token request parameter mapping
    /// </summary>
    class GetTokenBody
    {
        public string grant_type { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
    }

    /// <summary>
    /// access_token response mapping
    /// </summary>
    class GetTokenResponse
    {
        // access_token value
        public string access_token { get; set; }
        // refresh_token to get new access_token (see RefreshToken method)
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        // access_token valid time. when expired, using refresh_token to get new or require user re-authorize
        public int expires_in { get; set; }
    }

    /// <summary>
    /// Mapped with service response message
    /// </summary>
    public class ResponseMessage
    {
        public Guid ResponseID { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseContent { get; set; }

        // See reponse to custome this last property
        public object Content { get; set; }
    }

    /// <summary>
    /// Mapped with Content property in response message
    /// </summary>
    public class Account
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // -----------------------------------------
        // ...More properties 
        // See json response to add more
        // -----------------------------------------

        // Complex property
        public List<Group> Groups { get; set; }
    }

    /// <summary>
    /// If 'Content' property contain complex property, create new mapped class like this
    /// </summary>
    public class Group
    {
        public string ID { get; set; }
        public string AdminEmail { get; set; }
        public string TenNhom { get; set; }

        // -----------------------------------------
        // ...More properties 
        // See json response to add more
        // -----------------------------------------
    }
}
