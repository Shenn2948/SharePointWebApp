using System;
using System.Configuration;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SP.DataAccess.Utils;

namespace SP.DataAccess
{
    public class SharePointRestClient : IDisposable
    {
        private readonly WebClient _webClient;

        public SharePointRestClient(Uri uri)
        {
            _webClient = new WebClient { Credentials = GetCredentials() };
            _webClient.Headers.Add(SharePointWebHeaderCreator.GetAuthHeaders());

            Uri = uri;
        }

        public Uri Uri { get; }

        public async Task<JToken> GetListItemsAsync(string listName)
        {
            Uri endPointUri = GetListItemsEndPoint(listName);
            string result = await _webClient.DownloadStringTaskAsync(endPointUri);
            JToken token = JToken.Parse(result);

            return token["d"]?["results"];
        }

        public async Task<JToken> GetListItemAsync(string listName, string listItemId)
        {
            Uri endPointUri = GetSpecificListItemEndPoint(listName, listItemId);
            string result = await _webClient.DownloadStringTaskAsync(endPointUri);
            JToken token = JToken.Parse(result);

            return token["d"];
        }

        public async Task InsertListItemAsync(string listName, object item)
        {
            string formDigestValue = await GetFormDigestValueAsync();
            _webClient.Headers.Add(SharePointWebHeaderCreator.GetInsertHeaders(formDigestValue));

            Uri endPointUri = GetListItemsEndPoint(listName);
            string serializedObject = JsonConvert.SerializeObject(item);

            await _webClient.UploadStringTaskAsync(endPointUri, "POST", serializedObject);
        }

        public async Task UpdateListItemAsync(string listName, string listItemId, object updatedItem)
        {
            string formDigestValue = await GetFormDigestValueAsync();
            _webClient.Headers.Add(SharePointWebHeaderCreator.GetUpdateHeaders(formDigestValue));

            Uri endPointUri = GetSpecificListItemEndPoint(listName, listItemId);
            string serializedObject = JsonConvert.SerializeObject(updatedItem);

            await _webClient.UploadStringTaskAsync(endPointUri, "POST", serializedObject);
        }

        public async Task DeleteListItemAsync(string listName, string listItemId)
        {
            string formDigestValue = await GetFormDigestValueAsync();
            _webClient.Headers.Add(SharePointWebHeaderCreator.GetDeleteHeaders(formDigestValue));

            Uri endPointUri = GetSpecificListItemEndPoint(listName, listItemId);
            await _webClient.UploadStringTaskAsync(endPointUri, "POST", string.Empty);
        }

        public void Dispose()
        {
            _webClient?.Dispose();
        }

        private async Task<string> GetFormDigestValueAsync()
        {
            Uri uri = new Uri(Uri, "_api/contextinfo");
            string result = await _webClient.UploadStringTaskAsync(uri, "POST");
            JToken token = JToken.Parse(result);
            return token["d"]?["GetContextWebInformation"]?["FormDigestValue"]?.ToString();
        }

        private static SharePointOnlineCredentials GetCredentials()
        {
            string userName = ConfigurationManager.AppSettings["SPAccountUserName"];
            string password = ConfigurationManager.AppSettings["SPAccountPassword"];
            SecureString securePassword = new SecureString();
            foreach (char c in password)
            {
                securePassword.AppendChar(c);
            }

            return new SharePointOnlineCredentials(userName, securePassword);
        }

        private Uri GetListItemsEndPoint(string listName)
        {
            return new Uri(Uri, $"_api/web/lists/getByTitle('{listName}')/items");
        }

        private Uri GetSpecificListItemEndPoint(string listName, string listItemId)
        {
            return new Uri(Uri, $"_api/web/lists/getByTitle('{listName}')/items('{listItemId}')");
        }
    }
}