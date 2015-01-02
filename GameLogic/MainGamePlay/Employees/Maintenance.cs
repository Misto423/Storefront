using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.GameLogic.MainGamePlay.Employees
{
    public class Maintenance : EmployeeMain
    {
        public static byte currentlyEmployed = 0;
        public static readonly byte MAXALLOWED = 3;

        public Maintenance(Random rand)
            : base(rand)
        {

        }

        public override void passiveAbility()
        {
            //no passive ability
        }

        public void repairShelf()
        {

        }
    }
}
