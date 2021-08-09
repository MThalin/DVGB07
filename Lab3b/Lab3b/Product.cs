using System;

namespace Lab3b
{

    public class Product
    {
        String name;
        int number;
        int amount;
        Double price;

        public Product()
        {
            name = "";
            number = 0;
            amount = 0;
            price = 0;
        }

        //Returnerar namn på klassinstans.
        public String GetName()
        {
            return name;
        }

        //Tar emot nytt värde för namn på klassinstans.
        public void SetName(String nameIn)
        {
            name = nameIn;
        }

        //Returnerar produktnummer på klassinstans.
        public int GetNumber()
        {
            return number;
        }

        //Tar emot nytt värde för produktnummer på klassinstans.
        public void SetNumber(int numberIn)
        {
            number = numberIn;
        }

        //Returnerar antal på klassinstans.
        public int GetAmount()
        {
            return amount;
        }

        //Tar emot och räknar ut nytt värde för antal på klassinstans.
        public void SetAmount(int amountIn)
        {
            amount = amount + amountIn;
        }

        //Returnerar pris på klassinstans.
        public double GetPrice()
        {
            return price;
        }

        //Tar emot nytt värde för pris på klassinstans.
        public void SetPrice(double priceIn)
        {
            price = priceIn;
        }

    }
}
