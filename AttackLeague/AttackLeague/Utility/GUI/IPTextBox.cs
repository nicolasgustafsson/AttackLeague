using AttackLeague.Utility.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackLeague.Utility.GUI
{
    [Serializable]
    class IPTextBox : TextBox
    {
        // TODO if wants

        static readonly char[] AcceptedCharacters = new char[] { '.', '\r', '\b', '\t' };

        protected override void gotabokstav(object aSender, CharacterEventArgs argies) 
        {
            if (argies.Character >= '0' && argies.Character <= '9' || AcceptedCharacters.Contains(argies.Character))
                base.gotabokstav(aSender, argies);
        }


    }
}
