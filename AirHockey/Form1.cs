using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirHockey
{
    public partial class Form1 : Form
    {
        int paddle1X = 10;
        int paddle1Y = 150;
        int player1Score = 0;

        int paddle2X = 700;
        int paddle2Y = 150;
        int player2Score = 0;

        int paddleWidth = 10;
        int paddleHeight = 60;
        int paddleSpeed = 8;

        int puckX = 295;
        int puckY = 195;
        int ballXSpeed = 7;
        int ballYSpeed = -7;
        int ballWidth = 10;
        int ballHeight = 10;

        bool wDown = false;
        bool sDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;

        bool aDown = false;
        bool dDown = false;
        bool leftDown = false;
        bool rightDown = false;

        SolidBrush blueBrush = new SolidBrush(Color.DodgerBlue);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        Font screenFont = new Font("Consolas", 12);
        Pen borderPen = new Pen(Color.White);
        SolidBrush redBrush = new SolidBrush(Color.Red);


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.A:
                    aDown = true; break;
                case Keys.D:
                    dDown = true; break;
                case Keys.Left:
                    leftDown = true; break;
                case Keys.Right:
                    rightDown = true; break;

            }

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.A:
                    aDown = false; break;
                case Keys.D:
                    dDown = false; break;
                case Keys.Left:
                    leftDown = false; break;
                case Keys.Right:
                    rightDown = false; break;

            }

        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move ball
            puckX -= ballXSpeed;
            puckY += ballYSpeed;

            //move player 1
            if (wDown == true && paddle1Y > 0)
            {
                paddle1Y -= paddleSpeed;
            }

            if (sDown == true && paddle1Y < this.Height - paddleHeight)
            {
                paddle1Y += paddleSpeed;
            }

            //move player 2
            if (upArrowDown == true && paddle2Y > 0)
            {
                paddle2Y -= paddleSpeed;
            }

            if (downArrowDown == true && paddle2Y < this.Height - paddleHeight)
            {
                paddle2Y += paddleSpeed;
            }

            //move player 1 left and right
            if (aDown == true && paddle1X > 0)
            {
                paddle1X -= paddleSpeed;
            }

            if (dDown == true && paddle1X < this.Width - paddleWidth)
            {
                paddle1X += paddleSpeed;
            }

            //move player 2 left and right 
            if (leftDown == true && paddle2X > 0)
            {
                paddle2X -= paddleSpeed;
            }

            if (rightDown == true && paddle2X < this.Width - paddleWidth)
            {
                paddle2X += paddleSpeed;
            }

            //top and bottom wall collision
            if (puckY < 0 || puckY > this.Height - ballHeight)
            {
                ballYSpeed *= -1;  // or: ballYSpeed = -ballYSpeed;
            }

            // left and right wall collosion
            if (puckX < 0 || puckX > this.Width - ballWidth)
            {
                ballXSpeed *= -1; 
            }

            //create Rectangles of objects on screen to be used for collision detection
            Rectangle player1Rec = new Rectangle(paddle1X, paddle1Y, paddleWidth, paddleHeight);
            Rectangle player2Rec = new Rectangle(paddle2X, paddle2Y, paddleWidth, paddleHeight);
            Rectangle ballRec = new Rectangle(puckX, puckY, ballWidth, ballHeight);

            //check if ball hits either paddle. If it does change the direction
            //and place the ball in front of the paddle hit
            if (player1Rec.IntersectsWith(ballRec))
            {
                ballXSpeed *= -1;
                puckX = paddle1X + paddleWidth + 1;
            }
            else if (player2Rec.IntersectsWith(ballRec))
            {
                ballXSpeed *= -1;
                puckX = paddle2X - ballWidth - 1;
            }

            if (puckX < 0 && puckY > 100 && puckY < 250)
            {
                player2Score++;
                puckX = 295;
                puckY = 195;

                paddle1Y = 150;
                paddle2Y = 150;
            }
            else if (puckX > 700 && puckY > 100 && puckY < 250)
            {
                player1Score++;

                puckX = 295;
                puckY = 195;

                paddle1Y = 150;
                paddle2Y = 150;
            }

            if (player1Score == 3 || player2Score == 3)
            {
                gameTimer.Enabled = false;
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(whiteBrush, puckX, puckY, ballWidth, ballHeight);

            e.Graphics.FillRectangle(blueBrush, paddle1X, paddle1Y, paddleWidth, paddleHeight);
            e.Graphics.FillRectangle(blueBrush, paddle2X, paddle2Y, paddleWidth, paddleHeight);

            e.Graphics.FillRectangle(redBrush, -10, 100, 100, 10);
            e.Graphics.FillRectangle(redBrush, -10, 250, 100, 10);

            e.Graphics.FillRectangle(redBrush, 630, 100, 100, 10);
            e.Graphics.FillRectangle(redBrush, 630, 250, 100, 10);

            e.Graphics.FillRectangle(redBrush, 90, 100, 10, 160);
            e.Graphics.FillRectangle(redBrush, 620, 100, 10, 160);

            e.Graphics.DrawString($"{player1Score}", screenFont, whiteBrush, 280, 10);
            e.Graphics.DrawString($"{player2Score}", screenFont, whiteBrush, 310, 10);

        }
    }
}
