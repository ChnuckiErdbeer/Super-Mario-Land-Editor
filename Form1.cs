using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace SMLEdit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        public Form1(RomHeader romHeader)
        {
            this.romHeader = romHeader;
            InitializeComponent();
        }

        public RomHeader romHeader = new RomHeader();
        public Level[] levels = new Level[32];
        public int lvlInFocus = 26;


        //Functions:

        private int calcRomAdr(int level, int address)             
        {
            return 0x4000 * (level + 5) + address - 0x4000;
        }


        //Resizes a bitmap with nearest neighbour interpolation mode. (Thanks to Richard Knop and Michael for this code: http://stackoverflow.com/questions/4120339/resize-a-bitmap-like-ms-paint-no-antialiasing)
        private Bitmap ResizeBitmap(Bitmap b, int nWidth, int nHeight)              
        {
           
            Bitmap result = new Bitmap(nWidth, nHeight);
            using (Graphics g = Graphics.FromImage((Image)result))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(b, 0, 0, nWidth, nHeight);
            }
            return result;
        }






        private void openSMLROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String filename = string.Empty;
            

            OpenFileDialog ofdialog = new OpenFileDialog();

            ofdialog.Filter = "Prepared SML-ROM (*.gb)|*.gb|All files (*.*) |*.*";
            ofdialog.InitialDirectory = "D:\\In Arbeit\\SML\\rom backup";
            ofdialog.Title = "Yea und Yea!";


            if (ofdialog.ShowDialog() == DialogResult.OK)
                filename = ofdialog.FileName;

            //Open the stream and read it back.
            try
            {
                using (FileStream fs = new FileStream (filename,FileMode.Open, FileAccess.Read))
                {
                    //Read the supposedly prepared ROM-file into a byte array:

                    bool romCorrect = true;                         // To check for problems.

                    if (fs.Length != 1048611)                       // Check if ROM has the right size.
                    {
                        romCorrect = false;
                        MessageBox.Show("Sorry. The ROM's filesize has to be exactly 1MB. Did you pick a rom prepared for SMLED? If not try \"file->import ROM\" to prepare and open your ROM.");
                    }

                    
                    
                    byte[] rom = new byte[fs.Length];               //Create a byte array to contain the rom.
                    int spareBytes = (int)fs.Length;                //Number of byes yet to be read.
                    int readBytes = 0;                              //Number of bytes read.


                    // Byte-step through the romfile and copy each byte to the byte-array "rom".

                    while (spareBytes > 0)
                    {
                        int n = fs.Read(rom, readBytes, spareBytes);

                        if (n == 0)
                            break;

                        readBytes += n;
                        spareBytes -= n;
                        
                    }
                    

                    // The ROM is now in memory! Lets fill the romHeader-object:


                    if (romCorrect) romCorrect = romHeader.set_entryPoint(rom);             //Set entry-point.
                    if (romCorrect) romCorrect = romHeader.set_title(rom);                  //Set title.
                    if (romCorrect) romCorrect = romHeader.set_cardridgeType(rom);          //Set cardridge type.
                    if (romCorrect) romCorrect = romHeader.set_romSize(rom);                //Set rom size
                    if (romCorrect) romCorrect = romHeader.set_ramSize(rom);                //Set ram size
                    if (romCorrect) romCorrect = romHeader.set_destinationCode(rom);        //Set destination code
                    if (romCorrect) romCorrect = romHeader.set_oldLicenseeCode(rom);        //Set old licensee code
                    if (romCorrect)              romHeader.set_romVersionNuber(rom[0x14C]); //Set rom version number (Blank space intended for readability.)
                    if (romCorrect) romCorrect = romHeader.set_headerChecksum(rom);         //Set header checksum
                    if (romCorrect)              romHeader.set_globalChecksum(rom);         //Set global checksum
                   

                    // Set level objects:    
                    
                    for (int curLevNum = 0; curLevNum < 27; curLevNum++)                    //do this for all 27 possible levels.
                    {
                        Level cur_level = new Level();                                      //Create a new temp-level to fill it and add it to the list.

                        // Set variables from the level header:

                        if (romCorrect) romCorrect = cur_level.set_type      (rom[calcRomAdr(curLevNum, 0x4040)]);   //Set Level-Type (located at 0x4040 in current bank).
                        if (romCorrect) romCorrect = cur_level.set_music     (rom[calcRomAdr(curLevNum, 0x4041)]);   //Set Level-Music.
                        if (romCorrect) romCorrect = cur_level.set_musicBonus(rom[calcRomAdr(curLevNum, 0x4042)]);   //Set Coinroom-Music.
                        if (romCorrect) romCorrect = cur_level.set_countdown (rom[calcRomAdr(curLevNum, 0x4043)]);   //Set Level-Music.

                        // Set more complex level values:

                        if (romCorrect)              cur_level.set_bonusScreenList(rom, curLevNum);                  //Set levels BonusScreenList.
                        if (romCorrect)              cur_level.set_screenOrderList(rom, curLevNum);                  //Set levels ScreenOrderList.
                        if (romCorrect)              cur_level.set_checkpointList (rom, curLevNum);                  //Set levels CheckpointList.
                        if (romCorrect)              cur_level.set_bonuspipeList  (rom, curLevNum);                  //Set levels BonusPipeList.
                        if (romCorrect)              cur_level.set_entityList     (rom, curLevNum);                  //Set levels EntityList.
                        if (romCorrect)              cur_level.set_itemList       (rom, curLevNum);                  //Set levels ItemList.
                        if (romCorrect)              cur_level.set_tileMatrix     (rom, curLevNum);                  //Set levels ItemList.
                        if (romCorrect)              cur_level.set_TilePalette    (rom, curLevNum);                  //Set levels TilePalette.






                       
                       

                        // FEHLT: Fill the remaining screens with 0xFF!!

                        levels[curLevNum] = cur_level;
                    }
                }

                MessageBox.Show("SPIELWIESE:");

                /// Spielwiese >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><
                
                Bitmap citrus = new Bitmap("d:\\citrus.bmp");
                Point pnt = new Point(100, 100);
                Size tile_on_screen_size = new Size(17,17);
                Size screen_on_screen_size = new Size(340, 272);






                // ENDE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><
                MessageBox.Show("ENDE SPIELWIESE");
                // ENDE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><

                PictureBox[] tilepal = new PictureBox[256];

                for (ushort i = 0; i < 256; i++)
                {
                    System.Windows.Forms.PictureBox testbox = new System.Windows.Forms.PictureBox();

                    testbox.Size = tile_on_screen_size;
                    testbox.Margin = new Padding (1);
                    testbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    testbox.BackgroundImage = levels[lvlInFocus].tilePal.getTile(i);

                    tilepal[i] = testbox;
                    
                }
                TilePaletteDisplay.Controls.AddRange(tilepal);                                                //



                int numOfScreens = 32;
                

                PictureBox[] levelLayout = new PictureBox[numOfScreens];

                levels[lvlInFocus].tileMatrix.update_all_screens(levels[lvlInFocus].tilePal);

                for (int i = 0; i < numOfScreens; i++)                                                  //Put each screen into a single picture box.
                {
                    System.Windows.Forms.PictureBox tmpPicBox = new System.Windows.Forms.PictureBox();  //Create temporary picture box.

                    tmpPicBox.Size = screen_on_screen_size;                                               //Configure the picture box.
                    tmpPicBox.Margin = new Padding(1);


                    Bitmap tmpScreen = levels[lvlInFocus].tileMatrix.screens[i];

                    tmpPicBox.BackgroundImage = tmpScreen;

                    levelLayout[i] = tmpPicBox;
                  
                    
                    
                }
               
                LevelDisplay.Controls.AddRange(levelLayout);
        

                

           

              

                
            }   
            
            catch (FileNotFoundException ioEx)
            {
                Console.WriteLine(ioEx.Message);
            }
        }

       


        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            
        }
     

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvlInFocus = Convert.ToInt16(comboBox1.Text);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void TileMatrix_Paint(object sender, PaintEventArgs e)
        {              
            
        }

        private void testpanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap citrus = new Bitmap("d:\\citrus.bmp");
            Size tile_on_screen_size = new Size(16, 16);

 
            System.Windows.Forms.PictureBox testbox = new System.Windows.Forms.PictureBox();

            testbox.Size = tile_on_screen_size;
            testbox.Margin = new Padding(1);
            testbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            testbox.Image = ResizeBitmap(citrus, 16, 16);


            TilePaletteDisplay.Controls[41].BackgroundImage = citrus;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

   
    }
}
