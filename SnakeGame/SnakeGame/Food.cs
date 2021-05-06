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
    class Food
    {
        Ellipse food;
        int height;
        int width;
        int x;
        int y;
        const int AnimationDelay = 4;

        int animationDelayTimer = AnimationDelay;
        int increaseBy = 2;
        int animation;
        bool animationRotate;
        Canvas screen;
        public Food(int height, int width, Canvas screen)
        {
            this.height = height;
            this.width = width;
            this.screen = screen;
            food = new Ellipse();
            food.Height = height;
            food.Width = width;
            food.Fill = new SolidColorBrush(Colors.Yellow);
            animation = 1;
            animationRotate = false;
        }


        public void Spawn(int x, int y)
        {
            screen.Children.Add(food);
            Canvas.SetLeft(food, x);
            Canvas.SetTop(food, y);
            this.x = x;
            this.y = y;
        }

        public bool Inside(Snake snake)
        {
            int midX = x + width / 2;
            int midY = y + height / 2;
            if (midY > snake.Y 
                && midX > snake.X
                && midY < snake.Y + snake.Height
                && midX < snake.X + snake.Width)
            {
                screen.Children.Remove(food);
                return true;
            }

            return false;
        }

        public void Animation()
        {
            if (animationDelayTimer > 0)
            {
                animationDelayTimer--;
                return;
            }
            else
                animationDelayTimer = AnimationDelay;
            if (!animationRotate)
            {
                animation++;
                food.Height = food.Height + (increaseBy);
                food.Width = food.Width + (increaseBy);
                x = x - increaseBy / 2;
                y = y - increaseBy / 2;
                Canvas.SetLeft(food, x);
                Canvas.SetTop(food, y);
                if (animation == 5)
                    animationRotate = true;
            }
            else
            {
                animation--;
                food.Height = food.Height - (increaseBy);
                food.Width = food.Width - (increaseBy);
                x = x + increaseBy / 2;
                y = y + increaseBy / 2;
                Canvas.SetLeft(food, x);
                Canvas.SetTop(food, y);
                if (animation == 1)
                    animationRotate = false;
            }
        }

    }
}
