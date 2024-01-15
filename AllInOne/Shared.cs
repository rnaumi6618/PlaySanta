/* PlaySanta.cs
* PlaySanta
* Revision History
* EunHeui Jo,Rafia Naumi, 2023.11.02: Created
* EunHeui Jo,Rafia Naumi, 2023.11.02-2023.11.09: Added code
* EunHeui Jo,Rafia Naumi, 2023.11.09: Debugging complete
* EunHeui Jo,Rafia Naumi, 2023.11.09: Comments added 
*/
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllInOne
{
    /// <summary>
    /// Provides shared resources across different parts of the game.
    /// </summary>
    public class Shared
    {
        /// <summary>
        /// Represents the dimensions of the game stage or screen.
        /// This static property can be accessed globally within the game to reference the size of the game area.
        /// </summary>
        public static Vector2 stage;
    }
}
