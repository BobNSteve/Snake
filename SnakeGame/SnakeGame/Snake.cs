using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGame
{
    class Snake
    {
        private Ellipse snake;
        private double x;
        private double y;
        int height;
        int width;
        Canvas screen;
        private double moveSpeed;
        List<SnakeParts> snakeParts;
        int snakeLenght;
        public int SnakeLenght { get { return snakeLenght; } }
        public double X { get { return x; } }
        public double Y { get { return y; } }
        public double Speed { get { return moveSpeed; } }
        public int Height { get { return height; } }
        public double Width { get { return width; } }
        public Snake(double moveSpeed, int height, int width, double x, double y, Canvas screen, string imagePath)
        {
            this.moveSpeed = moveSpeed;
            this.height = height;
            this.width = width;
            this.x = x;
            this.y = y;
            this.screen = screen;
            snake = new Ellipse();
            snake.Height = height;
            snake.Width = width;
            snake.Fill = new SolidColorBrush(Colors.DarkGreen);
            snakeLenght = 0;
        }

        public void SnakeEat(Food food)
        {

        }

        public void Update(double x, double y)
        {
            this.x = x;
            this.y = y;

            Canvas.SetLeft(snake, x);
            Canvas.SetTop(snake, y);
        }

        public void MoveBody(double firstX, double firstY)
        {
            if (snakeParts == null)
                snakeParts = new List<SnakeParts>();
            if (snakeParts.Count == 0)
                return;
            for (int i = snakeParts.Count; i > 1; i--)
            {
                snakeParts[i - 1].Update(snakeParts[i - 2].X, snakeParts[i - 2].Y);
            }
            snakeParts[0].Update(firstX, firstY);
        }

        public void Spawn()
        {
            screen.Children.Add(snake);
            Canvas.SetLeft(snake, x);
            Canvas.SetTop(snake, y);
        }
        public void Grow(int growAmount, bool moveLeft)
        {
            SnakeParts snakePart;
            for(int i = 0; i < growAmount; i++)
            {
                snakePart = new SnakeParts(Speed, height, width, x, y, screen);
                snakeParts.Add(snakePart);
                snakePart.Spawn();
                snakeLenght++;
            }
            return;
        }
        public void Remove()
        {
            screen.Children.Remove(snake);
        }
        
        public void Respawn()
        {
            Remove();
            Spawn();
            if (snakeParts == null)
                return;
            foreach (SnakeParts snakePart in snakeParts)
            {
                snakePart.Remove();
                snakePart.Spawn();
            }
        }
        public bool CheckCollision(int tipX, int tipY)
        {
            foreach(SnakeParts snakePart in snakeParts)
            {
                if (tipX > snakePart.X
                    && tipX < snakePart.X + snakePart.Width
                    && tipY > snakePart.Y
                    && tipY < snakePart.Y + snakePart.height)
                    return true;
            }
            return false;
        }
    }
}
