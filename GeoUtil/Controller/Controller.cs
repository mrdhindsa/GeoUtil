using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GeoUtil.Controller
{
    public class LocationResponse
    {
        [JsonProperty("zip")]
        public string? ZipCode { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("lat")]
        public double? Latitude { get; set; }

        [JsonProperty("lon")]
        public double? Longitude { get; set; }

        [JsonProperty("state")]
        public string? State { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }
    }

    static class Controller
    {
        private static readonly string AppId = "f897a99d971b5eef57be6fafa0d83239";
        
        public static async Task<LocationResponse?> LocationDataByZip(int zipcode)
        {
            string url = $"https://api.openweathermap.org/geo/1.0/zip?zip={zipcode}&appid={AppId}";
            
            LocationResponse? location = null;
            
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    
                    try
                    {
                        var locations = JsonConvert.DeserializeObject<LocationResponse[]>(responseBody);
                        location = locations?.FirstOrDefault();
                    }
                    catch (JsonSerializationException)
                    {
                        location = JsonConvert.DeserializeObject<LocationResponse>(responseBody);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }

            return location;
        }

        public static async Task<LocationResponse?> LocationDataByCityState(string city, string state)
        {
            if (state.Length == 2) // Try to get state name from abrev
            {
                state = StateLookup.GetStateName(state.ToUpper());
            }

            string url = $"https://api.openweathermap.org/geo/1.0/direct?q={city},{state}&limit=5&appid={AppId}";

            LocationResponse? location = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    try
                    {
                        var locations = JsonConvert.DeserializeObject<LocationResponse[]>(responseBody);
                        location = locations?.FirstOrDefault();
                    }
                    catch (JsonSerializationException)
                    {
                        location = JsonConvert.DeserializeObject<LocationResponse>(responseBody);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }

            return location;
        }

        public static void PrintLocationResponses(string location, LocationResponse locationresponse)
        {
            Console.WriteLine($"Processed Location: {location}");
            Console.WriteLine($"ZIP Code: {locationresponse?.ZipCode ?? "N/A"}");
            Console.WriteLine($"Name: {locationresponse?.Name ?? "N/A"}");
            Console.WriteLine($"Latitude: {locationresponse?.Latitude?.ToString() ?? "N/A"}");
            Console.WriteLine($"Longitude: {locationresponse?.Longitude?.ToString() ?? "N/A"}");
            Console.WriteLine($"State: {locationresponse?.State ?? "N/A"}");
            Console.WriteLine($"Country: {locationresponse?.Country ?? "N/A"}");
            Console.WriteLine(new string('-', 30)); // Separator for readability
        }
    }
}
