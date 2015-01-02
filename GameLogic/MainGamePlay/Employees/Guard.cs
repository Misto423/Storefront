using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.GameLogic.MainGamePlay.Employees
{
    public class Guard : EmployeeMain
    {
        public static byte currentlyEmployed = 0;
        public static readonly byte MAXALLOWED = 3;

        public Guard(Random rand)
            : base(rand)
        {

        }

        public override void passiveAbility()
        {
            
        }
    }
}
