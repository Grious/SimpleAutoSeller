using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Core;
using AOSharp.Core.UI;
using AOSharp.Core.Inventory;
using AOSharp.Common.GameData;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;
using SmokeLounge.AOtomation.Messaging.GameData;

namespace SAS
{
    public class SimpleAutoSeller : AOPluginEntry
    {
        public static bool Toggle = false;
        public static bool SetBags = false;
        protected double LastZonedTime = Time.NormalTime;

        public override void Run(string pluginDir)
        {
            try
            {
                Chat.WriteLine("SimpeAutoSeller loaded!");
                Chat.WriteLine("Use Specialist Commerce in ICC");
                Chat.WriteLine("/sas for toggle.");                

                Game.OnUpdate += OnUpdate;   

                Chat.RegisterCommand("sas", (string command, string[] param, ChatWindow chatWindow) =>
                {
                    Toggle = !Toggle;
                    SetBags = false;
                    Chat.WriteLine($"SAS Active : {Toggle}");
                });
            }
            catch (Exception e)
            {
                Chat.WriteLine(e.Message);
            }
        }



        private void OnUpdate(object s, float deltaTime)
        {
            if (!SetBags)
            {
                foreach (Backpack backpack in Inventory.Backpacks)
                {
                    List<Item> bags = Inventory.Items.Where(c => c.UniqueIdentity.Type == IdentityType.Container && c.Name.Contains("loot")).ToList();
                    foreach (Item bag in bags)
                    {
                        bag.Use();
                        bag.Use();
                    }
                }
                SetBags = true;
            }

            if (Toggle)
            {
                if (!Inventory.Items.Any(c => c.Name.Contains("Pearl") || c.Name.Contains("Monster Parts") 
                || c.Name.Contains("Pattern") || c.Name.Contains("Blood Plasma")))
                {
                    Container _bag = Inventory.Backpacks.FirstOrDefault(c => c.IsOpen && c.Items.Count() > 0 && c.Name.Contains("loot"));

                    if (_bag != null)
                        foreach (Item MoveItem in _bag?.Items.Take(Inventory.NumFreeSlots-1))
                            MoveItem?.MoveToInventory();
                }
                if (DynelManager.Find("Specialist Commerce", out SimpleItem SpecCom))


                if (Inventory.Find("Pearl", out Item pearl))
                    if (Inventory.Find("Jensen Gem Cutter", out Item cutter))
                        cutter.CombineWith(pearl);

                if (Inventory.Find("Monster Parts", out Item parts))
                    if (Inventory.Find("Advanced Bio-Comminutor", out Item bio))
                        bio.CombineWith(parts);

                foreach (Item SellItem in Inventory.Items.Where(c => c.Slot.Type ==IdentityType.Inventory))
                {                    
                    SpecCom.Use();
                    if (SellItem.Name.Contains("Blood Plasma") || SellItem.Name.Contains("Pattern") || SellItem.Name.Contains("Pearl"))
                        Trade.AddItem(DynelManager.LocalPlayer.Identity, SellItem.Slot); 
                        
                    Trade.Accept(Identity.None);
                }

            }
        }

        public override void Teardown()
        {
        }
    }
}
