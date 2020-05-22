
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
