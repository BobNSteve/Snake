using System.Windows.Controls;

namespace SnakeGame
{
    internal class SnakeParts : Snake
    {

        public SnakeParts(double moveSpeed, int height, int width, double x, double y, Canvas screen) 
            : base(moveSpeed, height, width, x, y, screen, null)
        {
            
        }

    }
}