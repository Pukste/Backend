using System;
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
            //Console.WriteLine(args[0]);
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
        private int returnVal;
        private bool stationFound = false;
        
            
            public async Task<int> GetBikeCountInStation(string stationNAme)
            {
                try {
                if(stationNAme.Any(char.IsDigit)){
                    throw new FormatException();
                }
                }
                catch (FormatException e) {
                    Console.WriteLine("Invalid argument ", e);
                }
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
                                stationFound = true;
                                returnVal= bikes;

                            }

                        }}
                        try{
                        if(!stationFound){
                            throw new NotFoundExeption();
                        }}
                        catch(NotFoundExeption ex){
                            Console.WriteLine("Not Found: ", ex);
                        }
            

                    }
                    return returnVal;
                }
                
                
                



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
