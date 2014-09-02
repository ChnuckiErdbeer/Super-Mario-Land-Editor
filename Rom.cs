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
    /// <summary>
    /// Structure containing all parameters needed for one working bonus-pipe.
    /// </summary>
    public struct bonusPipe                                 
    {
        public byte entryScreen;    // The screen of the level, containing the pipe Mario will be able to enter.            
        public byte xPos;           // The number of the tile-column containing the top-left tile of a pipe (tile 0x70). If the tile is not there Mario will not be able to enter the pipe!!            
        public byte targetScreen;   // The screen from the bonusscreen-list the pipe will lead to. This screen will not scroll.
        public byte returnScreen;   // The screen Mario ends up when leaving the bonusscreen. In the original game pipes lead always back to the entryscreen, but other screens are absolutely possible. Remember though that if the screen lies before the entryscreen Mario will be able to enter the pipe again.
        public byte returnXPos;     // Pixel-accurate horizontal position Mario appears when returning to the returnscreen.
        public byte returnYPos;     // Pixel-accurate vertical position Mario appears when returning to the returnscreen.
    }


    /// <summary>
    /// Structure containing all parameters needed for one working entity. 
    /// (Such as monsters, bullets and some other animated sprites.)
    /// </summary>
    public struct entity                                    
    {
        public int xPos;            //Pixel-accurate horizontal position in the whole level(!!!), where the entity appears. Note, that the game is not pixel accurate but rather rasters the level in 4 pixel steps.
        public byte yPos;           //Tile-accurate vertical position.
        public byte type;           //Type of entity. See readme.txt for possible values.
        public bool hardmode;       //If true entity will only be loaded if game runs in hardmode.
    }

    /// <summary>
    /// Structure containing all parameters needed for one working item.
    /// </summary>
    public struct item                                      
    {
        public byte screenNr;       //The number of the screen the item appers in.
        public byte xPos;           //Tile-accurate horizontal position of the item. The tilecolumn has to have either a ?-block(0x81), a destructable block (0x80) or an invisible block(0xDF) for the item to appear in. 
        public byte type;           //The type of the item: 0x28 = Mushroom, 0x2A = 1up, 0x2C = Star, 0xC0 = Multible-Coin-Box, 0x70 Elevator, 0xF0 Apearing Stoneblock.
    }


    /// <summary>
    /// Object containing all parameters obtainable from the ROM's header.
    /// </summary>
    /// 
    public class RomHeader
    {
        public RomHeader() { }                //constructor 

 
        //ENTRYPOINT:

        /// <summary>
        /// The entrypoint is the adress the GB will jump to to start the main routine. In case of SML it
        /// should be 00 C3 50 01 (Jump to adress 0x00000150). Probably of not much use for the editor exept for bad rom checking.
        /// </summary>
        /// 
        private ushort entryPoint;

        /// <summary>
        /// Sets the entrypoint to an ushort value.
        /// </summary>
        /// <param name="x">The value to set the entrypoint to.</param>
        /// <returns>true if x is the standard value in SML (0x0150)</returns>
        /// 
        public bool set_entryPoint(ushort x)        //Set entrypoint to an ushort value.
        { 
            this.entryPoint = x;
            if (x != 5001) {MessageBox.Show("ERROR! Entrypoint not 0x5001."); return false;}      //Error if entrypoint value is not the expected one.
            return true;
        }       
  
        /// <summary>
        /// Sets the entrypoint to the value at bytes 0x102 and 0x103 in an forwarded byte array containing 
        /// a whole patched SML-ROM.
        /// </summary>
        /// <param name="rom">This byte array should contain a patched SML-ROM.</param>
        /// <returns>true if x is the standard value in SML (0x0150)</returns>
    
        public bool set_entryPoint (byte[] rom)     //Set entrypoint to the according value in a rom read into an byte array. 
        {
             int x = rom[0X102];
             x = x << 8;
             x += rom[0x103];                       //0x103 is the adress of the entrypoint in the rom.
             this.entryPoint = (ushort)x;
             
            if (x != 0x5001) {MessageBox.Show("ERROR! Entrypoint not 0x5001."); return false;}       //Error if entrypoint value is not the expected one.
             return true;
        }

        /// <summary>
        /// Returns the current value of entryPoint.
        /// </summary>
        /// <returns>The value of entryPoint</returns>
        /// 
        public ushort get_entryPoint() { return this.entryPoint; }


        //TITLE:

        /// <summary>
        /// The title of the Rom. This is a 19 character string. The first six characters have to be 
        /// "SMLED " or the Rom will be denied.
        /// </summary>
        private String title;

        /// <summary>
        /// Sets title to the value at bytes 0x134 to 0x144 in an forwarded byte array containing 
        /// a whole patched SML-ROM.
        /// </summary>
        /// <param name="x">This byte array should contain a patched SML-ROM.</param>
        /// <returns>true if the title starts with "SMLED ".</returns>
        public bool set_title(byte[] rom) 
        { 
            

            StringBuilder titlestring = new StringBuilder("", 16);

            for (int i = 0; i < 6; i++)
            {
                titlestring.Append((char)rom[0x134 + i]);
            }

            if (titlestring.ToString() != "SMLED ")
            {
                MessageBox.Show("ERROR! ROM is not prepared for SMLED. Rom-Title has to start with \"SMLED \".");
                return false;
            }


            for (int i = 5; i < 16; i++)
            {
                titlestring.Append((char)rom[0x134 + i]);
            }

            this.title = titlestring.ToString();
            return true;
        }                    

        /// <summary>
        /// Returns the current value of title.
        /// </summary>
        /// <returns>Returns the value of title.</returns>
        /// 
        public String get_title() { return this.title; }


        // CARTRIDGE TYPE:

        /// <summary>
        /// The type of cartridge the rom is based on. Should be 0x19 (MBC5) for patched SML-ROM. 
        /// </summary>
        private byte cardridgeType;

        /// <summary>
        /// Sets cardridgeType to the value at byte 0x147 in an forwarded byte array containing 
        /// a whole patched SML-ROM.
        /// </summary>
        /// <param name="rom">This byte array should contain a patched SML-ROM.</param>
        /// <returns>true if cardridge type is 0x19 (MBC5).</returns>
        
        public bool set_cardridgeType(byte[] rom) 
        { 
            if (rom[0x147] != 19)
            {
                MessageBox.Show("ERROR! ROM is not prepared for SMLED. Cartridge type must be MBC5.");
                return false;
            }
            else this.cardridgeType = rom[0x147];        // set cartridge type
            return true;
        }        

        /// <summary>
        /// Returns the current value of cardridgeType.
        /// </summary>
        /// <returns>Returns the value of cardridgeType.</returns>
        public byte get_cardridgeType() {return this.cardridgeType;}


        //ROM SIZE:

        /// <summary>
        /// Contains the size the rom thinks it has. If it is set to another value than 5 (indicating an 1MB
        /// ROM) an error occures. Another one occures if this value ist read from a ROM that is not exactly 1MB in size. 
        /// </summary>
        /// 
        private byte romSize;

        /// <summary>
        /// Sets title to the value at byte 0x148 in an forwarded byte array containing 
        /// a whole patched SML-ROM.
        /// </summary>
        /// <param name="rom">This byte array should contain a patched SML-ROM.</param>
        /// <returns>true if both size of the ROM and the length of the passed array indicate an 1MB ROM.</returns>
        public bool set_romSize(byte[] rom) 
        {
            if (rom[0x148] != 5)      
            {
                MessageBox.Show("ERROR! ROM size given in header must be 1MB. \n(Value at 0x148 must be 5.");
                return false;
            }

            if (rom.Length != 1048611)
            {
                MessageBox.Show("ERROR! ROM must be exatly 1MB.");
                return false;
            }

            this.romSize = rom[0x148];
            return true;
        }

        /// <summary>
        /// Returns the current value of romSize.
        /// </summary>
        /// <returns>Returns the value of romSize.</returns>
        /// 
        public byte get_romSize() { return this.romSize; }


        // RAM SIZE:

        /// <summary>
        /// Contains the size of the RAM the game can access. For a SML-ROM this should be 0.
        /// </summary>
        ///    
        private byte ramSize;

        /// <summary>
        /// Sets ramSize to the value at byte 0x149 in an forwarded byte array containing 
        /// a whole patched SML-ROM.
        /// </summary>
        /// <param name="rom">This byte array should contain a patched SML-ROM.</param>
        /// <returns>true if the value found in rom is 0 as ist should be in a SML-ROM.</returns>
        /// 
        public bool set_ramSize(byte[] rom) 
        {
            if (rom[0x149] != 0)
            {
                MessageBox.Show("ERROR! RAM-size is not 0.");
                return false;
            }
            this.ramSize = rom[0x149];
            return true;
        }

        /// <summary>
        /// Returns the current value of ramSize.
        /// </summary>
        /// <returns>Returns the value of ramSize.</returns>
        /// 
        public byte get_ramSize() { return this.ramSize; }


        // DESTINATION CODE:

        /// <summary>
        /// Contains the destination code for the ROM. Can be 00 for Japanese or 01 for non Japanese. 
        /// I feel generous so I'll allow both values.
        /// </summary>
        /// 
        private byte destinationCode;
        
        /// <summary>
        /// Sets destinationCode to the value at byte 0x14A in an forwarded byte array containing 
        /// a whole patched SML-ROM.
        /// </summary>
        /// <param name="rom">This byte array should contain a patched SML-ROM.</param>
        /// <returns>true if the value found in rom is 0 or 1 as ist should be in a SML-ROM.</returns>
        /// 
        public bool set_destinationCode(byte[] rom) 
        {
            if ((rom[0x14A] & 0xFC) != 0)
            {
                MessageBox.Show("ERROR! Uncnown destination code.");
                return false;
            }
            this.destinationCode = rom[0x14A];
            return true;
        }

        /// <summary>
        /// Returns the current value of destinationCode.
        /// </summary>
        /// <returns>Returns the value of destinationCode.</returns>
        /// 
        public byte get_destinationCode() { return this.destinationCode; }


        // OLD LICENSEE CODE:

        /// <summary>
        /// Contains the old licensee code for the ROM. Should be 01.
        /// </summary>
        /// 
        private byte oldLicenseeCode;

        /// <summary>
        /// Sets oldLicenseeCode to the value at byte 0x14B in an forwarded byte array containing 
        /// a whole patched SML-ROM.
        /// </summary>
        /// <param name="rom">This byte array should contain a patched SML-ROM.</param>
        /// <returns>true if the value found in rom is 1 as ist should be in a SML-ROM.</returns>
        /// 
        public bool set_oldLicenseeCode(byte[] rom) 
        { 
            if (rom[0x14B] != 1)
            {
                MessageBox.Show("WARNING! Wrong licensee code in ROM.(Should be 1.)");
            }
            this.oldLicenseeCode = rom[0x14B]; 
            return true;
        }  
        public byte get_oldLicenseeCode() { return this.oldLicenseeCode; }


        // ROM VERSION NUMBER:

        /// <summary>
        /// Contains the ROM version number for the ROM.
        /// </summary>
        /// 
        private byte romVersionNuber;

        /// <summary>
        /// Sets romVersionNumber to a passed byte value.
        /// </summary>
        /// <param name="x">Value to set romVersionNumber to.</param>
        /// 
        public void set_romVersionNuber(byte x) { this.romVersionNuber = x; }

        /// <summary>
        /// Returns the current value of romVersionNumber.
        /// </summary>
        /// <returns>Returns the value of romVersionNumber.</returns>
        /// 
        public byte get_romVersionNuber() { return this.romVersionNuber; }

        
        // HEADER CHECKSUM:

        /// <summary>
        /// Contains the header checksum for the ROM. The checksum is calculated via substracting all bytes
        /// from 0x134 to 0x14C of the rom from an variable initlized as 0 and additionally substracting 1 for 
        /// each of those bytes.
        /// </summary>
        /// 
        private byte headerChecksum;

        /// <summary>
        /// Sets headerChecksum to the value at byte 0x14D in an forwarded byte array containing 
        /// a whole patched SML-ROM.
        /// </summary>
        /// <param name="rom">This byte array should contain a patched SML-ROM.</param>
        /// <returns>true if the value found in rom matches the actual header-checksum.</returns>
        /// 
        public bool set_headerChecksum(byte[] rom) 
        {
            int realChecksum = 0;                                       //Generate the actual header-checksum from the passed ROM.
            for (int i = 0x0134; i < 0x014D; i++)
            {
                realChecksum = (realChecksum - rom[i] - 1);
            }
            realChecksum = realChecksum & 0xFF;
            if (realChecksum != ( rom[0x014D]))                         //If generated checksum differs from the one in the ROM error out.
            {
                MessageBox.Show("Warning! Wrong header-checksum.");
                return false;
            }

            this.headerChecksum = rom[0x14D];
            return true;
        }

        /// <summary>
        /// Sets headerChecksum to a checksum geneated from a byte array containing 
        /// a whole patched SML-ROM.
        /// </summary>
        /// <param name="rom">This byte array should contain a patched SML-ROM.</param>
        public void update_headerChecksum(int[] rom)
        {
            int realChecksum = 0;                                       //Generate the actual header-checksum from the passed ROM.
            for (int i = 0x0134; i < 0x014D; i++)
            {
                realChecksum = (realChecksum - rom[i] - 1);
            }
            this.headerChecksum = (byte) (realChecksum & 0xFF);
        }

        /// <summary>
        /// Returns the current value of headerChecksum.
        /// </summary>
        /// <returns>Returns the value of headerChecksum.</returns>
        /// 
        public byte get_headerChecksum() { return this.headerChecksum; }

        
        // GLOBAL CHECKSUM:

        /// <summary>
        /// Contains the global checksum for the ROM. The checksum is calculated by adding all bytes of the rom exept for
        /// the two containing the checksum itself into a 16bit number.
        /// </summary>
        /// 
        private ushort globalChecksum;

        /// <summary>
        /// Sets globalChecksum to the value at bytes 0x14E and 0x14F in an forwarded byte array containing 
        /// a whole patched SML-ROM.
        /// </summary>
        /// <param name="rom">This byte array should contain a patched SML-ROM.</param>
        /// 
        public void set_globalChecksum(byte[] rom) 
        {
            int checksumInRom = rom[0X14E];                             //Read the checksum contained in rom.
            checksumInRom = checksumInRom << 8;
            checksumInRom += rom[0x14F];

            ushort actualChecksum = 0;                                  //Calculate the actual checksum by adding all bytes of rom.

            for (int i = 0; i < rom.Length; i++)                
            {
                actualChecksum = (ushort)(actualChecksum + rom[i]);
            }
            actualChecksum = (ushort) (actualChecksum - rom[0x14E] - rom[0x14F]);   //Substract the two bytes containing the checksum.

            if (actualChecksum != checksumInRom)
            {
                MessageBox.Show("WARNING! Global checksum is wrong.");
            }
            this.globalChecksum = (ushort)checksumInRom;
        }

        /// <summary>
        /// Sets globalChecksum to a checksum geneated from a byte array containing 
        /// a whole patched SML-ROM.
        /// </summary>
        /// <param name="rom">This byte array should contain a patched SML-ROM.</param>
        public void update_globalChecksum(byte[] rom)
        {
            ushort actualChecksum = 0;                                  //Calculate the actual checksum by adding all bytes of rom.

            for (int i = 0; i < rom.Length; i++)
            {
                actualChecksum = (ushort)(actualChecksum + rom[i]);
            }
            actualChecksum = (ushort)(actualChecksum - rom[0x14E] - rom[0x14F]);   //Substract the two bytes containing the checksum.

            this.globalChecksum = actualChecksum; 
        }

        /// <summary>
        /// Returns the current value of globalChecksum.
        /// </summary>
        /// <returns>Returns the value of globalChecksum.</returns>
        /// 
        public ushort get_globalChecksum() { return this.globalChecksum; }
    }

    // LEVEL:

    /// <summary>
    /// Contains all the info needed to define a SML-level.
    /// </summary>
    /// 
    public class Level
    {
        public Level() { }          //Constructor.

        //Variables and setters/getters from level-header:

        /// <summary>
        /// Level header entry "type". 
        /// 
        ///     0x00: normal level 
        ///     0x0A: Marine-Pop 
        ///     0x0B: Sky-Pop
        ///     
        /// </summary>
        /// 
        private byte type;

        /// <summary>
        /// Sets the type of the level.
        /// </summary>
        /// <param name="x">One of three possible values: 0x0, 0xA or 0xB.</param>
        /// <returns>True if one of the three possible values was passed.</returns>
        /// 
        public bool set_type(byte x)                            
        { 
            if ((x != 0) && (x != 0xA) && (x != 0xB)) 
            {
                MessageBox.Show("ERROR! Level type not 00, 0A or 0B!");
                return false;
            }

            this.type = x;
            return true;
        }

        /// <summary>
        /// Returns the type of the current level.
        /// </summary>
        /// <returns>Type of the current level.</returns>
        public byte get_type() { return this.type; }

        /// <summary>
        /// 1 byte value containing the music to be played in the level background.
        /// Possible values are 0x00 - 0x13. (See readme.txt for corresponding song-titles.)
        /// </summary>
        /// 
        private byte music;   
        
        /// <summary>
        /// Sets the music that plays in the level.
        /// </summary>
        /// <param name="x">A byte value referencing the song. Possible values range from 0x00 to 0x13.</param>
        /// <returns>True if x is not smaller than 0x13.</returns>
        ///                     
        public bool set_music(byte x) 
        {
            if (x > 0x13)
            {
                MessageBox.Show("ERROR! Level-music has to be between 0x00 and 0x13.");
                return false; 
            }
            this.music = x;
            return true;
        }

        /// <summary>
        /// Returns the number of the song to be played in the level.
        /// </summary>
        /// <returns>The number of the song to be player in the level.</returns>
        /// 
        public byte get_music() { return this.music; }


        /// <summary>
        /// 1 byte value containing the music to be played in bonus rooms.
        /// Possible values are 0x00 - 0x13. (See readme.txt for corresponding song-titles.)
        /// </summary>
        /// 
        private byte musicBonus;    
                                
        public bool set_musicBonus(byte x) 
        {
            if (x > 0x13)
            {
                MessageBox.Show("ERROR! Bonusroom-music has to be between 0x00 and 0x13.");
                return false;
            }
            this.musicBonus = x; 
            return true;
       }

        /// <summary>
        /// Returns the number of the song to be played in the level's bonus rooms.
        /// </summary>
        /// <returns>The number of the song to be played in the level's bonus rooms.</returns>
        /// 
        public byte get_musicBonus() { return this.musicBonus; }


        /// <summary>
        /// Byte value containing the initial value of the countdown timer. 
        /// 
        ///     Minimum value: 01 = 100 "seconds" 
        ///     Maximum value: 09 = 900 "seconds" 
        ///     
        ///     (Values up to 0F are possible, but garble up the countdown graphic. 
        ///     Homework: test what happens if 00. Is it possible to deactivate the timer this way?
        ///     
        /// </summary>
        /// 
        private byte countdown;                                     
        
        
        /// <summary>
        /// Set the initial value of the countdown timer.
        /// </summary>
        /// <param name="x">The value to set the timer to.</param>
        /// <returns>True if value is smaller than 0x0F</returns>
        public bool set_countdown(byte x) 
        {
            if (x > 0x0F)
            {
                MessageBox.Show("ERROR! Countdown value bigger than 0x0F.");
                return false;
            }

            if (x > 0x09) MessageBox.Show("WARNING! Countdown value bigger than 0x09. (And smaller than 0x0F.) \nWorks but wil garble up the countdown graphic.");
            
            this.countdown = x;
            return true;
        }

        /// <summary>
        /// Returns the current initial value of the countdown timer.
        /// </summary>
        /// <returns>The current initial value of the countdown timer.</returns>
        public byte get_countdown() { return this.countdown; }




        /// <summary>
        /// An array containing all 16Bit adresses to possible bonus screens.
        /// </summary>
        /// 
        private ushort[] bonusScreenList;

        /// <summary>
        /// Sets the bonusScreenList to the corresponding values in an  byte array containing 
        /// a whole patched SML-ROM.
        /// </summary>
        /// <param name="rom">This byte array should contain a patched SML-ROM.</param>
        /// <param name="level">The number of the level to obtain the screenorder from.</param>
        /// 
        public void set_bonusScreenList(byte[] rom, int level)
        {
            List<ushort> bonusScreens = new List<ushort>();

            ushort cur = 0;                                        //If there are less than 32 bonus screens, the last one is referenced twice as kind of an "end of list".
            ushort prev = 0;                                       //These two variables will be used to detect this mechanic.

            for (int curBonScr = 0; curBonScr < 32; curBonScr++)
            {
                prev = cur;                                                  //Save cur in prev.
                cur  = rom[(level + 5) * 0x4000 + 0x4C0 + (curBonScr * 2) + 0];
                cur  = (ushort)(cur << 8);
                cur += rom[(level + 5) * 0x4000 + 0x4C0 + (curBonScr * 2) + 1];


                if (cur == prev)                                             //If current and previous pointers where the same, stop adding screens.
                    break;
                bonusScreens.Add(cur);

            }

            bonusScreenList = bonusScreens.ToArray();
        }

        /// <summary>
        /// Returns the adress to the data of one bonusscreen.
        /// </summary>
        /// <param name="screennr">The the number of the screen in bonusScreenList.</param>
        /// <returns>The requested value if screennr smaller than 32 or 0.</returns>
        public ushort get_bonusScreenListEntry(int screennr)
        {
            if (screennr > 31)
            {
                MessageBox.Show("ERROR! Bonus screen with a number larger than 31 requested. Will return 00.");
                return (0);
            }
            return this.bonusScreenList[screennr];
        }
        
        
        /// <summary>
        /// An array sequentially containing all 16Bit adresses to the screenOrder.
        /// </summary>
        /// 
        private ushort[] screenOrderList;

        
        /// <summary>
        /// Sets the screenOrderList to the corresponding values taken from a byte array containing 
        /// a whole patched SML-ROM.
        /// 
        /// The screenOrderList is little endian but will be communicated as big endian.
        /// </summary>
        /// <param name="rom">This byte array should contain a patched SML-ROM.</param>
        /// <param name="level">The number of the level to obtain the screenOrder from.</param>
        /// 
        public void set_screenOrderList(byte[] rom, int level)
        {
            List<ushort> screenOrder = new List<ushort>();

            ushort currentScreen = 0;                                        
                                               
            for (int curScr = 0; curScr < 32; curScr++)
            {

                currentScreen = rom[(level + 5) * 0x4000 + 0x100 + (curScr * 2) + 0];

                

                currentScreen = (ushort)(currentScreen << 8);
                currentScreen += rom[(level + 5) * 0x4000 + 0x100 + (curScr * 2) + 1];

                if (currentScreen == 0xFF00) break;

                screenOrder.Add(currentScreen);
            }

            screenOrderList = screenOrder.ToArray();  
        }


        /// <summary>
        /// Sets a specific entry of the screenOrderList to a new adress value.
        /// </summary>
        /// <param name="entry">The screenorderList entry to set.</param>
        /// <param name="address">The value to set the entry to. (Takes a big endian value and writes it as little endian.)</param>
        public void set_screenOrderList_Entry(int entry, ushort address)
        {
            //Switch the bytes:

            ushort tmp = (ushort)(address & 0x00FF);                        //Set tmp to lower byte of adress.
            tmp = (ushort)(tmp << 8);                                       //Shift the lower byte of tmp to the upper byte.
            address = (ushort)(address & 0xFF00);                           //Reset the lower byte of address.
            address = (ushort)(address >> 8);                               //Shift the upper byte of adress to the lower byte.
            address += tmp;                                                 //Add tmp to address.

            //Set the screenorderList entry:
            screenOrderList[entry] = address;                               
        }

        /// <summary>
        /// Returns the adress to the data of one screenOrderList-entry.
        /// </summary>
        /// <param name="screennr">The the number of the screen in screenOrderList.</param>
        /// <returns>The requested value if screennr exists or 0 if not. Value is retrieved as little endian but returned as big endian.</returns>
        /// 
        public ushort get_screenOrderListEntry(int screennr)
        {
            if (screennr >= screenOrderList.Length)
            {
                MessageBox.Show("ERROR! Tryed to get data for non defined ScreenOrderList-entry.");
                return (0);
            }
            ushort address = screenOrderList[screennr];
            ushort tmp = (ushort)(address & 0x00FF);                        //Set tmp to lower byte of adress.
            tmp = (ushort)(tmp << 8);                                       //Shift the lower byte of tmp to the upper byte.
            address = (ushort)(address & 0xFF00);                           //Reset the lower byte of address.
            address = (ushort)(address >> 8);                               //Shift the upper byte of adress to the lower byte.
            address += tmp;
            return address;
        }



        /// <summary>
        /// An array containing all checkpoints for the level.
        /// </summary>
        /// 
        private ushort[] checkpointList;


         /// <summary>
        /// Sets the screenOrderList to the corresponding values taken from a byte array containing 
        /// a whole patched SML-ROM.
        /// </summary>
        /// <param name="rom">This byte array should contain a patched SML-ROM.</param>
        /// <param name="level">The number of the level to obtain the screenOrder from.</param>
        /// 


        public void set_checkpointList(byte[] rom, int level)
        {
            List<ushort> checkpoints = new List<ushort>();

            ushort cur;

            for (int curCp = 0; curCp < 32; curCp++)
            {

                cur = rom[(level + 5) * 0x4160 + curCp];

                if (cur == 0xFF)
                    break;

                checkpoints.Add(cur);
            }
            this.checkpointList = checkpoints.ToArray();
        }


        /// <summary>
        /// Returns a single checkpointList-entry.
        /// </summary>
        /// <param name="screennr">The the number the checkpointList-entry.</param>
        /// <returns>The requested value if screennr exists or 0 if not.</returns>
        /// 
        public ushort get_checkpointListEntry(int cpnr)
        {
            if (cpnr >= checkpointList.Length)
            {
                MessageBox.Show("ERROR! Tryed to get data for non defined checkpointList-entry.");
                return (0);
            }
            return this.checkpointList[cpnr];
        }




        /// <summary>
        /// List of bonus-pipes in the level.
        /// </summary>
        /// 
        bonusPipe[] bonuspipeList;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rom">This byte array should contain a patched SML-ROM.</param>
        /// <param name="level">The number of the level to obtain the screenorder from.</param>
        /// 
        public void set_bonuspipeList(byte[] rom, int level)
        {
            List<bonusPipe> bonusPipes = new List<bonusPipe>();

            for (int curBp = 0; curBp < 32; curBp++)
            {
                bonusPipe cur_pipe;

                cur_pipe.entryScreen = rom[(level + 5) * 0x4200 + curBp * 6];

                if (cur_pipe.entryScreen == 0xFF)       //If current bonus pipe is the last one...
                    break;

                cur_pipe.xPos         = rom[(level + 5) * 0x4201 + curBp];
                cur_pipe.targetScreen = rom[(level + 5) * 0x4202 + curBp];
                cur_pipe.returnScreen = rom[(level + 5) * 0x4203 + curBp];
                cur_pipe.returnXPos   = rom[(level + 5) * 0x4204 + curBp];
                cur_pipe.returnYPos   = rom[(level + 5) * 0x4205 + curBp];

                bonusPipes.Add(cur_pipe);
            }

            this.bonuspipeList = bonusPipes.ToArray();
        }

        /// <summary>
        /// Returns a single bonuspipeList-entry.
        /// </summary>
        /// <param name="bpnr">The the number the bonuspipeList-entry.</param>
        /// <returns>The requested value if bpnr exists or 0 if not.</returns>
        /// 
        public bonusPipe get_bonuspipeListEntry(int bpnr)
        {
            if (bpnr >= bonuspipeList.Length)
            {
                MessageBox.Show("ERROR! Tryed to get data for non defined bonuspipeList-entry.");                
            }
            return this.bonuspipeList[bpnr];
        }




        /// <summary>
        /// An array containing all the entities of the level.
        /// </summary>
        /// 
        entity[] entityList;


        /// <summary>
        /// Sets all the entityList-values from an passed byte-array containing a patched SML-ROM.
        /// </summary>
        /// <param name="rom">This byte array should contain a patched SML-ROM.</param>
        /// <param name="level">The number of the level to obtain the entityList from.</param>
        /// 
        public void set_entityList(byte[] rom, int level)
        {
            List<entity> entitys = new List<entity>();

            for (int curEn = 0; curEn < 575; curEn++)
            {
                entity cur_entity;

                int xPos;

                // The horizontal entity position is rastered by 4 pixels. I will however read it as if it were 
                // rasterd in single pixels. In the rom the value is stored in 18 bits starting from 
                // adress 0x440.


                xPos = rom[(level + 5) * 0x4400 + (curEn * 4) + 0];          //Read the first 8 bits.
                if (xPos == 0xFF)                                            //If stumbled upon end-of-list-determinator FF: break!
                    break;

                xPos = xPos << 8;                                            //Leftshift them to make room for the next 8 bits.
                xPos += rom[(level + 5) * 0x4400 + (curEn * 4) + 1];         //Add the next 8 bits to the value.
                xPos = xPos << 4;                                            //Leftshift the value 4 times to make room for a pixel exact xPos. (Bits 0 and 1 will always be 0 because of the 4 pixel raster.)

                {
                    int tmp = rom[(level + 5) * 0x4400 + (curEn * 4) + 2];  //Create tmp-variable and fill it with the next 8 bits.
                    tmp = tmp & 0xC0;                                        //0xCO = 0b 1100 0000 (This &-operation clears all bits exept 6 and 7.)
                    tmp = tmp >> 4;                                          //Rightshift 4 times to focus on bits 4 to 7.
                    xPos = xPos + tmp;                                       //Add tmp to xPos to get the final value.
                }

                cur_entity.xPos = xPos;                                      //Set our temporary entity's xPos.

                int yPos;

                //Vertical entity position shares a byte with the last two bits of xPos. (0x4402)
                //The value is encoded as a six bit value, but only values between 0 (most upper tile)
                //and 18 (bottom most tile) render the entity on screen. Higher values result in the 
                //entity being placed in the void under the map. Chibobo-hell!

                yPos = rom[(level + 5) * 0x4400 + (curEn * 4) + 2];                     //Again read the byte at 0x440
                yPos = yPos & 0x3F;                                                     //0x3F = 0b 0011 1111 (Kills the last two bits belonging to xPos.)


                cur_entity.yPos = (byte)yPos;                                           //Set our temporary entity's yPos.
                cur_entity.hardmode = Convert.ToBoolean(cur_entity.yPos & 0x80);        //Set hardmode to true if yPo's eight's bit is set or to false if not.
                cur_entity.yPos = Convert.ToByte(cur_entity.yPos & 0x7F);               //Reset yPos eigth's bit.

                cur_entity.type = rom[(level + 5) * 0x4400 + (curEn * 4) + 3];          //Read the byte for current entity's type into the corrsponding variable of our temp entity.

                entitys.Add(cur_entity);                                                //Add entity to temporary entity-list.
            }

            this.entityList = entitys.ToArray();
        }


        /// <summary>
        /// Returns a single entityList-entry.
        /// </summary>
        /// <param name="enr">The number of the entityList-entry.</param>
        /// <returns>The requested value.</returns>
        /// 
        public entity get_entityListEntry(int enr)
        {
            if (enr >= entityList.Length)
            {
                MessageBox.Show("ERROR! Tryed to get data for non defined entityList-entry.");
            }
            return this.entityList[enr];
        }      



        /// <summary>
        /// An array containing all the items in the level.
        /// </summary>
        /// 
        item[] itemList;



        /// <summary>
        /// Sets all the itemList-values from an passed byte-array containing a patched SML-ROM.
        /// </summary>
        /// <param name="rom">This byte array should contain a patched SML-ROM.</param>
        /// <param name="level">The number of the level to obtain the itemList from.</param>
        /// 
        public void set_itemList(byte[] rom, int level)
        {

            List<item> items = new List<item>();

            for (int curIt = 0; curIt < 255; curIt++)
            {
                item cur_item;

                

                if (rom[(level + 5) * 0x4D00 + (curIt * 3) + 0]  == 0xFF)          //If the current item happens to be the final item: break!
                    break;
                cur_item.screenNr = rom[(level + 5) * 0x4D00 + (curIt * 3) + 0];   //Get current items Screennr.
                cur_item.xPos = rom[(level + 5) * 0x4D00 + (curIt * 3) + 1];       //Get current items xPos.
                cur_item.type = rom[(level + 5) * 0x4D00 + (curIt * 3) + 2];       //Get current items Type. 

                items.Add(cur_item);                                               //Add current item to temporary item-list.
            }
            this.itemList = items.ToArray();                                       //Set levels ItemList.
        }


        /// <summary>
        /// Returns a single itemList-entry.
        /// </summary>
        /// <param name="inr">The number of the itemList-entry.</param>
        /// <returns>The requested value.</returns>
        /// 
        public item get_itemListEntry(int inr)
        {
            if (inr >= itemList.Length)
            {
                MessageBox.Show("ERROR! Tryed to get data for non defined itemList-entry.");
            }
            return this.itemList[inr];
        }      
        
        

        /// <summary>
        /// This object contains a map of all background tiles for the whole level.
        /// </summary>
        public Screendata tileMatrix = new Screendata();                          

        /// <summary>
        /// Sets all the tileMatrix-values from an passed byte-array containing a patched SML-ROM.
        /// </summary>
        /// <param name="rom">This byte array should contain a patched SML-ROM.</param>
        /// <param name="level">The number of the level to obtain the tileMatrix from.</param>
        /// 
        public void set_tileMatrix(byte[] rom, int level)
        {

            




            //Step through the screenorder list:
            for (int cur_screen = 0; cur_screen < screenOrderList.Length; cur_screen++)
            {

                ushort cur_sle = get_screenOrderListEntry(cur_screen);                 //Set current screenorder list entry.

                int byteCounter = 0;
                byte   cur_sdb = rom[(level + 5) * 0x4000 + cur_sle - 0x4000 + byteCounter];

                
                
                //Step through columns:

                int cur_col = 0;

                while (cur_col < 20)   
                {

                    
                    

                    if (cur_sdb == 0xFE)                                                        //0xFE marks the end of data for the current row.
                    {
                        cur_col++;
                    }
                    else
                    {
                        
                        int colPos = (cur_sdb & 0xF0) >> 4;                                     //Position of the first tile to set.
                        int numOfTiles = (cur_sdb & 0x0F);                                      //number of tiles to set from "rowPos".

                        if (cur_sdb == 0)                                                       //If data byte is 00 it means no tile is empty.
                        {
                            numOfTiles = 16;
                        }

                        bool isFD = false;                                                      //The tile-id 0xFD is abused as an compression indicator. If a tile with the value 0xFD is encountered, all subsequent tiles in the series will be drawn as the tile following the 0xFD.

                        for (int i = colPos; i < (colPos + numOfTiles); i++)                    //Read the next "numOfTiles" bytes and put them into the screendata from "colPos" on.
                        {

                            if (!isFD)                                                          //Should be the case most of the time.
                            {
                                byteCounter++;
                                cur_sdb = rom[(level + 5) * 0x4000 + cur_sle - 0x4000 + byteCounter];    //load the next tile.

                                if (cur_sdb == 0xFD)                                            //If the tile is 0xFD...
                                {
                                    byteCounter++;
                                    cur_sdb = rom[(level + 5) * 0x4000 + cur_sle - 0x4000 + byteCounter];//... load the next tile...
                                    isFD = true;                                                //... and stop loading new tiles for this series.
                                }
                            }


                            tileMatrix.set_tile(cur_screen, cur_col, i, cur_sdb); 
                        }
                    }

                    byteCounter++;                                                              //Read the next byte to process.
                    cur_sdb = rom[(level + 5) * 0x4000 + cur_sle - 0x4000 + byteCounter];
                 


                }






            }
        }




        /// <summary>
        /// This object contains a array of bitmaps representing the tilepalette for the level.
        /// </summary>
        public TilePalette tilePal = new TilePalette();                            //All tiles of the tilepalette in bitmap format.
        


        /// <summary>
        /// Reads the tiles from a level in an passed byte array containing a whole rom.
        /// 
        /// Tiles are rearranged to be directly acessible via the values in a tile-matrix.
        /// First 128 tiles from 0x5000 to 0x57FF are read to tilePal-entries 0x00 to 0x7F.
        /// Second 128 tiles from 0x4800 to 0x4FFF are read to tilePal-entries 0x80 to 0xFF.
        /// Third 128 tiles from 0x4000 to 0x47FF are read to tilePal-entries 0x100 to 0x17F.
        /// The third group of tiles cannot be used in maps and is only read for future advanced
        /// features like enemy editing and so on.
        /// </summary>
        /// <param name="rom"></param>
        /// <param name="level"></param>
        public void set_TilePalette(byte[] rom, int level)
        {
            int byteCounter = 0;                        //Let's count the bytes.
            int tileCounter;                            //And let's also count tiles this time.

            Bitmap[] tilePal = new Bitmap[256];         //Our tile-palette-storage.


            for (tileCounter = 0; tileCounter < 128; tileCounter++)      //Read 128 tiles from 0x5000 to 0x57FF.
            {
                Bitmap cur_tile = new Bitmap(8,8 );                      //Bitmap to be added to tilePal in the end.


                //Eight times read two bytes and decode the pixels from GB-format.

                for (int ypos = 0; ypos < 8; ypos++)
                {
                    
                    byte a = rom[((level + 0x25) * 0x4000) + 0x1000 + byteCounter];  //Read first byte.
                    byteCounter++;
                    byte b = rom[(level + 0x25) * 0x4000 + 0x1000 + byteCounter];  //Read second byte.
                    byteCounter++;

                    //Decode pixel by pixel:

                    int pixel;

                    for (int xpos = 0; xpos < 8; xpos++)
                    {
                        pixel = ((a & 0x80) >> 6);                  //Set pixel's bit 1 to a's bit 7.
                        pixel += ((b & 0x80) >> 7);                 //Set pixel's bit 0 to b's bit 7. 

                        a = (byte)(a << 1);                         //Lefthift both bytes once to focus on the next bit.
                        b = (byte)(b << 1);

                        //KKKcur_tile.SetPixel(xpos,ypos,
                        this.tilePal.setPixel(tileCounter, xpos, ypos, pixel);
                    }
                }
            }

            for (tileCounter = 128; tileCounter < 256; tileCounter++)    //Read 128 tiles from 0x4800 to 0x4FFF
            {
                Bitmap cur_tile = new Bitmap(8, 8);                      //Bitmap to be added to tilePal in the end.


                //Eight times read two bytes and decode the pixels from GB-format.

                for (int ypos = 0; ypos < 8; ypos++)
                {

                    byte a = rom[((level + 0x25) * 0x4000) + 0x000 + byteCounter];  //Read first byte.
                    byteCounter++;
                    byte b = rom[(level + 0x25) * 0x4000 + 0x000 + byteCounter];  //Read second byte.
                    byteCounter++;

                    //Decode pixel by pixel:

                    int pixel;

                    for (int xpos = 0; xpos < 8; xpos++)
                    {
                        pixel = ((a & 0x80) >> 6);                  //Set pixel's bit 1 to a's bit 7.
                        pixel += ((b & 0x80) >> 7);                 //Set pixel's bit 0 to b's bit 7. 

                        a = (byte)(a << 1);                         //Lefthift both bytes once to focus on the next bit.
                        b = (byte)(b << 1);

                        this.tilePal.setPixel(tileCounter, xpos, ypos, pixel);
                    }
                }
            }


            for (tileCounter = 256; tileCounter < 384; tileCounter++)    //Read 128 tiles from 0x4000 to 0x47FF.
            {
                Bitmap cur_tile = new Bitmap(8, 8);                      //Bitmap to be added to tilePal in the end.


                //Eight times read two bytes and decode the pixels from GB-format.

                for (int ypos = 0; ypos < 8; ypos++)
                {

                    byte a = rom[((level + 0x25) * 0x4000) + 0x000 + byteCounter];  //Read first byte.
                    byteCounter++;
                    byte b = rom[(level + 0x25) * 0x4000 + 0x000 + byteCounter];  //Read second byte.
                    byteCounter++;

                    //Decode pixel by pixel:

                    int pixel;

                    for (int xpos = 0; xpos < 8; xpos++)
                    {
                        pixel = ((a & 0x80) >> 6);                  //Set pixel's bit 1 to a's bit 7.
                        pixel += ((b & 0x80) >> 7);                 //Set pixel's bit 0 to b's bit 7. 

                        a = (byte)(a << 1);                         //Lefthift both bytes once to focus on the next bit.
                        b = (byte)(b << 1);

                        this.tilePal.setPixel(tileCounter, xpos, ypos, pixel);
                    }
                }
            }

            
        }




    }


    /// <summary>
    /// The Screendata is a set of tools to retrieve the level-screens from the ROM and present them in the editor.
    /// 
    public class Screendata
    {

        /// <summary>
        /// Constructs a new instance of Screendata with all tiles set to 0x2c (blank tile).
        /// The tiles are represented as byte values corresponding to the indexes of a tileMap class.
        /// </summary>
        /// 
        public Screendata() 
        { 
         
        // Set all tiles in tileMatrix to 0x2C (blank tile).
       
       
        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                for (int k = 0; k < 16; k++)
                {
                    tileMatrix[i, j, k] = 0x2C;
                }
            }
         }
     

        for (int i = 0; i < 32; i++)
        {
            Bitmap tempScreen = new Bitmap(340,272);        // 20 * 17, 16 * 17 (17 because I want to have a one pixel grid between the tiles.)
            this.screens[i] = tempScreen;
        }
        }


        /// <summary>
        /// A 32 x 20 x 17 ushort-array used to contain all tiles used in a level. 
        /// The dimensions are used in the following way:
        /// 
        ///     32 screens with 20 horizontal and 16 vertical tiles.
        ///     
        /// </summary>
        /// 
        private ushort[, ,] tileMatrix = new ushort[32, 20, 16];

        /// <summary>
        /// Public Bitmaps containing all 32 screens of the level.
        /// </summary>
        public Bitmap[] screens = new Bitmap[32];


        /// <summary>
        /// Updates a single screen for level display from the corresponding screenMatrix.
        /// </summary>
        /// <param name="screenNr">The number of the screen to update.</param>
        /// <param name="pal">A tile palette to retrieve the graphics from.</param>
        public void update_screen(int screenNr, TilePalette pal)
        {
            Graphics g = Graphics.FromImage(screens[screenNr]);                         //Get graphics-object for requested screen.
           
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    ushort cur_tile = tileMatrix[screenNr, i, j];
                    Bitmap tmpTile = pal.getTile(cur_tile);

                    g.DrawImage(tmpTile, new System.Drawing.Rectangle(i * 17, j * 17, 16, 16));
                }
            }
        }

        public void update_all_screens(TilePalette pal)
        {
            for (int i = 0; i < screens.Length; i++)
            {
                update_screen(i, pal);
            }
        }



        /// <summary>
        /// Fills all tiles of screen "screenNr" with tile "tileNr".
        /// </summary>
        /// <param name="screenNr">the screen to fill with a tile.</param>
        /// <param name="tileNr">the tile to fill the screen with</param>
        /// <returns>true if an existing screen was referenced.</returns>
        public bool fill_screen(int screenNr, byte tileNr) 
        {
            if ((0 <= screenNr) && (screenNr <= 31))
            {
                MessageBox.Show("ERROR! Instruction to fill a non existing screen.");
                return false;
            }

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    tileMatrix[screenNr, i, j] = tileNr;
                }
            }
            return true;
        }

        /// <summary>
        /// Sets a specific element of tileMatrix to tileNr. 
        /// </summary>
        /// <param name="screen">The number of the screen containing the tile to set.</param>
        /// <param name="xpos">The horizontal position inside the screen of the tile to set.</param>
        /// <param name="ypos">The vertical position inside the screen of the tile to set.</param>
        /// <param name="tile">A byte value referencing an entry of the tile palette indicating the graphics to set the tile to.</param>
        /// <returns></returns>
        public bool set_tile(int screen, int xpos, int ypos, ushort tile)    
        {
            if ((screen > 31) || (xpos > 19) || (ypos > 15))
            {
                MessageBox.Show("ERROR! Tried to set tile at a non-defined location.");
                return false;
            }
           
            tileMatrix[screen, xpos, ypos] = tile;
            return true;
        }
        
        /// <summary>
        /// Returns the tile-palette index of a specific tile in the tile matrix.
        /// </summary>
        /// <param name="screen">The screen containing the tile.</param>
        /// <param name="xpos">Horizontal position of the tile inside the sceen.</param>
        /// <param name="ypos">Vertical position of the tile inside the sceen.</param>
        /// <returns></returns>
        public ushort get_tile(int screen, int xpos, int ypos)                
        {
            if ((screen > 31) || (xpos > 19) || (ypos > 15))
            {
                MessageBox.Show("Warning! Tried to get tile from a non-defined location. Will return 0x2c (blanc tile) instead.");
                return 0x2c;
            }
            else return tileMatrix[screen, xpos, ypos];
        }

        /// <summary>
        /// Returns the tile-palette index of a specific tile in the tile matrix.
        /// </summary>
        /// <param name="ypos">The number of the tile in the tile matrix</param>
        public ushort get_tile(int number)
        {
            int screen = number / (20 * 16);
            int xpos = (number % (20 * 16)) / 16;
            int ypos = (number % (20 * 16)) % 16;
            if ((screen > 31) || (xpos > 19) || (ypos > 15))
            {
                MessageBox.Show("Warning! Tried to get tile from a non-defined location. Will return 0x2c (blanc tile) instead.");
                return 0x2c;
            }

            else return tileMatrix[screen, xpos, ypos];
            
        }

        /// <summary>
        /// Returns the number of tiles in the level.
        /// </summary>
        /// <returns>The number of tiles in the level.</returns>
        public int get_numberOfTiles()
        {
            return tileMatrix.Length;
        }
}


   

    /// <summary>
    /// Basically an array of 256 8x8 pixel bitmaps representing all entries of the roms tile palette.
    /// </summary>
    public class TilePalette
    {
        private Bitmap[] tilePalette;
        private static int numOfTiles = 384;

        public TilePalette()                                //Constructor
        {
            List<Bitmap> bmpList = new List<Bitmap>();
            for (int i = 0; i < numOfTiles; i++)
            {
                Bitmap tmpbmp = new Bitmap(16, 16);
                bmpList.Add(tmpbmp);
            }

            tilePalette = bmpList.ToArray();


        }


        /// <summary>
        /// Converts a color in the format used by SML to a C#-Color variable.
        /// Colors are interpreted as four shades of green.
        /// </summary>
        /// <param name="smlColor">Color value as represented in SML-ROM. Can't be bigger than 3.</param>
        /// <returns>The according greenshade if valid or red if not.</returns>
        private Color colorCalc(int smlColor)
        {
            Color realColor = Color.FromName("Red");    //If one of this red pixels shows something went wrong.

            if (smlColor == 0) realColor = Color.FromName("LemonChiffon");
            if (smlColor == 1) realColor = Color.FromName("DarkOliveGreen");
            if (smlColor == 2) realColor = Color.FromName("DarkSeaGreen");
            if (smlColor == 3) realColor = Color.FromName("Black");

            if (smlColor > 3)
            {
                MessageBox.Show("ERROR! Color value has to be between 0 and 3. Color was set to Red.");
            }

            return realColor;
        }

        /// <summary>
        /// Sets a single pixel of a tile in the tile palette. Actually the pixel is four pixels 
        /// forming a square. 
        /// </summary>
        /// <param name="tile">Number of the tile containing the pixel.</param>
        /// <param name="x">Horizontal position of the pixel.</param>
        /// <param name="y">Vertical position of the pixel.</param>
        /// <param name="color">The mew color for the pixel. 
        /// Possible values: 
        /// 
        ///     0 = lightest
        ///     1 = light
        ///     2 = dark
        ///     3 = darkest </param>
        ///     
        /// <returns>True if the color was valid.</returns>
        public bool setPixel(int tile, int x, int y, int color)
        {
            if (colorCalc(color) == Color.FromName("Red")) return false;            //Check whether the color is valid.

            tilePalette[tile].SetPixel(x * 2 + 0, y * 2 + 0, colorCalc(color));     //Set the (doubled) pixel.
            tilePalette[tile].SetPixel(x * 2 + 0, y * 2 + 1, colorCalc(color));
            tilePalette[tile].SetPixel(x * 2 + 1, y * 2 + 0, colorCalc(color));
            tilePalette[tile].SetPixel(x * 2 + 1, y * 2 + 1, colorCalc(color));

            return true;
        }

        /// <summary>
        /// Returns a tile from the tile palette.
        /// </summary>
        /// <param name="tile">Number of the tile to return.</param>
        /// <returns>The corresponding tile from the tile palette</returns>
        public Bitmap getTile(ushort tile)
        {
            return tilePalette[tile];
        }

    }



    
    
}
