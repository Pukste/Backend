using System;
using System.Collections.Generic;
using System.Linq;


namespace Assig2{
    
    class Program{
        
        static void Main(string[] args)
        {
            Players players = MakeNewPlayers.players;
            MakeNewPlayers newPlayers = new MakeNewPlayers();
            LiNQtest1 linqtest = new LiNQtest1();
            LiNQtest2 lingtest2 = new LiNQtest2();
            ActionTest aktion = new ActionTest();
            //1
            newPlayers.CreateGuids();
            Player bob=players.playerlist[0];
            //2
            Item item = bob.Items.returnhighersIlevel();
            Console.WriteLine("Highest Item ID is {0} and its Item Level is {1}",item.Id,item.Level);
            //3
            Array getitems = linqtest.GetItems(bob);
            Array getitemswithlinq =linqtest.GetItemsWithLinq(bob);
            Console.WriteLine("GetItems lenght {0}", getitems.Length);
            Console.WriteLine("GetItemsWithLinq lenght {0}", getitemswithlinq.Length);
            //4
            Console.WriteLine("Id for GetFirst {0}",lingtest2.FirstItem(bob).Id);
            Console.WriteLine("ID for GetFirstWithLinq {0}",lingtest2.FirstItemWithLinq(bob).Id);
            //5
            aktion.ProcessEachItem(bob, aktion.PrintItem());
            Console.WriteLine("meni jo");
        }
        


    }
    

        
    

    public class MakeNewPlayers{
        public static Players players = new Players();
        public void CreateGuids(){
            HashSet<Guid> hashset = new HashSet<Guid>();
            for(int i =0; i < 1000000; i++){
                Player p = new Player();
                p.Id = Guid.NewGuid();
                void paranoid(){
                    if(hashset.Contains(p.Id)){
                        p.Id= Guid.NewGuid();
                        Console.WriteLine("Miracle has happened");
                        paranoid();
                    }
                }
                hashset.Add(p.Id);
                p.Items = new List<Item>();
                players.playerlist.Add(p);
                
            }
            Item first = new Item();
            first.Id = Guid.NewGuid();
            Item second = new Item();
            second.Id = Guid.NewGuid();
            first.Level = 1;
            second.Level = 10;
            Player p1 = players.playerlist[0];
            p1.Items = new List<Item>();
            p1.Items.Add(first);
            p1.Items.Add(second);
            Console.WriteLine(players.playerlist.Count);
        }

    }

    public class LiNQtest1{
        
        
        public Item[] GetItems(Player p){         

            Item[] Playeritems = new Item[p.Items.Count];
            for(int i=0;i<p.Items.Count;i++){
                Playeritems[i] = p.Items[i];
            }
            return Playeritems;
        }
        public Item[] GetItemsWithLinq(Player p){          
            return p.Items.ToArray();
        }
    }

    public class LiNQtest2{
        public Item FirstItem(Player p){
            if(p.Items.Count >0){
                return p.Items[0];
            }
            else return null;           
        }
        public Item FirstItemWithLinq(Player p){
            return p.Items.First();
        }
    }

    public class ActionTest{
        public static void ProcessEachItem(Player player, Action<Item> process){
            foreach(var item in player.Items){
                process(item);
            }
        }

        public static void PrintItem(Item item){
            Console.WriteLine("Item ID: {0} and Item Level: {1}", item.Id, item.Level);
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