using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BattleShip
{
    public class BattleShipGrid
    {
        private char[] values;
        private List<Ship> ships;
        private int xSize = 0;
        private int ySize = 0;
        private int size = 0;
        private bool avoidAdjacentShips = false;
        private enum Orientation
        {
            Horizontal,
            Vertical
        };
        private Random random;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BattleShip.BattleShipGrid"/> class.
        /// </summary>
        /// <param name="xSize">X size.</param>
        /// <param name="ySize">Y size.</param>
        /// <param name="ships">Ships.</param>
        /// <param name="avoidAdjacentShips">If set to <c>true</c> avoid adjacent ships.</param>
        public BattleShipGrid(int xSize, int ySize, List<Ship> ships, bool avoidAdjacentShips)
        {
            this.xSize = xSize;
            this.ySize = ySize;
            this.size = xSize * ySize;
            this.ships = ships;
            this.avoidAdjacentShips = avoidAdjacentShips;

            values = new char[this.size];

            values = Enumerable.Repeat('.', this.size).ToArray();

            random = new Random(Guid.NewGuid().GetHashCode());
        }

        /// <summary>
        /// Display the Battleship Grid
        /// </summary>
        public void display()
        {
            for (int i = 0; i < this.size; i++)
            {

                if (i != 0 && ((i + 1) % this.xSize == 0))
                {
                    Console.WriteLine(values[i].Equals('!') ? '.' : values[i]);
                }
                else
                {
                    Console.Write(values[i].Equals('!') ? '.' : values[i]);
                }
            }
        }

        /// <summary>
        /// Gets the random coordinate.  Check to be sure this coordinate is not already occupied.
        /// </summary>
        /// <returns>The random coordinate.</returns>
        private int getRandomCoordinate()
        {
            int coordinate = random.Next(this.size);

            if (values[coordinate].Equals('.'))
            {
                return coordinate;
            }
            else
            {
                while (!values[coordinate].Equals('.'))
                {
                    coordinate = random.Next(this.size);
                }

                return coordinate;
            }
        }

        /// <summary>
        /// Gets the random orientation.
        /// </summary>
        /// <returns>The random orientation.</returns>
        private Orientation getRandomOrientation()
        {
            return random.Next(10000) % 2 == 0 ? Orientation.Vertical : Orientation.Horizontal;
        }

        /// <summary>
        /// Places the ships on the grid.
        /// </summary>
        public void placeShips()
        {
            foreach (Ship ship in ships)
            {
                Debug.WriteLine("Placing ship {0} of size {1} on the grid", ship.getName(), ship.getSize());
                placeShip(ship);
            }
        }

        /// <summary>
        /// Places the ship.
        /// This algorithm gets a random starting coordinate and orientation.
        /// Then, it checks which direction will not run off the board and picks that direction.  Preference is
        /// left or down.  Alternitive if those run off board is right or up.
        /// It then attemps to occupy all the spaces for the ship.  If it collides with another ship 
        /// (or its surrounding area when configured as such), it will make recursive call to try again.
        /// If avoidAdjacentShips is set, it will mark area around ship so that other ships will not occupy its surrounding
        /// area
        /// </summary>
        /// <returns><c>true</c>, if ship was placed, <c>false</c> otherwise.</returns>
        /// <param name="ship">Ship.</param>
        private bool placeShip(Ship ship)
        {
            bool spotTaken = false;

            int startPosition = getRandomCoordinate();
            Orientation shipOrientation = getRandomOrientation();

            int row = startPosition / this.xSize;
            int col = startPosition % this.xSize;

            Debug.WriteLine("Start Position is " + startPosition);
            Debug.WriteLine("Ship Direction is " + shipOrientation.ToString());
            Debug.WriteLine("Row={0}, Col={1}", row, col);

            if (shipOrientation.Equals(Orientation.Horizontal))
            {
                //See if ship fits going right
                if (ship.getSize() + col <= this.xSize)
                {
                    //See if spots already taken
                    //Because its horizontal we just go incrementally through grid array
                    for (int i = startPosition; i < (startPosition + ship.getSize()); i++)
                    {
                        if (!values[i].Equals('.'))
                        {
                            spotTaken = true;
                            Debug.WriteLine("Ship {0} had collision while trying to place", ship.getName());
                            break;
                        }
                    }

                    if (!spotTaken)
                    {
                        //Place ship here as it fits and no spots already occupied by another ship
                        for (int i = startPosition; i < (startPosition + ship.getSize()); i++)
                        {
                            values[i] = ship.getMarker();

                            if (avoidAdjacentShips)
                            {
                                //Mark area above ship
                                if (row != 0)
                                {
                                    values[i - this.xSize] = '!';
                                }

                                //Mark area below ship
                                if (row != this.ySize - 1)
                                {
                                    values[i + this.xSize] = '!';
                                }

                            }
                        }

                        if (avoidAdjacentShips)
                        {
                            //Mark area to left of ship
                            if (col > 0)
                            {
                                values[startPosition - 1] = '!';
                            }

                            //Mark area to right of ship
                            if (col + ship.getSize() < this.xSize)
                            {
                                values[startPosition + ship.getSize()] = '!';
                            }
                        }

                    }
                }
                //otherwise, it should always fit going left
                else
                {

                    //See if spots already taken
                    //Because its horizontal we just go incrementally through grid array
                    for (int i = startPosition; i > (startPosition - ship.getSize()); i--)
                    {
                        if (!values[i].Equals('.'))
                        {
                            spotTaken = true;
                            Debug.WriteLine("Ship {0} had collision while trying to place", ship.getName());
                            break;
                        }
                    }

                    if (!spotTaken)
                    {
                        //Place ship here as it fits and no spots already occupied by another ship
                        for (int i = startPosition; i > (startPosition - ship.getSize()); i--)
                        {
                            values[i] = ship.getMarker();

                            if (avoidAdjacentShips)
                            {
                                //Mark area above ship
                                if (row != 0)
                                {
                                    values[i - this.xSize] = '!';
                                }

                                //Mark area below ship
                                if (row != this.ySize - 1)
                                {
                                    values[i + this.xSize] = '!';
                                }

                            }
                        }

                        if (avoidAdjacentShips)
                        {
                            //Mark area to left of ship
                            if (col - ship.getSize() >= 0)
                            {
                                values[startPosition - ship.getSize()] = '!';
                            }

                            //Mark area to right of ship
                            if (col < this.xSize - 1)
                            {
                                values[startPosition + 1] = '!';
                            }
                        }
                    }
                }
            }
            else
            {
                //See if ship fits going down
                if (ship.getSize() + row < this.ySize)
                {
                    //See if spots already taken
                    //Because its vertical we check every position in column by adding row size
                    for (int i = startPosition; i <= (startPosition + ((ship.getSize() - 1) * this.xSize)); i = i + xSize)
                    {
                        if (!values[i].Equals('.'))
                        {
                            spotTaken = true;
                            Debug.WriteLine("Ship {0} had collision while trying to place", ship.getName());
                            break;
                        }
                    }

                    if (!spotTaken)
                    {
                        //Place ship here as it fits and no spots already occupied by another ship
                        for (int i = startPosition; i <= (startPosition + ((ship.getSize() - 1) * this.xSize)); i = i + xSize)
                        {
                            values[i] = ship.getMarker();

                            if (avoidAdjacentShips)
                            {
                                //Mark area to left of ship
                                if (col != 0)
                                {
                                    values[i - 1] = '!';
                                }

                                //Mark area to right of ship
                                if (col != this.xSize - 1)
                                {
                                    values[i + 1] = '!';
                                }
                            }
                        }

                        if (avoidAdjacentShips)
                        {
                            //Mark area above ship
                            if (row > 0)
                            {
                                values[startPosition - this.xSize] = '!';
                            }

                            //Mark area below ship
                            if (row != this.ySize - 1)
                            {
                                values[startPosition + (ship.getSize() * this.xSize)] = '!';
                            }
                        }
                    }
                }
                //otherwise, it should always fit going up
                else
                {
                    //See if spots already taken
                    //Because its horizontal we just go incrementally through grid array
                    for (int i = startPosition; i >= (startPosition - ((ship.getSize() - 1) * this.xSize)); i = i - xSize)
                    {
                        if (!values[i].Equals('.'))
                        {
                            spotTaken = true;
                            Debug.WriteLine("Ship {0} had collision while trying to place", ship.getName());
                            break;
                        }
                    }

                    if (!spotTaken)
                    {
                        //Place ship here as it fits and no spots already occupied by another ship
                        for (int i = startPosition; i >= (startPosition - ((ship.getSize() - 1) * this.xSize)); i = i - xSize)
                        {
                            values[i] = ship.getMarker();

                            if (avoidAdjacentShips)
                            {
                                //Mark area to left of ship
                                if (col != 0)
                                {
                                    values[i - 1] = '!';
                                }

                                //Mark area to right of ship
                                if (col != this.xSize - 1)
                                {
                                    values[i + 1] = '!';
                                }
                            }
                        }

                        if (avoidAdjacentShips)
                        {
                            //Mark area below ship (start position is at bottom of ship)
                            if (row < this.ySize - 1)
                            {
                                values[startPosition + this.xSize] = '!';
                            }

                            //Mark area above top of ship
                            if (row - ship.getSize() > 0)
                            {
                                values[startPosition - (ship.getSize() * this.xSize)] = '!';
                            }
                        }
                    }
                }
            }


            //If ship had collision, try again
            if (spotTaken)
            {
                return placeShip(ship);
            }
            else
            {
                return true;
            }

        }
    }
}
