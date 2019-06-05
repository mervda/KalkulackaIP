using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace KalkulackaIP
{
    public partial class Form1 : Form
    {
        IPSegment ip;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Defaultní hodnoty textových polí po spuštění programu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            tbIP.Text = "10.10.10.1";
            tbMaska.Text = "255.255.255.0";
            tbMaskaB.Text = "24";

            
            
        }

        /// <summary>
        /// Metoda spouštějící testy a následné výpočty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {



            string[] rozdelenaIP = tbIP.Text.Split('.');
            string[] rozdelenaMaska = tbMaska.Text.Split('.');

            if (rozdelenaIP.Length == 4 && rozdelenaMaska.Length == 4 &&
                jeIP(tbIP.Text) == true && jeIP(tbMaska.Text) == true)
            {
                ip = new IPSegment(tbIP.Text, tbMaska.Text);


                rtbVysledek.Text = "Adresa sítě: " + IntToIP(ip.AdresaSite) + "\n";
                rtbVysledek.Text += "Adresa broadcastu: " + IntToIP(ip.AdresaBroadcastu) + "\n";
                rtbVysledek.Text += "Počet možných klientů: " + ip.PocetKlientu + "\n";
                rtbVysledek.Text += "Adresy prvního a posledního možného klienta " +  "\n" + IntToIP(ip.AdresaSite+2) + " - " + IntToIP(ip.AdresaBroadcastu-1);
            }
            else
            {
                MessageBox.Show("Nezadal jsi správný formát IP.");
            }            
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            int pom =  Int32.Parse(tbMaskaB.Text);
            if (!(pom<=30) || pom <1) {
                MessageBox.Show("Nezadal jsi správný formát masky.");

            }
           
            
           tbMaska.Text = CIDR2DECIMAL(pom);           

            string[] rozdelenaIP = tbIP.Text.Split('.');
            string[] rozdelenaMaska = tbMaska.Text.Split('.');

            if (rozdelenaIP.Length == 4 && rozdelenaMaska.Length == 4 &&
                jeIP(tbIP.Text) == true && jeIP(tbMaska.Text) == true)
            {
                ip = new IPSegment(tbIP.Text, tbMaska.Text);


               
                rtbVysledek.Text = "Adresa sítě: " + IntToIP(ip.AdresaSite) + "\n";
                rtbVysledek.Text += "Adresa broadcastu: " + IntToIP(ip.AdresaBroadcastu) + "\n";
                rtbVysledek.Text += "Počet možných klientů: " + ip.PocetKlientu + "\n";
                rtbVysledek.Text += "Adresy prvního a posledního možného klienta " + "\n" + IntToIP(ip.AdresaSite + 2) + " - " + IntToIP(ip.AdresaBroadcastu - 1);
            }
            else
            {
                MessageBox.Show("Nezadal jsi správný formát IP.");
            }

        }


        /// <summary>
        /// Test tvaru IP - jestli jsou čísla v rozsahu 0-255 a jaká verze IP je zadaná
        /// </summary>
        /// <param name="text">IP</param>
        /// <returns></returns>
        private bool jeIP(string text)
        {
            IPAddress adresa;
            if (IPAddress.TryParse(text, out adresa))
            {
                if(adresa.AddressFamily == AddressFamily.InterNetwork)
                {
                    return true;
                }
            }
            return false;
        }

        public string CIDR2DECIMAL(int cidr)
        {

            string[] decim = new string[4];
            int pomocna = cidr;     
            for (int i = 0; i < 4; i++)
            {
                if (cidr > 8)
                {

                    decim[i] = "255";
                    cidr -= 8;

                }
                else
                {

                    int temp = 0;
                    for (int a = 7; cidr > 0; a--, cidr--)
                    {
                        temp += (int)Math.Pow(2, a);
                    }
                   
                    decim[i] = temp.ToString();
                }

            }
                string MaskaA = string.Join(".", decim);
                     return MaskaA;
        }


        private string IntToIP(int adresa)
        {
            uint bitovaMaska = 0xff000000;
            string[] castiIP = new string[4];
            for (int i = 0; i < castiIP.Length; i++)
            {
                // 1. KROK
                // adresa   00001010 00001010 00001010 00000000
                // bitovaM  11111111 00000000 00000000 00000000
                // &        00001010 00000000 00000000 00000000
                // cIP[0]   00000000 00000000 00000000 00001010
                // cIP[0]                                    10
                // 2. KROK
                // adresa   00001010 00001010 00001010 00000000
                // bitovaM  00000000 11111111 00000000 00000000
                // &        00000000 00001010 00000000 00000000
                // cIP[1]   00000000 00000000 00000000 00001010
                // cIP[1]                                    10
                // ...
                long vysledek = (adresa & bitovaMaska) >> ((3 - i) * 8);
                bitovaMaska >>= 8;
                castiIP[i] = vysledek.ToString();
            }
            return String.Join(".", castiIP);
        }



    }
}

