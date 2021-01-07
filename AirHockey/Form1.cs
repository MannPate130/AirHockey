/// created by : Mann Patel 
/// date       : January 7, 2021
/// description: A Fun Air Hockey/Soccer Game

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Threading;

namespace AirHockey
{
    public partial class Form1 : Form
    {
        int striker1X = 30;
        int striker1Y = 150;
        int player1Score = 0;

        int striker2X = 680;
        int striker2Y = 150;
        int player2Score = 0;

        int strikerWidth = 10;
        int strikerHeight = 60;
        int strikerSpeed = 8;

        int puckX = 295;
        int puckY = 195;
        int puckXSpeed = 7;
        int puckYSpeed = -7;
        int puckWidth = 10;
        int puckHeight = 10;

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

        SoundPlayer airHockeybuzzer = new SoundPlayer(Properties.Resources.air_hockey_buzzer);
        SoundPlayer puckHit = new SoundPlayer(Properties.Resources.Puck_hit_sound);

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
            puckX -= puckXSpeed;
            puckY += puckYSpeed;

            //move player 1
            if (wDown == true && striker1Y > 0)
            {
                striker1Y -= strikerSpeed;
            }

            if (sDown == true && striker1Y < this.Height - strikerHeight)
            {
                striker1Y += strikerSpeed;
            }

            //move player 2
            if (upArrowDown == true && striker2Y > 0)
            {
                striker2Y -= strikerSpeed;
            }

            if (downArrowDown == true && striker2Y < this.Height - strikerHeight)
            {
                striker2Y += strikerSpeed;
            }

            //move player 1 left and right
            if (aDown == true && striker1X > 0)
            {
                striker1X -= strikerSpeed;
            }

            if (dDown == true && striker1X < this.Width - strikerWidth)
            {
                striker1X += strikerSpeed;
            }

            //move player 2 left and right 
            if (leftDown == true && striker2X > 0)
            {
                striker2X -= strikerSpeed;
            }

            if (rightDown == true && striker2X < this.Width - strikerWidth)
            {
                striker2X += strikerSpeed;
            }

            //top and bottom wall collision
            if (puckY < 0 || puckY > this.Height - puckHeight)
            {
                puckYSpeed *= -1;  // or: puckYSpeed = -puckYSpeed;
            }

            // left and right wall collosion
            if (puckX < 0 || puckX > this.Width - puckWidth)
            {
                puckXSpeed *= -1; 
            }

            //create Rectangles of objects on screen to be used for collision detection
            Rectangle player1Rec = new Rectangle(striker1X, striker1Y, strikerWidth, strikerHeight);
            Rectangle player2Rec = new Rectangle(striker2X, striker2Y, strikerWidth, strikerHeight);
            Rectangle ballRec = new Rectangle(puckX, puckY, puckWidth, puckHeight);

            //check if ball hits either Striker. If it does change the direction
            //and place the ball in front of the Striker hit
            if (player1Rec.IntersectsWith(ballRec))
            {
                puckHit.Play();
                Thread.Sleep(1000);
                puckHit.Stop();
                puckXSpeed *= -1;
                puckX = striker1X + strikerWidth + 1;
            }
            else if (player2Rec.IntersectsWith(ballRec))
            {
                puckHit.Play();
                Thread.Sleep(1000);
                puckHit.Stop();
                puckXSpeed *= -1;
                puckX = striker2X - puckWidth - 1;
            }

            if (puckX < 0 && puckY > 100 && puckY < 250)
            {
                player2Score++;
                puckX = 295;
                puckY = 195;

                striker1Y = 150;
                striker2Y = 150;
            }
            else if (puckX > 700 && puckY > 100 && puckY < 250)
            {
                player1Score++;

                puckX = 295;
                puckY = 195;

                striker1Y = 150;
                striker2Y = 150;
            }

            if (player1Score == 3 || player2Score == 3)
            {
                airHockeybuzzer.Play();
                gameTimer.Enabled = false;
            }
           
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(whiteBrush, puckX, puckY, puckWidth, puckHeight);

            e.Graphics.FillRectangle(blueBrush, striker1X, striker1Y, strikerWidth, strikerHeight);
            e.Graphics.FillRectangle(blueBrush, striker2X, striker2Y, strikerWidth, strikerHeight);

            e.Graphics.FillRectangle(redBrush, -10, 100, 100, 10);
            e.Graphics.FillRectangle(redBrush, -10, 250, 100, 10);

            e.Graphics.FillRectangle(redBrush, 630, 100, 100, 10);
            e.Graphics.FillRectangle(redBrush, 630, 250, 100, 10);

            e.Graphics.FillRectangle(redBrush, 90, 100, 10, 160);
            e.Graphics.FillRectangle(redBrush, 620, 100, 10, 160);

            e.Graphics.DrawString($"{player1Score}", screenFont, whiteBrush, 310, 10);
            e.Graphics.DrawString($"{player2Score}", screenFont, whiteBrush, 345, 10);

            e.Graphics.DrawLine(borderPen, 335, 0, 335, 120);
            e.Graphics.DrawLine(borderPen, 335, 400, 335, 220);
            e.Graphics.DrawEllipse(borderPen, 285, 120, 100, 100);
        }
    }
}
