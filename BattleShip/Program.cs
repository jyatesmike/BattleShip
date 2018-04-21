using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BattleShip
{
    class Program
    {
        private const int XSIZE = 10;
        private const int YSIZE = 10;

        static void Main(string[] args)
        {
            //Create ships
            List<Ship> ships = new List<Ship>();

            ships.Add(new Ship("Cruiser", 5, 'C'));
            ships.Add(new Ship("BattleShip", 4, 'B'));
            ships.Add(new Ship("Destroyer", 3, 'D'));
            ships.Add(new Ship("Submarine", 3, 'S'));
            ships.Add(new Ship("Tug", 2, 'T'));

            //Setup battleship grid
            BattleShipGrid grid = new BattleShipGrid(XSIZE, YSIZE, ships, true);

            //Place ships on grid
            grid.placeShips();

            //Display grid to console
            grid.display();

        }
    }
}
