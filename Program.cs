using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace SMLEdit
{
    

    //Structures:

   


    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
             RomHeader romHeader = new RomHeader(); // Create romHeader-object.
        
      
            Level test = new Level();
         
            List<int> testlist = new List<int>();
            testlist.Insert(0,1);
            testlist.Insert(0, 2);
            testlist.Insert(0, 3);
            testlist.Insert(0, 4);
            testlist.Insert(0, 5);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(romHeader));
        }
    }
}
