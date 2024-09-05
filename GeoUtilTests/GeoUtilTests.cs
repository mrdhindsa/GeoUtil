using GeoUtil;

namespace GeoUtilTests
{
    public class Tests
    {
        private GeoService _geoService;

        [SetUp]
        public void Setup()
        {
            _geoService = new GeoService(TimeSpan.FromMinutes(5));
        }

        [Test]
        public async Task Test1()
        {
            string[] locations = { "Los Angeles, CA" };

            await _geoService.ProcessLocationsAsync(locations);

            var response = _geoService._responses.GetResponse(locations[0]);

            Assert.That(response?.Country == "US");
            Assert.That(response?.Latitude == 34.0536909);
            Assert.That(response?.Longitude == -118.242766);
            Assert.That(response?.Name == "Los Angeles");
            Assert.That(response?.State == "California");
            Assert.That(response?.ZipCode == null);
        }

        [Test]
        public async Task Test2()
        {
            string[] locations = { "Los Angeles, California" };

            await _geoService.ProcessLocationsAsync(locations);

            var response = _geoService._responses.GetResponse(locations[0]);

            Assert.That(response?.Country == "US");
            Assert.That(response?.Latitude == 34.0536909);
            Assert.That(response?.Longitude == -118.242766);
            Assert.That(response?.Name == "Los Angeles");
            Assert.That(response?.State == "California");
            Assert.That(response?.ZipCode == null);
        }

        [Test]
        public async Task Test3()
        {
            string[] locations = { "LosAngeles" }; // Invalid Address

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _geoService.ProcessLocationsAsync(locations);
            });

            Assert.That(ex.Message, Is.EqualTo("Invalid ZIP code: LosAngeles"));
        }

        [Test]
        public async Task Test4()
        {
            string[] locations = { "Los, Angeles, CA" }; // Invalid Address

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _geoService.ProcessLocationsAsync(locations);
            });

            Assert.That(ex.Message, Is.EqualTo("Invalid Location: Los, Angeles, CA"));
        }

        [Test]
        public async Task Test5()
        {
            string[] locations = { "99999" }; // Invalid Address - Won't be captured by local checks

            await _geoService.ProcessLocationsAsync(locations);

            var response = _geoService._responses.GetResponse(locations[0]);

            Assert.IsNull(response);
        }

        [Test]
        public async Task Test6()
        {
            string[] locations = { "Madison, WI", "12345", "Chicago, IL", "10001" }; 

            await _geoService.ProcessLocationsAsync(locations);

            // "Madison, WI"
            var response = _geoService._responses.GetResponse(locations[0]);

            Assert.That(response?.Country == "US");
            Assert.That(response?.Latitude == 43.074761);
            Assert.That(response?.Longitude == -89.3837613);
            Assert.That(response?.Name == "Madison");
            Assert.That(response?.State == "Wisconsin");
            Assert.That(response?.ZipCode == null);

            // "12345"
            response = _geoService._responses.GetResponse(locations[1]);

            Assert.That(response?.Country == "US");
            Assert.That(response?.Latitude == 42.8142);
            Assert.That(response?.Longitude == -73.9396);
            Assert.That(response?.Name == "Schenectady");
            Assert.That(response?.State == null);
            Assert.That(response?.ZipCode == "12345");

            // "Chicago, IL"
            response = _geoService._responses.GetResponse(locations[2]);

            Assert.That(response?.Country == "US");
            Assert.That(response?.Latitude == 41.8755616);
            Assert.That(response?.Longitude == -87.6244212);
            Assert.That(response?.Name == "Chicago");
            Assert.That(response?.State == "Illinois");
            Assert.That(response?.ZipCode == null);

            // "10001"
            response = _geoService._responses.GetResponse(locations[3]);

            Assert.That(response?.Country == "US");
            Assert.That(response?.Latitude == 40.7484);
            Assert.That(response?.Longitude == -73.9967);
            Assert.That(response?.Name == "New York");
            Assert.That(response?.State == null);
            Assert.That(response?.ZipCode == "10001");
        }
    }
}