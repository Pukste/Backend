using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;

namespace serverit
{
    
    
    class Program
    {
        static void Main(string[] args)
        {
            RealTimeCityBikeDataFetcher fetch = new RealTimeCityBikeDataFetcher();
            string option;
            Console.WriteLine(args[0]);
            Console.WriteLine("offline or realtime?");
            option = Console.ReadLine();
            if(option == "realtime"){
                Console.WriteLine("What station do you want? Petikontie");
                Console.WriteLine(fetch.GetBikeCountInStation(Console.ReadLine()));
                Task.WaitAll();
            }
            else if(option == "offline"){
                // ToBeImplemented
            }
            else{
                Console.WriteLine("Invalid Input.");
            }
        }     
    }

    public class RealTimeCityBikeDataFetcher : ICityBikeDataFetcher
    {
        static HttpClient client = new HttpClient();

            public async Task<int> GetBikeCountInStation(string stationNAme)
            {
                string uri = "http://api.digitransit.fi/routing/v1/routers/hsl/bike_rental";
                try {
                if(stationNAme.Any(char.IsDigit)){
                    throw new FormatException();
                }
                }
                catch (FormatException e) {
                    Console.WriteLine("Invalid argument ", e);
                }
                string resp = await client.GetStringAsync(uri);
                var stationlist = JsonConvert.DeserializeObject<RootObject>(resp).stations;
                Console.WriteLine("ei");
                foreach(var station in stationlist){
                    if(station.name == stationNAme){            
                        Console.WriteLine(station.bikesAvailable);
                        return station.bikesAvailable;
                    }

                }        
                    /*  dynamic x = JsonConvert.DeserializeObject(resp);
                        var stations = x.stations;
                        var bikes = x.bikesAvailable;
                        var name = x.name;
                        foreach(var station in stations){
                            Console.WriteLine(bikes, name);
                            if(name==stationNAme){
                                stationFound = true;
                                returnVal= bikes;

                            }

                        }
                        }*/
                try{             
                    throw new NotFoundExeption();
                }
                catch(NotFoundExeption ex){
                    Console.WriteLine("Not Found: ", ex);
                }
                return -1;
            }

    public class Station
    {
        public string id { get; set; }
        public string name { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public int bikesAvailable { get; set; }
        public int spacesAvailable { get; set; }
        public bool allowDropoff { get; set; }
        public bool isFloatingBike { get; set; }
        public bool isCarStation { get; set; }
        public string state { get; set; }
        public List<string> networks { get; set; }
        public bool realTimeData { get; set; }
    }

     public class RootObject
    {
        public List<Station> stations { get; set; }
    }   

    public class NotFoundExeption : System.Exception
    {
        public NotFoundExeption() { }
        public NotFoundExeption(string message) : base(message) { }
        public NotFoundExeption(string message, System.Exception inner) : base(message, inner) { }
        protected NotFoundExeption(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

        
    }

public interface ICityBikeDataFetcher {
    Task<int> GetBikeCountInStation(string stationNAme);
}
}