using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int score;
        int amountFood = 2;
        Food[] foodArr;

        const int SnakeHeight = 25;
        const int SnakeWidth = 25;

        const int FoodHeight = 10;
        const int FoodWidth = 10;

        const double SnakeSpeed = 4;

        const int GrowAmount = 20;
        private enum MoveDirection { Right, Left, Up, Down}
        private MoveDirection move;

        Snake snake;

        DispatcherTimer gameTick;
        Random random;
        bool gameInProgress = false;

        string snakeHeadImagePath = "Images/Snake_head.png";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Restart()
        {
            Screen.Children.Clear();
            beginTB.Visibility = Visibility.Hidden;
            score = 0;
            random = new Random();
            if (gameTick != null)
                gameTick.Stop();
            gameTick = new DispatcherTimer();
            gameTick.Tick += GameTick_Tick;
            gameTick.Interval = TimeSpan.FromMilliseconds(10);
            gameTick.Start();
            move = MoveDirection.Right;
            gameInProgress = true;
            SpawnSnake(SnakeSpeed, 0, 0);
            foodArr = new Food[amountFood];
            CreateFood();
            score = 0;
            ScoreTB.Text = "" + score;
        }

        private void SpawnSnake(double moveSpeed, int x, int y)
        {
            snake = new Snake(moveSpeed, SnakeHeight, SnakeWidth, x, y, Screen, snakeHeadImagePath);
            snake.Spawn();
        }

        private void GameTick_Tick(object sender, EventArgs e)
        {
            MoveSnake(move);
            CheckEat();
            if (CheckCollision())
                GameOver();
            FoodAnimation();
        }

        private void MoveSnake(MoveDirection moveDirection)
        {
            double prevX = snake.X;
            double prevY = snake.Y;
            if(moveDirection == MoveDirection.Right)
                snake.Update(snake.X + snake.Speed, snake.Y);
            else if(moveDirection == MoveDirection.Left)
                snake.Update(snake.X - snake.Speed, snake.Y);
            else if (moveDirection == MoveDirection.Up)
                snake.Update(snake.X, snake.Y - snake.Speed);
            else
                snake.Update(snake.X, snake.Y + snake.Speed);
            snake.MoveBody(prevX, prevY);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Key k = e.Key;
            if (k == Key.Space && !gameInProgress)
            {
                Restart();
                return;
            }
            if (!gameInProgress)
                return;
            switch (k)
            {
                case Key.G:
                    snake.Grow(1, move==MoveDirection.Left);
                    break;
                case Key.Up:
                    if (snake.SnakeLenght > 0 && move == MoveDirection.Down)
                        return;
                    move = MoveDirection.Up;
                    break;
                case Key.Down:
                    if (snake.SnakeLenght > 0 && move == MoveDirection.Up)
                        return;
                    move = MoveDirection.Down;
                    break;
                case Key.Left:
                    if (snake.SnakeLenght > 0 && move == MoveDirection.Right)
                        return;
                    move = MoveDirection.Left;
                    break;
                case Key.Right:
                    if (snake.SnakeLenght > 0 && move == MoveDirection.Left)
                        return;
                    move = MoveDirection.Right;
                    break;
            }
        }

        private void CreateFood()
        {
            for (int i = 0; i < foodArr.Count(); i++)
            {
                foodArr[i] = new Food(FoodHeight, FoodWidth, Screen);
                GenerateFood(foodArr[i]);
            }
        }
        private void GenerateFood(Food food)
        {
            int x = random.Next(0 + 15, (int)Screen.ActualWidth - 15);
            int y = random.Next(0 + 15, (int)Screen.ActualHeight - 15);
            food.Spawn(x, y);
            snake.Respawn();
        }

        private void CheckEat()
        {
            foreach(Food food in foodArr)
            {
                if (food.Inside(snake))
                {
                    GenerateFood(food);
                    snake.Grow(GrowAmount, move == MoveDirection.Left);
                    score++;
                    ScoreTB.Text = "" + score;
                    return;
                }
            }
        }

        private bool CheckCollision()
        {
            if (snake.X + snake.Width > Screen.ActualWidth
                || snake.X < 0)
                return true;
            if (snake.Y + snake.Height > Screen.ActualHeight
                || snake.Y < 0)
                return true;
            if (move == MoveDirection.Down)
                return snake.CheckCollision((int)(snake.X + snake.Width / 2), (int)(snake.Y + snake.Height));
            if (move == MoveDirection.Up)
                return snake.CheckCollision((int)(snake.X + snake.Width / 2), (int)(snake.Y));
            if (move == MoveDirection.Right)
                return snake.CheckCollision((int)(snake.X + snake.Width), (int)(snake.Y + snake.Height / 2));
            if (move == MoveDirection.Left)
                return snake.CheckCollision((int)(snake.X), (int)(snake.Y + snake.Height / 2));
            return false;
        }

        private void GameOver()
        {
            MessageBox.Show("Game Over");
            Restart();
        }

        private void FoodAnimation()
        {
            foreach (Food food in foodArr)
                food.Animation();
        }

    }
}
