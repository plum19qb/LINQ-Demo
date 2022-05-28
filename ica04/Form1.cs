/* ICA04 - LINQ
 * September 28 2021
 * By Liam Carroll for CMPE2800 1211
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ica04
{
    public partial class Form1 : Form
    {

        List<string> sourcestrings = new List<string>(
            new string[] { "Caballo", "Gato", "Perro", "Conejo", "Tortuga", "Cangrejo" });

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //select the strings where the sum is less than 600
            var partA1 = from animals in sourcestrings where animals.Sum(ch => ch) < 600 select animals;
            foreach (var animal in partA1) tbxOutput.Text += $"{animal}{Environment.NewLine}";
            //same thing but put them into an anonymous type
            var partA2 = from animals in sourcestrings where animals.Sum(ch => ch) < 600 select new { Str = animals, Sum = animals.Sum(ch => ch) };
            foreach (var animal in partA2) tbxOutput.Text += $"{animal}{Environment.NewLine}";
            //Order the anonymous types (see above) by the sum and select the anonymous types
            var partA3 = from animals in sourcestrings where animals.Sum(ch => ch) < 600 select new { Str = animals, Sum = animals.Sum(ch => ch) } into newSet orderby newSet.Sum descending select newSet;
            foreach (var animal in partA3) tbxOutput.Text += $"{animal}{Environment.NewLine}";

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            FileDialog dialog = new OpenFileDialog();

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                tbxOutput.Text = "";

                var sr = new StreamReader(dialog.FileName);
                var fileBuffer = sr.ReadToEnd();

                var fileContents = from i in fileBuffer.Split(new char[] { '\r', '\n', '\t' }) where i != "" select i;

                //Create a grouped collection where the key is the sum and the group is the items that share that sum
                var fileGrouped = from i in fileContents group i by i.Sum(ch => ch);

                //Create a new grouped collection sorted by key, select first group and return key
                tbxOutput.Text += $"Lowest ASCII Sum : {(from i in fileGrouped orderby i.Key ascending select i).First().Key} {Environment.NewLine}";
                //Select the items from the grouped collection sorted by key, order them and return the first item from that group
                tbxOutput.Text += $"Lowest String : {(from j in (from i in fileGrouped orderby i.Key ascending select i).First() orderby j select j).First() }{Environment.NewLine}";
                //Create a new grouped collection sorted by key, select last group and return key
                tbxOutput.Text += $"Highest ASCII Sum : {(from i in fileGrouped orderby i.Key ascending select i).Last().Key} {Environment.NewLine}";
                //select the items from the grouped collection sorted by key, order them and return the last item from that group / do the same thing but order that and do a string concat on the character selector
                tbxOutput.Text += $"Lowest String : {(from j in (from i in fileGrouped orderby i.Key ascending select i).Last() orderby j select j).Last() }" +
                    $"/{String.Concat(from ch in ((from j in (from i in fileGrouped orderby i.Key ascending select i).Last() orderby j select j).Last()) orderby ch select ch) } " +
                    $"{Environment.NewLine}";
                //Create a new grouped collection sorted by count of items in groups, return the last groups count, and then select its key
                tbxOutput.Text += $"Biggest Collection Count : {(from i in fileGrouped orderby i.Count() select i).Last().Count()} " +
                    $"- ASCII Sum : {(from i in fileGrouped orderby i.Count() select i).Last().Key}";

            }
        }

        
    }
}
