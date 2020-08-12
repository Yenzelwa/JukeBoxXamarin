
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JukeBox.Models;
using Xamarin.Forms;

namespace JukeBox.BLL.Library
{
    public class Library
    {
        public static async Task<ApiLibraryTypeResponse> GetLibraryTypes()
        {



            using (HttpClient client = new HttpClient())
            {
                // Call asynchronous network methods in a try/catch block to handle exceptions
                try
                {
                    var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
                    HttpResponseMessage response = await client.GetAsync($"{apiSecurity}/api/library/library/type");
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    //  Console.WriteLine(responseBody);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        if (response.StatusCode != HttpStatusCode.Accepted)
                        {
                            //  throw new Exception(response.StatusDescription, new Exception(response.Content));
                        }
                    }
                    if (responseBody.Contains("ResponseObject"))
                    {
                        var data = JsonConvert.DeserializeObject<ApiLibraryTypeResponse>(responseBody);
                        return data;
                    }
                    return null;
                }
                catch (Java.Net.SocketException e)
                {
                    // Console.WriteLine("\nException Caught!");
                    // Console.WriteLine("Message :{0} ", e.Message);

                    return null;
                }
                catch (System.Net.Http.HttpRequestException)
                {
                    return null;
                }

            }
        }
        public static async Task<LibraryResponse> GetLibrary(int filter, int? clientId)
        {




            // Call asynchronous network methods in a try/catch block to handle exceptions
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                    var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
                    HttpResponseMessage response = await client.GetAsync($"{apiSecurity}/api/library/{filter}?clientid={clientId}", HttpCompletionOption.ResponseHeadersRead);

                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        if (response.StatusCode != HttpStatusCode.Accepted)
                        {
                            //  throw new Exception(response.StatusDescription, new Exception(response.Content));
                        }
                    }
                    if (responseBody.Contains("ResponseObject"))
                    {
                        var data = JsonConvert.DeserializeObject<LibraryResponse>(responseBody);
                        return data;
                    }
                    return null;
                }
            }


            catch (Java.Net.SocketException e)
            {

                return null;
            }
            catch (System.Net.Http.HttpRequestException)
            {
                return null;
            }
        
    }
        public static async Task<PromotionTypeResponse> GetPromotionType()
        {




            // Call asynchronous network methods in a try/catch block to handle exceptions
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                    var url = "http://www.apigagasimedia.co.za";
                    HttpResponseMessage response = await client.GetAsync($"{url}/api/promotion/type", HttpCompletionOption.ResponseHeadersRead);

                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        if (response.StatusCode != HttpStatusCode.Accepted)
                        {
                            //  throw new Exception(response.StatusDescription, new Exception(response.Content));
                        }
                    }
                    if (responseBody.Contains("ResponseObject"))
                    {
                        var data = JsonConvert.DeserializeObject<PromotionTypeResponse>(responseBody);
                        return data;
                    }
                    return null;
                }
            }

            catch (Java.Net.UnknownHostException)
            {
                return null;
            }
            catch (Java.Net.SocketException e)
            {

                return null;
            }
            catch (System.Net.Http.HttpRequestException)
            {
                return null;
            }

        }
        public static async Task<PromotionCategoryResponse> GetPromotionCategory(int PromotionTypeId)
        {

            // Call asynchronous network methods in a try/catch block to handle exceptions
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                    var url = "http://www.apigagasimedia.co.za";
                    HttpResponseMessage response = await client.GetAsync($"{url}/api/promotion/category/{PromotionTypeId}", HttpCompletionOption.ResponseHeadersRead);

                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        if (response.StatusCode != HttpStatusCode.Accepted)
                        {
                            //  throw new Exception(response.StatusDescription, new Exception(response.Content));
                        }
                    }
                    if (responseBody.Contains("ResponseObject"))
                    {
                        var data = JsonConvert.DeserializeObject<PromotionCategoryResponse>(responseBody);
                        return data;
                    }
                    return null;
                }
            }
            catch (Java.Net.UnknownHostException)
            {
                return null;
            }

            catch (Java.Net.SocketException e)
            {

                return null;
            }
            catch (System.Net.Http.HttpRequestException)
            {
                return null;
            }

        }

        public static async Task<PromotionResultResponse> GetPromotionResult(int promotionTypeId, int?promotionCategoryId) 
        {




            // Call asynchronous network methods in a try/catch block to handle exceptions
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                    var apiSecurity = "http://www.apigagasimedia.co.za";
                    HttpResponseMessage response = await client.GetAsync($"{apiSecurity}/api/promotion/result?promotionTypeId={promotionTypeId}&promotionCategoryId={promotionCategoryId}", HttpCompletionOption.ResponseHeadersRead);

                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        if (response.StatusCode != HttpStatusCode.Accepted)
                        {
                            //  throw new Exception(response.StatusDescription, new Exception(response.Content));
                        }
                    }
                    if (responseBody.Contains("ResponseObject"))
                    {
                        var data = JsonConvert.DeserializeObject<PromotionResultResponse>(responseBody);
                        return data;
                    }
                    return null;
                }
            }

            catch (Java.Net.UnknownHostException)
            {
                return null;
            }
            catch (Java.Net.SocketException e)
            {

                return null;
            }
            catch (System.Net.Http.HttpRequestException)
            {
                return null;
            }

        }
        public static async Task<ApiResponse> Vote(PromotionResultRequest promotionResultRequest)
        {

            try
            {


                var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
                var request = JsonConvert.SerializeObject(promotionResultRequest);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                // client.DefaultRequestHeaders.Authorization =
                //    new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(apiSecurity);
                var url = string.Format("{0}{1}", "/api/promotion", "/vote");
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ApiResponse>(result);
            }
            catch (Java.Net.SocketException e)
            {
                // Console.WriteLine("\nException Caught!");
                // Console.WriteLine("Message :{0} ", e.Message);

                return null;
            }
            catch (System.Net.Http.HttpRequestException)
            {
                return null;
            }


        }

        public static async Task<LibraryDetailResponse> GetLibraryDetail(long libraryId , int? clientId)
        {



            using (HttpClient client = new HttpClient())
            {
                // Call asynchronous network methods in a try/catch block to handle exceptions
                try
                {
                    var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
                    HttpResponseMessage response = await client.GetAsync($"{apiSecurity}/api/library/detail/{libraryId}?clientid={clientId}");
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                  //  Console.WriteLine(responseBody);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        if (response.StatusCode != HttpStatusCode.Accepted)
                        {
                            //  throw new Exception(response.StatusDescription, new Exception(response.Content));
                        }
                    }
                    if (responseBody.Contains("ResponseObject"))
                    {
                        var data = JsonConvert.DeserializeObject<LibraryDetailResponse>(responseBody);
                        return data;
                    }
                    return null;
                }
                catch (Java.Net.SocketException e)
                {
                   // Console.WriteLine("\nException Caught!");
                  // Console.WriteLine("Message :{0} ", e.Message);

                    return null;
                }
                catch (System.Net.Http.HttpRequestException)
                {
                    return null;
                }

            }
        }
    }
}
