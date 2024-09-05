using System;
using System.Reflection.Emit;

namespace GeoUtil
{
    class Program
    {
        static async Task Main(string[] locations)
        {
            var geoService = new GeoService(TimeSpan.FromMinutes(5));
            await geoService.ProcessLocationsAsync(locations);
        }
    }
}
