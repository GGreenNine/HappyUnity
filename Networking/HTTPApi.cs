using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace HappyUnity.Networking
{
    public static class HTTPApi
    {
        public static string _serverAdress = "http://192.168.1.196:8001/";

        public delegate void AuthorizationStateHandler();

        public static event AuthorizationStateHandler IsAuthorized;
        public static event AuthorizationStateHandler IsLogouted;

        public static HttpClient _httpClient;

        private static void Authorize()
        {
            IsAuthorized();
        }

        public static async Task<T> PostAsync<T>(string requestUri, T item)
        {
            if (requestUri is null) throw new ArgumentNullException(nameof(requestUri));
            if (Equals(item, default(T))) throw new ArgumentNullException(nameof(item));

            string json = JsonConvert.SerializeObject(item);

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage resp = await _httpClient.PostAsync(requestUri, content);

            Debug.Log($"status from POST {resp.StatusCode}");
            resp.EnsureSuccessStatusCode();
            Debug.Log($"added resource at {resp.Headers.Location}");
            json = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(json);
        }

        public static async Task PutAsync<T>(string requestUri, T item)
        {
            if (requestUri is null) throw new ArgumentNullException(nameof(requestUri));
            if (Equals(item, default(T))) throw new ArgumentNullException(nameof(item));

            string json = JsonConvert.SerializeObject(item);

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage resp = await _httpClient.PutAsync(requestUri, content);

            Debug.Log($"status from PUT {resp.StatusCode}");
            resp.EnsureSuccessStatusCode();
        }
        
        public static async Task<D> PatchAsync<T, D>(string requestUri, T content, string fieldName)
        {
            if (requestUri == null) throw new ArgumentNullException(nameof(requestUri));
            string json = await PatchInternalAsync(requestUri, content, fieldName);
            return JsonConvert.DeserializeObject<D>(json);
        }
        
        /// <summary>
        /// Patch internal method
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <param name="fieldName">Field name for new JObject </param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static async Task<string> PatchInternalAsync<T>(string requestUri, T content, string fieldName)
        {
            if (requestUri == null) throw new ArgumentNullException(nameof(requestUri));
            if (_httpClient.BaseAddress == null)
            {
                InitialyzeHTTPClient("//todo");
            }

            string json = JsonConvert.SerializeObject(content);

            JObject message = new JObject();
            message.Add("state",JToken.Parse(json));

            HttpResponseMessage resp = new HttpResponseMessage();
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
            {
                Content = new StringContent(message.ToString(), Encoding.UTF8,
                    "application/json")
            };

            resp = await _httpClient.SendAsync(request);

            return await resp.Content.ReadAsStringAsync();
        }

        public static async Task DeleteAsync(string requestUri)
        {
            if (requestUri is null) throw new ArgumentNullException(nameof(requestUri));

            HttpResponseMessage resp = await _httpClient.DeleteAsync(requestUri);
            Debug.Log($"status from DELETE {resp.StatusCode}");

            resp.EnsureSuccessStatusCode();
        }
        
        private static void InitialyzeHTTPClient(string token)
        {
            _httpClient.BaseAddress = new Uri(_serverAdress);
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Token", token);
        }

        private static async Task<string> GetInternalAsync(string requestUri)
        {
            if (requestUri == null) throw new ArgumentNullException(nameof(requestUri));

            HttpResponseMessage resp = await _httpClient.GetAsync(requestUri);
            Debug.Log($"status from GET {resp.StatusCode}");
            resp.EnsureSuccessStatusCode();

            return await resp.Content.ReadAsStringAsync();
        }

        public static async Task<T> GetAsync<T>(string requestUri)
        {
            if (requestUri == null) throw new ArgumentNullException(nameof(requestUri));
            string json = await GetInternalAsync(requestUri);

            return JsonConvert.DeserializeObject<T>(json);
        }

        public static async Task<IEnumerable<T>> GetAllAsync<T>(string requestUri)
        {
            if (requestUri == null) throw new ArgumentNullException(nameof(requestUri));
            string json = await GetInternalAsync(requestUri);
            return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
        }

        #region NotRecommeded

        private static void DefaultDelete(string api, Action<UnityWebRequest> callback)
        {
            var entry = UnityWebRequest.Delete(_serverAdress + api);
                entry.SetRequestHeader("Authorization", "Token ");
                var sendWebRequest = entry.SendWebRequest();

                while (!entry.isDone)
                    Debug.Log(api + " Waiting");

                sendWebRequest.completed += operation =>
                {
                    if (entry.isNetworkError || entry.isHttpError)
                    {
                        Debug.Log(entry.error.ToString());
                        Debug.Log(entry.downloadHandler.text);
                    }

                    callback(entry);
                    entry.Dispose();
                };
        }

        private static void DefaultPost(string api, string jsonString, Action<UnityWebRequest> Callback)
        {
            UnityWebRequest entry = UnityWebRequest.Put(_serverAdress + api, jsonString);
            entry.method = "POST";
            entry.SetRequestHeader("Authorization", "Token ");
            entry.SetRequestHeader("Content-Type", "application/json");


            var sendWebRequest = entry.SendWebRequest();

            sendWebRequest.completed += delegate(AsyncOperation operation)
            {
                if ((entry.isNetworkError || entry.isHttpError))
                {
                    Debug.LogException(new WebException(entry.error));
                    return;
                }

                Callback(entry);
                entry.Dispose();
            };
        }

        private static void DefaultPatch(string api, string json, Action<UnityWebRequest> callback)
        {
            var entry = UnityWebRequest.Put(_serverAdress + api, json);
                entry.method = "PATCH";
                entry.SetRequestHeader("Authorization", "Token ");
                entry.SetRequestHeader("Content-Type", "application/json");

                var sendWebRequest = entry.SendWebRequest();

                sendWebRequest.completed += operation =>
                {
                    if (entry.isNetworkError || entry.isHttpError)
                    {
                        Debug.Log(entry.error.ToString());
                        Debug.Log(entry.downloadHandler.text);
                    }

                    callback(entry);
                    entry.Dispose();
                };
        }

        #endregion
    }
}