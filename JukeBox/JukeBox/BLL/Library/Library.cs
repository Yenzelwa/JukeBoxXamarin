
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

                    var data = JsonConvert.DeserializeObject<ApiLibraryTypeResponse>(responseBody);
                    return data;
                }
                catch (HttpRequestException e)
                {
                    // Console.WriteLine("\nException Caught!");
                    // Console.WriteLine("Message :{0} ", e.Message);

                    return new ApiLibraryTypeResponse();
                }

            }
        }
        public static async Task<LibraryResponse> GetLibrary(int filter , int? clientId)
        {
         


            using (HttpClient client = new HttpClient())
            {
                // Call asynchronous network methods in a try/catch block to handle exceptions
                try
                {
                    client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                    var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
                    HttpResponseMessage response = await client.GetAsync($"{apiSecurity}/api/library/{filter}?clientid={clientId}", HttpCompletionOption.ResponseHeadersRead);
                    
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Above three lines can be replaced with new helper method below
                    // string responseBody = await client.GetStringAsync(uri);

                   // Console.WriteLine(responseBody);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        if (response.StatusCode != HttpStatusCode.Accepted)
                        {
                            //  throw new Exception(response.StatusDescription, new Exception(response.Content));
                        }
                    }

                    var data = JsonConvert.DeserializeObject<LibraryResponse>(responseBody);
                    return data;
                }
                catch (HttpRequestException e)
                {
                 //   Console.WriteLine("\nException Caught!");
                  //  Console.WriteLine("Message :{0} ", e.Message);

                    return new LibraryResponse();
                }

                // var client = new HttpClient();
                // client.BaseAddress = new Uri("http://localhost:58806");

                //string jsonData = @"{""username"" : ""myusername"", ""password"" : ""mypassword""}"

                // var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                // HttpResponseMessage response = await client.GetAsync("/api/library");

                // this result string should be something like: "{"token":"rgh2ghgdsfds"}"
                // var result = await response.Content.ReadAsStringAsync();
                //var client = new RestClient(orionApiUrl);
                //var request = new RestRequest(Method.POST);
                //  var client = new RestClient("/api/library");
                //  var request = new RestRequest(Method.GET);
                // request.AddHeader("cache-control", "no-cache");
                // request.AddHeader("content-type", "text/xml");
                //  var response =  client.Execute(request);

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

                    var data = JsonConvert.DeserializeObject<LibraryDetailResponse>(responseBody);
                    return data;
                }
                catch (HttpRequestException e)
                {
                   // Console.WriteLine("\nException Caught!");
                  // Console.WriteLine("Message :{0} ", e.Message);

                    return new LibraryDetailResponse();
                }

            }
        }
    }
}
