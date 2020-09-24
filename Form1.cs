using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using EVE.ISXEVE;
using LavishScriptAPI;
using LavishVMAPI;

namespace melt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static List<int> toMove = new List<int>();


        private void button1_Click(object sender, EventArgs e)
        {
            string import = Clipboard.GetText();
            string[] lines = Regex.Split(import, "\r\n|\r|\n");
            bool r = false;
            decimal totalvalue = 0;
            foreach (string line in lines)
            {
                if (line == "EVE Online and the EVE logo are the registered trademarks of CCP hf. All rights are reserved worldwide. All other trademarks are the property of their respective owners. EVE Online, the EVE logo, EVE and all associated logos and designs are the intellectual property of CCP hf. All artwork, screenshots, characters, vehicles, storylines, world facts or other recognizable features of the intellectual property relating to these trademarks are likewise the intellectual property of CCP hf. CCP hf. has granted permission to fuzzwork.co.uk to use EVE Online and all associated logos and designs for promotional and information purposes on its website but does not endorse, and is not in any way affiliated with, fuzzwork.co.uk. CCP is in no way responsible for the content on or functioning of this website, nor can it be liable for any damage arising from the use of this website.")
                {
                    break;
                }
                if (r)
                {
                    string[] sp = line.Split('\t');
                    if (Decimal.Parse(sp[5]) > 0)
                    {
                        listBox1.Items.Add(sp[0] + "    " + sp[1] + "    " + sp[5]);
                        totalvalue += Decimal.Parse(sp[5]);
                        toMove.Add(int.Parse(sp[0]));
                    }
                }
                if (line == "id	Name	Quantity	Melt value (each)	Sale price (each)	Difference")
                {
                    r = true;
                }
            }
            label1.Text = "Items above are worth " + Decimal.Round(totalvalue/1000000, 1) + "m more if melted down";
                
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            using (new FrameLock(true))
            {
                EVE.ISXEVE.Me me = new EVE.ISXEVE.Me();
                EVE.ISXEVE.MyShip myship = new EVE.ISXEVE.MyShip();
                foreach (Item i in me.GetHangarItems())
                {
                    if (toMove.Contains(i.TypeID))
                    {
                        i.MoveTo(myship.ID, "CargoHold");
                        return;
                    }
                }
                timer1.Stop();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.fuzzwork.co.uk/evaluator/");
        }
    }
}
