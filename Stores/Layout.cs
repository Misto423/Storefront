using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storefront.Stores
{
    public class Layout
    {
        private bool?[,] walkingSpaceArray;
        private byte layout;
        private Microsoft.Xna.Framework.Vector2 doorPosition;

        public Layout(byte layout)
        {
            this.layout = layout;
            setupArray();
        }

        private void setupArray()
        {
            switch (layout)
            {
                default:
                case 1:
                    //allocate space of the building space array (all stores have same overall layout).
                    walkingSpaceArray = new bool?[8, 12];
                    //Sets all spaces in the store to unoccupied.
                    for (int vert = 0; vert < 8; vert++)
                    {
                        for (int horz = 0; horz < 12; horz++)
                        {
                            walkingSpaceArray[vert, horz] = true;
                        }
                    }
                    //Sets the entrance of the store (outside area) to occupied.
                    for (int i = 4; i < 8; i++)
                    {
                        walkingSpaceArray[6, i] = null;
                        walkingSpaceArray[7, i] = null;
                    }
                    doorPosition = new Microsoft.Xna.Framework.Vector2(5, 5);
                    break;
                case 2:
                    //allocate space of the building space array (all stores have same overall layout).
                    walkingSpaceArray = new bool?[8, 16];
                    //Sets all spaces in the store to unoccupied.
                    for (int vert = 0; vert < 8; vert++)
                    {
                        for (int horz = 0; horz < 16; horz++)
                        {
                            walkingSpaceArray[vert, horz] = true;
                        }
                    }
                    //Sets the entrance of the store (outside area) to occupied.
                    for (int i = 6; i < 10; i++)
                    {
                        walkingSpaceArray[0, i] = null;
                        walkingSpaceArray[1, i] = null;
                        walkingSpaceArray[2, i] = null;
                        walkingSpaceArray[7, i] = null;
                    }
                    doorPosition = new Microsoft.Xna.Framework.Vector2(7, 6);
                    break;
                case 3:
                    //allocate space of the building space array (all stores have same overall layout).
                    walkingSpaceArray = new bool?[10, 10];
                    //Sets all spaces in the store to unoccupied.
                    for (int vert = 0; vert < 10; vert++)
                    {
                        for (int horz = 0; horz < 10; horz++)
                        {
                            walkingSpaceArray[vert, horz] = true;
                        }
                    }
                    //Sets the entrance of the store (outside area) to occupied.
                    for (int i = 0; i < 2; i++)
                    {
                        walkingSpaceArray[0, i] = null;
                        walkingSpaceArray[1, i] = null;
                        walkingSpaceArray[8, i] = null;
                        walkingSpaceArray[9, i] = null;
                    }
                    for (int i = 8; i < 10; i++)
                    {
                        walkingSpaceArray[0, i] = null;
                        walkingSpaceArray[1, i] = null;
                        walkingSpaceArray[8, i] = null;
                        walkingSpaceArray[9, i] = null;
                    }
                    doorPosition = new Microsoft.Xna.Framework.Vector2(4, 9);
                    break;
                case 4:
                    //allocate space of the building space array (all stores have same overall layout).
                    walkingSpaceArray = new bool?[8, 8];
                    //Sets all spaces in the store to unoccupied.
                    for (int vert = 0; vert < 8; vert++)
                    {
                        for (int horz = 0; horz < 8; horz++)
                        {
                            walkingSpaceArray[vert, horz] = true;
                        }
                    }
                    doorPosition = new Microsoft.Xna.Framework.Vector2(3, 7);
                    break;
            }
        }

        public bool?[,] StoreLayout
        {
            get { return walkingSpaceArray; }
            set { walkingSpaceArray = value; }
        }
        public Microsoft.Xna.Framework.Vector2 DoorPos
        {
            get { return doorPosition; }
        }

        public byte LayoutIndex
        {
            get { return layout; }
            set
            {
                layout = value;
                setupArray();
            }
        }
    }
}
