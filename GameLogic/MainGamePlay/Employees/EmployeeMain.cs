using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.GameLogic.MainGamePlay.Employees
{
    public enum MoodType
    {
        Ecstatic,
        Pleased,
        Happy,
        Content,
        Indifferent,
        Discontent,
        Annoyed,
        Frustrated,
        Angry
    }

    public abstract class EmployeeMain
    {
        private int[] experienceChart = { 0, 250, 600, 1500, 3500, 6000, 10000, 25000, 55000, 100000, 200000 };
        private readonly byte MAXLEVEL = 10;

        private string name;
        private byte level;
        private int wage, expWage;
        private MoodType mood;
        private int exp, nextLevel;

        #region Properties
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public byte Level
        {
            get { return level; }
            set { level = value; }
        }

        public int Wage
        {
            get { return wage; }
            set { wage = value; }
        }

        public int ExpectedWage
        {
            get { return expWage; }
            set { expWage = value; }
        }

        public MoodType Mood
        {
            get { return mood; }
            set { mood = value; }
        }

        public int Experience
        {
            get { return exp; }
            set { exp = value; }
        }

        public int NextLevel
        {
            get { return nextLevel; }
            set { nextLevel = value; }
        }
        #endregion

        public EmployeeMain(Random rand)
        {
            //randomize name
            int first = rand.Next(1, 16);
            int second = rand.Next(1, 13);
            string temp = "Name" + first;
            object o = Resources.Names.ResourceManager.GetObject(temp);
            string fn = o.ToString();
            string ln = Resources.LastNames.ResourceManager.GetObject("LastName" + second).ToString();
            name = fn + " " + ln;

            //randomize starting level
            level = (byte)rand.Next(1, 6);
            //set mood to indifferent
            Mood = MoodType.Indifferent;
            //randomize the expected wage
            expWage = (int)((rand.Next(25, 100) * rand.Next(1, level + 1)) / 2.25);
            wage = 0;
            //set initial experience levels
            exp = experienceChart[level - 1];
            nextLevel = experienceChart[level];
        }

        /// <summary>
        /// Awards experience and levels up the employee if necessary.
        /// </summary>
        /// <param name="amount">The amount of experience to award the employee.</param>
        public void AwardExperience(int amount)
        {
            //if the customer is not max level
            if (level < MAXLEVEL)
            {
                //award the amount of experience specified
                exp += amount;
                //level up and shift to the next amount of experience needed.
                if (exp >= nextLevel)
                {
                    level++;
                    nextLevel = experienceChart[level];
                }
            }
        }

        public abstract void passiveAbility();
    }
}
