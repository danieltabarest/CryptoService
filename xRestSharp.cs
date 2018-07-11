using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace NSU.Utilities.Extensions
{
    /// <summary>
    /// Rest Client Helper Class
    /// </summary>
    public static class RestClientHelper
    {

        /// <summary>
        /// Restful http request wrapper using basic authorization
        /// </summary>
        /// <typeparam name="T">Generic object response</typeparam>
        /// <param name="verb">Enumerator for Http verb (Method.GET, Method.POST)</param>
        /// <param name="parameters">DIctionary with all the Query string parameters</param>
        /// <param name="resource">Resource url</param>
        /// <param name="user">Basic authentication user</param>
        /// <param name="password">Basic authentication password</param>
        /// <param name="throwException">Enabled the API client to throw exceptions. by default is true</param>
        /// <param name="body">The body.</param>
        /// <returns>
        /// Data retreived by the resquest
        /// </returns>
        public static T DoCall<T>(Method verb, Dictionary<string, string> parameters, 
            string resource, string user, string password, bool throwException = true, object body = null) where T : new()
        {
            var client = new RestClient(resource);

            var request = new RestRequest(verb);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    request.AddQueryParameter(param.Key, param.Value);
                }
            }

            if (body != null)
            {
                request.RequestFormat = DataFormat.Json;
                request.AddBody(body);
            }

            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password))
            {
                var credentials = Encoding.ASCII.GetBytes(user + ":" + password);
                var basicToken = "Basic " + Convert.ToBase64String(credentials);
                request.AddHeader("authorization", basicToken);
            }

            var response = client.Execute(request);

            if (response.ErrorException != null)
            {
                var message = "Error retrieving response: (" + response.ErrorMessage + "). Check inner details for more info.";
                var exception = new ApplicationException(message, response.ErrorException);
                throw exception;
            }

            var serializedObject = default(T);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    serializedObject = JsonConvert.DeserializeObject<T>(response.Content);
                }
                catch (Exception e)
                {
                    const string message = "Error deserializing response.  Check inner details for more info.";
                    var exception = new ApplicationException(message, e);
                    throw exception;
                }
            }
            else
            {
                if (!throwException) return serializedObject;
                var message = "(" + response.StatusCode + ") " + response.StatusDescription + " was raised by the submitted request!!!    Content: " + response.Content;
                var exception = new ApplicationException(message);
                throw exception;
            }

            return serializedObject;
        }


        /// <summary>
        /// Restful http request wrapper
        /// </summary>
        /// <typeparam name="T">Generic object response</typeparam>
        /// <param name="verb">Enumerator for Http verb (Method.GET, Method.POST)</param>
        /// <param name="parameters">DIctionary with all the Query string parameters</param>
        /// <param name="resource">Resource url</param>
        /// <param name="token">Authorization token</param>
        /// <param name="throwException">Enabled the API client to throw exceptions. by default is true</param>
        /// <param name="body">The body.</param>
        /// <returns>
        /// Data retreived by the resquest
        /// </returns>
        public static T DoCall<T>(Method verb, Dictionary<string, string> parameters, 
            string resource, string token = "", bool throwException = true, object body = null) where T : new()
        {
            var client = new RestClient(resource);

            var request = new RestRequest(verb);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    request.AddQueryParameter(param.Key, param.Value);
                }
            }

            if (body != null)
            {
                request.RequestFormat = DataFormat.Json;
                request.AddBody(body);
            }

            if (!string.IsNullOrEmpty(token))
            {
                request.AddHeader("authorization", token);
            }

            var response = client.Execute(request);

            if (response.ErrorException != null)
            {
                var message = "Error retrieving response: (" + response.ErrorMessage + "). Check inner details for more info.";
                var exception = new ApplicationException(message, response.ErrorException);
                throw exception;
            }

            var serializedObject = default(T);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    serializedObject = JsonConvert.DeserializeObject<T>(response.Content);
                }
                catch (Exception e)
                {
                    const string message = "Error deserializing response.  Check inner details for more info.";
                    var exception = new ApplicationException(message, e);
                    throw exception;
                }
            }
            else
            {
                if (!throwException) return serializedObject;
                var message = "(" + response.StatusCode + ") " + response.StatusDescription + " was raised by the submitted request!!!    Content: " + response.Content;
                var exception = new ApplicationException(message);
                throw exception;
            }

            return serializedObject;
        }



        /// /// <summary>
        /// Restful Async http request wrapper using basic authorization
        /// </summary>
        /// <typeparam name="T">Generic object response</typeparam>
        /// <param name="verb">Enumerator for Http verb (Method.GET, Method.POST)</param>
        /// <param name="parameters">DIctionary with all the Query string parameters</param>
        /// <param name="resource">Resource url</param>
        /// <param name="token">Authorization token</param>
        /// <param name="throwException">Enabled the API client to throw exceptions. by default is true </param>
        /// <param name="body">Body params for POST request</param>
        /// <returns> Task with the data retreived by the resquest</returns>
        public static Task<T> DoCallAsync<T>(Method verb, Dictionary<string, string> parameters, 
            string resource, string token = "", bool throwException = true, object body = null,string JsonBody = null) where T : new()
        {
            var tsc = new TaskCompletionSource<T>();

            var client = new RestClient(resource);

            var request = new RestRequest(verb);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    request.AddQueryParameter(param.Key, param.Value);
                }
            }

            if (body!=null)
            {
                request.RequestFormat = DataFormat.Json;
                request.AddBody(body);
            }
            if (JsonBody != null)
            {
                request.AddParameter("application/json", JsonBody, ParameterType.RequestBody);
            }
            

            if (!string.IsNullOrEmpty(token))
            {
                request.AddHeader("authorization", token);
            }

            var response = client.Execute(request);

            if (response.ErrorException != null)
            {
                var message = "Error retrieving response: (" + response.ErrorMessage + "). Check inner details for more info.";
                var exception = new ApplicationException(message, response.ErrorException);
                throw exception;
            }

            var serializedObject =  default(T);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    serializedObject = JsonConvert.DeserializeObject<T>(response.Content);
                }
                catch (Exception e)
                {
                    const string message = "Error deserializing response.  Check inner details for more info.";
                    var exception = new ApplicationException(message, e);
                    throw exception;
                }
            }
            else
            {
                if (throwException)
                {
                    var message = "(" + response.StatusCode + ") " + response.StatusDescription + " was raised by the submitted request!!! error:" + response.ErrorMessage +" "+ response.ErrorException+"   Content: " + response.Content;
                    var exception = new ApplicationException(message);
                    throw exception;
                }
            }

            tsc.TrySetResult(serializedObject);

            return tsc.Task;
        }


        /// <summary>
        /// Restful Async http request wrapper
        /// </summary>
        /// <typeparam name="T">Generic object response</typeparam>
        /// <param name="verb">Enumerator for Http verb (Method.GET, Method.POST)</param>
        /// <param name="parameters">DIctionary with all the Query string parameters</param>
        /// <param name="resource">Resource url</param>
        /// <param name="user">Basic authentication user</param>
        /// <param name="password">Basic authentication password</param>
        /// <param name="throwException">Enabled the API client to throw exceptions. by default is true </param>
        /// <param name="body">Body params for POST request</param>
        /// <returns>Task with the data retreived by the resquest</returns>
        public static Task<T> DoCallAsync<T>(Method verb, Dictionary<string, string> parameters,
            string resource, string user, string password, bool throwException = true, object body = null) where T : new()
        {
            var tsc = new TaskCompletionSource<T>();

            var client = new RestClient(resource);

            var request = new RestRequest(verb);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    request.AddQueryParameter(param.Key, param.Value);
                }
            }

            if (body != null)
            {
                request.RequestFormat = DataFormat.Json;
                request.AddBody(body);
            }
            

            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password))
            {
                var credentials = Encoding.ASCII.GetBytes(user + ":" + password);
                var basicToken = "Basic " + Convert.ToBase64String(credentials);
                request.AddHeader("authorization", basicToken);
            }

            var response = client.Execute(request);

            if (response.ErrorException != null)
            {
                var message = "Error retrieving response: (" + response.ErrorMessage + "). Check inner details for more info.";
                var exception = new ApplicationException(message, response.ErrorException);
                throw exception;
            }

            var serializedObject = default(T);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    serializedObject = JsonConvert.DeserializeObject<T>(response.Content);
                }
                catch (Exception e)
                {
                    const string message = "Error deserializing response.  Check inner details for more info.";
                    var exception = new ApplicationException(message, e);
                    throw exception;
                }
            }
            else
            {
                if (throwException)
                {
                    var message = "(" + response.StatusCode + ") " + response.StatusDescription + " was raised by the submitted request!!!    Content: " + response.Content;
                    var exception = new ApplicationException(message);
                    throw exception;
                }
                
            }

            tsc.TrySetResult(serializedObject);

            return tsc.Task;
        }



    }
}
