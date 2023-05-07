using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace BinConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button_Run_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() != DialogResult.OK) return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = ofd.FileName + ".c";



            //if (sfd.ShowDialog() != DialogResult.OK) return;

            string filepass = ofd.FileName;

            FileStream fs = new FileStream(filepass,
                FileMode.Open,
                FileAccess.Read,
                FileShare.None);

            //バイナリ形式でプログラム内に読み込む
            BinaryReader br = new BinaryReader(fs);

            byte[] data;    //データを格納する変数

            //ファイルサイズを求める
            FileInfo fi = new FileInfo(filepass);
            long filesize = fi.Length;

            //配列の長さをファイルサイズにして定義

            data = new byte[filesize];

            //１バイトずつ取得しながら１６進数で表示

            List<string> Lines = new List<string>();

            int cnt = 1;

            for (long i = 1; i < filesize; i++)
            {
                data[i] = br.ReadByte();
                //Console.Write("{0,2:x} ", data[i]);
                //25個表示したら改行
                if (cnt == 25)
                {
                    Lines.Add(string.Format("0x{0,0:x2}\r\n", data[i]));
                    cnt = 0;
                }
                else
                {
                    Lines.Add(string.Format("0x{0,0:x2}", data[i]));



                }
                cnt++;
            }

            br.Close();
            br.Dispose();

            Bitmap img = new Bitmap(ofd.FileName);

            textBox1.Text =
                "/// @brief ImageSize = "
                + img.Width.ToString() +","+img.Height.ToString()+"\r\n"
                + "unsigned char "
                + Path.GetFileNameWithoutExtension(ofd.FileName)
                +"[" + filesize.ToString() + "]=\r\n{"
                +string.Join(",", Lines)
                + "\r\n};"
                ;

            img.Dispose();

            textBox2.Text += "extern unsigned char "
                + Path.GetFileNameWithoutExtension(ofd.FileName)
                + "[" + filesize.ToString() + "];\r\n";

            Clipboard.SetText(textBox1.Text);

        }
    }
}
