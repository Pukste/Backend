using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;


namespace serverit
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine(args[0]);
        }
        
    
    }

    class RealTimeCityBikeDataFetcher : ICityBikeDataFetcher
    {
        private int returnVal;
        
        public async Task<int> GetBikeCountInStation(string stationNAme)
        {

            using(var client = new HttpClient()){
                HttpResponseMessage response = await client.GetAsync("http://api.digitransit.fi/routing/v1/routers/hsl/bike_rental");
                using (HttpContent content = response.Content){
                    string resp = await response.Content.ReadAsStringAsync();
                    dynamic x = JsonConvert.DeserializeObject(resp);
                    var stations = x.stations;
                    var bikes = x.bikesAvailable;
                    var name = x.name;
                    foreach(var station in stations){
                        if(name==stationNAme){
                            returnVal= bikes;

                        }

                    }
                }
            }
            
            //poistaa virheen niin kauan kunnes saa json parserin toimimaan
            return returnVal;



        }

        
    }
}

public interface ICityBikeDataFetcher {
    Task<int> GetBikeCountInStation(string stationNAme);
}
