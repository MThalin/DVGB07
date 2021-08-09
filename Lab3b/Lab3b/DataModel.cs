using System;
using System.Collections.Generic;

namespace Lab3b
{
    class DataModel
    {
        Form1 frm;
        FileHandler FH;
        List<Product> storageList;

        public DataModel(Form1 form1)
        {
            frm = form1;
            FH = new FileHandler();
            storageList = new List<Product>();

            try { Load(); }
            catch (Exception ex) { Console.WriteLine("Load failed! Exception: " + ex); }
        }

        //Kontrollerar ifall en sträng endast innehåller siffror.
        public bool IsAllDigits(String s)
        {
            foreach (char c in s)
            {
                if (!Char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

        //Kontrollerar ifall ett specifikt produktnummer redan existerar.
        public Boolean NumberChecker(String nr)
        {
            int number = int.Parse(nr);

            for (int i = 0; i < storageList.Count; i++)
            {
                if (storageList[i].GetNumber() == number)
                {
                    return false;
                }
            }

            return true;
        }

        //Tar emot produktinformation och lägger till den nya produkten i lagret.
        public void NewProduct(String name, int number, double price)
        {
            Product temp = new Product();
            temp.SetName(name);
            temp.SetNumber(number);
            temp.SetAmount(0);
            temp.SetPrice(price);

            storageList.Add(temp);

            frm.InvNameBox.Items.Add(storageList[storageList.Count - 1].GetName());
            frm.InvNumberBox.Items.Add(storageList[storageList.Count - 1].GetNumber());
            frm.InvAmountBox.Items.Add(storageList[storageList.Count - 1].GetAmount());
            frm.InvPriceBox.Items.Add(storageList[storageList.Count - 1].GetPrice());
        }

        //Tar bort utvald produkt från lagret.
        public void DeleteProduct(int selected, int index)
        {
            for (int i = 0; i <= storageList.Count - 1; i++)
            {
                if (storageList[i].GetNumber() == selected)
                {
                    storageList.RemoveAt(i);
                    frm.InvNameBox.Items.RemoveAt(index);
                    frm.InvNumberBox.Items.RemoveAt(index);
                    frm.InvAmountBox.Items.RemoveAt(index);
                    frm.InvPriceBox.Items.RemoveAt(index);
                    break;
                }
            }

            UpdateInvLabel();
            UpdateStock();
        }

        //Justerar antalet av en utvald produkt, med angiven mängd.
        public void EditAmount(int selected, int amount, int index)
        {
            for (int i = 0; i < storageList.Count; i++)
            {
                if (storageList[i].GetNumber() == selected)
                {
                    storageList[i].SetAmount(amount);
                    frm.InvAmountBox.Items[index] = storageList[i].GetAmount();
                    break;
                }
            }

            UpdateInvLabel();
            UpdateStock();
        }

        //Uppdaterar en Label med det totala antalet produkter i lagret.
        public void UpdateInvLabel()
        {
            int total = 0;

            for (int i = 0; i < storageList.Count; i++)
            {
                total = total + storageList[i].GetAmount();
            }

            String text = total.ToString();
            frm.TotLabel.Text = "Items: " + text;
        }

        //Uppdaterar Labels med det totala antalet samt totala priset av produkter i kassan.
        public void UpdateCheckLabels()
        {
            int items = 0;
            double cost = 0;

            foreach (var item in frm.CheckAmountBox.Items)
            {
                items = items + (int)item;
            }

            foreach (var item in frm.CheckTotBox.Items)
            {
                cost = cost + (double)item;
            }

            String itemsT = items.ToString();
            String costT = cost.ToString();
            frm.ItemLabel.Text = "Items: " + itemsT;
            frm.CostLabel.Text = "Cost: " + costT;
        }

        //Uppdaterar ListBoxes som visar fulla lagerinformationen.
        public void UpdateInventory()
        {
            frm.InvNameBox.Items.Clear();
            frm.InvNumberBox.Items.Clear();
            frm.InvAmountBox.Items.Clear();
            frm.InvPriceBox.Items.Clear();

            for (int i = 0; i < storageList.Count; i++)
            {
                frm.InvNameBox.Items.Add(storageList[i].GetName());
                frm.InvNumberBox.Items.Add(storageList[i].GetNumber());
                frm.InvAmountBox.Items.Add(storageList[i].GetAmount());
                frm.InvPriceBox.Items.Add(storageList[i].GetPrice());
            }
        }

        //Uppdaterar ListBoxes som visar information om produkter som finns i lager.
        public void UpdateStock()
        {
            frm.StockNameBox.Items.Clear();
            frm.StockNumberBox.Items.Clear();
            frm.StockAmountBox.Items.Clear();

            for (int i = 0; i < storageList.Count; i++)
            {
                if (storageList[i].GetAmount() > 0)
                {
                    frm.StockNameBox.Items.Add(storageList[i].GetName());
                    frm.StockNumberBox.Items.Add(storageList[i].GetNumber());
                    frm.StockAmountBox.Items.Add(storageList[i].GetAmount());
                }
            }
        }

        //Lägger till önskad mängd av angiven produkt i kassan. Kontrollerar först om produkten redan ligger i kassan, utökar isåfall mängd och räknar om pris.
        public Boolean SellItem(int number, int amount)
        {
            Boolean invFound = false;
            Boolean amtFound = false;
            Boolean checkFound = false;
            String nr = number.ToString();
            int i = 0;
            int index = 0;
            int newAmount = amount;

            for (i = 0; i < storageList.Count; i++)
            {
                if (storageList[i].GetNumber() == number)
                {
                    invFound = true;

                    foreach (var item in frm.CheckNumberBox.Items)
                    {
                        if (item.ToString().Equals(nr))
                        {
                            checkFound = true;
                            index = frm.CheckNumberBox.Items.IndexOf(item);
                            newAmount = (int)frm.CheckAmountBox.Items[index] + amount;
                        }
                    }

                    if (storageList[i].GetAmount() >= newAmount)
                    {
                        amtFound = true;
                    }

                    break;
                }
            }

            if (invFound == true && amtFound == true && checkFound == true)
            {
                double newTot = storageList[i].GetPrice() * newAmount;
                frm.CheckAmountBox.Items[index] = newAmount;
                frm.CheckTotBox.Items[index] = newTot;
            }

            else if (invFound == true && amtFound == true && checkFound == false)
            {
                frm.CheckNameBox.Items.Add(storageList[i].GetName());
                frm.CheckNumberBox.Items.Add(number);
                frm.CheckAmountBox.Items.Add(amount);
                frm.CheckPriceBox.Items.Add(storageList[i].GetPrice());
                frm.CheckTotBox.Items.Add((storageList[i].GetPrice()) * (amount));
            }

            if (invFound == false || amtFound == false)
            {
                return false;
            }

            else
            {
                UpdateCheckLabels();
                return true;
            }
        }

        //Reducerar antalet på de produkter som "gått igenom" kassan.
        public void Checkout()
        {
            foreach (var item in frm.CheckNumberBox.Items)
            {
                int index = frm.CheckNumberBox.Items.IndexOf(item);
                int amount = (int)frm.CheckAmountBox.Items[index];
                int number = (int)item;

                for (int i = 0; i < storageList.Count; i++)
                {
                    if (storageList[i].GetNumber() == number)
                    {
                        storageList[i].SetAmount(-amount);
                    }
                }
            }

            UpdateInventory();
            UpdateStock();
        }

        //Kallar på metod för att ladda fil i FileHandler, sen uppdaterar lagret med hämtad data.
        public void Load()
        {
            storageList = FH.LoadFile();

            UpdateInventory();
            UpdateInvLabel();
            UpdateStock();
        }

        //Kallar på metod för att spara fil i FileHandler.
        public void Save()
        {
            FH.SaveFile(storageList);
        }
    }
}  

