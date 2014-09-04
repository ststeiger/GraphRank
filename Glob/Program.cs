using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Glob
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (false)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }


            // http://stackoverflow.com/questions/12745703/how-can-i-use-the-windows-ui-namespace-from-a-regular-non-store-win32-net-app
            var characterGroupings = new Windows.Globalization.Collation.CharacterGroupings();
            // Get the number of CharacterGrouping objects.


            // http://stackoverflow.com/questions/18178990/finding-out-installed-metro-applications-on-a-machine
            // http://code.msdn.microsoft.com/windowsdesktop/Sending-toast-notifications-71e230a2

             // If you do need to cast explicitly, for example if you want to call GetEnumerator, 
            // cast to IEnumerable<T> with a CharacterGrouping constraint.
            
            /*
            <PropertyGroup>
               <TargetPlatformVersion>8.0</TargetPlatformVersion>
            </PropertyGroup>
             */
            // Reference System.Runtime
            // C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETCore\v4.5.1\


            int size = characterGroupings.Count;
            
            if (size > 0)
            {

                // Get the first characterGrouping.
                //var characterGrouping = characterGroupings.getAt(0);

                

                Windows.Globalization.Collation.CharacterGrouping characterGrouping = characterGroupings.ElementAt(0);

                // Get the first item in this characterGrouping.
                var first = characterGrouping.First;
                // Get the label of the first item in this characterGrouping.
                var label = characterGrouping.Label;
            }
            
        }
    }
}
