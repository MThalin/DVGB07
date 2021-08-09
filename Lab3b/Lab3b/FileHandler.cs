using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Lab3b
{
    class FileHandler
    {
        //Läser in produktlista från XML-fil, returnerar listan.
        public List<Product> LoadFile()
        {
            XDocument myFile = XDocument.Load("SaveFile");
            List<Product> tempList = new List<Product>();

            foreach (var product in myFile.Descendants("Product"))
            {
                Product temp = new Product();
                temp.SetName(product.Element("Name").Value);
                temp.SetNumber(int.Parse(product.Element("Number").Value));
                temp.SetAmount(int.Parse(product.Element("Amount").Value));
                temp.SetPrice(double.Parse(product.Element("Price").Value));

                tempList.Add(temp);
            }

            return tempList;
        }

        //Tar emot variabler för att skriva specifikt värde under specifik tag i XML-filen, kortar bara ner koden.
        public void GetElements(XmlWriter writer, String category, String value)
        {
            writer.WriteStartElement(category);
            writer.WriteString(value);
            writer.WriteEndElement();
        }

        //Tar emot produktlista som sen skrivs till XML-fil.
        public void SaveFile(List<Product> storageList)
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                XmlWriter writer = XmlWriter.Create("SaveFile", settings);

                writer.WriteStartDocument();
                writer.WriteStartElement("Inventory");

                for (int i = 0; i < storageList.Count; i++)
                {
                    writer.WriteStartElement("Product");

                    GetElements(writer, "Name", storageList[i].GetName());
                    GetElements(writer, "Number", storageList[i].GetNumber().ToString());
                    GetElements(writer, "Amount", storageList[i].GetAmount().ToString());
                    GetElements(writer, "Price", storageList[i].GetPrice().ToString());

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine("Save failed! Exception: " + ex);
            }
        }
    }
}
