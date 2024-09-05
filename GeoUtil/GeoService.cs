using GeoUtil.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoUtil
{
    public class GeoService
    {
        public ResponseCache _responses;
        
        public GeoService(TimeSpan _expirationTime)
        {
            _responses = new ResponseCache(_expirationTime);
        }

        public async Task ProcessLocationsAsync(string[] locations)
        {
            if (locations.Length == 0)
            {
                Console.WriteLine("No arguments provided.");
                return;
            }

            foreach (string location in locations)
            {
                if (location.Contains(',')) // Verify valid City, State Combination
                {
                    string[] slocation = location.Split(',');

                    if (slocation.Length != 2)
                    {
                        throw new ArgumentException($"Invalid Location: {location}");
                    }

                    string city = slocation[0].Trim();
                    string state = slocation[1].Trim();

                    var response = await GeoUtil.Controller.Controller.LocationDataByCityState(city, state);

                    GeoUtil.Controller.Controller.PrintLocationResponses(location, response);

                    _responses.AddResponse(location, response);
                }
                else // Verify valid Zip Code
                {
                    if (int.TryParse(location, out int zipcode) && (location.Length == 4 || location.Length == 5))
                    {
                        var response = await GeoUtil.Controller.Controller.LocationDataByZip(zipcode);

                        GeoUtil.Controller.Controller.PrintLocationResponses(location, response);

                        _responses.AddResponse(location, response);
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid ZIP code: {location}");
                    }
                }
            }
        }
    }
}
