using System;
using System.Collections.Generic;
using System.Linq;


namespace Assig2{
    
    class Program{

        static void Main(string[] args)
        {
            MakeNewPlayers newPlayers = new MakeNewPlayers();
            juttuja testaa = new juttuja();
            LiNQtest1 linqtest = new LiNQtest1();
            LiNQtest2 lingtest2 = new LiNQtest2();
            Console.WriteLine(args[0]);
            newPlayers.CreateGuids();
            testaa.TestItems();
            Player bob=new Player();
            Array getitems = linqtest.GetItems(bob);
            Array getitemswithlinq =linqtest.GetItemsWithLinq(bob);
            Console.WriteLine("GetItems lenght {0}", getitems.Length);
            Console.WriteLine("GetItemsWithLinq lenght {0}", getitemswithlinq.Length);
            Console.WriteLine("Id for GetFirst {0}",lingtest2.FirstItem(bob).Id);
            Console.WriteLine("ID for GetFirstWithLinq {0}",lingtest2.FirstItemWithLinq(bob).Id);
            Console.WriteLine("meni jo");
        }
        


    }
    

        
    

    public class MakeNewPlayers{
        Players players = new Players();
        public void CreateGuids(){
            for(int i =0; i < 1000; i++){
                Player p = new Player();
                p.Id = Guid.NewGuid();
                CheckIfIdInUse(p);
                p.Items = new List<Item>();
                players.playerlist.Add(p);
            }
                void CheckIfIdInUse(Player x){
                if(players.playerlist.Count>1){
                    foreach(var p in players.playerlist){
                        
                        if(x.Id == p.Id){
                            x.Id= Guid.NewGuid();
                            CheckIfIdInUse(x);
                        }
                    }
                }
            }
            Console.WriteLine(players.playerlist.Count);
        }

    }
    public class juttuja{
        Players players = new Players();
        public Item TestItems(){
            Item first = new Item();
            first.Id = Guid.NewGuid();
            Item second = new Item();
            second.Id = Guid.NewGuid();
            Player p = new Player();
            first.Level = 1;
            second.Level = 10;
            p.Items = new List<Item>();
            p.Items.Add(first);
            p.Items.Add(second);
                    
            Item highest = p.Items.returnhighersIlevel();
            Console.WriteLine("Highest Item ID {0} and Highest Item Level {1}",highest.Id,highest.Level);
            return highest;
        }
    }
    public class LiNQtest1{
        
        
        public Item[] GetItems(Player p){
            p.Items = new List<Item>();
            Item first = new Item();
            Item second = new Item();
            p.Items.Add(first);

            p.Items.Add(second);

            

            Item[] Playeritems = new Item[p.Items.Count];
            for(int i=0;i<p.Items.Count;i++){
                Playeritems[i] = p.Items[i];
            }
            return Playeritems;
        }
        public Item[] GetItemsWithLinq(Player p){
            p.Items = new List<Item>();
            Item first = new Item();
            Item second = new Item();
            p.Items.Add(first);
            p.Items.Add(second);

            
            return p.Items.ToArray();
        }
    }

    public class LiNQtest2{
        public Item FirstItem(Player p){
            p.Items = new List<Item>();
            Item first = new Item();
            Item second = new Item();
            p.Items.Add(first);
            p.Items.Add(second);
            p.Items[0].Id = Guid.NewGuid();
            Console.WriteLine("ID of first item for without LINQ {0}", first.Id);
            p.Items[1].Id = Guid.NewGuid();
            Console.WriteLine("ID of second item for without LINQ {0}", second.Id);  
            if(p.Items.Count >0){
                Item it = p.Items[0];
                return it;
            }
            else return null;           
        }
        public Item FirstItemWithLinq(Player p){
            p.Items = new List<Item>();
            Item first = new Item();
            Item second = new Item();
            p.Items.Add(first);
            p.Items.Add(second);
            p.Items[0].Id = Guid.NewGuid();
            Console.WriteLine("ID of first item for with LINQ {0}", first.Id);
            p.Items[1].Id = Guid.NewGuid();
            Console.WriteLine("ID of second item for with LINQ {0}", second.Id);
            return p.Items.First();
        }
    }

    public interface IPlayer{
        int Score { get; set; }
    }

    public class Players{
        public List<Player> playerlist = new List<Player>();
    }
    public class Player : IPlayer{
        public Guid Id { get; set; }
        public int Score { get; set; }
        public List<Item> Items { get; set; }
    }

    public class Item{
        public Guid Id { get; set; }
        public int Level { get; set; }
    }
    
    public static class MethodExtension{
        public static Item returnhighersIlevel(this List<Item> list){
            Item it = list[0];
            foreach(var item in list){
                if(item.Level > it.Level)
                    it.Level=item.Level;
                    
            }
            return it;
        }  
    }


}