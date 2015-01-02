using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.GameLogic.MainGamePlay.Employees
{
    public class Slave : EmployeeMain
    {
        public static byte currentlyEmployed = 0;
        public static readonly byte MAXALLOWED = 5;

        #region Properties
        public byte AvailableSpots
        {
            get { return (byte)(Slave.MAXALLOWED - (Slave.currentlyEmployed + Clerk.currentlyEmployed)); }
        }
        #endregion

        public Slave(Random rand)
            : base(rand)
        {

        }

        public override void passiveAbility()
        {
            
        }
    }
}
