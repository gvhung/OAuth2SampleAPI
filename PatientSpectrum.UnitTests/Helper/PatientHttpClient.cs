using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System.Text;
using IdentityModel;
using System.Security.Cryptography.X509Certificates;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using PatientJourney.GlobalConstants;
using System.Web;
using System.Web.Http;

namespace PatientSpectrum.UnitTests.Helper
{
    public class PatientHttpClient
    {
        public string AccessToken { get; set; }
        public string TokenLifetime { get; set; }
        public string TokenIssuedAt { get; set; }
        public string RefreshToken { get; set; }

        private HttpClient _client;
        public PatientHttpClient(string uri, string scope, string user_5_1_1_id, string another_user_param)
        {
            HttpClient client = new HttpClient();
            var accessToken = string.Empty;

            client.BaseAddress = new Uri(uri);

            if(String.IsNullOrEmpty(this.AccessToken))
            {
                //get new token
                // Resource owner credentials flow
                this.AccessToken = RequestAccessTokenROCFFlow(scope, user_5_1_1_id, another_user_param);
            }
            
            client.SetBearerToken(this.AccessToken);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            _client = client;
        }

        public HttpClient GetClient()
        {
            return _client;
        }

        private string GetAccessTokenFromRefreshToken(string refreshToken)
        {
            var tokenClient = new TokenClient(
              OAuthPJConstants.OAuthServerTokenEndPoint,
              "refreshclient",
               OAuthPJConstants.ROFCClientSecret);

            var response = tokenClient.RequestRefreshTokenAsync(refreshToken).Result;

            PersistTokens(response);

            return response.AccessToken;
        }
        
        private string RequestAccessTokenROCFFlow(string scope, string user_5_1_1_id, string another_user_param)
        {
            if (!String.IsNullOrEmpty(this.AccessToken) && !String.IsNullOrEmpty(this.TokenLifetime) && !String.IsNullOrEmpty(this.TokenIssuedAt))
            {
                //check if the token has expired

                var issuedAt = DateTime.Parse(this.TokenIssuedAt);
                var expiresInSeconds = long.Parse(this.TokenLifetime);
                var expiryTimeStamp = issuedAt.AddSeconds(expiresInSeconds).AddSeconds(-10);

                var isMatchOrExpired = DateTime.Compare(DateTime.UtcNow, expiryTimeStamp);

                if (isMatchOrExpired == -1 || isMatchOrExpired == 0)
                {
                    //if token is valid return token
                    return this.AccessToken;
                }
                else
                {
                    if (this.RefreshToken != string.Empty)
                    {
                        return GetAccessTokenFromRefreshToken(this.RefreshToken);
                    }
                }
            }

            var tokenClient = new TokenClient(
               OAuthPJConstants.OAuthServerTokenEndPoint,
               "refreshclient",
                OAuthPJConstants.ROFCClientSecret);
            
            var tokenResponse = tokenClient.RequestResourceOwnerPasswordAsync(user_5_1_1_id, another_user_param, scope).Result;

            PersistTokens(tokenResponse);

            return tokenResponse.AccessToken;
        }
        
        private void StoreAccessToken(TokenResponse tokenResponse)
        {
            if (tokenResponse.AccessToken != null)
            {
                this.AccessToken = tokenResponse.AccessToken;
                this.TokenIssuedAt = DateTime.UtcNow.ToString();
                this.TokenLifetime = tokenResponse.ExpiresIn.ToString();
            }
            else
            {
                throw new Exception(tokenResponse.Error);
            }
        }

        private void StoreRefreshToken(TokenResponse response)
        {
            this.RefreshToken = response.RefreshToken;
        }

        private void PersistTokens(TokenResponse respone)
        {
            StoreAccessToken(respone);

            StoreRefreshToken(respone);
        }

    }
}
