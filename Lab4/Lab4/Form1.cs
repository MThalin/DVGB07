using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Lab4
{
    public partial class Form1 : Form
    {
        String sourcePath;
        String destinationPath;
        FileSystemWatcher watcher;

        public Form1()
        {
            InitializeComponent();
            myTextBox.Text = "";
            watcher = new FileSystemWatcher();
        }

        private void myButton_Click(object sender, EventArgs e)
        {
            myTextBox.Text = "";
            
            if (SetSource())
            {
                SetDestination();
            }
        }

        public void Feed(String s)
        {
            if (this.InvokeRequired) { this.Invoke(new MethodInvoker(() => Feed(s))); }
            else 
            {
                myTextBox.AppendText(s);
                myTextBox.AppendText(Environment.NewLine);
            } 
        }

        public Boolean SetSource()
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.Description = "Choose folder to monitor:";

            if (browser.ShowDialog() == DialogResult.OK)
            {
                sourcePath = browser.SelectedPath;
                
                watcher.Path = sourcePath;
                watcher.Filter = "SaveFile";
                watcher.EnableRaisingEvents = true;
                watcher.Created += new FileSystemEventHandler(ReAction);
                watcher.Deleted += new FileSystemEventHandler(ReAction);
                watcher.Changed += new FileSystemEventHandler(ReAction);

                Console.WriteLine("Folder being monitored: " + sourcePath);
                Feed("Folder being monitored: " + sourcePath);
                return true;
            }

            else
            {
                Feed("No source folder selected, setup aborted.");
                return false;
            }
        }

        public void SetDestination()
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.Description = "Choose destination folder:";

            if (browser.ShowDialog() == DialogResult.OK)
            {
                destinationPath = browser.SelectedPath;
                Console.WriteLine("File will be saved to: " + destinationPath);
                Feed("File will be saved to: " + destinationPath);
            }
            else 
            {
                Feed("No destination folder selected, setup aborted.");
            }
        }

        //Konverterar min sparfil till textfil, då jag redan hade valt att spara som XML i labb3.
        //(gör det svårare för mig, för att uppfylla labbkraven, istället för att ändra i labb3)
        public void Convert()
        {
            try
            {
                String[] oldXML = Directory.GetFiles(sourcePath, "SaveFile");
                XDocument myFile = XDocument.Load(oldXML[0]);
                String myTextFile = "";

                foreach (var product in myFile.Descendants("Product"))
                {
                    if (myTextFile != "") { myTextFile += Environment.NewLine; }
                    myTextFile += (product.Element("Name").Value + ";");
                    myTextFile += (product.Element("Number").Value + ";");
                    myTextFile += (product.Element("Amount").Value + ";");
                    myTextFile += product.Element("Price").Value;
                }

                using (StreamWriter writer = new StreamWriter("Converted.txt"))
                {
                    writer.WriteLine(myTextFile);
                }

                Integrate();
            }

            catch (Exception e) { Console.WriteLine("Exception: " + e); }
        }

        public void GetElements(XmlWriter writer, String category, String value)
        {
            writer.WriteStartElement(category);
            writer.WriteString(value);
            writer.WriteEndElement();
        }

        public void Integrate()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            String row;
            String[] product;

            try
            {
                using (StreamReader reader = new StreamReader("Converted.txt"))
                {
                    XmlWriter writer = XmlWriter.Create(destinationPath + @"\Imported.xml", settings);
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Inventory");

                    while ((row = reader.ReadLine()) != null)
                    {
                        product = row.Split(';');

                        writer.WriteStartElement("Item");

                        GetElements(writer, "Name", product[0]);
                        GetElements(writer, "Count", product[2]);
                        GetElements(writer, "Price", product[3]);
                        GetElements(writer, "Comment", "n/a");
                        GetElements(writer, "Artist", "n/a");
                        GetElements(writer, "Publisher", "n/a");
                        GetElements(writer, "Genre", "n/a");
                        GetElements(writer, "Year", "0000");
                        GetElements(writer, "ProductID", product[1]);

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.Close();
                }

                Feed("File was imported to selected destination. (" + DateTime.Now.ToLocalTime() + ")");
            }

            catch (Exception e) { Console.WriteLine("Exception: " + e); }
        }

        public void ReAction(object sender, FileSystemEventArgs e)
        {
            try
            {
                watcher.EnableRaisingEvents = false;

                Console.WriteLine("File: '" + e.Name + "' was " + e.ChangeType.ToString() + ". (" + DateTime.Now.ToLocalTime() + ")");
                Feed("File: '" + e.Name + "' was " + e.ChangeType.ToString() + ". (" + DateTime.Now.ToLocalTime() + ")");

                Convert();
            }

            finally { watcher.EnableRaisingEvents = true; }
        }
    }
}
