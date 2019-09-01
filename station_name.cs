using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using System.Text;

namespace serverit{  
    class Program{
        static void Main(string[] args){ 
            string option;
            Console.WriteLine(args[0]);
            Console.WriteLine("offline or realtime?");
            option = Console.ReadLine();
            Console.WriteLine("What station do you want?");
            string answer = Console.ReadLine();
            try{
                if(option == "realtime"){
                    RealTimeCityBikeDataFetcher fetch = new RealTimeCityBikeDataFetcher();
                    var task = fetch.GetBikeCountInStation(answer);
                    Task.WaitAll(task);
                    Console.WriteLine(task.Result);
                    
                }
                else if(option == "offline"){
                    OfflineCityBikeDataFetcher fetch = new OfflineCityBikeDataFetcher();
                    var task = fetch.GetBikeCountInStation(answer);
                    Task.WaitAll(task);
                    Console.WriteLine(task.Result);
                }
                else{
                    Console.WriteLine("Invalid Input.");
                }
            }
            catch (FormatException e) {
                Console.WriteLine("Invalid argument "+ e);
            }
            catch(NotFoundExeption e){
                Console.WriteLine("Not Found: "+ e);
            }
            catch(Exception e){
                Console.WriteLine("Something went horribly wrong, goodbye!"+ e);
            }
             
        }
    }

    public class RealTimeCityBikeDataFetcher : ICityBikeDataFetcher{
        static HttpClient client = new HttpClient();
        public async Task<int> GetBikeCountInStation(string stationNAme)
        {
            string uri = "http://api.digitransit.fi/routing/v1/routers/hsl/bike_rental";
            
            if(stationNAme.Any(char.IsDigit)){
                throw new FormatException();
            }
            string resp = await client.GetStringAsync(uri);
            var stationlist = JsonConvert.DeserializeObject<RootObject>(resp).stations;
            Console.WriteLine("ei");
            foreach(var station in stationlist){
                if(station.name.ToLower() == stationNAme.ToLower()){            
                    return station.bikesAvailable;
                }
            }        
            throw new NotFoundExeption();
            

            
        }
    }
    public class OfflineCityBikeDataFetcher : ICityBikeDataFetcher{
        public async Task<int> GetBikeCountInStation(string stationNAme){
            int result = 0;
            if(stationNAme.Any(char.IsDigit)){
                throw new FormatException();
            }
            var offlineBackup = await File.ReadAllLinesAsync("bikedata.txt", Encoding.Default);
            List<OfflineStation> offSta = new List<OfflineStation>();
            foreach(string line in offlineBackup){
                string[] temp = line.Split(":");
                OfflineStation sta = new OfflineStation();
                sta.name = temp[0].Trim();
                sta.bikeCount = temp[1].Trim();
                offSta.Add(sta);
            }
            foreach(var station in offSta){

                if(station.name.ToLower() == stationNAme.ToLower()){
                    try{
                        result = Int32.Parse(station.bikeCount);
                        }
                        catch(FormatException){
                        Console.WriteLine("offline backup corrupted");
                    }
                    return result;
                }
            }
            throw new NotFoundExeption();
            
        }
    }
    

    public class OfflineStation{
        public string name { get; set; }
        public string bikeCount { get; set; }
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

        
    

    public interface ICityBikeDataFetcher {
        Task<int> GetBikeCountInStation(string stationNAme);
    }
}