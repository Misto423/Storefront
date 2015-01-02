using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Data;

namespace Storefront.GameLogic.MainGamePlay
{
    public enum CustomerClass
    {
        Knight,
        Mage,
        Civilian
    }

    public struct CustomerItem
    {
        public Database.ShelfItem item;
        public bool inInv;

        public CustomerItem(Database.ShelfItem i, bool inInv)
        {
            item = i;
            this.inInv = inInv;
        }

        public bool Equals(CustomerItem ci)
        {
            return this.item.Name == ci.item.Name;
        }
    }

    struct AStarGrid
    {
        private Vector2 position;
        private Vector2 parent;
        private int F, G, H;

        public AStarGrid(Vector2 pos, Vector2 par, int f, int g, int h)
        {
            position = pos;
            parent = par;
            F = f;
            G = g;
            H = h;
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public int G_Value
        {
            get { return G; }
            set { G = value; }
        }

        public int H_Value
        {
            get { return H; }
            set { H = value; }
        }

        public int F_Value
        {
            get { return F; }
            set { F = value; }
        }
    }

    public class Customer
    {
        #region Vars

        private Texture2D mapSprite;
        private Texture2D portrait;
        private Vector2 drawStart;
        private string thoughts;
        private List<CustomerItem> inventory;
        private int money, actualMoney; //the amount of money the player has - total amount in items.
        private bool custType;
        private string name;
        private CustomerClass custClass;
        private byte satisfaction;

        private int searchIndex = 0;

        private Database.DBConnector dbc = new Database.DBConnector();

        //path finding vars
        private Vector2 screenPosition, gridPosition;
        private Vector2 moveAmt;
        private const int OrthagonalD = 10;
        private List<Vector2> pathToShelf;
        private bool relayPointHit = true;
        private Vector2 nextGridPosition = new Vector2();
        private Vector2 nextScreenPos = new Vector2();
        private int currentPathIndex = 0;
        private const int SPEED = 8;
        private const int FASTSPEED = 24;

        private int itemIndex = -1;
        private int shelfIndex = -1;

        //Random Movement Vars
        Random rand = new Random();
        private bool[] checkedDirection = {false, false, false, false};
        private float[] movePercentages = { 65, 12.5f, 12.5f, 10 };
        private byte previousDirection = 0;
        //Browsing shelf unit vars
        //flag to determine whether the customer should start browsing a shelf.
        private bool enterShelf = false;
        private float timeInShelf = 0;
        private float exitShelfTime;
        private bool generateTime = false;
        //flag to determine if a customer was recently browsing
        //if so they need to move at least 2 spaces before entering a new shelf.
        private bool lastInShelf = false;
        private byte lastInShelfCounter = 0;
        //exiting vars
        private bool isExiting = false;
        private bool exited = false;
        private bool generateExitTime = true;
        private float exitStoreTime;
        private float timeInStore = 0;

        #endregion

        #region Attributes
        public string Name
        {
            get { return name; }
        }
        public Texture2D Portrait
        {
            get { return portrait; }
        }
        public CustomerClass CustClass
        {
            get { return custClass; }
        }
        public byte Satisfaction
        {
            get { return satisfaction; }
            set { satisfaction = value; }
        }

        public Vector2 Position
        {
            get { return screenPosition; }
            set { screenPosition = value; }
        }
        public string Thoughts
        {
            get { return thoughts; }
            set { thoughts = value; }
        }
        public List<CustomerItem> Inventory
        {
            get { return inventory; }
            set { inventory = value; }
        }
        public int Money
        {
            get { return money; }
            set { money = value; }
        }
        public Texture2D SpriteSheet
        {
            get { return mapSprite; }
            set { mapSprite = value; }
        }
        public bool GetPathStatus
        {
            get { return pathToShelf.Count > 0; }
        }
        public bool CustomerType
        {
            get { return custType; }
            set { custType = value; }
        }
        public bool Exited
        {
            get { return exited; }
        }
        public bool InShelf
        {
            get { return enterShelf; }
        }
        #endregion

        public Customer()
        {
            inventory = new List<CustomerItem>();
            pathToShelf = new List<Vector2>();
            name = Resources.Names.ResourceManager.GetObject("Name" + rand.Next(1, 4)).ToString() + " " +
                              Resources.LastNames.ResourceManager.GetObject("LastName" + rand.Next(1, 4)).ToString();
        }

        public Customer(int money, Texture2D sprite, Vector2 start, Vector2 drawStart)
        {
            //starting money for this customer.
            this.money = money;
            this.actualMoney = money;
            this.mapSprite = sprite;
            this.drawStart = drawStart;
            screenPosition = start;
            gridPosition = GameGlobal.player.PlayerStore.DoorPosition;
            inventory = new List<CustomerItem>();
            pathToShelf = new List<Vector2>();
            name = Resources.Names.ResourceManager.GetObject("Name" + rand.Next(1, 4)).ToString() + " " +
                   Resources.LastNames.ResourceManager.GetObject("LastName" + rand.Next(1,4)).ToString();
        }

        public void initCustomer(bool type)
        {
            custType = type;
            if (custType)
            {
                //generate prefered items (items the customer will search for)
                //generate a random number to determine how many prefered items the customer has (max 5)
                int preferedItems = rand.Next(1, 6);

                //get all values in database
                DataTable dt = dbc.runQuery("SELECT * FROM " + GameGlobal.player.TableName + " WHERE Demand >= 80;");

                //for each prefered item
                for (int i = 0; i < preferedItems; i++)
                {
                    //pull a random value from the table
                    int rIndex = rand.Next(0, dt.Rows.Count);

                    int isininv = inventory.FindIndex(delegate(CustomerItem ci)
                    { return ci.item.Name == dt.Rows[rIndex].ItemArray[1].ToString().Trim(); });
                    //add to list
                    if (dt.Rows.Count > 0)
                    {
                        if (isininv == -1) //if it is not already in the inventory, add it.
                        {
                            inventory.Add(new CustomerItem(new Database.ShelfItem(dt.Rows[rIndex].ItemArray[1].ToString().Trim(),
                                0, (int)dt.Rows[rIndex].ItemArray[6], (float)dt.Rows[rIndex].ItemArray[10],
                                (short)dt.Rows[rIndex].ItemArray[4]), false));
                        }
                    }
                }

                //unload unneeded stuff
                dt.Dispose();

                //search for the first item.
                ItemSearch();
            }
        }

        public void updateCustomer(GameTime gt)
        {
            //generate the time for the customer to remain in the store.
            if (generateExitTime)
            {
                exitStoreTime = 1;//rand.Next(3, 8);
                generateExitTime = false;
            }
            else //if time is already generated
            {
                // update the time in store.
                if (!isExiting)
                {
                    if (timeInStore < exitStoreTime)
                    {
                        //update time in store.
                        timeInStore += (float)gt.ElapsedGameTime.TotalMinutes;
                    }
                    else
                    { //exit the store.
                        isExiting = true;
                        timeInStore = 0;
                        //get a path to the exit.
                        pathToShelf = generatePath(-1, true);
                    }

                    //exit the store if the customer is done searching for prefered items before time runs out.
                    if (custType && searchIndex == -1)
                    {
                        isExiting = true;
                        timeInStore = 0;
                        //exit the store.
                        pathToShelf = generatePath(-1, true);
                    }
                }
            }

            if (!isExiting)
            {
                if (custType) //smart AI
                {
                    //if there is no path but the customer has prefered items, search for prefered items
                    //and generate an appropriate path.
                    if (!GetPathStatus && !enterShelf)
                    {
                        //search for the items they want if there are still items they havent searched for
                        if (searchIndex < inventory.Count)
                        {
                            ItemSearch();
                        }
                        else //if they searched for all the items they wanted, switch to random browsing.
                        {
                            custType = false;
                        }
                    }
                    //if the next grid space is reached, calculate the movement vector and update target.
                    if (UpdateRelayPoint())
                    {
                        moveAmt = MoveSprite((float)gt.ElapsedGameTime.TotalMilliseconds);
                    }
                }
                else //random AI
                {
                    //if the customer reached the next grid space, randomize the next space to move to.
                    if (relayPointHit)
                    {
                        //if there is a nearby shelf
                        if (NearestShelf(gridPosition) > -1)
                        {
                            //and the customer was not recently in a shelf.
                            if (!lastInShelf)
                            {
                                //allow them to enter a new shelf.
                                enterShelf = true;
                                generateTime = true;
                            }
                        }
                        else //if there isnt a nearby shelf
                        {
                            if (!enterShelf) //and not currently in a shelf
                            {
                                //update movement pattern
                                RandomizeMovement();
                                moveAmt = MoveSprite((float)gt.ElapsedGameTime.TotalMilliseconds);
                                //update counter so customer doesn't repeated enter and exit same shelf.
                                if (lastInShelfCounter < 2)
                                {
                                    lastInShelfCounter++;
                                }
                                else if (lastInShelfCounter == 2)
                                {
                                    lastInShelf = false;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //If the customer does not yet have an exit path - generate one
                if (!GetPathStatus)
                {
                    //customer reached the end, kills him (and buy items with small chance to steal)
                    PurchaseItems();
                    exited = true;
                }
                //if the next grid space is reached, calculate the movement vector and update target.
                if (UpdateRelayPoint())
                {
                    moveAmt = MoveSprite((float)gt.ElapsedGameTime.TotalMilliseconds);
                }       
            }

            //enter shelf logic
            if (enterShelf)
            {
                //generate a random time to be in the shelf
                if (generateTime)
                {
                    exitShelfTime = (float)rand.Next(3, 13);
                    generateTime = false;
                }
                else
                {
                    // update the time in shelf.
                    if (timeInShelf < exitShelfTime)
                    {
                        timeInShelf += (float)gt.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    { //exit the shelf.
                        enterShelf = false;
                        lastInShelf = true;
                        generateTime = true;
                        lastInShelfCounter = 0;
                        timeInShelf = 0;
                        //take items from the shelf
                        TakeItems();

                        //if not looking for any items, exit store
                        if (searchIndex == -1)
                        {
                            pathToShelf = generatePath(-1, isExiting);
                            currentPathIndex = 0;
                        }
                    }
                }
            }
            else
            {
                switch (GameGlobal.player.TimeInGame.GameMoveSpeed)
                {
                    case GameLogic.Chronology.GameSpeed.Pause:

                        break;
                    case GameLogic.Chronology.GameSpeed.Normal:
                    case GameLogic.Chronology.GameSpeed.Fast:
                    default:
                        //update the position of the customer based on the movement vector.
                        screenPosition.X += moveAmt.X;
                        screenPosition.Y += moveAmt.Y;
                        if ((int)screenPosition.X >= ((int)nextScreenPos.X - 1) &&
                            (int)screenPosition.X <= ((int)nextScreenPos.X + 1) &&
                            (int)screenPosition.Y >= ((int)nextScreenPos.Y - 1) &&
                            (int)screenPosition.Y <= ((int)nextScreenPos.Y + 1))
                        { //if the customer reaches the next space, trigger an update to find next grid space.
                            relayPointHit = true;
                            if (!custType)
                            { //update the current position for random movement.
                                gridPosition = nextGridPosition;
                            }
                        }
                        break;
                }
            }
        }

        public void drawCustomer(SpriteBatch sb)
        {
            if (!enterShelf)
            {
                sb.Begin();
                //sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, 
                //    SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
                sb.Draw(mapSprite, screenPosition, Color.White);
                sb.End();
            }
        }

        #region Pathfinding

        private void ItemSearch()
        {
            itemIndex = -1;
            shelfIndex = -1;
            while (searchIndex < inventory.Count && searchIndex >= 0)
            {
                foreach (Rooms.Room r in GameGlobal.player.PlayerStore.RoomList)
                {
                    shelfIndex++;
                    //Display room object for casting.
                    Rooms.DisplayRoom dr;
                    //if the store type is 1, it is a display room
                    if (r.RoomType() == 1)
                    {
                        //cast it to a display room
                        dr = (Rooms.DisplayRoom)r;
                    }
                    else { continue; } //if not a display room, move to the next room in the list.
                    //get the index of the item the function is searching for.
                    itemIndex = dr.Inventory.FindIndex(
                            delegate(Database.ShelfItem si)
                            {
                                return si.Name.Trim() == inventory[searchIndex].item.Name;
                            });
                    if (itemIndex >= 0) { break; }
                }

                if (shelfIndex > -1 && itemIndex > -1)
                {
                    pathToShelf = generatePath(shelfIndex, false);
                    break;
                }
                else
                {
                    //item not found, move to the next item.
                    searchIndex++;
                    //reset
                    itemIndex = -1;
                    shelfIndex = -1;
                }
            }
        }

        private List<Vector2> generatePath(int index, bool exiting)
        {
            //find a compatible browse spot
            Vector2 dest;
            if (exiting)
            {
                dest = GameGlobal.player.PlayerStore.DoorPosition;
            }
            else
            {
                dest = findDestination(index);
            }

            List<Vector2> path = new List<Vector2>();
            List<AStarGrid> closedList = new List<AStarGrid>();
            List<AStarGrid> openList = new List<AStarGrid>();
            int lowestIndex = 0;
            int closedIndex = 0;
            
            AStarGrid temp;

            //add starting point to open list
            openList.Add(new AStarGrid(gridPosition, Vector2.Zero, 0,0,0));

            do
            {
                //if open list is empty, stop loop
                if (openList.Count == 0)
                {
                    break;
                }
                //find lowest F cost for all squares in open list,
                //add it to the closeList and remove it from the open list.
                closedList.Add(findMinimumF(ref openList, out lowestIndex));

                //if the target is moved to the closed list, end the loop
                if (closedList.FindIndex(delegate(AStarGrid space) { return space.Position == dest; }) >= 0)
                {
                    break;
                }

                openList.RemoveAt(lowestIndex);
                //search all adajcent grid spaces and calculate F scores
                #region Adajcent Space Search
                //search left side
                #region Left and Right Side
                for (int x = (int)closedList[closedIndex].Position.X - 1;
                    x <= (int)closedList[closedIndex].Position.X + 1; x++)
                {
                    //check out of bounds and current square
                    if (x == (int)closedList[closedIndex].Position.X) { continue; }
                    if (x < 0 || x >= GameGlobal.player.PlayerStore.BuildingArray.GetLength(1)) { continue; }
                    //if the space is empty, it is available to walk in, calulate scores
                    if (GameGlobal.player.PlayerStore.BuildingArray[(int)closedList[closedIndex].Position.Y, x] == true)
                    {
                        //if the space is not already in the closed list
                        if (closedList.FindIndex(delegate(AStarGrid space) { return space.Position == new Vector2(x, closedList[closedIndex].Position.Y); }) < 0)
                        {
                            int G, H;
                            //calculate G and H values
                            G = closedList[closedIndex].G_Value + OrthagonalD;
                            H = CalcHVal(new Vector2(x, (int)closedList[closedIndex].Position.Y), dest);
                            //create the grid space with F, G, and H values filled in and attached to a parent space
                            temp = new AStarGrid(new Vector2(x, (int)closedList[closedIndex].Position.Y),
                                closedList[closedIndex].Position, G + H, G, H);
                            //check to see if this node is already on the open list
                            int isinList = openList.FindIndex(
                                delegate(AStarGrid gridSpace)
                                {
                                    return gridSpace.Position == temp.Position;
                                });
                            //add it to the open list if it is not in the list.
                            if (isinList < 0)
                            {
                                openList.Add(temp);
                            }
                            else // if it is in the list check to see if the current path is less then the old path.
                            { //if it is, change the parent to the current node being examined and redo values.
                                if (openList[isinList].G_Value > temp.G_Value)
                                {
                                    //store the grid space in a temp var
                                    AStarGrid LOLTEMPSPACE = openList[isinList];
                                    //remove the old one from open list
                                    openList.RemoveAt(isinList);
                                    //reset parent
                                    LOLTEMPSPACE.Parent = closedList[closedIndex].Position;
                                    //get new f, g and h values
                                    if (LOLTEMPSPACE.Position.X == closedList[closedIndex].Position.X ||
                                        LOLTEMPSPACE.Position.Y == closedList[closedIndex].Position.Y)
                                    {
                                        LOLTEMPSPACE.G_Value = closedList[closedIndex].G_Value + OrthagonalD;
                                    }
                                    LOLTEMPSPACE.H_Value = CalcHVal(LOLTEMPSPACE.Position, dest);
                                    LOLTEMPSPACE.F_Value = LOLTEMPSPACE.G_Value + LOLTEMPSPACE.H_Value;
                                    //add it back to the open list
                                    openList.Add(LOLTEMPSPACE);
                                }
                            }
                        }
                    }
                }
                #endregion
                
                //Check top and bottom sides
                #region Top and Bottom
                //check top & bottom nodes node
                for (int y = (int)closedList[closedIndex].Position.Y - 1;
                    y <= (int)closedList[closedIndex].Position.Y + 1; y++)
                {
                    //dont check current node
                    if (y == (int)closedList[closedIndex].Position.Y) { continue; }
                    if (y < 0 || y >= GameGlobal.player.PlayerStore.BuildingArray.GetLength(0)) { continue; }
                    //if the space is empty, it is available to walk in, calulate scores
                    if (GameGlobal.player.PlayerStore.BuildingArray[y, (int)closedList[closedIndex].Position.X] == true)
                    {
                        //if the space is not already in the closed list
                        if (closedList.FindIndex(delegate(AStarGrid space) { return space.Position == new Vector2((int)closedList[closedIndex].Position.X, y); }) < 0)
                        {
                            int G, H;
                            //calculate G and H values
                            G = closedList[closedIndex].G_Value + OrthagonalD;
                            H = CalcHVal(new Vector2(closedList[closedIndex].Position.X, y), dest);
                            //create the grid space with F, G, and H values filled in and attached to a parent space
                            temp = new AStarGrid(new Vector2(closedList[closedIndex].Position.X, y),
                                closedList[closedIndex].Position, G + H, G, H);
                            //check to see if this node is already on the open list
                            int isinList = openList.FindIndex(
                                delegate(AStarGrid gridSpace)
                                {
                                    return gridSpace.Position == temp.Position;
                                });
                            //add it to the open list if it is not in the list.
                            if (isinList == -1)
                            {
                                openList.Add(temp);
                            }
                            else // if it is in the list check to see if the current path is less then the old path.
                            { //if it is, change the parent to the current node being examined and redo values.
                                if (openList[isinList].G_Value > temp.G_Value)
                                {
                                    //store the grid space in a temp var
                                    AStarGrid LOLTEMPSPACE = openList[isinList];
                                    //remove the old one from open list
                                    openList.RemoveAt(isinList);
                                    //reset parent
                                    LOLTEMPSPACE.Parent = closedList[closedIndex].Position;
                                    //get new f, g and h values
                                    if (LOLTEMPSPACE.Position.X == closedList[closedIndex].Position.X ||
                                        LOLTEMPSPACE.Position.Y == closedList[closedIndex].Position.Y)
                                    {
                                        LOLTEMPSPACE.G_Value = closedList[closedIndex].G_Value + OrthagonalD;
                                    }
                                    LOLTEMPSPACE.H_Value = CalcHVal(LOLTEMPSPACE.Position, dest);
                                    LOLTEMPSPACE.F_Value = LOLTEMPSPACE.G_Value + LOLTEMPSPACE.H_Value;
                                    //add it back to the open list
                                    openList.Add(LOLTEMPSPACE);
                                }
                            }
                        }
                    }
                }
                #endregion
                #endregion

                closedIndex++;

            } while (true);

            //create the path
            openList.Clear();

            //build the path, starting from the destination and tracing the parents back to the start.
            Stack<AStarGrid> reverse = new Stack<AStarGrid>();
            //add the destination to the stack
            reverse.Push(closedList[closedIndex]);
            //loop through the closed list and trace the parents back to the start
            while (closedList.FindIndex(delegate(AStarGrid space) { return space.Position == gridPosition; }) >= 0)
            {
                //get the index of the parent
                closedIndex = closedList.FindIndex(delegate(AStarGrid space) 
                { return space.Position == closedList[closedIndex].Parent; });
                //push the parent into the stack.
                if (closedIndex == -1) { break; }
                reverse.Push(closedList[closedIndex]);
            }

            //now that path is created, flip the stack onto a list
            do
            {
                path.Add(reverse.Pop().Position);
            } while (reverse.Count > 0);

            return path;
        }

        /// <summary>
        /// Calculate the Distance between the current node and destination node
        /// using the Chebyshev distance formula.
        /// </summary>
        /// <param name="start">Current grid space being examined.</param>
        /// <param name="dest">Destination grid space.</param>
        /// <returns>An integer value estimating the distance between the two grid spaces.</returns>
        private int CalcHVal(Vector2 start, Vector2 dest)
        {
            int h = 0;
            int disX, disY;
            disX = Math.Abs((int)start.X - (int)dest.X);
            disY = Math.Abs((int)start.Y - (int)dest.Y);
            h = (OrthagonalD * (disX + disY));
            return h;
        }

        private Vector2 findDestination(int shelfIndex)
        {
            List<Vector2> compatible = new List<Vector2>();

            Vector2 roomLocation = GameGlobal.player.PlayerStore.RoomList[shelfIndex].RoomLocation;
            Vector2 roomSize = GameGlobal.player.PlayerStore.RoomList[shelfIndex].TileDimensions;

            //check sides (not diagonals) of shelf
            //check left side
            for (int y = (int)roomLocation.Y; y < (int)roomLocation.Y + roomSize.Y; y++)
            {
                //if the y index causes out of bounds, stop loop
                if (y >= GameGlobal.player.PlayerStore.BuildingArray.GetLength(0)) { break; }
                //if the x index is out of bounds, dont check left side.
                if (roomLocation.X - 1 < 0) { break; }

                if (GameGlobal.player.PlayerStore.BuildingArray[y, (int)roomLocation.X - 1] == true)
                {
                    compatible.Add(new Vector2(roomLocation.X - 1, y));
                }
            }
            //check right side
            for (int y = (int)roomLocation.Y; y < (int)roomLocation.Y + roomSize.Y; y++)
            {
                //if the y index causes out of bounds, stop loop
                if (y >= GameGlobal.player.PlayerStore.BuildingArray.GetLength(0)) { break; }
                //if the x index is out of bounds, dont check right side.
                if (roomLocation.X + (int)roomSize.X >= GameGlobal.player.PlayerStore.BuildingArray.GetLength(1)) { break; }

                if (GameGlobal.player.PlayerStore.BuildingArray[y, (int)roomLocation.X + (int)roomSize.X] == true)
                {
                    compatible.Add(new Vector2(roomLocation.X + roomSize.X, y));
                }
            }
            //check top side
            for (int x = (int)roomLocation.X; x < (int)roomLocation.X + roomSize.X; x++)
            {
                //if the x index causes out of bounds, stop loop
                if (x >= GameGlobal.player.PlayerStore.BuildingArray.GetLength(1)) { break; }
                //if the y index is out of bounds, dont check top side.
                if (roomLocation.Y - 1 < 0) { break; }

                if (GameGlobal.player.PlayerStore.BuildingArray[(int)roomLocation.Y - 1, x] == true)
                {
                    compatible.Add(new Vector2(x, (int)roomLocation.Y - 1));
                }
            }
            //check bottom side
            for (int x = (int)roomLocation.X; x < (int)roomLocation.X + roomSize.X; x++)
            {
                //if the x index causes out of bounds, stop loop
                if (x >= GameGlobal.player.PlayerStore.BuildingArray.GetLength(1)) { continue; }
                //if the y index is out of bounds, dont check top side.
                if (roomLocation.Y + (int)roomSize.Y >= GameGlobal.player.PlayerStore.BuildingArray.GetLength(0)) { break; }

                if (GameGlobal.player.PlayerStore.BuildingArray[(int)roomLocation.Y + (int)roomSize.Y, x] == true)
                {
                    compatible.Add(new Vector2(x, (int)roomLocation.Y + (int)roomSize.Y));
                }
            }
            
            return compatible[rand.Next(0,compatible.Count)];
        }

        private AStarGrid findMinimumF(ref List<AStarGrid> open, out int lIndex)
        {
            AStarGrid lowest = open[0];
            lIndex = 0;
            foreach (AStarGrid asg in open)
            {
                if (asg.F_Value < lowest.F_Value)
                {
                    lowest = asg;
                    lIndex = open.IndexOf(asg);
                }
            }

            //open.RemoveAll(delegate(AStarGrid space)
            //{
            //    return space.F_Value == lowest.F_Value;
            //});

            return lowest;
        }     

        private bool UpdateRelayPoint()
        {
            //if the customer reaches the next grid space, calculate path to next sqaure
            if (relayPointHit)
            {
                if (pathToShelf.Count > 0)
                {
                    gridPosition = pathToShelf[currentPathIndex];
                    //get a location on the next space in the path
                    if (currentPathIndex + 1 >= pathToShelf.Count)
                    {
                        //path is finished.
                        pathToShelf.Clear();
                        moveAmt = Vector2.Zero;
                        enterShelf = true;
                        generateTime = true;
                        return false;
                    }         
                    nextGridPosition = pathToShelf[currentPathIndex + 1];
                    nextScreenPos.X = rand.Next((int)drawStart.X + (GameGlobal.TILESIZE * (int)nextGridPosition.X),
                        ((int)drawStart.X + (GameGlobal.TILESIZE * (int)nextGridPosition.X)) + (GameGlobal.TILESIZE / 2));
                    nextScreenPos.Y = rand.Next((int)drawStart.Y + (GameGlobal.TILESIZE * (int)nextGridPosition.Y),
                        ((int)drawStart.Y + (GameGlobal.TILESIZE * (int)nextGridPosition.Y)) + (GameGlobal.TILESIZE / 2));
                    relayPointHit = false;
                    currentPathIndex++;
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Random Browsing

        private void AllocatePercents()
        {
            int count = 0;
            for (int x = 0; x < 4; x++)
            {
                if (checkedDirection[x] == true)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                movePercentages[0] = 65;
                movePercentages[1] = 12.5f;
                movePercentages[2] = 12.5f;
                movePercentages[3] = 10;
            }
            else if (count == 1)
            {
                if (checkedDirection[0])
                {
                    movePercentages[0] = 0;
                    movePercentages[1] = 45f;
                    movePercentages[2] = 45f;
                    movePercentages[3] = 10;
                }
                else if (checkedDirection[1])
                {
                    movePercentages[0] = 65;
                    movePercentages[1] = 0;
                    movePercentages[2] = 25f;
                    movePercentages[3] = 10;
                }
                else if (checkedDirection[2])
                {
                    movePercentages[0] = 65;
                    movePercentages[1] = 25;
                    movePercentages[2] = 0f;
                    movePercentages[3] = 10;
                }
            }
            else if (count == 2)
            {
                if (!checkedDirection[0])
                {
                    movePercentages[0] = 90;
                    movePercentages[1] = 0;
                    movePercentages[2] = 0;
                    movePercentages[3] = 10;
                }
                else
                {
                    if (checkedDirection[1])
                    {
                        movePercentages[0] = 0;
                        movePercentages[1] = 0;
                        movePercentages[2] = 90;
                        movePercentages[3] = 10;
                    }
                    else if (checkedDirection[2])
                    {
                        movePercentages[0] = 0;
                        movePercentages[1] = 90;
                        movePercentages[2] = 0;
                        movePercentages[3] = 10;
                    }
                }
            }
            else if (count == 3)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (checkedDirection[x] == false)
                    {
                        movePercentages[x] = 100;
                        break;
                    }
                }
            }
        } 

        private bool RandomizeMovement()
        {
            bool exitLoop = false;
            do
            {
                float chance = (float)rand.NextDouble() * 100;
                if (chance < movePercentages[0])
                {
                    if (!checkedDirection[0])
                    {
                        if (GridCheck(DirectionAdjustment(0)))
                        {
                            nextScreenPos.X = rand.Next((int)drawStart.X + (GameGlobal.TILESIZE * (int)nextGridPosition.X),
                                ((int)drawStart.X + (GameGlobal.TILESIZE * (int)nextGridPosition.X)) + (GameGlobal.TILESIZE / 2));
                            nextScreenPos.Y = rand.Next((int)drawStart.Y + (GameGlobal.TILESIZE * (int)nextGridPosition.Y),
                                ((int)drawStart.Y + (GameGlobal.TILESIZE * (int)nextGridPosition.Y)) + (GameGlobal.TILESIZE / 2));
                            relayPointHit = false;
                            exitLoop = true;
                        }
                        else
                        {
                            checkedDirection[0] = true;
                            AllocatePercents();
                        }    
                    }
                }
                else if (chance >= movePercentages[0] && chance < (movePercentages[0] + movePercentages[1]))
                {
                    if (!checkedDirection[1])
                    {
                        if (GridCheck(DirectionAdjustment(1)))
                        {
                            nextScreenPos.X = rand.Next((int)drawStart.X + (GameGlobal.TILESIZE * (int)nextGridPosition.X),
                                ((int)drawStart.X + (GameGlobal.TILESIZE * (int)nextGridPosition.X)) + (GameGlobal.TILESIZE / 2));
                            nextScreenPos.Y = rand.Next((int)drawStart.Y + (GameGlobal.TILESIZE * (int)nextGridPosition.Y),
                                ((int)drawStart.Y + (GameGlobal.TILESIZE * (int)nextGridPosition.Y)) + (GameGlobal.TILESIZE / 2));
                            relayPointHit = false;
                            exitLoop = true;
                        }
                        else
                        {
                            checkedDirection[1] = true;
                            AllocatePercents();
                        }
                    }
                }
                else if (chance >= (movePercentages[0] + movePercentages[1]) &&
                    chance < (movePercentages[0] + movePercentages[1] + movePercentages[2]))
                {
                    if (!checkedDirection[2])
                    {
                        if (GridCheck(DirectionAdjustment(2)))
                        {
                            nextScreenPos.X = rand.Next((int)drawStart.X + (GameGlobal.TILESIZE * (int)nextGridPosition.X),
                                ((int)drawStart.X + (GameGlobal.TILESIZE * (int)nextGridPosition.X)) + (GameGlobal.TILESIZE / 2));
                            nextScreenPos.Y = rand.Next((int)drawStart.Y + (GameGlobal.TILESIZE * (int)nextGridPosition.Y),
                                ((int)drawStart.Y + (GameGlobal.TILESIZE * (int)nextGridPosition.Y)) + (GameGlobal.TILESIZE / 2));
                            relayPointHit = false;
                            exitLoop = true;
                        }
                        else
                        {
                            checkedDirection[2] = true;
                            AllocatePercents();
                        }
                    }
                }
                else
                {
                    if (!checkedDirection[3])
                    {
                        if (GridCheck(DirectionAdjustment(3)))
                        {
                            nextScreenPos.X = rand.Next((int)drawStart.X + (GameGlobal.TILESIZE * (int)nextGridPosition.X),
                                ((int)drawStart.X + (GameGlobal.TILESIZE * (int)nextGridPosition.X)) + (GameGlobal.TILESIZE / 2));
                            nextScreenPos.Y = rand.Next((int)drawStart.Y + (GameGlobal.TILESIZE * (int)nextGridPosition.Y),
                                ((int)drawStart.Y + (GameGlobal.TILESIZE * (int)nextGridPosition.Y)) + (GameGlobal.TILESIZE / 2));
                            relayPointHit = false;
                            exitLoop = true;
                        }
                        else
                        {
                            checkedDirection[3] = true;
                            AllocatePercents();
                        }
                    }
                }
            } while (!exitLoop);

            if (exitLoop)
            {
                for (int x = 0; x < 4; x++)
                {
                    checkedDirection[x] = false;
                }
                AllocatePercents();
                return true;
            }
            
            return false;
        }

        private byte DirectionAdjustment(byte directionCheck)
        {
            switch (previousDirection)
            {
                case 0:
                default:
                    switch (directionCheck)
                    {
                        case 0:
                        default:
                            return 0;
                        case 1:
                            return 1;
                        case 2:
                            return 2;
                        case 3:
                            return 3;
                    }
                case 1:
                    switch (directionCheck)
                    {
                        case 0:
                        default:
                            return 1;
                        case 1:
                            return 3;
                        case 2:
                            return 1;
                        case 3:
                            return 2;
                    }
                case 2:
                    switch (directionCheck)
                    {
                        case 0:
                        default:
                            return 2;
                        case 1:
                            return 0;
                        case 2:
                            return 3;
                        case 3:
                            return 1;
                    }
                case 3:
                    switch (directionCheck)
                    {
                        case 0:
                        default:
                            return 3;
                        case 1:
                            return 2;
                        case 2:
                            return 1;
                        case 3:
                            return 0;
                    }
            }
        }

        private bool GridCheck(byte directionCheck)
        {
            switch (directionCheck)
            {
                case 0:
                default:
                    if ((int)gridPosition.Y - 1 >= 0)
                    {
                        if (GameGlobal.player.PlayerStore.BuildingArray[(int)gridPosition.Y - 1,
                            (int)gridPosition.X] == true)
                        {
                            nextGridPosition = new Vector2(gridPosition.X, gridPosition.Y - 1);
                            previousDirection = 0;
                            return true;
                        }
                    }
                    break;
                case 1:
                    if ((int)gridPosition.X - 1 >= 0)
                    {
                        if (GameGlobal.player.PlayerStore.BuildingArray[(int)gridPosition.Y,
                            (int)gridPosition.X - 1] == true)
                        {
                            nextGridPosition = new Vector2(gridPosition.X - 1, gridPosition.Y);
                            previousDirection = 1;
                            return true;
                        }
                    }
                    break;
                case 2:
                    if ((int)gridPosition.X + 1 < GameGlobal.player.PlayerStore.BuildingArray.GetLength(1))
                    {
                        if (GameGlobal.player.PlayerStore.BuildingArray[(int)gridPosition.Y,
                            (int)gridPosition.X + 1] == true)
                        {
                            nextGridPosition = new Vector2(gridPosition.X + 1, gridPosition.Y);
                            previousDirection = 2;
                            return true;
                        }
                    }
                    break;
                case 3:
                    if ((int)gridPosition.Y + 1 < GameGlobal.player.PlayerStore.BuildingArray.GetLength(0))
                    {
                        if (GameGlobal.player.PlayerStore.BuildingArray[(int)gridPosition.Y + 1,
                            (int)gridPosition.X] == true)
                        {
                            nextGridPosition = new Vector2(gridPosition.X, gridPosition.Y + 1);
                            previousDirection = 3;
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }

        public int NearestShelf(Vector2 location)
        {
            int counter = -1;
            foreach (Rooms.Room rm in GameGlobal.player.PlayerStore.RoomList)
            {
                counter++;
                if (rm.RoomType() == 1)
                {
                    //check if cusotmer is next to this shelf
                    //check below the shelf
                    if ((int)gridPosition.Y == (int)rm.RoomLocation.Y - 1 &&
                        (int)gridPosition.X >= (int)rm.RoomLocation.X &&
                        (int)gridPosition.X < (int)rm.RoomLocation.X + (int)rm.TileDimensions.X)
                    {
                        if (EnterShelfProb())
                        {
                            shelfIndex = counter;
                            return counter;
                        }
                    }

                    //check above the shelf
                    if ((int)gridPosition.Y == (int)rm.RoomLocation.Y + 1 &&
                        (int)gridPosition.X >= (int)rm.RoomLocation.X &&
                        (int)gridPosition.X < (int)rm.RoomLocation.X + (int)rm.TileDimensions.X)
                    {
                        if (EnterShelfProb())
                        {
                            shelfIndex = counter;
                            return counter;
                        }
                    }

                    //check to left the shelf
                    if ((int)gridPosition.X == (int)rm.RoomLocation.X - 1 &&
                        (int)gridPosition.Y >= (int)rm.RoomLocation.Y &&
                        (int)gridPosition.Y < (int)rm.RoomLocation.Y + (int)rm.TileDimensions.Y)
                    {
                        if (EnterShelfProb())
                        {
                            shelfIndex = counter;
                            return counter;
                        }
                    }

                    //check to right the shelf
                    if ((int)gridPosition.X == (int)rm.RoomLocation.X + 1 &&
                        (int)gridPosition.Y >= (int)rm.RoomLocation.Y &&
                        (int)gridPosition.Y < (int)rm.RoomLocation.Y + (int)rm.TileDimensions.Y)
                    {
                        if (EnterShelfProb())
                        {
                            shelfIndex = counter;
                            return counter;
                        }
                    }
                }
            }
            counter = -1;
            return counter;
        }

        private bool EnterShelfProb()
        {
            int flip = rand.Next(0, 2);
            if (flip == 0) return false;
            else return true;
        }

        #endregion

        /// <summary>
        /// Generates the amount to move that frame between the current gird space and the next grid space.
        /// </summary>
        /// <param name="millis">The time in milliseconds since the last frame.</param>
        /// <returns>A vector representing how much to move in the x and y directions.</returns>
        private Vector2 MoveSprite(float millis)
        {
            Vector2 moveAmount = new Vector2();

            //get the x line and y line from the current grid space and the space to move to.
            moveAmount.X = nextScreenPos.X - screenPosition.X;
            moveAmount.Y = nextScreenPos.Y - screenPosition.Y;

            //get the length of the hypotenuse of that triangle.
            float dis = (float)Math.Sqrt((moveAmount.X * moveAmount.X) + (moveAmount.Y * moveAmount.Y));

            //divide the x and y by hypotenuse length.
            moveAmount.X /= dis;
            moveAmount.Y /= dis;

            //move the player by the speed amount (based on time state)
            if (GameGlobal.player.TimeInGame.GameMoveSpeed == Chronology.GameSpeed.Normal)
            {
                moveAmount.X *= SPEED;
                moveAmount.Y *= SPEED;
            }
            else
            {
                moveAmount.X *= FASTSPEED;
                moveAmount.Y *= FASTSPEED;
            }

            //multiply by the time span
            moveAmount.X *= millis;
            moveAmount.Y *= millis;

            //divide by 1000 to get small increment amounts to create smooth movement.
            moveAmount.X /= 1000;
            moveAmount.Y /= 1000;

            //normalize vector
            moveAmount.Normalize();

            //reaccount for speed
            if (GameGlobal.player.TimeInGame.GameMoveSpeed == Chronology.GameSpeed.Normal)
            {
                moveAmount.X *= (SPEED / 20f);
                moveAmount.Y *= (SPEED / 20f);
            }
            else
            {
                moveAmount.X *= (FASTSPEED / 20f);
                moveAmount.Y *= (FASTSPEED / 20f);
            }

            //return the movement vector.
            return moveAmount;
        }

        private void TakeItems()
        {
            if (custType)
            { //has preferred items, take which one they are currently looking for, remove once it is grabbed.
                if (shelfIndex > -1 && itemIndex > -1)
                {
                    //if the customer can afford the item
                    if (CanAffordItem(shelfIndex, itemIndex))
                    {  //GOING TO NEED A CHECK TO DETERMINE ITEMS ARE STILL LEFT
                        //add 1 to the customers inventory of that item
                        inventory[searchIndex].item.OnShelf += 1;
                        //flag it as in inventory if not already
                        if (inventory[searchIndex].inInv == false)
                        {
                            CustomerItem c = inventory[searchIndex];
                            c.inInv = true;
                            inventory[searchIndex] = c;
                        }
                        //update the room and item information.
                        Rooms.DisplayRoom temp = (Rooms.DisplayRoom)GameGlobal.player.PlayerStore.RoomList[shelfIndex];
                        float wUpdate = temp.Inventory[itemIndex].Weight / temp.Inventory[itemIndex].OnShelf;
                        temp.Inventory[itemIndex].OnShelf -= 1;
                        temp.Inventory[itemIndex].Weight = temp.Inventory[itemIndex].OnShelf * wUpdate;
                        //update in the database
                        dbc.UpdateDatabaseInStock(temp.Inventory[itemIndex].Name.Trim(), GameGlobal.player.TableName);

                        //if that was the last item on the shelf
                        if (temp.Inventory[itemIndex].OnShelf == 0)
                        {
                            temp.Inventory.RemoveAt(itemIndex);
                        }
                        GameGlobal.player.PlayerStore.RoomList[shelfIndex] = temp;

                        //move to next item to search for.
                        if (searchIndex == inventory.Count - 1)
                        {
                            searchIndex = -1; //done searching
                            isExiting = true;
                        }
                        else
                        {
                            searchIndex++;
                            ItemSearch();
                        }
                    }
                }
            }
            else
            { //no preferred items, chance to purchase items based on current demand
                if (shelfIndex > -1)
                {
                    Rooms.DisplayRoom temp = (Rooms.DisplayRoom)GameGlobal.player.PlayerStore.RoomList[shelfIndex];
                    int num = temp.Inventory.Count;
                    int chance;
                    for (int x = 0; x < num; x++)
                    { // loop through the available items on the shelf and calculate chance to purchase.
                        chance = rand.Next(0, 100);
                        if (chance < temp.Inventory[x].CurrentDemand) //take item
                        {
                            if (CanAffordItem(shelfIndex, x))
                            {
                                //update the shelf
                                float wUpdate = temp.Inventory[x].Weight / temp.Inventory[x].OnShelf;
                                temp.Inventory[x].OnShelf -= 1;
                                temp.Inventory[x].Weight = temp.Inventory[x].OnShelf * wUpdate;

                                //update in the database
                                dbc.UpdateDatabaseInStock(temp.Inventory[x].Name, GameGlobal.player.TableName);

                                //update player inventory
                                //see if it is already in the customers inventory
                                int i = inventory.FindIndex(delegate(CustomerItem ci)
                                { return ci.item.Name == temp.Inventory[x].Name; });
                                if (i > -1)
                                { //if the item is found, increment it
                                    inventory[i].item.OnShelf++;
                                }
                                else
                                { //otherwise add it into the inventory.
                                    inventory.Add(new CustomerItem(new Database.ShelfItem(temp.Inventory[x].Name,
                                        1, temp.Inventory[x].Cost, temp.Inventory[x].Weight, 
                                        temp.Inventory[x].CurrentDemand), true));
                                }
                            }
                        }
                    }

                    //remove any that are at zero.
                    temp.Inventory.RemoveAll(delegate(Database.ShelfItem si)
                    { return si.OnShelf == 0; });

                    //reassign to update inventory.
                    GameGlobal.player.PlayerStore.RoomList[shelfIndex] = temp;
                }
            }
        }

        /// <summary>
        /// Determines if the customer can afford the item they are looking for, and if they can subtracts the
        /// cost from their amount of money.
        /// </summary>
        /// <param name="si">The shelf index they are browsing.</param>
        /// <param name="ii">The item index they are loooking at.</param>
        /// <returns>True if the item is affordable, false if not.</returns>
        private bool CanAffordItem(int si, int ii)
        {
            Rooms.DisplayRoom temp = (Rooms.DisplayRoom)GameGlobal.player.PlayerStore.RoomList[shelfIndex];
            if (temp.Inventory[ii].Cost < this.actualMoney)
            {
                this.actualMoney -= temp.Inventory[ii].Cost;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Loop through each item in the player's inventory and add the money to the store's money.
        /// </summary>
        private void PurchaseItems()
        {
            if (rand.Next(0, 100) <= GameGlobal.player.PlayerStore.SecurityValue)
            {
                if (rand.Next(0, 100) <= GameGlobal.player.PlayerStore.StealChance)
                {
                    //steal successful, place event here.
                    //foreach (CustomerItem ci in inventory)
                    //{
                    //    dbc.UpdateDatabaseInStock(ci.item.Name, GameGlobal.player.TableName);
                    //}
                    return;
                }
                else
                {
                    //steal attempted and failed, place event here.
                }
            }
            else
            {
                foreach (CustomerItem ci in inventory)
                {
                    if (ci.inInv)
                    {
                        GameGlobal.player.CurrentMoney += ci.item.Cost;
                    }
                    //dbc.UpdateDatabaseInStock(ci.item.Name, GameGlobal.player.TableName);
                }
            }
        }

        public void unloadCustomer()
        {
            mapSprite.Dispose(); 
            drawStart = Vector2.Zero;
            thoughts = "";
            inventory.Clear();
            dbc = null;     
            screenPosition = Vector2.Zero;
            gridPosition = Vector2.Zero;
            moveAmt = Vector2.Zero;
            pathToShelf.Clear();
            nextGridPosition = Vector2.Zero;
            nextScreenPos = Vector2.Zero;
        }
    }
}