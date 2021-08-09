using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Lab3b
{
    public partial class Form1 : Form
    {
        DataModel DM;

        public Form1()
        {
            InitializeComponent();
            DM = new DataModel(this);
            CheckPanel.BringToFront();
        }

        //Byter till kassa-vyn.
        private void CheckSwitchButton_Click(object sender, EventArgs e)
        {
            CheckPanel.BringToFront();
        }

        //Byter till lager-vyn.
        private void InvSwitchButton_Click(object sender, EventArgs e)
        {
            InvPanel.BringToFront();
        }

        //Ser till att alla fält markeras för den produkt som valts, i lagret.
        private void InvNameBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            InvPriceBox.SelectedIndex = InvNameBox.SelectedIndex;
            InvNumberBox.SelectedIndex = InvNameBox.SelectedIndex;
            InvAmountBox.SelectedIndex = InvNameBox.SelectedIndex;

            if (InvNameBox.SelectedIndex != -1)
            {
                InvRemoveButton.Enabled = true;
                InvDeliveryButton.Enabled = true;
            }
            else
            {
                InvRemoveButton.Enabled = false;
                InvDeliveryButton.Enabled = false;
            }
        }

        //Ser till att alla fält markeras för den produkt som valts, i kassan.
        private void CheckNameBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckNumberBox.SelectedIndex = CheckNameBox.SelectedIndex;
            CheckAmountBox.SelectedIndex = CheckNameBox.SelectedIndex;
            CheckPriceBox.SelectedIndex = CheckNameBox.SelectedIndex;
            CheckTotBox.SelectedIndex = CheckNameBox.SelectedIndex;

            if (CheckNameBox.SelectedIndex != -1)
            {
                CheckRemoveButton.Enabled = true;
            }
            else
            {
                CheckRemoveButton.Enabled = false;
            }
        }

        //Tar emot input för ny produkt, kontrollerar och kallar på relevant metod i DataModel.
        private void InvAddButton_Click(object sender, EventArgs e)
        {
            Boolean digit = false;
            String name = Interaction.InputBox("Enter product name:", "Name", "");
            if (name.Length == 0) { return; }
            String tmpNumber = "";
            String tmpPrice = "";

            while (digit == false)
            {
                tmpNumber = Interaction.InputBox("Enter product number:", "Number", "");
                if (tmpNumber.Length == 0) { return; }
                if (DM.IsAllDigits(tmpNumber)) 
                { 
                    digit = true;

                    if (!DM.NumberChecker(tmpNumber)) 
                    { 
                        digit = false; MessageBox.Show("Number already in use!"); 
                    }
                }
                else { MessageBox.Show("Only digits allowed!"); }
                
            }

            digit = false;
            while (digit == false)
            {
                tmpPrice = Interaction.InputBox("Enter product price:", "Price", "");
                if (tmpPrice.Length == 0) { return; }
                if (DM.IsAllDigits(tmpPrice)) { digit = true; }
                else { MessageBox.Show("Only digits allowed!"); }
            }

            int number = int.Parse(tmpNumber);
            double price = double.Parse(tmpPrice);
            
            DM.NewProduct(name, number, price);
        }

        //Registrerar att en produkt ska tas bort, kontrollerar och kallar på relevant metod i DataModel.
        private void InvRemoveButton_Click(object sender, EventArgs e)
        {
            String temp = InvNumberBox.GetItemText(InvNumberBox.SelectedItem);
            int index = InvNumberBox.SelectedIndex;
            int selected = int.Parse(temp);

            if (InvAmountBox.Items[index].ToString() != "0")
            {
                DialogResult result = MessageBox.Show("Stock is not empty for this product, delete anyway?", "Warning!", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    DM.DeleteProduct(selected, index);
                }
                else if (result == DialogResult.No)
                {
                    return;
                }
            }

            else
            {
                DM.DeleteProduct(selected, index);
            }
        }

        //Registrerar att antalet ska ökas på en produkt, kontrollerar och kallar på relevant metod i DataModel.
        private void InvDeliveryButton_Click(object sender, EventArgs e)
        {
            Boolean digit = false;
            String temp = InvNumberBox.GetItemText(InvNumberBox.SelectedItem);
            int index = InvNumberBox.SelectedIndex;
            int selected = int.Parse(temp);
            String tmp = "";

            while (digit == false)
            {
                tmp = Interaction.InputBox("Enter amount of delivered products:", "Amount", "");
                if (tmp.Length == 0) { return; }
                if (DM.IsAllDigits(tmp)) { digit = true; }
                else { MessageBox.Show("Only digits allowed!"); }
            }

            int amount = int.Parse(tmp);

            DM.EditAmount(selected, amount, index);
        }

        //Tar emot input för vilken produkt som ska läggas i kassan, och hur många, kontrollerar och kallar på relevant metod i DataModel.
        private void CheckAddButton_Click(object sender, EventArgs e)
        {
            Boolean digit = false;
            String nrTemp = "";
            String amTemp = "";

            while (digit == false)
            {
                nrTemp = Interaction.InputBox("Enter product number:", "Number", "");
                if (nrTemp.Length == 0) { return; }
                if (DM.IsAllDigits(nrTemp)) { digit = true; }
                else { MessageBox.Show("Only digits allowed!"); }
            }

            digit = false;
            while (digit == false)
            {
                amTemp = Interaction.InputBox("Enter amount of products:", "Amount", "");
                if (amTemp.Length == 0) { return; }
                if (DM.IsAllDigits(amTemp)) { digit = true; }
                else { MessageBox.Show("Only digits allowed!"); }
            }
            int number = int.Parse(nrTemp);
            int amount = int.Parse(amTemp);

            if (!DM.SellItem(number, amount))
            {
                MessageBox.Show("Item was not found, or stock was too low. Try again.");
            }

            else 
            {
                CheckOutButton.Enabled = true;
                CheckClearButton.Enabled = true;
            }
        }

        //Tar bort vald produkt från kassan.
        private void CheckRemoveButton_Click(object sender, EventArgs e)
        {
            int index = CheckNameBox.SelectedIndex;

            CheckNameBox.Items.RemoveAt(index);
            CheckNumberBox.Items.RemoveAt(index);
            CheckAmountBox.Items.RemoveAt(index);
            CheckPriceBox.Items.RemoveAt(index);
            CheckTotBox.Items.RemoveAt(index);

            DM.UpdateCheckLabels();

            if (CheckNameBox.Items.Count == 0)
            {
                CheckClearButton.Enabled = false;
                CheckOutButton.Enabled = false;
            }
        }

        //Rensar kassan.
        private void CheckClearButton_Click(object sender, EventArgs e)
        {
            CheckNameBox.Items.Clear();
            CheckNumberBox.Items.Clear();
            CheckAmountBox.Items.Clear();
            CheckPriceBox.Items.Clear();
            CheckTotBox.Items.Clear();

            DM.UpdateCheckLabels();
            CheckClearButton.Enabled = false;
            CheckOutButton.Enabled = false;
            CheckRemoveButton.Enabled = false;
        }

        //Slutför köpet och därmed kallar på relevant metod i DataModel samt rensar kassan.
        private void CheckOutButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Purchase complete!");
            DM.Checkout();

            CheckNameBox.Items.Clear();
            CheckNumberBox.Items.Clear();
            CheckAmountBox.Items.Clear();
            CheckPriceBox.Items.Clear();
            CheckTotBox.Items.Clear();

            DM.UpdateCheckLabels();
            CheckOutButton.Enabled = false;
            CheckRemoveButton.Enabled = false;
            CheckClearButton.Enabled = false;
            InvRemoveButton.Enabled = false;
            InvDeliveryButton.Enabled = false;
        }

        //Kallar på metod i DataModel för att spara.
        private void SaveButton_Click(object sender, EventArgs e)
        {
            DM.Save();
        }

    }
}
