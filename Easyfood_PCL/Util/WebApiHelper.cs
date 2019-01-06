using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Easyfood_Xamarin.Util
{
    public class WebApiHelper
    {
        public HttpClient Client { get; set; }
        public string Route { get; set; }

        public static String DEFAULT_API_URI = "http://192.168.1.8:58299";
        public static String DEFAULT_API_PREFIX = "api";

        #region Constructors
        public WebApiHelper()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(DEFAULT_API_URI);

            this.Route = DEFAULT_API_PREFIX;
        }

        public WebApiHelper(string route)
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(DEFAULT_API_URI);

            this.Route = DEFAULT_API_PREFIX + "/" + route;
        }
        public WebApiHelper(string uri, string route)
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(uri);

            this.Route = DEFAULT_API_PREFIX + "/" + route;
        }
        #endregion

        #region PostMethods
        //// api/Endpoint
        public async Task<HttpResponseMessage> PostResponse(object obj)
        {
            var jsonObject = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            return await Client.PostAsync(Route, jsonObject);
        }
        // api/Endpoint/{param}
        public async Task<HttpResponseMessage> PostResponse(string param, object obj)
        {
            var jsonObject = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            HttpResponseMessage result = await Client.PostAsync(Route + "/" + param + "/", jsonObject);
            return result;
        }
        #endregion

        #region PutMethods
        // api/Endpoint/{id}/
        // ili api/Endpoint/{id}/segment1/segment2/.../segmentN/
        public HttpResponseMessage PutResponse(int id, Object existingObj, List<string> dodatniUrlSegmenti = null)
        {
            string endRoute = Route + "/" + id + "/";
            if (dodatniUrlSegmenti != null && dodatniUrlSegmenti.Count() > 0)
            {
                foreach (string segment in dodatniUrlSegmenti)
                {
                    endRoute += segment + "/";
                }
            }
            var jsonObject = new StringContent(JsonConvert.SerializeObject(existingObj), Encoding.UTF8, "application/json");
            return Client.PostAsync(endRoute, jsonObject).Result;
        }
        public async Task<HttpResponseMessage> PutResponse(string param, Object existingObj)
        {
            var jsonObject = new StringContent(JsonConvert.SerializeObject(existingObj), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Client.PutAsync(Route + "/" + param + "/", jsonObject);
            return response;
        }
        #endregion

        #region GetMethods
        // api/Endpoint
        public async Task<HttpResponseMessage> GetResponse()
        {
            HttpResponseMessage result = await Client.GetAsync(Route);
            return result;
        }

        // api/Endpoint/{param}
        public async Task<HttpResponseMessage> GetResponse(string param)
        {
            HttpResponseMessage result = await Client.GetAsync(Route + "/" + param + "/");
            return result;
        }

        // api/Endoint/?param1=value1&param2=value2....paramN=valueN
        public async Task<HttpResponseMessage> GetResponse<T>(Dictionary<string, T> queryParams)
        {
            //redirektaj na GetResponse ako je Dict. kojim slučajem prazan
            if (queryParams.Count == 0)
            {
                return await GetResponse();
            }

            //build query
            string query = "?";
            foreach (KeyValuePair<string, T> item in queryParams)
            {
                if (!item.Equals(queryParams.First()))
                    query += "&";

                query += item.Key + "=" + item.Value.ToString();
            }

            //execute
            HttpResponseMessage result = await Client.GetAsync(Route + query);
            return result;
        }
        #endregion

        #region DeleteMethods
        // api/Endpoint/{id}
        public async Task<HttpResponseMessage> DeleteResponse(int resourceId)
        {
            HttpResponseMessage result = await Client.DeleteAsync(Route + "/" + resourceId + "/");
            return result;
        }
        // api/Endpoint/Param/{id}
        public async Task<HttpResponseMessage> DeleteResponse(string param, int resourceId)
        {
            HttpResponseMessage result = await Client.DeleteAsync(Route + "/" + param + "/" + resourceId + "/");
            return result;
        }
        #endregion
    }
}
