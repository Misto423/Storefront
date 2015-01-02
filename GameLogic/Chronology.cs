using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Storefront.GameLogic
{
    /// <summary>
    /// Contains information and functions used for the time in game.
    /// </summary>
    public class Chronology
    {
        public enum DateFormat
        {
            Descending,
            Ascending,
            American
        }

        public enum GameSpeed
        {
            Pause,
            Normal,
            Fast
        }

        private int year;
        private byte month;
        private byte day;
        private byte dayOfWeek;
        //format to display the date in.
        private static DateFormat format = DateFormat.Descending;

        //speed of gameplay
        private static GameSpeed speed = GameSpeed.Normal;

        //counters to adjust time
        private const byte DAYTIME_NORMAL = 45;
        private const byte DAYTIME_FAST = 5;
        private float counter = 0;

        public Chronology()
        {
            year = 1300;
            month = 1;
            day = 1;
            dayOfWeek = 1;
        }

        /// <summary>
        /// Gets or sets the year in the game.
        /// </summary>
        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        /// <summary>
        /// Gets or sets the month number.
        /// </summary>
        public byte Month
        {
            get { return month; }
            set { month = value; }
        }

        /// <summary>
        /// Gets or sets the day of the month in number format.
        /// </summary>
        public byte Day
        {
            get { return day; }
            set { day = value; }
        }

        /// <summary>
        /// Gets or sets the number of the day of the week.  For example, Friday would be 5.
        /// </summary>
        public byte Day_Of_Week
        {
            get { return dayOfWeek; }
            set { dayOfWeek = value; }
        }

        public DateFormat DateDisplayFormat
        {
            get { return format; }
            set { format = value; }
        }

        public GameSpeed GameMoveSpeed
        {
            get { return speed; }
            set { speed = value; }
        }

        /// <summary>
        /// Converts the month number to the string name.
        /// </summary>
        /// <returns>The name of the month.</returns>
        private string MonthString()
        {
            switch (month)
            {
                default:
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
            }
        }

        /// <summary>
        /// Converts the day number to the string name.
        /// </summary>
        /// <returns>The name of the current day of the week.</returns>
        private string DayString()
        {
            switch (dayOfWeek)
            {
                default:
                case 1:
                    return "Monday";
                case 2:
                    return "Tuesday";
                case 3:
                    return "Wednesday";
                case 4:
                    return "Thursday";
                case 5:
                    return "Friday";
                case 6:
                    return "Saturday";
                case 7:
                    return "Sunday";
            }
        }

        /// <summary>
        /// Gets the string representation of the date in game.
        /// </summary>
        /// <returns>The full date as a string.</returns>
        public override string ToString()
        {
            if (format == DateFormat.American)
            {
                return DayString() + ", " + MonthString() + " " + day + ", " + year;
            }
            else if (format == DateFormat.Ascending)
            {
                return DayString() + ", " + day + " " + MonthString() + " " + year;
            }
            else
            {
                return year + " " + MonthString() + " " + day + ", " + DayString();
            }
        }

        /// <summary>
        /// Called every frame and updates the game time accordingly.
        /// </summary>
        /// <param name="gt">GameTime object that keeps track of running times.</param>
        public void updateChronology(GameTime gt)
        {
            if (speed == GameSpeed.Normal)
            {
                counter += (float)gt.ElapsedGameTime.TotalSeconds;
                if (counter >= DAYTIME_NORMAL)
                {
                    counter = 0;
                    //update the day.
                    if (day == 30)
                    {
                        day = 1;
                        //update the month if 30 days passed.
                        month++;
                    }
                    else
                    {
                        day++;
                    }
                    //update the day of week
                    if (dayOfWeek == 7)
                    {
                        dayOfWeek = 1;
                        //order items for the week
                        GameGlobal.orderHandler.placeOrder();
                    }
                    else
                    {
                        dayOfWeek++;
                    }
                    //update the month and year
                    if (month >= 13)
                    {
                        month = 1;
                        year++;
                    }
                }
            }
            else if (speed == GameSpeed.Fast)
            {
                counter += (float)gt.ElapsedGameTime.TotalSeconds;
                if (counter >= DAYTIME_FAST)
                {
                    counter = 0;
                    //update the day.
                    if (day == 30)
                    {
                        day = 1;
                        //update the month if 30 days passed.
                        month++;
                    }
                    else
                    {
                        day++;
                    }
                    //update the day of week
                    if (dayOfWeek == 7)
                    {
                        dayOfWeek = 1;
                        //order items for the week
                        GameGlobal.orderHandler.placeOrder();
                    }
                    else
                    {
                        dayOfWeek++;
                    }
                    //update the month and year
                    if (month >= 13)
                    {
                        month = 1;
                        year++;
                    }
                }
            }
        }
    }
}
