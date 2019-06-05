using System;

namespace KalkulackaIP
{
    internal class IPSegment
    {
        private int IP;
        private int maskaSite;

        /// <summary>
        /// Konstruktor pro údaje klienta
        /// </summary>
        /// <param name="_ip">IP klienta</param>
        /// <param name="_maska">maska sítě</param>
        public IPSegment(string _ip, string _maska)
        {
            this.IP = ParseIP(_ip);
            this.maskaSite = ParseIP(_maska);
        }

        // mask 0.0.0.255       00000000.00000000.00000000.11111111
        // kl                   255 + 1 = 256
        public int PocetKlientu
        {
            get { return ~maskaSite -2; }
        }

        // IP   10.10.10.10     00001010.00001010.00001010.00001010
        // mask 255.255.255.0   11111111.11111111.11111111.00000000
        // &    10.10.10.0      00001010.00001010.00001010.00000000 
        public int AdresaSite
        {
            get { return IP & maskaSite; }
        }

        // síť  10.10.10.0      00001010.00001010.00001010.00000000
        // mask 0.0.0.255       00000000.00000000.00000000.11111111
        // +    10.10.10.255    00001010.00001010.00001010.11111111
        public int AdresaBroadcastu
        {
            get { return AdresaSite + ~maskaSite; }
        }
        public int PrvniKlient
        {
            get { return IP & maskaSite + 2; }
        }


    


        /// <summary>
        /// Konverze IP (string) na ip (int)
        /// Vytvoření 4 prvkové pole, každé políčko má jednu část IP (oddělené tečkou)
        /// "ip" získáme součtem jednotlivých částí pole, včetně bitového posunu před každým součtem
        /// Bitový posun: 255 << 8 = ? -> 11111111 << 8 = 1111111100000000
        /// </summary>
        /// <param name="ipecko"></param>
        /// <returns></returns>
        private int ParseIP(string ipecko)
        {
            string[] rozdelenaIP = ipecko.Split('.');
            int ip = 0;
            for (int i = 0; i < rozdelenaIP.Length; i++)
            {
                ip = (ip << 8) + Int32.Parse(rozdelenaIP[i]);
            }
            return ip;
        }
    }
}