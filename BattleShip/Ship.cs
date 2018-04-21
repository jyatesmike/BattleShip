using System;
namespace BattleShip
{
    public class Ship
    {
        private int size = 0;
        private char marker;
        private String name;

        public Ship(String name, int size, char marker)
        {
            this.size = size;
            this.marker = marker;
            this.name = name;
        }

        public char getMarker()
        {
            return this.marker;
        }

        public int getSize()
        {
            return this.size;
        }

        public String getName()
        {
            return this.name;
        }
    }
}
