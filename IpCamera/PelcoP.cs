using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace IpCamera
{
    class PelcoP
    {
        private const byte STX = 0xA0;
        private const byte ETX = 0xAF;

        public byte Adress { get; set; }
        public int CommandLength { get; private set; }
        public PelcoP(byte adress)
        {
            Adress = adress; // adreslar nol dan boshlnadi
            CommandLength = 8;
        }
        public byte[] CamStop()
        {
            byte[] response = new byte[8];
            byte[] data = new byte[7];
            response[0] = STX;
            response[1] = Adress;
            response[2] = 0x00;
            response[3] = 0x00;//  02
            response[4] = 0x00;//0x00; // 20
            response[5] = 0x00;
            response[6] = ETX;
            for (int i = 0; i < 7; i++)
                data[i] = response[i];
            response[7] = CheckSum(data);         
            return response;
        }
        public byte[] CamToLeft()
        {
            byte[] response = new byte[8];
            byte[] data = new byte[7];
            response[0] = STX;
            response[1] = Adress;
            response[2] = 0x00;
            response[3] = 0x02;
            response[4] = 0x40;//0x20;
            response[5] = 0x00;
            response[6] = ETX;
            for (int i = 0; i < 7; i++)
            {
                data[i] = response[i];
            }
            response[7] = CheckSum(data);
            return response;

        }


        public byte[] CamToRight()
        {
            byte[] response = new byte[8];
            byte[] data = new byte[7];
            response[0] = STX;
            response[1] = Adress;
            response[2] = 0x00;
            response[3] = 0x04;
            response[4] = 0x3F;
            response[5] = 0x00;
            response[6] = ETX;
            for (int i = 0; i < 7; i++)
            {
                data[i] = response[i];
            }
            response[7] = CheckSum(data);
            return response;

        }
        public byte[] CamToDown()
        {
            byte[] response = new byte[8];
            byte[] data = new byte[7];
            response[0] = STX;
            response[1] = Adress;
            response[2] = 0x00;
            response[3] = 0x08;
            response[4] = 0x00;
            response[5] = 0x3F;//0x20;
            response[6] = ETX;
            for (int i = 0; i < 7; i++)
            {
                data[i] = response[i];
            }
            response[7] = CheckSum(data);
            return response;

        }
        public byte[] CamToUp()
        {
            byte[] response = new byte[8];
            byte[] data = new byte[7];
            response[0] = STX;
            response[1] = Adress;
            response[2] = 0x00;
            response[3] = 0x10;
            response[4] = 0x00;
            response[5] = 0x3F;//0x20;
            response[6] = ETX;
            for (int i = 0; i < 7; i++)
            {
                data[i] = response[i];
            }
            response[7] = CheckSum(data);
            return response;

        }

        public byte CheckSum(byte[] Data)
        {
            return (byte)(Data[0] ^ Data[1] ^ Data[2] ^ Data[3] ^ Data[4] ^ Data[5] ^ Data[6]);
        }

    }
}
