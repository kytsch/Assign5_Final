/*********************************************************************************
 CSCI 473 - Assignment 5 - Spring 2020

 Programmers : Kyle Schultz and Sean Kohl
 Z-IDs       : z1837807   and   z1836529
 Date Due    : Thursday, April 16th, 2020

 Purpose: This program is a chess game! Have Fun! Please google the rules........
           The program will give tips such as time taken each turn, who's turn it is,
           among various other things. 
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Diagnostics;

namespace ChessGame_KyleSean
{
    public partial class Form1 : Form
    {
        bool[,] possibleMove = new bool[8, 8];

        //White Pieces
        PictureBox whitePawn0 = new PictureBox();
        PictureBox whitePawn1 = new PictureBox();
        PictureBox whitePawn2 = new PictureBox();
        PictureBox whitePawn3 = new PictureBox();
        PictureBox whitePawn4 = new PictureBox();
        PictureBox whitePawn5 = new PictureBox();
        PictureBox whitePawn6 = new PictureBox();
        PictureBox whitePawn7 = new PictureBox();
        PictureBox whiteRook0 = new PictureBox();
        PictureBox whiteRook1 = new PictureBox();
        PictureBox whiteKnight0 = new PictureBox();
        PictureBox whiteKnight1 = new PictureBox();
        PictureBox whiteBishop0 = new PictureBox();
        PictureBox whiteBishop1 = new PictureBox();
        PictureBox whiteKing = new PictureBox();
        PictureBox whiteQueen = new PictureBox();

        //Black Pieces
        PictureBox blackPawn0 = new PictureBox();
        PictureBox blackPawn1 = new PictureBox();
        PictureBox blackPawn2 = new PictureBox();
        PictureBox blackPawn3 = new PictureBox();
        PictureBox blackPawn4 = new PictureBox();
        PictureBox blackPawn5 = new PictureBox();
        PictureBox blackPawn6 = new PictureBox();
        PictureBox blackPawn7 = new PictureBox();
        PictureBox blackRook0 = new PictureBox();
        PictureBox blackRook1 = new PictureBox();
        PictureBox blackKnight0 = new PictureBox();
        PictureBox blackKnight1 = new PictureBox();
        PictureBox blackBishop0 = new PictureBox();
        PictureBox blackBishop1 = new PictureBox();
        PictureBox blackKing = new PictureBox();
        PictureBox blackQueen = new PictureBox();

        static Label overallGameTime_label = new Label();
        static Label overallGameTime_value = new Label();

        TextBox whoseTurn = new TextBox();

        List<PictureBox> whitePieces = new List<PictureBox>();       //List of white/black PictureBox objects
        List<PictureBox> blackPieces = new List<PictureBox>();

        int whitePiecesRemaining = 15;                               //variables to assist in collision detection 
        int blackPiecesRemaining = 15;

        int moveCount = 2;                                           //Even == White Move, Odd == Black Move

        Point OGLocation = new Point(0, 0);                          //Assist in capturing Old/New locations at Global level
        Point newLocation = new Point(0, 0);

        Point move = new Point(0, 0);                                //Assist in capturing amount of space traveled by Mouse

        Point pieceLoc = new Point(0, 0);                            //Two points to assist in Initialization of new Pieces
        Point tempLoc = new Point(0, 0);

        Nullable<bool>[ , ] spaceTaken = new Nullable<bool>[8, 8];   //Data Representation of board in a few Formats
        string[,] boardData = new string[8, 8];

        PictureBox clickedPiece;

        static Stopwatch overallTime = new Stopwatch();
        Stopwatch turnTime = new Stopwatch();
        private static System.Timers.Timer refreshGameTime;

        public Form1()
        {
            InitializeComponent();

            this.Size = new Size(725, 800);

            //Initialize boardData array to empty
            for(int i = 0; i < 8; i++)
            {
                for(int k = 0; k < 8; k++)
                {

                    boardData[k, i] = "";
                }
            }

            createPieces();

            //Initialize 2D array for taken spaces to the Default for a new Chess game
            for(int i = 0; i < 8; i++)
            {

                for (int k = 0; k < 8; k++)
                {

                    possibleMove[i, k] = false;

                    if (i == 0 || i == 1 || i == 6 || i ==7)
                    {
                        spaceTaken[k, i] = true;
                    }
                    else
                    {
                        spaceTaken[k, i] = false;
                    }
                }
            }

            overallGameTime_label.Text = "Overall Game Time: ";
            overallGameTime_label.Font = new Font(overallGameTime_label.Font, FontStyle.Bold);
            overallGameTime_label.Location = new Point(56, 685);
            overallGameTime_label.AutoSize = true;
            this.Controls.Add(overallGameTime_label);

            overallGameTime_value.Text = "hello";
            overallGameTime_value.Font = new Font(overallGameTime_label.Font, FontStyle.Bold);
            overallGameTime_value.Location = new Point(175, 685);
            overallGameTime_value.AutoSize = true;
            this.Controls.Add(overallGameTime_value);

            whoseTurn.Text = "Turn: White";
            whoseTurn.TextAlign = HorizontalAlignment.Center;
            whoseTurn.Location = new Point(550, 685);
            this.Controls.Add(whoseTurn);

            SetTimer();

            turnTime.Start();

            //chessBoard.Refresh();
        }

        /*********************************************************************************
         Method     : UpdateGameTimer
         Purpose    : Update the value of the overallGameTime label with the correct 
                       elapsed game time
         Parameters : 1. sender - Object that uses this event code
                      2. e      - Elapsed Event Arguments sent by the event
         Returns    : N/A
        *********************************************************************************/

        private static void UpdateGameTimer(Object source, ElapsedEventArgs e)
        {

            TimeSpan time = overallTime.Elapsed;

            overallGameTime_value.Text = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", time.Hours, time.Minutes, time.Seconds, time.Milliseconds / 10);
        }

        /*********************************************************************************
         Method     : SetTimer
         Purpose    : Start the overall game timer and set the timer to trigger an event
                       that will update/display the time elapsed in the game
         Parameters : N/A
         Returns    : N/A
        *********************************************************************************/

        private static void SetTimer()
        {
            overallTime.Start();

            refreshGameTime = new System.Timers.Timer(100);
            refreshGameTime.Elapsed += UpdateGameTimer;
            refreshGameTime.AutoReset = true;
            refreshGameTime.Enabled = true;
        }

        /*********************************************************************************
         Method     : chessBoard_Paint
         Purpose    : Create the "checkered" chess board on the Canvas
         Parameters : 1. sender - Object that uses this event code
                      2. e      - Paint Arguments sent by the event
         Returns    : N/A
        *********************************************************************************/

        private void chessBoard_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            SolidBrush myBrush1 = new SolidBrush(Color.Gray);        //Primary Board Color
            SolidBrush myBrush2 = new SolidBrush(Color.LightBlue);   //Secondary Board Color

            int startX = 0;
            int startY = 0;

            for (int i = 0; i <= 7; i++)
            {
                startX = 0;

                //Primary square first in row
                if (i % 2 == 0)
                {

                    for (int k = 0; k <= 7; k++)
                    {

                        if (k % 2 == 0)
                        {

                            Rectangle rect = new Rectangle(startX, startY, 75, 75);

                            e.Graphics.FillRectangle(myBrush1, rect);

                            startX += 75;
                            //startY += 100;
                        }
                        else
                        {

                            Rectangle rect = new Rectangle(startX, startY, 75, 75);

                            e.Graphics.FillRectangle(myBrush2, rect);

                            startX += 75;
                            //startY += 100;
                        }
                    }
                }

                //First square is secondary color
                else
                {

                    for (int k = 0; k <= 7; k++)
                    {

                       if (k % 2 == 0)
                       {

                            Rectangle rect = new Rectangle(startX, startY, 75, 75);

                            e.Graphics.FillRectangle(myBrush2, rect);

                            startX += 75;
                            //startY += 100;
                       }
                       else
                       {

                            Rectangle rect = new Rectangle(startX, startY, 75, 75);

                            e.Graphics.FillRectangle(myBrush1, rect);

                            startX += 75;
                            //startY += 100;
                       }
                    }
                }
                //Go to next row of the board
                startY += 75;
            }
                myBrush1.Dispose();
                myBrush2.Dispose();
        }

        /*********************************************************************************
         Method     : determineLocation
         Purpose    : This method is used in the process of initializing new Pieces (the 
                       need for newLoc). This function is also used to determine the color
                       of a space that is being moved to. 
         Parameters : 1. p      - Current X/Y location on the canvas
                      2. newLoc - New X/Y location on the canvas
         Returns    : Proper color for piece according to the space being placed on
        *********************************************************************************/

        private Color determineLocation(Point p, ref Point newLoc)
        {

            int X = p.X / 75;
            int Y = p.Y / 75;

            newLoc.X = X * 75;
            newLoc.Y = Y * 75;

            //First square is Primary color
            if(X % 2 == 0)
            {

                //Background is Primary color
                if (Y % 2 == 0)
                {
                    return Color.Gray;
                }
                //Background is Secondary color
                else
                {
                    return Color.LightBlue;
                }
            }

            //First square is Secondary color
            else
            {

                //Background is Secondary color
                if (Y % 2 == 0)
                {
                    return Color.LightBlue;
                }
                //Background is Primary color
                else
                {
                    return Color.Gray;
                }
            }
        }

        /*********************************************************************************
         Method     : pieceMouseDown
         Purpose    : When a piece is clicked on and moved, a fucntion is called for each 
                       piece. Those functions will then calculate possible moves for each of
                       the pieces that will be used during the pieceMouseUp event based on 
                       Color and Type of piece selected.
         Parameters : 1. sender - Object that uses this event code
                      2. e      - Mouse Arguments sent by the event
         Returns    : N/A
        *********************************************************************************/

        private void pieceMouseDown(object sender, MouseEventArgs e)
        {

            //Reset the possible moves table.
            for(int i = 0; i < 8; i++)
            {

                for(int j = 0; j < 8; j++)
                {

                    possibleMove[i, j] = false;

                }

            }

            //Set the selected piece variable to the one that has just been clicked.
            //This will be used later when the mouse button is released.
            clickedPiece = sender as PictureBox;

            int tempX = e.X / 75;
            int tempY = e.Y / 75;

            OGLocation.X = e.X;
            OGLocation.Y = e.Y;

            //Initialize a new picture as the sender so its data can be used.
            PictureBox currPiece = sender as PictureBox;

            //Even -> White's Turn
            if (moveCount % 2 == 0)
            {

                if (currPiece.Name.Substring(0, 1) == "b")
                {
                    MessageBox.Show("It is White's Turn to move");
                    return;
                }
            }
            //Odd -> Black's Turn
            else
            {

                if (currPiece.Name.Substring(0, 1) == "w")
                {
                    MessageBox.Show("It is Black's Turn to move");
                    return;
                }
            }

            //Get the beginning of the piece's type and create a value for if it is
            //white or not. The type is used for a switch and the color flag is used when
            //checking for possible moves.
            string pieceType = currPiece.Name.Substring(5, 3);
            bool isWhite = true;

            //If the piece happens to be a black piece, flip the isWhite flag.
            if (currPiece.Name.Substring(0, 1) == "b")
                isWhite = false;

            //Use the name of the piece for the switch statement.
            //For each case, the function for the particular piece is triggered.
            switch (pieceType)
            {

                case "Paw":
                    pawn(currPiece.Location.X / 75, currPiece.Location.Y / 75, isWhite);
                    break;

                case "Bis":
                    bishop(currPiece.Location.X / 75, currPiece.Location.Y / 75, isWhite);
                    break;

                case "Kni":
                    knight(currPiece.Location.X / 75, currPiece.Location.Y / 75, isWhite);
                    break;

                case "Que":
                    queen(currPiece.Location.X / 75, currPiece.Location.Y / 75, isWhite);
                    break;
                case "Kin":
                    king(currPiece.Location.X / 75, currPiece.Location.Y / 75, isWhite);
                    break;
            }

        }

        /*********************************************************************************
         Method     : pieceMouseUp
         Purpose    : Once the selected piece has been let go of, the program will 
                       determine if the space moved to is a valid move based on the Color
                       and Type of piece selected
         Parameters : 1. sender - Object that uses this event code
                      2. e      - Mouse Arguments sent by the event
         Returns    : N/A
        *********************************************************************************/

        private void pieceMouseUp(object sender, MouseEventArgs e)
        {

            PictureBox selectedPiece = (PictureBox)sender;
            Point temp = selectedPiece.Location;

            //Get the grid indices that the mouse was released in.
            //These are used when checking if the piece was released in a valid spot.
            int droppedX = (Form1.MousePosition.X - this.Location.X - chessBoard.Location.X) / 75;
            int droppedY = (Form1.MousePosition.Y - this.Location.Y - chessBoard.Location.Y - 30) / 75;

            //If the mouse landed out of bounds, set the coordinates to the closest grid square.
            if (droppedX < 0)
            {
                droppedX = 0;
            }
            else if (droppedX > 7)
            {
                droppedX = 7;
            }

            if (droppedY < 0)
            {
                droppedY = 0;
            }
            else if (droppedY > 7)
            {
                droppedY = 7;
            }

            //Console.WriteLine("Dropped location: " + droppedY + ", " + droppedX);

            //Flag for the color of the piece. This will be used later
            bool isWhite = true;
            if (selectedPiece.Name.Substring(0, 1) == "b")
                isWhite = false;

            //Save coordinates of old space
            int oldX = temp.X / 75;
            int oldY = temp.Y / 75;

            //Save distance from old space
            newLocation.X = e.X;
            newLocation.Y = e.Y;
            move = e.Location;

            //Get amount of spaces moved from current space
            int tempX = e.X / 75;
            int tempY = e.Y / 75;

            if (tempX < 0)
                tempX -= 75;

            //MessageBox.Show(tempX + " " + tempY);

            //Calculate coordinates of new space
            int spaceX = temp.X + (75 * tempX);
            int spaceY = temp.Y + (75 * tempY);
            spaceX = spaceX / 75;
            spaceY = spaceY / 75;

            int newX = 0;  //Final X-Coordinate of new Space
            int newY = 0;  //Final Y-Coordinate of new Space

            //Find the new coordinates
            if(e.X < 0)
            {
                if(e.X >= -75)
                {
                    if (spaceX > 0)
                        newX = spaceX - 1;

                    newY = spaceY;
                }
                else if(e.X < -75)
                {

                    int X = 0;
                    int Slacker = e.X * -1;

                    X = Slacker / 75;
                    Slacker = Slacker - (X * 75);

                    //Check for added space
                    if (Slacker % 75 != 0)
                    {
                        X++;
                    }

                    newX = oldX - X;
                    newY = spaceY;
                }
            }
            else if(e.Y < 0)
            {
                if (e.Y >= -75)
                {
                    if(spaceY > 0)
                        newY = spaceY - 1;

                    newX = spaceX;
                }
                else if(e.Y < -75)
                {

                    int Y = 0;
                    int Slacker = e.Y * -1;

                    Y = Slacker / 75;
                    Slacker = Slacker - (Y * 75);

                    //Check for added space
                    if (Slacker % 75 != 0)
                    {
                        Y++;
                    }

                    newY = oldY - Y;
                    newX = spaceX;
                }
            }
            else if(e.X >= 0 && e.Y >= 0)
            {
                newX = spaceX;
                newY = spaceY;
            }

            //Get the type of piece selected 
            string piece = selectedPiece.Name.Substring(5, 3);

            //Movement function if the piece is a rook.
            if(piece == "Roo")
            {
                rook(oldX, oldY, newX, newY, isWhite, selectedPiece);
            }

            //For the other pieces, check if the piece was dropped in a valid location
            //based on the table of possible moves.
            if (possibleMove[droppedX, droppedY] == true)
            {
                intersection(selectedPiece);

                temp.X = temp.X + (75 * tempX);
                temp.Y = temp.Y + (75 * tempY);

                //Move the piece and then check if the player's king is in danger.
                //If the king is in check after moving their own piece, then the player must be informed and the piece is put back.
                //It continues to be that player's turn until they make a valid move since the move counter does not update.
                selectedPiece.Location = new Point(droppedX * 75, droppedY * 75);
                spaceTaken[oldX, oldY] = false;
                spaceTaken[droppedX, droppedY] = true;
                if (moveCount % 2 == 0 && tileIsDangerous(whiteKing.Location.X / 75, whiteKing.Location.Y / 75, true) || moveCount % 2 == 1 && tileIsDangerous(blackKing.Location.X / 75, blackKing.Location.Y / 75, false))
                {

                    MessageBox.Show("You cannot put yourself in check.");
                    selectedPiece.Location = new Point((oldX * 75), (oldY * 75));
                    spaceTaken[oldX, oldY] = true;
                    spaceTaken[droppedX, droppedY] = false;

                }
                //If the king is fine after the move, then the turn continues as normal.
                else
                {

                    //Update board data table for the rook function.
                    boardData[oldX, oldY] = "";
                    boardData[droppedX, droppedY] = selectedPiece.Name;

                    //Get the new location of the piece and use it to change its
                    //back color to the color of whatever space it is on.
                    pieceLoc = new Point((droppedX * 75), (droppedY * 75));
                    selectedPiece.BackColor = determineLocation(pieceLoc, ref tempLoc);

                    //Check if a piece has been moved over another and remove the colored piece.
                    intersection(clickedPiece);

                    //Inform the players if a king has been put in check by checking if the king's
                    //square can be attacked by anything.
                    if (tileIsDangerous(blackKing.Location.X / 75, blackKing.Location.Y / 75, false))
                        MessageBox.Show("Black king has been checked!");
                    if (tileIsDangerous(whiteKing.Location.X / 75, whiteKing.Location.Y / 75, true))
                        MessageBox.Show("White kinf has been checked!");

                    moveCount++;  //Increment moveCount for Turn Tracking

                    TimeSpan time = turnTime.Elapsed;

                    //THIS MODULUS DIVISION IS DIFFERENT THAN OTHERS USING THIS SAME METHOD, HENCE IT'S BACKWARDS.
                    //EVEN -> Black Move   ODD -> White Move
                    //Black made a move
                    if (moveCount % 2 == 0)
                    {
                        turnTime_listBox.Items.Insert(0, String.Format("{0}", moveCount) + ". Black - " + String.Format("{0:00}:{1:00}.{2:00}", time.Minutes, time.Seconds, time.Milliseconds));

                        whoseTurn.Text = "Turn: White";
                        whoseTurn.ForeColor = Color.Black;
                        whoseTurn.BackColor = Color.White;
                    }
                    //White made a move
                    else
                    {
                        turnTime_listBox.Items.Insert(0, String.Format("{0}", moveCount) + ". White - " + String.Format("{0:00}:{1:00}.{2:00}", time.Minutes, time.Seconds, time.Milliseconds));

                        whoseTurn.Text = "Turn: Black";
                        whoseTurn.ForeColor = Color.White;
                        whoseTurn.BackColor = Color.Black;
                    }

                    turnTime.Reset();
                    turnTime.Start();

                    chessBoard.Refresh();

                    //turnTime_listBox.Items.Insert(0, String.Format("{0}", moveCount) + ". Black - " + String.Format("{0:00}.{1:00}}", time.Seconds, time.Milliseconds));

                }
            }
            else
            {

                MessageBox.Show("Sorry, that is an invalid move.");
                    
            }
        }

        /*********************************************************************************
         Method     : intersection
         Purpose    : This method is used ONLY WHEN A PIECE IS GOING TO BE ELIMINATED. This
                       method will loop through the opposing pieces that are remaining on 
                       the board and remove the piece that it is overlapping with, effectively
                       eliminating it from the game.
         Parameters : 1. selectedPiece - the piece that was moved
         Returns    : N/A
        *********************************************************************************/

        private void intersection(object selectedPiece)
        {

            //Cast passed in object as a PictureBox
            PictureBox selected = (PictureBox)selectedPiece;

            //Get the Color of the selected Piece
            string color = selected.Name.Substring(0, 5);

            //White Piece Moved
            if (color == "white")
            {

                bool found = false;
                int i = 0;

                //Find the correct piece to remove
                while (!found)
                {

                    bool overlapped = false;
                    overlapped = selected.Bounds.IntersectsWith(blackPieces[i].Bounds);

                    if (overlapped)
                    {

                        found = true;
                        blackPieces[i].Hide();
                        blackPieces[i].Location = new Point(-200, -200);
                        blackPiecesRemaining--;
                    }

                    i++;

                    if (i > whitePiecesRemaining)
                        found = true;
                }
            }

            //Black Piece Moved
            else
            {

                bool found = false;
                int i = 0;

                //Find the correct piece to remove
                while (!found)
                {

                    bool overlapped = false;
                    overlapped = selected.Bounds.IntersectsWith(whitePieces[i].Bounds);

                    if (overlapped)
                    {

                        found = true;
                        whitePieces[i].Hide();
                        whitePieces[i].Location = new Point(-200, -200);
                        whitePiecesRemaining--;
                    }

                    i++;

                    if (i > blackPiecesRemaining)
                        found = true;
                }
            }
        }

        /*********************************************************************************
         Method     : rook
         Purpose    : This method will accept the old and new coordinates of the Rook
                       that is being moved. Each space that is moved across is checked for
                       a friendly in the way for a valid move. Rooks can move any number of
                       spaces vertically or horizontally. 
         Parameters : 1. oldX          - X-Coordinate of old position
                      2. oldY          - Y-Coordinate of old position
                      3. newX          - X-Coordinate of new position
                      4. newY          - Y-Coordinate of new position
                      5. isWhite       - boolean determining the color of the selected piece
                      6. selectedPiece - the piece selected 
         Returns    : N/A
        *********************************************************************************/

        private void rook(int oldX, int oldY, int newX, int newY, bool isWhite, PictureBox selectedPiece)
        {

            //Check if friendly is occupying space Rook is moving to
            if(spaceTaken[newX, newY] == true)
            {
                string check1 = boardData[oldX, oldY].Substring(0, 5);
                string check2 = boardData[newX, newY].Substring(0, 5);

                if (check1 == check2)
                {
                    MessageBox.Show("Not a Valid Move");
                    return;
                }
            }

            //Assure move is within bounds of the Board
            if(newX < 0)
            {
                newX = 0;
            }
            else if(newX > 7)
            {
                newX = 7;
            }

            if (newY < 0)
            {
                newY = 0;
            }
            else if (newY > 7)
            {
                newY = 7;
            }

            //White Rook Selected
            if (isWhite)
            {

                //Move is Down/Up
                if (oldX == newX && oldY != newY)
                {

                    //Check for friendly in the way of move
                    //Piece was moved DOWN
                    if (oldY < newY)
                    {
                        for (int i = oldY + 1; i <= newY; i++)
                        {

                            string color = "";
                            if (boardData[oldX, i] != "")
                                color = boardData[oldX, i].Substring(0, 5);

                            if (color == "white")
                            {

                                MessageBox.Show("Friendly Detected");
                                return;
                            }
                        }
                    }
                    //Piece was moved UP
                    else
                    {
                        for (int i = oldY - 1; i >= newY; i--)
                        {

                            string color = "";
                            if (boardData[oldX, i] != "")
                                color = boardData[oldX, i].Substring(0, 5);

                            if (color == "white")
                            {

                                MessageBox.Show("Friendly Detected");
                                return;
                            }
                        }
                    }

                    //Move the Piece
                    selectedPiece.Location = new Point(selectedPiece.Location.X, (newY * 75));
                    Point temp = selectedPiece.Location;
                    selectedPiece.BackColor = determineLocation(temp, ref tempLoc);

                    //Update spaceTaken array
                    spaceTaken[oldX, oldY] = false;
                    spaceTaken[newX, newY] = true;

                    //Spcae is taken by the enemy, eliminate them
                    if (spaceTaken[newX, newY] == true)
                    {
                        intersection(selectedPiece);
                    }

                    //Update boardData array
                    boardData[oldX, oldY] = "";
                    boardData[newX, newY] = selectedPiece.Name;
                }

                //Move is Right/Left
                else if (oldY == newY && oldX != newX)
                {

                    //Check for friendly in the way of move
                    //Piece was moved RIGHT
                    if (oldX < newX)
                    {
                        for (int i = oldX + 1; i <= newX; i++)
                        {

                            string color = "";
                            if (boardData[i, oldY] != "")
                                color = boardData[i, oldY].Substring(0, 5);

                            if (color == "white")
                            {

                                MessageBox.Show("Friendly Detected");
                                return;
                            }
                        }
                    }
                    //Piece was moved LEFT
                    else
                    {
                        for (int i = oldX - 1; i >= newX; i--)
                        {

                            //MessageBox.Show(String.Format("{0}", i));

                            string color = "";
                            if (boardData[i, oldY] != "")
                                color = boardData[i, oldY].Substring(0, 5);

                            if (color == "white")
                            {
                                MessageBox.Show("Friendly Detected");
                                return;
                            }
                        }
                    }

                    //Move the Piece
                    selectedPiece.Location = new Point((newX * 75), selectedPiece.Location.Y);
                    Point temp = selectedPiece.Location;
                    selectedPiece.BackColor = determineLocation(temp, ref tempLoc);
                    
                    //Update spaceTaken array
                    spaceTaken[oldX, oldY] = false;
                    spaceTaken[newX, newY] = true;

                    //Spcae is taken by the enemy, eliminate them
                    if (spaceTaken[newX, newY] == true)
                    {
                        intersection(selectedPiece);
                    }

                    //Update boardData array
                    boardData[oldX, oldY] = "";
                    boardData[newX, newY] = selectedPiece.Name;
                }
                //Move not Valid
                else
                {

                    MessageBox.Show("not valid 1");
                }
            }

            //Black Rook Selected
            else if(!isWhite)
            {

                //Move is Down/Up
                if (oldX == newX && oldY != newY)
                {

                    //Check for friendly in the way of move
                    //Piece was moved DOWN
                    if (oldY < newY)
                    {
                        for (int i = oldY + 1; i <= newY; i++)
                        {

                            string color = "";
                            if (boardData[oldX, i] != "")
                                color = boardData[oldX, i].Substring(0, 5);

                            if (color == "black")
                            {

                                MessageBox.Show("Friendly Detected");
                                return;
                            }
                        }
                    }
                    //Piece was moved UP
                    else
                    {
                        for (int i = oldY - 1; i >= newY; i--)
                        {

                            string color = "";
                            if (boardData[oldX, i] != "")
                                color = boardData[oldX, i].Substring(0, 5);

                            if (color == "black")
                            {

                                MessageBox.Show("Friendly Detected");
                                return;
                            }
                        }
                    }

                    //Move the Piece
                    selectedPiece.Location = new Point(selectedPiece.Location.X, (newY * 75));
                    Point temp = selectedPiece.Location;
                    selectedPiece.BackColor = determineLocation(temp, ref tempLoc);
                    
                    //Update spaceTaken array
                    spaceTaken[oldX, oldY] = false;
                    spaceTaken[newX, newY] = true;

                    //Spcae is taken by the enemy, eliminate them
                    if (spaceTaken[newX, newY] == true)
                    {
                        intersection(selectedPiece);
                    }

                    //Update boardData array
                    boardData[oldX, oldY] = "";
                    boardData[newX, newY] = selectedPiece.Name;
                }

                //Move is Right/Left
                else if (oldY == newY && oldX != newX)
                {

                    //Check for friendly in the way of move
                    //Piece was moved RIGHT
                    if (oldX < newX)
                    {
                        for (int i = oldX + 1; i <= newX; i++)
                        {

                            string color = "";
                            if (boardData[i, oldY] != "")
                                color = boardData[i, oldY].Substring(0, 5);

                            if (color == "black")
                            {

                                MessageBox.Show("Friendly Detected");
                                return;
                            }
                        }
                    }
                    //Piece was moved LEFT
                    else
                    {
                        for (int i = oldX - 1; i >= newX; i--)
                        {

                            //MessageBox.Show(String.Format("{0}", i));

                            string color = "";
                            if (boardData[i, oldY] != "")
                                color = boardData[i, oldY].Substring(0, 5);

                            if (color == "black")
                            {
                                MessageBox.Show("Friendly Detected");
                                return;
                            }
                        }
                    }

                    //Move the Piece
                    selectedPiece.Location = new Point((newX * 75), selectedPiece.Location.Y);
                    Point temp = selectedPiece.Location;
                    selectedPiece.BackColor = determineLocation(temp, ref tempLoc);
                    
                    //Update spaceTaken array
                    spaceTaken[oldX, oldY] = false;
                    spaceTaken[newX, newY] = true;

                    //Spcae is taken by the enemy, eliminate them
                    if (spaceTaken[newX, newY] == true)
                    {
                        intersection(selectedPiece);
                    }

                    //Update boardData array
                    boardData[oldX, oldY] = "";
                    boardData[newX, newY] = selectedPiece.Name;
                }
                //Move not Valid
                else
                {

                    MessageBox.Show("not valid 2");
                }
            }
        }

        /*********************************************************************************
         Method     : createPieces
         Purpose    : This method will create all 32 pieces for the chess game and place 
                       them in a default format for a new chess game. This method will 
                       add mouseDown and mouseUp events to each picture box to be able
                       to capture when one is going to be moved. Each piece is then added
                       to its respective List for white or black pieces.
         Parameters : N/A
         Returns    : N/A
        *********************************************************************************/

        private void createPieces()
        {

            //White Pieces
            whitePawn0.Size = new Size(75, 75);
            whitePawn0.Name = "whitePawn0";
            whitePawn0.Location = new Point(0, 75);
            pieceLoc = whitePawn0.Location;
            whitePawn0.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whitePawn0.Location = tempLoc;
            whitePawn0.Image = Image.FromFile("../../ChessPieces/whitepawn.png");
            whitePawn0.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(whitePawn0);
            whitePawn0.MouseUp += new MouseEventHandler(pieceMouseUp);
            whitePawn0.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[0, 1] = whitePawn0.Name;
            whitePieces.Add(whitePawn0);

            whitePawn1.Size = new Size(75, 75);
            whitePawn1.Name = "whitePawn1";
            whitePawn1.Location = new Point(75, 75);
            pieceLoc = whitePawn1.Location;
            whitePawn1.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whitePawn1.Location = tempLoc;
            whitePawn1.Image = Image.FromFile("../../ChessPieces/whitepawn.png");
            whitePawn1.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(whitePawn1);
            whitePawn1.MouseUp += new MouseEventHandler(pieceMouseUp);
            whitePawn1.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[1, 1] = whitePawn1.Name;
            whitePieces.Add(whitePawn1);

            whitePawn2.Size = new Size(75, 75);
            whitePawn2.Name = "whitePawn2";
            whitePawn2.Location = new Point(150, 75);
            pieceLoc = whitePawn2.Location;
            whitePawn2.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whitePawn2.Location = tempLoc;
            whitePawn2.Image = Image.FromFile("../../ChessPieces/whitepawn.png");
            whitePawn2.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(whitePawn2);
            whitePawn2.MouseUp += new MouseEventHandler(pieceMouseUp);
            whitePawn2.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[2, 1] = whitePawn2.Name;
            whitePieces.Add(whitePawn2);

            whitePawn3.Size = new Size(75, 75);
            whitePawn3.Name = "whitePawn3";
            whitePawn3.Location = new Point(225, 75);
            pieceLoc = whitePawn3.Location;
            whitePawn3.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whitePawn3.Location = tempLoc;
            whitePawn3.Image = Image.FromFile("../../ChessPieces/whitepawn.png");
            whitePawn3.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(whitePawn3);
            whitePawn3.MouseUp += new MouseEventHandler(pieceMouseUp);
            whitePawn3.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[3, 1] = whitePawn3.Name;
            whitePieces.Add(whitePawn3);

            whitePawn4.Size = new Size(75, 75);
            whitePawn4.Name = "whitePawn4";
            whitePawn4.Location = new Point(300, 75);
            pieceLoc = whitePawn4.Location;
            whitePawn4.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whitePawn4.Location = tempLoc;
            whitePawn4.Image = Image.FromFile("../../ChessPieces/whitepawn.png");
            whitePawn4.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(whitePawn4);
            whitePawn4.MouseUp += new MouseEventHandler(pieceMouseUp);
            whitePawn4.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[4, 1] = whitePawn4.Name;
            whitePieces.Add(whitePawn4);

            whitePawn5.Size = new Size(75, 75);
            whitePawn5.Name = "whitePawn5";
            whitePawn5.Location = new Point(375, 75);
            pieceLoc = whitePawn5.Location;
            whitePawn5.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whitePawn5.Location = tempLoc;
            whitePawn5.Image = Image.FromFile("../../ChessPieces/whitepawn.png");
            whitePawn5.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(whitePawn5);
            whitePawn5.MouseUp += new MouseEventHandler(pieceMouseUp);
            whitePawn5.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[5, 1] = whitePawn5.Name;
            whitePieces.Add(whitePawn5);

            whitePawn6.Size = new Size(75, 75);
            whitePawn6.Name = "whitePawn6";
            whitePawn6.Location = new Point(450, 75);
            pieceLoc = whitePawn6.Location;
            whitePawn6.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whitePawn6.Location = tempLoc;
            whitePawn6.Image = Image.FromFile("../../ChessPieces/whitepawn.png");
            whitePawn6.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(whitePawn6);
            whitePawn6.MouseUp += new MouseEventHandler(pieceMouseUp);
            whitePawn6.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[6, 1] = whitePawn6.Name;
            whitePieces.Add(whitePawn6);

            whitePawn7.Size = new Size(75, 75);
            whitePawn7.Name = "whitePawn7";
            whitePawn7.Location = new Point(525, 75);
            pieceLoc = whitePawn7.Location;
            whitePawn7.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whitePawn7.Location = tempLoc;
            whitePawn7.Image = Image.FromFile("../../ChessPieces/whitepawn.png");
            whitePawn7.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(whitePawn7);
            whitePawn7.MouseUp += new MouseEventHandler(pieceMouseUp);
            whitePawn7.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[7, 1] = whitePawn0.Name;
            whitePieces.Add(whitePawn7);

            whiteRook0.Size = new Size(75, 75);
            whiteRook0.Name = "whiteRook0";
            whiteRook0.Location = new Point(0, 0);
            pieceLoc = whiteRook0.Location;
            whiteRook0.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whiteRook0.Location = tempLoc;
            whiteRook0.Image = Image.FromFile("../../ChessPieces/WhiteRook.png");
            whiteRook0.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(whiteRook0);
            whiteRook0.MouseUp += new MouseEventHandler(pieceMouseUp);
            whiteRook0.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[0, 0] = whiteRook0.Name;
            whitePieces.Add(whiteRook0);

            whiteRook1.Size = new Size(75, 75);
            whiteRook1.Name = "whiteRook1";
            whiteRook1.Location = new Point(525, 0);
            pieceLoc = whiteRook1.Location;
            whiteRook1.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whiteRook1.Location = tempLoc;
            whiteRook1.Image = Image.FromFile("../../ChessPieces/WhiteRook.png");
            whiteRook1.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(whiteRook1);
            whiteRook1.MouseUp += new MouseEventHandler(pieceMouseUp);
            whiteRook1.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[7, 0] = whiteRook1.Name;
            whitePieces.Add(whiteRook1);

            whiteKnight0.Size = new Size(75, 75);
            whiteKnight0.Name = "whiteKnight0";
            whiteKnight0.Location = new Point(75, 0);
            pieceLoc = whiteKnight0.Location;
            whiteKnight0.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whiteKnight0.Location = tempLoc;
            whiteKnight0.Image = Image.FromFile("../../ChessPieces/WhiteKnight.png");
            whiteKnight0.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(whiteKnight0);
            whiteKnight0.MouseUp += new MouseEventHandler(pieceMouseUp);
            whiteKnight0.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[1, 0] = whiteKnight0.Name;
            whitePieces.Add(whiteKnight0);

            whiteKnight1.Size = new Size(75, 75);
            whiteKnight1.Name = "whiteKnight1";
            whiteKnight1.Location = new Point(450, 0);
            pieceLoc = whiteKnight1.Location;
            whiteKnight1.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whiteKnight1.Location = tempLoc;
            whiteKnight1.Image = Image.FromFile("../../ChessPieces/WhiteKnight.png");
            whiteKnight1.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(whiteKnight1);
            whiteKnight1.MouseUp += new MouseEventHandler(pieceMouseUp);
            whiteKnight1.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[6, 0] = whiteKnight1.Name;
            whitePieces.Add(whiteKnight1);

            whiteBishop0.Size = new Size(75, 75);
            whiteBishop0.Name = "whiteBishop0";
            whiteBishop0.Location = new Point(150, 0);
            pieceLoc = whiteBishop0.Location;
            whiteBishop0.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whiteBishop0.Location = tempLoc;
            whiteBishop0.Image = Image.FromFile("../../ChessPieces/WhiteBishop.png");
            whiteBishop0.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(whiteBishop0);
            whiteBishop0.MouseUp += new MouseEventHandler(pieceMouseUp);
            whiteBishop0.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[2, 0] = whiteBishop0.Name;
            whitePieces.Add(whiteBishop0);

            whiteBishop1.Size = new Size(75, 75);
            whiteBishop1.Name = "whiteBishop1";
            whiteBishop1.Location = new Point(375, 0);
            pieceLoc = whiteBishop1.Location;
            whiteBishop1.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whiteBishop1.Location = tempLoc;
            whiteBishop1.Image = Image.FromFile("../../ChessPieces/WhiteBishop.png");
            whiteBishop1.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(whiteBishop1);
            whiteBishop1.MouseUp += new MouseEventHandler(pieceMouseUp);
            whiteBishop1.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[5, 0] = whiteBishop1.Name;
            whitePieces.Add(whiteBishop1);

            whiteKing.Size = new Size(75, 75);
            whiteKing.Name = "whiteKing";
            whiteKing.Location = new Point(225, 0);
            pieceLoc = whiteKing.Location;
            whiteKing.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whiteKing.Location = tempLoc;
            whiteKing.Image = Image.FromFile("../../ChessPieces/WhiteKing.png");
            whiteKing.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(whiteKing);
            whiteKing.MouseUp += new MouseEventHandler(pieceMouseUp);
            whiteKing.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[3, 0] = whiteKing.Name;
            whitePieces.Add(whiteKing);

            whiteQueen.Size = new Size(75, 75);
            whiteQueen.Name = "whiteQueen";
            whiteQueen.Location = new Point(300, 0);
            pieceLoc = whiteQueen.Location;
            whiteQueen.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whiteQueen.Location = tempLoc;
            whiteQueen.Image = Image.FromFile("../../ChessPieces/WhiteQueen.png");
            whiteQueen.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(whiteQueen);
            whiteQueen.MouseUp += new MouseEventHandler(pieceMouseUp);
            whiteQueen.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[4, 0] = whiteQueen.Name;
            whitePieces.Add(whiteQueen);

            //Black Pieces
            blackPawn0.Size = new Size(75, 75);
            blackPawn0.Name = "blackPawn0";
            blackPawn0.Location = new Point(0, 450);
            pieceLoc = blackPawn0.Location;
            blackPawn0.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackPawn0.Location = tempLoc;
            blackPawn0.Image = Image.FromFile("../../ChessPieces/blackpawn.png");
            blackPawn0.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(blackPawn0);
            blackPawn0.MouseUp += new MouseEventHandler(pieceMouseUp);
            blackPawn0.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[0, 6] = blackPawn0.Name;
            blackPieces.Add(blackPawn0);

            blackPawn1.Size = new Size(75, 75);
            blackPawn1.Name = "blackPawn1";
            blackPawn1.Location = new Point(75, 450);
            pieceLoc = blackPawn1.Location;
            blackPawn1.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackPawn1.Location = tempLoc;
            blackPawn1.Image = Image.FromFile("../../ChessPieces/blackpawn.png");
            blackPawn1.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(blackPawn1);
            blackPawn1.MouseUp += new MouseEventHandler(pieceMouseUp);
            blackPawn1.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[1, 6] = blackPawn1.Name;
            blackPieces.Add(blackPawn1);

            blackPawn2.Size = new Size(75, 75);
            blackPawn2.Name = "blackPawn2";
            blackPawn2.Location = new Point(150, 450);
            pieceLoc = blackPawn2.Location;
            blackPawn2.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackPawn2.Location = tempLoc;
            blackPawn2.Image = Image.FromFile("../../ChessPieces/blackpawn.png");
            blackPawn2.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(blackPawn2);
            blackPawn2.MouseUp += new MouseEventHandler(pieceMouseUp);
            blackPawn2.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[2, 6] = blackPawn2.Name;
            blackPieces.Add(blackPawn2);

            blackPawn3.Size = new Size(75, 75);
            blackPawn3.Name = "blackPawn3";
            blackPawn3.Location = new Point(225, 450);
            pieceLoc = blackPawn3.Location;
            blackPawn3.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackPawn3.Location = tempLoc;
            blackPawn3.Image = Image.FromFile("../../ChessPieces/blackpawn.png");
            blackPawn3.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(blackPawn3);
            blackPawn3.MouseUp += new MouseEventHandler(pieceMouseUp);
            blackPawn3.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[3, 6] = blackPawn3.Name;
            blackPieces.Add(blackPawn3);

            blackPawn4.Size = new Size(75, 75);
            blackPawn4.Name = "blackPawn4";
            blackPawn4.Location = new Point(300, 450);
            pieceLoc = blackPawn4.Location;
            blackPawn4.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackPawn4.Location = tempLoc;
            blackPawn4.Image = Image.FromFile("../../ChessPieces/blackpawn.png");
            blackPawn4.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(blackPawn4);
            blackPawn4.MouseUp += new MouseEventHandler(pieceMouseUp);
            blackPawn4.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[4, 6] = blackPawn4.Name;
            blackPieces.Add(blackPawn4);

            blackPawn5.Size = new Size(75, 75);
            blackPawn5.Name = "blackPawn5";
            blackPawn5.Location = new Point(375, 450);
            pieceLoc = blackPawn5.Location;
            blackPawn5.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackPawn5.Location = tempLoc;
            blackPawn5.Image = Image.FromFile("../../ChessPieces/blackpawn.png");
            blackPawn5.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(blackPawn5);
            blackPawn5.MouseUp += new MouseEventHandler(pieceMouseUp);
            blackPawn5.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[5, 6] = blackPawn5.Name;
            blackPieces.Add(blackPawn5);

            blackPawn6.Size = new Size(75, 75);
            blackPawn6.Name = "blackPawn6";
            blackPawn6.Location = new Point(450, 450);
            pieceLoc = blackPawn6.Location;
            blackPawn6.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackPawn6.Location = tempLoc;
            blackPawn6.Image = Image.FromFile("../../ChessPieces/blackpawn.png");
            blackPawn6.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(blackPawn6);
            blackPawn6.MouseUp += new MouseEventHandler(pieceMouseUp);
            blackPawn6.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[6, 6] = blackPawn6.Name;
            blackPieces.Add(blackPawn6);

            blackPawn7.Size = new Size(75, 75);
            blackPawn7.Name = "blackPawn7";
            blackPawn7.Location = new Point(525, 450);
            pieceLoc = blackPawn7.Location;
            blackPawn7.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackPawn7.Location = tempLoc;
            blackPawn7.Image = Image.FromFile("../../ChessPieces/blackpawn.png");
            blackPawn7.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(blackPawn7);
            blackPawn7.MouseUp += new MouseEventHandler(pieceMouseUp);
            blackPawn7.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[7, 6] = blackPawn7.Name;
            blackPieces.Add(blackPawn7);

            blackRook0.Size = new Size(75, 75);
            blackRook0.Name = "blackRook0";
            blackRook0.Location = new Point(0, 525);
            pieceLoc = blackRook0.Location;
            blackRook0.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackRook0.Location = tempLoc;
            blackRook0.Image = Image.FromFile("../../ChessPieces/BlackRook.png");
            blackRook0.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(blackRook0);
            blackRook0.MouseUp += new MouseEventHandler(pieceMouseUp);
            blackRook0.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[0, 7] = blackRook0.Name;
            blackPieces.Add(blackRook0);

            blackRook1.Size = new Size(75, 75);
            blackRook1.Name = "blackRook1";
            blackRook1.Location = new Point(525, 525);
            pieceLoc = blackRook1.Location;
            blackRook1.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackRook1.Location = tempLoc;
            blackRook1.Image = Image.FromFile("../../ChessPieces/BlackRook.png");
            blackRook1.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(blackRook1);
            blackRook1.MouseUp += new MouseEventHandler(pieceMouseUp);
            blackRook1.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[7, 7] = blackRook1.Name;
            blackPieces.Add(blackRook1);

            blackKnight0.Size = new Size(75, 75);
            blackKnight0.Name = "blackKnight0";
            blackKnight0.Location = new Point(75, 525);
            pieceLoc = blackKnight0.Location;
            blackKnight0.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackKnight0.Location = tempLoc;
            blackKnight0.Image = Image.FromFile("../../ChessPieces/BlackKnight.png");
            blackKnight0.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(blackKnight0);
            blackKnight0.MouseUp += new MouseEventHandler(pieceMouseUp);
            blackKnight0.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[1, 7] = blackKnight0.Name;
            blackPieces.Add(blackKnight0);

            blackKnight1.Size = new Size(75, 75);
            blackKnight1.Name = "blackKnight1";
            blackKnight1.Location = new Point(450, 525);
            pieceLoc = blackKnight1.Location;
            blackKnight1.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackKnight1.Location = tempLoc;
            blackKnight1.Image = Image.FromFile("../../ChessPieces/BlackKnight.png");
            blackKnight1.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(blackKnight1);
            blackKnight1.MouseUp += new MouseEventHandler(pieceMouseUp);
            blackKnight1.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[6, 7] = blackKnight1.Name;
            blackPieces.Add(blackKnight1);

            blackBishop0.Size = new Size(75, 75);
            blackBishop0.Name = "blackBishop0";
            blackBishop0.Location = new Point(150, 525);
            pieceLoc = blackBishop0.Location;
            blackBishop0.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackBishop0.Location = tempLoc;
            blackBishop0.Image = Image.FromFile("../../ChessPieces/BlackBishop.png");
            blackBishop0.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(blackBishop0);
            blackBishop0.MouseUp += new MouseEventHandler(pieceMouseUp);
            blackBishop0.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[2, 7] = blackBishop0.Name;
            blackPieces.Add(blackBishop0);

            blackBishop1.Size = new Size(75, 75);
            blackBishop1.Name = "blackBishop1";
            blackBishop1.Location = new Point(375, 525);
            pieceLoc = blackBishop1.Location;
            blackBishop1.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackBishop1.Location = tempLoc;
            blackBishop1.Image = Image.FromFile("../../ChessPieces/BlackBishop.png");
            blackBishop1.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(blackBishop1);
            blackBishop1.MouseUp += new MouseEventHandler(pieceMouseUp);
            blackBishop1.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[5, 7] = blackBishop1.Name;
            blackPieces.Add(blackBishop1);

            blackKing.Size = new Size(75, 75);
            blackKing.Name = "blackKing";
            blackKing.Location = new Point(300, 525);
            pieceLoc = blackKing.Location;
            blackKing.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackKing.Location = tempLoc;
            blackKing.Image = Image.FromFile("../../ChessPieces/BlackKing.png");
            blackKing.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(blackKing);
            blackKing.MouseUp += new MouseEventHandler(pieceMouseUp);
            blackKing.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[4, 7] = blackKing.Name;
            blackPieces.Add(blackKing);

            blackQueen.Size = new Size(75, 75);
            blackQueen.Name = "blackQueen";
            blackQueen.Location = new Point(225, 525);
            pieceLoc = blackQueen.Location;
            blackQueen.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackQueen.Location = tempLoc;
            blackQueen.Image = Image.FromFile("../../ChessPieces/BlackQueen.png");
            blackQueen.SizeMode = PictureBoxSizeMode.StretchImage;
            chessBoard.Controls.Add(blackQueen);
            blackQueen.MouseUp += new MouseEventHandler(pieceMouseUp);
            blackQueen.MouseDown += new MouseEventHandler(pieceMouseDown);
            boardData[3, 7] = blackQueen.Name;
            blackPieces.Add(blackQueen);
        }

        /*********************************************************************************
         Method     : pawn
         Purpose    : This method populates a 2D array of boolean values that will represent
                       possible moves on the board with the constraints of a pawn. First move
                       can be two spaces, every other move is one space as long as the space 
                       is unoccupied. Pawns eliminate by diagonal moves. 
         Parameters : 1. xIdnex  - X-Coordinate of selected piece
                      2. yIndex  - Y-Coordinate of selected piece
                      3. isWhite - boolean determining the color of the selected piece
         Returns    : N/A
        *********************************************************************************/

        private void pawn(int xIndex, int yIndex, bool isWhite)
        {

            //yDirection is added to the pawn's y-coordinate.
            //Its polarity changes based on color so it can only move in the correct direction.
            int yDirection = -1;
            if (isWhite)
                yDirection *= -1;

            //Check if the pawn can actually move forward.
            //This statement covers both black and white.
            if (yIndex + yDirection < 8 && yIndex + yDirection > 0) {

                //If the space in front of the pawn is open, update the table so that it is a valid move.
                if (spaceTaken[xIndex, yIndex + yDirection] == false)
                {
                    possibleMove[xIndex, yIndex + yDirection] = true;

                    //If the pawn has not moved yet and the space two squares in front is open, then add
                    //that square as a valid move.
                    if (isWhite && yIndex == 1 && spaceTaken[xIndex, yIndex + (yDirection * 2)] == false || !isWhite && yIndex == 6 && spaceTaken[xIndex, yIndex + (yDirection * 2)] == false)
                    {

                        possibleMove[xIndex, yIndex + (yDirection * 2)] = true;

                    }

                }

                //If there is room to the left, check if the space forward and to the left has an enemy.
                //If it does, then it is a valid move.
                if (xIndex > 0 && spaceTaken[xIndex - 1, yIndex + yDirection] == true)
                {

                    //If the piece is white, check the black pieces and see if they are in the space.
                    //If so, it's valid. Otherwise, it is left alone.
                    if (isWhite)
                    {

                        foreach (PictureBox pic in blackPieces)
                        {

                            if (pic.Location.X == ((xIndex - 1) * 75) && pic.Location.Y == ((yIndex + yDirection) * 75))
                            {

                                possibleMove[xIndex - 1, yIndex + yDirection] = true;
                                break;

                            }

                        }

                    }
                    else
                    {

                        foreach (PictureBox pic in whitePieces)
                        {

                            if (pic.Location.X == ((xIndex - 1) * 75) && pic.Location.Y == ((yIndex + yDirection) * 75))
                            {

                                possibleMove[xIndex - 1, yIndex + yDirection] = true;
                                break;

                            }

                        }

                    }

                }

                //Same as before, but if the right is open.
                if (xIndex < 7 && spaceTaken[xIndex + 1, yIndex + yDirection] == true)
                {

                    if (isWhite)
                    {

                        foreach (PictureBox pic in blackPieces)
                        {

                            if (pic.Location.X == ((xIndex + 1) * 75) && pic.Location.Y == ((yIndex + yDirection) * 75))
                            {

                                possibleMove[xIndex + 1, yIndex + yDirection] = true;
                                break;

                            }

                        }

                    }
                    else
                    {

                        foreach (PictureBox pic in whitePieces)
                        {

                            if (pic.Location.X == ((xIndex + 1) * 75) && pic.Location.Y == ((yIndex + yDirection) * 75))
                            {

                                possibleMove[xIndex + 1, yIndex + yDirection] = true;
                                break;

                            }

                        }

                    }

                }

            }

        }

        /*********************************************************************************
         Method     : bishop
         Purpose    : This method populates a 2D array of boolean values that will represent
                       possible moves on the board. Bishops may move any number of vacant 
                       squares in any diagonal direction.
         Parameters : 1. xIdnex  - X-Coordinate of selected piece
                      2. yIndex  - Y-Coordinate of selected piece
                      3. isWhite - boolean determining the color of the selected piece
         Returns    : N/A
        *********************************************************************************/

        private void bishop(int xIndex, int yIndex, bool isWhite)
        {

            //Booleans for each direction so a single search can run uninterrupted even
            //if a piece has been detected in one or more directions.
            bool upLeft = true;
            bool upRight = true;
            bool downLeft = true;
            bool downRight = true;

            //Initialize a counter at 1 so we can immmediately check the next squares rather than start
            //on the calling piece.
            for (int i = 1; i < 8; i++)
            {
                
                //This part checks the diagonal that runs up and to the right.
                //If the search indices are not out of bounds and the flag for this direction has not been flipped,
                //then the next diagonal tile is considered.
                //If the square is open, then it is added as a possible move.
                if (xIndex + i < 8 && yIndex + i < 8 && spaceTaken[xIndex + i, yIndex + i] == false && downRight == true)
                    possibleMove[xIndex + i, yIndex + i] = true;
                //If the square is occupied, flip the direction's flag so the bishop no longer searches in this direction
                //for the rest of the loop. Search through the pieces of the opposite color and see if an enemy occupies the square.
                //If so, this is a valid move. If the piece is of the same color, then the bishop cannot move there.
                else if (xIndex + i < 8 && yIndex + i < 8 && spaceTaken[xIndex + i, yIndex + i] == true && downRight == true)
                {

                    downRight = false;

                    if (isWhite)
                    {
                        foreach (PictureBox pic in blackPieces)
                        {

                            if (pic.Location.X == ((xIndex + i) * 75) && pic.Location.Y == ((yIndex + i) * 75))
                            {

                                possibleMove[xIndex + i, yIndex + i] = true;
                                break;

                            }

                        }
                    }
                    else
                    {

                        foreach (PictureBox pic in whitePieces)
                        {

                            if (pic.Location.X == ((xIndex + i) * 75) && pic.Location.Y == ((yIndex + i) * 75))
                            {

                                possibleMove[xIndex + i, yIndex + i] = true;
                                break;

                            }

                        }

                    }

                }

                //The rest of the code is the same except it checks the directions seen at the right end of the conditions.

                if (xIndex + i < 8 && yIndex - i >= 0 && spaceTaken[xIndex + i, yIndex - i] == false && upRight == true)
                    possibleMove[xIndex + i, yIndex - i] = true;
                else if (xIndex + i < 8 && yIndex - i >= 0 && spaceTaken[xIndex + i, yIndex - i] == true && upRight == true)
                {

                    upRight = false;

                    if (isWhite)
                    {
                        foreach (PictureBox pic in blackPieces)
                        {

                            if (pic.Location.X == ((xIndex + i) * 75) && pic.Location.Y == ((yIndex + i) * 75))
                            {

                                possibleMove[xIndex + i, yIndex - i] = true;
                                break;

                            }

                        }
                    }
                    else
                    {

                        foreach (PictureBox pic in whitePieces)
                        {

                            if (pic.Location.X == ((xIndex + i) * 75) && pic.Location.Y == ((yIndex + i) * 75))
                            {

                                possibleMove[xIndex + i, yIndex - i] = true;
                                break;

                            }

                        }

                    }

                }

                if (xIndex - i >= 0 && yIndex + i < 8 && spaceTaken[xIndex - i, yIndex + i] == false && downLeft == true)
                    possibleMove[xIndex - i, yIndex + i] = true;
                else if (xIndex - i >= 0 && yIndex + i < 8 && spaceTaken[xIndex - i, yIndex + i] == true && downLeft == true)
                {

                    downLeft = false;

                    if (isWhite)
                    {
                        foreach (PictureBox pic in blackPieces)
                        {

                            if (pic.Location.X == ((xIndex + i) * 75) && pic.Location.Y == ((yIndex + i) * 75))
                            {

                                possibleMove[xIndex - i, yIndex + i] = true;
                                break;

                            }

                        }
                    }
                    else
                    {

                        foreach (PictureBox pic in whitePieces)
                        {

                            if (pic.Location.X == ((xIndex + i) * 75) && pic.Location.Y == ((yIndex + i) * 75))
                            {

                                possibleMove[xIndex - i, yIndex + i] = true;
                                break;

                            }

                        }

                    }

                }

                if (xIndex - i >= 0 && yIndex - i >= 0 && spaceTaken[xIndex - i, yIndex - i] == false && upLeft == true)
                    possibleMove[xIndex - i, yIndex - i] = true;
                else if (xIndex - i >= 0 && yIndex - i >= 0 && spaceTaken[xIndex - i, yIndex - i] == true && upLeft == true)
                {

                    upLeft = false;

                    if (isWhite)
                    {
                        foreach (PictureBox pic in blackPieces)
                        {

                            if (pic.Location.X == ((xIndex - i) * 75) && pic.Location.Y == ((yIndex - i) * 75))
                            {

                                possibleMove[xIndex - i, yIndex - i] = true;
                                break;

                            }

                        }
                    }
                    else
                    {

                        foreach (PictureBox pic in whitePieces)
                        {

                            if (pic.Location.X == ((xIndex - i) * 75) && pic.Location.Y == ((yIndex - i) * 75))
                            {

                                possibleMove[xIndex - i, yIndex - i] = true;
                                break;

                            }

                        }

                    }

                }

            }

        }

        /*********************************************************************************
         Method     : knight
         Purpose    : This method populates a 2D array of boolean values that will represent
                       possible moves on the board. Knights move in an "L" formation.
         Parameters : 1. xIdnex  - X-Coordinate of selected piece
                      2. yIndex  - Y-Coordinate of selected piece
                      3. isWhite - boolean determining the color of the selected piece
         Returns    : N/A
        *********************************************************************************/

        private void knight(int xIndex, int yIndex, bool isWhite)
        {

            //Start by checking if the knight has enough room to jump two spaces to the left for that
            //pair of jumps.
            if (xIndex - 2 >= 0)
            {

                //If the knight can also move up one space and the space at the end of that L is empty,
                //Then this is a valid move. If an enemy occupies it, the move is valid.
                //x
                //---
                if (yIndex - 1 >= 0)
                {

                    if (spaceTaken[xIndex - 2, yIndex - 1] == false)
                        possibleMove[xIndex - 2, yIndex - 1] = true;
                    else
                    {

                        if (isWhite)
                        {

                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex - 2) * 75) && pic.Location.Y == ((yIndex - 1) * 75))
                                {

                                    possibleMove[xIndex - 2, yIndex - 1] = true;
                                    break;

                                }

                            }

                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex - 2) * 75) && pic.Location.Y == ((yIndex - 1) * 75))
                                {

                                    possibleMove[xIndex - 2, yIndex - 1] = true;
                                    break;

                                }

                            }

                        }

                    }

                }

                //Check if the knight can move down one for the other space of the L
                //---
                //x
                if (yIndex + 1 < 8)
                {

                    if (spaceTaken[xIndex - 2, yIndex + 1] == false)
                        possibleMove[xIndex - 2, yIndex + 1] = true;
                    else
                    {

                        if (isWhite)
                        {

                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex - 2) * 75) && pic.Location.Y == ((yIndex + 1) * 75))
                                {

                                    possibleMove[xIndex - 2, yIndex + 1] = true;
                                    break;

                                }

                            }

                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex - 2) * 75) && pic.Location.Y == ((yIndex + 1) * 75))
                                {

                                    possibleMove[xIndex - 2, yIndex + 1] = true;
                                    break;

                                }

                            }

                        }

                    }

                }

            }

            //Do the same as the above, this time checking if the knight can jump two spaces to the right.
            if (xIndex + 2 < 8)
            {
                //  x
                //---
                if (yIndex - 1 >= 0)
                {

                    if (spaceTaken[xIndex + 2, yIndex - 1] == false)
                        possibleMove[xIndex + 2, yIndex - 1] = true;
                    else
                    {

                        if (isWhite)
                        {

                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex + 2) * 75) && pic.Location.Y == ((yIndex - 1) * 75))
                                {

                                    possibleMove[xIndex + 2, yIndex - 1] = true;
                                    break;

                                }

                            }

                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex + 2) * 75) && pic.Location.Y == ((yIndex - 1) * 75))
                                {

                                    possibleMove[xIndex + 2, yIndex - 1] = true;
                                    break;

                                }

                            }

                        }

                    }

                }

                //---
                //  x
                if (yIndex + 1 < 8)
                {

                    if (spaceTaken[xIndex + 2, yIndex + 1] == false)
                        possibleMove[xIndex + 2, yIndex + 1] = true;
                    else
                    {

                        if (isWhite)
                        {

                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex + 2) * 75) && pic.Location.Y == ((yIndex + 1) * 75))
                                {

                                    possibleMove[xIndex + 2, yIndex + 1] = true;
                                    break;

                                }

                            }

                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex + 2) * 75) && pic.Location.Y == ((yIndex + 1) * 75))
                                {

                                    possibleMove[xIndex + 2, yIndex + 1] = true;
                                    break;

                                }

                            }

                        }

                    }

                }

            }

            //The following pairs of base if statements do essentially the same thing as the previous two.
            //This time, the knight checks if it can jump two spaces either up or down, then checks if it
            //can move one to the left or right.
            if (yIndex - 2 >= 0)
            {
                //x-
                // -
                // -
                if (xIndex - 1 >= 0)
                {

                    if (spaceTaken[xIndex - 1, yIndex - 2] == false)
                        possibleMove[xIndex - 1, yIndex - 2] = true;
                    else
                    {

                        if (isWhite)
                        {

                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex - 1) * 75) && pic.Location.Y == ((yIndex - 2) * 75))
                                {

                                    possibleMove[xIndex - 1, yIndex - 2] = true;
                                    break;

                                }

                            }

                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex - 1) * 75) && pic.Location.Y == ((yIndex - 2) * 75))
                                {

                                    possibleMove[xIndex - 1, yIndex - 2] = true;
                                    break;

                                }

                            }

                        }

                    }

                }

                //-x
                //-
                //-
                if (xIndex + 1 < 8)
                {

                    if (spaceTaken[xIndex + 1, yIndex - 2] == false)
                        possibleMove[xIndex + 1, yIndex - 2] = true;
                    else
                    {

                        if (isWhite)
                        {

                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex + 1) * 75) && pic.Location.Y == ((yIndex - 2) * 75))
                                {

                                    possibleMove[xIndex + 1, yIndex - 2] = true;
                                    break;

                                }

                            }

                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex + 1) * 75) && pic.Location.Y == ((yIndex - 2) * 75))
                                {

                                    possibleMove[xIndex + 1, yIndex - 2] = true;
                                    break;

                                }

                            }

                        }

                    }

                }

            }

            if (yIndex + 2 < 8)
            {

                // -
                // -
                //x-
                if (xIndex - 1 >= 0)
                {

                    if (spaceTaken[xIndex - 1, yIndex + 2] == false)
                        possibleMove[xIndex - 1, yIndex + 2] = true;
                    else
                    {

                        if (isWhite)
                        {

                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex - 1) * 75) && pic.Location.Y == ((yIndex + 2) * 75))
                                {



                                    possibleMove[xIndex - 1, yIndex + 2] = true;
                                    break;

                                }

                            }

                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex - 1) * 75) && pic.Location.Y == ((yIndex + 2) * 75))
                                {

                                    possibleMove[xIndex - 1, yIndex + 2] = true;
                                    break;

                                }

                            }

                        }

                    }

                }

                //-
                //-
                //-x
                if (xIndex + 1 < 8)
                {

                    if (spaceTaken[xIndex + 1, yIndex + 2] == false)
                        possibleMove[xIndex + 1, yIndex + 2] = true;
                    else
                    {

                        if (isWhite)
                        {

                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex + 1) * 75) && pic.Location.Y == ((yIndex + 2) * 75))
                                {

                                    possibleMove[xIndex + 1, yIndex + 2] = true;
                                    break;

                                }

                            }

                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex + 1) * 75) && pic.Location.Y == ((yIndex + 2) * 75))
                                {

                                    possibleMove[xIndex + 1, yIndex + 2] = true;
                                    break;

                                }

                            }

                        }

                    }

                }

            }

        }

        /*********************************************************************************
         Method     : queen
         Purpose    : This method populates a 2D array of boolean values that will represent
                       possible moves on the board. Queen may move any number of vacant spaces
                       diagonally, horizontally, or vertically.
         Parameters : 1. xIdnex  - X-Coordinate of selected piece
                      2. yIndex  - Y-Coordinate of selected piece
                      3. isWhite - boolean determining the color of the selected piece
         Returns    : N/A
        *********************************************************************************/

        private void queen(int xIndex, int yIndex, bool isWhite)
        {

            //Boolean values indicating a piece has not been found in that direction
            bool up = true;
            bool down = true;
            bool left = true;
            bool right = true;

            bool upLeft = true;
            bool upRight = true;
            bool downLeft = true;
            bool downRight = true;

            //Find possible Up/Down/Right/Left moves
            for(int i = 1; i < 8; i++)
            {
                //Right Moves
                if (xIndex + i < 8 && yIndex < 8 && spaceTaken[xIndex + i, yIndex] == false && right == true)
                    possibleMove[xIndex + i, yIndex] = true;
                else if (xIndex + i < 8 && yIndex < 8 && spaceTaken[xIndex + i, yIndex] == true && right == true)
                {
                    right = false;

                    foreach (PictureBox pic in blackPieces)
                    {

                        if (pic.Location.X == ((xIndex + i) * 75) && pic.Location.Y == (yIndex * 75))
                        {

                            possibleMove[xIndex + i, yIndex] = true;
                            break;

                        }

                    }
                }

                //Left Moves
                if (xIndex - i >= 0 && xIndex - i < 8 && yIndex < 8 && spaceTaken[xIndex - i, yIndex] == false && left == true)
                    possibleMove[xIndex - i, yIndex] = true;
                else if (xIndex - i >= 0 && xIndex - i < 8 && yIndex < 8 && spaceTaken[xIndex - i, yIndex] == true && left == true)
                {
                    left = false;

                    foreach (PictureBox pic in blackPieces)
                    {

                        if (pic.Location.X == ((xIndex - i) * 75) && pic.Location.Y == (yIndex * 75))
                        {

                            possibleMove[xIndex - i, yIndex] = true;
                            break;

                        }

                    }
                }

                //Down Moves
                if (xIndex < 8 && yIndex + i < 8 && spaceTaken[xIndex, yIndex + i] == false && down == true)
                    possibleMove[xIndex, yIndex + i] = true;
                else if (xIndex < 8 && yIndex + i < 8 && spaceTaken[xIndex, yIndex + i] == true && down == true)
                {

                    down = false;

                    foreach (PictureBox pic in blackPieces)
                    {

                        if (pic.Location.X == (xIndex * 75) && pic.Location.Y == ((yIndex + i) * 75))
                        {

                            possibleMove[xIndex, yIndex + i] = true;
                            break;

                        }

                    }

                }

                //Up Moves
                if (xIndex < 8 && yIndex - i >= 0 && spaceTaken[xIndex, yIndex - i] == false && up == true)
                    possibleMove[xIndex, yIndex - i] = true;
                else if (xIndex < 8 && yIndex - i >= 0 && spaceTaken[xIndex, yIndex - i] == true && up == true)
                {

                    up = false;

                    foreach (PictureBox pic in blackPieces)
                    {

                        if (pic.Location.X == (xIndex * 75) && pic.Location.Y == ((yIndex - i) * 75))
                        {

                            possibleMove[xIndex, yIndex - i] = true;
                            break;

                        }

                    }

                }
            }

            //Diagonal Down-Right Moves
            for (int i = 1; i < 8; i++)
            {

                if (xIndex + i < 8 && yIndex + i < 8 && spaceTaken[xIndex + i, yIndex + i] == false && downRight == true)
                    possibleMove[xIndex + i, yIndex + i] = true;
                else if (xIndex + i < 8 && yIndex + i < 8 && spaceTaken[xIndex + i, yIndex + i] == true && downRight == true)
                {

                    downRight = false;

                    foreach (PictureBox pic in blackPieces)
                    {

                        if (pic.Location.X == ((xIndex + i) * 75) && pic.Location.Y == ((yIndex + i) * 75))
                        {

                            possibleMove[xIndex + i, yIndex + i] = true;
                            break;

                        }

                    }

                }

                //Diagonal Up-Right Moves
                if (xIndex + i < 8 && yIndex - i >= 0 && spaceTaken[xIndex + i, yIndex - i] == false && upRight == true)
                    possibleMove[xIndex + i, yIndex - i] = true;
                else if (xIndex + i < 8 && yIndex - i >= 0 && spaceTaken[xIndex + i, yIndex - i] == true && upRight == true)
                {

                    upRight = false;

                    foreach (PictureBox pic in blackPieces)
                    {

                        if (pic.Location.X == ((xIndex + i) * 75) && pic.Location.Y == ((yIndex + i) * 75))
                        {

                            possibleMove[xIndex + i, yIndex - i] = true;
                            break;

                        }

                    }

                }

                //Diagonal Down-Left Moves
                if (xIndex - i >= 0 && yIndex + i < 8 && spaceTaken[xIndex - i, yIndex + i] == false && downLeft == true)
                    possibleMove[xIndex - i, yIndex + i] = true;
                else if (xIndex - i >= 0 && yIndex + i < 8 && spaceTaken[xIndex - i, yIndex + i] == true && downLeft == true)
                {

                    downLeft = false;

                    foreach (PictureBox pic in blackPieces)
                    {

                        if (pic.Location.X == ((xIndex + i) * 75) && pic.Location.Y == ((yIndex + i) * 75))
                        {

                            possibleMove[xIndex - i, yIndex + i] = true;
                            break;

                        }

                    }

                }

                //Diagonal Up-Left Moves
                if (xIndex - i >= 0 && yIndex - i >= 0 && spaceTaken[xIndex - i, yIndex - i] == false && upLeft == true)
                    possibleMove[xIndex - i, yIndex - i] = true;
                else if (xIndex - i >= 0 && yIndex - i >= 0 && spaceTaken[xIndex - i, yIndex - i] == true && upLeft == true)
                {

                    upLeft = false;

                    foreach (PictureBox pic in blackPieces)
                    {

                        if (pic.Location.X == ((xIndex - i) * 75) && pic.Location.Y == ((yIndex - i) * 75))
                        {

                            possibleMove[xIndex - i, yIndex - i] = true;
                            break;

                        }

                    }

                }

            }
        }

        /*********************************************************************************
         Method     : king
         Purpose    : This method populates a 2D array of boolean values that will represent
                       possible moves on the board. Kings may move one square horizontally,
                       vertically, or diagonally. Allowed to "castle".
         Parameters : 1. xIdnex  - X-Coordinate of selected piece
                      2. yIndex  - Y-Coordinate of selected piece
                      3. isWhite - boolean determining the color of the selected piece
         Returns    : N/A
        *********************************************************************************/

        private void king(int xIndex, int yIndex, bool isWhite)
        {

            //Start by checking if the king has room to move up the board.
            //If so, check the three tiles above the king. If the tile can be attacked by an enemy,
            //then the move is not valid and the king cannot move there.
            //Also, if the tile has an enemy and it is not dangerous, then that square is valid.
            if (yIndex - 1 >= 0)
            {
                //Check left
                if (xIndex - 1 >= 0 && tileIsDangerous(xIndex - 1, yIndex - 1, isWhite) == false)
                {

                    if (spaceTaken[xIndex - 1, yIndex - 1] == false)
                        possibleMove[xIndex - 1, yIndex - 1] = true;
                    else
                    {

                        if (isWhite)
                        {
                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex - 1) * 75) && pic.Location.Y == ((yIndex - 1) * 75))
                                {

                                    possibleMove[xIndex - 1, yIndex - 1] = true;
                                    break;

                                }

                            }
                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex - 1) * 75) && pic.Location.Y == ((yIndex - 1) * 75))
                                {

                                    possibleMove[xIndex - 1, yIndex - 1] = true;
                                    break;

                                }

                            }

                        }

                    }

                }

                //Check right
                if (xIndex + 1 >= 0 && tileIsDangerous(xIndex + 1, yIndex - 1, isWhite) == false)
                {

                    if (spaceTaken[xIndex + 1, yIndex - 1] == false)
                        possibleMove[xIndex + 1, yIndex - 1] = true;
                    else
                    {

                        if (isWhite)
                        {
                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex + 1) * 75) && pic.Location.Y == ((yIndex - 1) * 75))
                                {

                                    possibleMove[xIndex + 1, yIndex - 1] = true;
                                    break;

                                }

                            }
                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex + 1) * 75) && pic.Location.Y == ((yIndex - 1) * 75))
                                {

                                    possibleMove[xIndex + 1, yIndex - 1] = true;
                                    break;

                                }

                            }

                        }

                    }

                }

                //Check middle.
                if (tileIsDangerous(xIndex, yIndex - 1, isWhite) == false)
                {

                    if (spaceTaken[xIndex, yIndex - 1] == false)
                        possibleMove[xIndex, yIndex - 1] = true;
                    else
                    {

                        if (isWhite)
                        {
                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex) * 75) && pic.Location.Y == ((yIndex - 1) * 75))
                                {

                                    possibleMove[xIndex, yIndex - 1] = true;
                                    break;

                                }

                            }
                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex) * 75) && pic.Location.Y == ((yIndex - 1) * 75))
                                {

                                    possibleMove[xIndex, yIndex - 1] = true;
                                    break;

                                }

                            }

                        }

                    }

                }

            }

            //Same as above, but checks for the tiles below the king.
            if (yIndex + 1 < 8)
            {

                if (xIndex - 1 >= 0 && tileIsDangerous(xIndex - 1, yIndex + 1, isWhite) == false)
                {

                    if (spaceTaken[xIndex - 1, yIndex + 1] == false)
                        possibleMove[xIndex - 1, yIndex + 1] = true;
                    else
                    {

                        if (isWhite)
                        {
                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex - 1) * 75) && pic.Location.Y == ((yIndex + 1) * 75))
                                {

                                    possibleMove[xIndex - 1, yIndex + 1] = true;
                                    break;

                                }

                            }
                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex - 1) * 75) && pic.Location.Y == ((yIndex + 1) * 75))
                                {

                                    possibleMove[xIndex - 1, yIndex + 1] = true;
                                    break;

                                }

                            }

                        }

                    }

                }

                if (xIndex + 1 >= 0 && tileIsDangerous(xIndex + 1, yIndex + 1, isWhite) == false)
                {

                    if (spaceTaken[xIndex + 1, yIndex + 1] == false)
                        possibleMove[xIndex + 1, yIndex + 1] = true;
                    else
                    {

                        if (isWhite)
                        {
                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex + 1) * 75) && pic.Location.Y == ((yIndex + 1) * 75))
                                {

                                    possibleMove[xIndex + 1, yIndex + 1] = true;
                                    break;

                                }

                            }
                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex + 1) * 75) && pic.Location.Y == ((yIndex + 1) * 75))
                                {

                                    possibleMove[xIndex + 1, yIndex + 1] = true;
                                    break;

                                }

                            }

                        }

                    }

                }

                if (tileIsDangerous(xIndex, yIndex + 1, isWhite) == false)
                {

                    if (spaceTaken[xIndex, yIndex + 1] == false)
                        possibleMove[xIndex, yIndex + 1] = true;
                    else
                    {

                        if (isWhite)
                        {
                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex) * 75) && pic.Location.Y == ((yIndex + 1) * 75))
                                {

                                    possibleMove[xIndex, yIndex + 1] = true;
                                    break;

                                }

                            }
                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex) * 75) && pic.Location.Y == ((yIndex + 1) * 75))
                                {

                                    possibleMove[xIndex, yIndex + 1] = true;
                                    break;

                                }

                            }

                        }

                    }

                }

            }

            //Check the tile directly left of the king.
            if (xIndex - 1 >= 0 && tileIsDangerous(xIndex - 1, yIndex, isWhite) == false)
            {

                if (spaceTaken[xIndex - 1, yIndex] == false)
                    possibleMove[xIndex - 1, yIndex] = true;
                else
                {

                    if (isWhite)
                    {
                        foreach (PictureBox pic in blackPieces)
                        {

                            if (pic.Location.X == ((xIndex - 1) * 75) && pic.Location.Y == ((yIndex) * 75))
                            {

                                possibleMove[xIndex - 1, yIndex] = true;
                                break;

                            }

                        }
                    }
                    else
                    {

                        foreach (PictureBox pic in whitePieces)
                        {

                            if (pic.Location.X == ((xIndex - 1) * 75) && pic.Location.Y == ((yIndex) * 75))
                            {

                                possibleMove[xIndex - 1, yIndex] = true;
                                break;

                            }

                        }

                    }

                }

            }

            //Check directly right.
            if (xIndex + 1 < 8 && tileIsDangerous(xIndex - 1, yIndex, isWhite) == false)
            {

                if (spaceTaken[xIndex + 1, yIndex] == false)
                    possibleMove[xIndex + 1, yIndex] = true;
                else
                {

                    if (isWhite)
                    {
                        foreach (PictureBox pic in blackPieces)
                        {

                            if (pic.Location.X == ((xIndex + 1) * 75) && pic.Location.Y == ((yIndex) * 75))
                            {

                                possibleMove[xIndex + 1, yIndex] = true;
                                break;

                            }

                        }
                    }
                    else
                    {

                        foreach (PictureBox pic in whitePieces)
                        {

                            if (pic.Location.X == ((xIndex + 1) * 75) && pic.Location.Y == ((yIndex) * 75))
                            {

                                possibleMove[xIndex + 1, yIndex] = true;
                                break;

                            }

                        }

                    }

                }

            }

        }

        /*********************************************************************************
         Method     : tileIsDangerous
         Purpose    : This function checks if the tile at the input coordinates can be
                      attacked by the enemy player. This is mostly used for checking if
                      the king is in check.
         Parameters : 1. xIdnex  - X-Coordinate of selected piece
                      2. yIndex  - Y-Coordinate of selected piece
                      3. isWhite - boolean determining the color of the selected piece
         Returns    : N/A
        *********************************************************************************/

        private bool tileIsDangerous(int xIndex, int yIndex, bool isWhite)
        {

            //Each check works fundamentally the same as the piece that the check looks for.
            //Starting from the given square, check the squares that a piece of that type can travel.
            //If, along the way, a piece of that type is encountered and it is an enemy, then the square is considered not safe and the
            //function returns true.

            //If the square makes it all the way through each check, then it is considered safe for the color piece that was passed in.

            //Check for bishops and queens
            bool upLeft = true;
            bool upRight = true;
            bool downLeft = true;
            bool downRight = true;
            for (int i = 1; i < 8; i++)
            {

                if (xIndex + i < 8 && yIndex + i < 8 && spaceTaken[xIndex + i, yIndex + i] == true && downRight == true)
                {

                    downRight = false;

                    if (isWhite)
                    {
                        foreach (PictureBox pic in blackPieces)
                        {

                            if (pic.Location.X == ((xIndex + i) * 75) && pic.Location.Y == ((yIndex + i) * 75))
                            {

                                if (pic.Name.Substring(5, 3) == "Bis" || pic.Name.Substring(5, 3) == "Que")
                                    return true;
                                else
                                    break;

                            }

                        }
                    }
                    else
                    {

                        foreach (PictureBox pic in whitePieces)
                        {

                            if (pic.Location.X == ((xIndex + i) * 75) && pic.Location.Y == ((yIndex + i) * 75))
                            {

                                if (pic.Name.Substring(5, 3) == "Bis" || pic.Name.Substring(5, 3) == "Que")
                                    return true;
                                else
                                    break;

                            }

                        }

                    }

                }

                if (xIndex + i < 8 && yIndex - i >= 0 && spaceTaken[xIndex + i, yIndex - i] == true && upRight == true)
                {

                    upRight = false;

                    if (isWhite)
                    {
                        foreach (PictureBox pic in blackPieces)
                        {

                            if (pic.Location.X == ((xIndex + i) * 75) && pic.Location.Y == ((yIndex - i) * 75))
                            {

                                if (pic.Name.Substring(5, 3) == "Bis" || pic.Name.Substring(5, 3) == "Que")
                                    return true;
                                else
                                    break;

                            }

                        }
                    }
                    else
                    {

                        foreach (PictureBox pic in whitePieces)
                        {

                            if (pic.Location.X == ((xIndex + i) * 75) && pic.Location.Y == ((yIndex - i) * 75))
                            {

                                if (pic.Name.Substring(5, 3) == "Bis" || pic.Name.Substring(5, 3) == "Que")
                                    return true;
                                else
                                    break;

                            }

                        }

                    }

                }

                if (xIndex - i >= 0 && yIndex + i < 8 && spaceTaken[xIndex - i, yIndex + i] == true && downLeft == true)
                {

                    downLeft = false;

                    if (isWhite)
                    {
                        foreach (PictureBox pic in blackPieces)
                        {

                            if (pic.Location.X == ((xIndex - i) * 75) && pic.Location.Y == ((yIndex + i) * 75))
                            {

                                if (pic.Name.Substring(5, 3) == "Bis" || pic.Name.Substring(5, 3) == "Que")
                                    return true;
                                else
                                    break;

                            }

                        }
                    }
                    else
                    {

                        foreach (PictureBox pic in whitePieces)
                        {

                            if (pic.Location.X == ((xIndex - i) * 75) && pic.Location.Y == ((yIndex + i) * 75))
                            {

                                if (pic.Name.Substring(5, 3) == "Bis" || pic.Name.Substring(5, 3) == "Que")
                                    return true;
                                else
                                    break;

                            }

                        }

                    }

                }

                if (xIndex - i >= 0 && yIndex - i >= 0 && spaceTaken[xIndex - i, yIndex - i] == true && upLeft == true)
                {

                    upLeft = false;

                    if (isWhite)
                    {
                        foreach (PictureBox pic in blackPieces)
                        {

                            if (pic.Location.X == ((xIndex - i) * 75) && pic.Location.Y == ((yIndex - i) * 75))
                            {

                                if (pic.Name.Substring(5, 3) == "Bis" || pic.Name.Substring(5, 3) == "Que")
                                    return true;
                                else
                                    break;

                            }

                        }
                    }
                    else
                    {

                        foreach (PictureBox pic in whitePieces)
                        {

                            if (pic.Location.X == ((xIndex - i) * 75) && pic.Location.Y == ((yIndex - i) * 75))
                            {

                                if (pic.Name.Substring(5, 3) == "Bis" || pic.Name.Substring(5, 3) == "Que")
                                    return true;
                                else
                                    break;

                            }

                        }

                    }

                }

            }

            //Check for pawns
            if (isWhite)
            {

                if (xIndex - 1 >= 0 && yIndex + 1 < 8)
                {

                    foreach (PictureBox pic in blackPieces)
                    {

                        if (pic.Location.X == ((xIndex - 1) * 75) && pic.Location.Y == ((yIndex + 1) * 75))
                        {

                            if (pic.Name.Substring(5, 3) == "Paw")
                                return true;
                            else
                                return false;

                        }

                    }

                }
                if (xIndex + 1 < 8 && yIndex + 1 < 8)
                {

                    foreach (PictureBox pic in blackPieces)
                    {

                        if (pic.Location.X == ((xIndex + 1) * 75) && pic.Location.Y == ((yIndex + 1) * 75))
                        {

                            if (pic.Name.Substring(5, 3) == "Paw")
                                return true;
                            else
                                return false;

                        }

                    }

                }

            }
            else
            {

                if (xIndex - 1 >= 0 && yIndex - 1 >= 0)
                {

                    foreach (PictureBox pic in whitePieces)
                    {

                        if (pic.Location.X == ((xIndex - 1) * 75) && pic.Location.Y == ((yIndex - 1) * 75))
                        {

                            if (pic.Name.Substring(5, 3) == "Paw")
                                return true;
                            else
                                return false;

                        }

                    }

                }
                if (xIndex + 1 < 8 && yIndex - 1 >= 0)
                {

                    foreach (PictureBox pic in whitePieces)
                    {

                        if (pic.Location.X == ((xIndex + 1) * 75) && pic.Location.Y == ((yIndex - 1) * 75))
                        {

                            if (pic.Name.Substring(5, 3) == "Paw")
                                return true;
                            else
                                return false;

                        }

                    }

                }

            }

            //Check for knights
            if (xIndex - 2 >= 0)
            {

                if (yIndex - 1 >= 0)
                {

                    if (spaceTaken[xIndex - 2, yIndex - 1] == true)
                    {

                        if (isWhite)
                        {

                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex - 2) * 75) && pic.Location.Y == ((yIndex - 1) * 75) && pic.Name.Substring(5, 3) == "Kni")
                                {

                                    return true;

                                }

                            }

                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex - 2) * 75) && pic.Location.Y == ((yIndex - 1) * 75) && pic.Name.Substring(5, 3) == "Kni")
                                {

                                    return true;

                                }

                            }

                        }

                    }

                }

                if (yIndex + 1 < 8)
                {

                    if (spaceTaken[xIndex - 2, yIndex + 1] == true)
                    {

                        if (isWhite)
                        {

                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex - 2) * 75) && pic.Location.Y == ((yIndex + 1) * 75) && pic.Name.Substring(5, 3) == "Kni")
                                {

                                    return true;

                                }

                            }

                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex - 2) * 75) && pic.Location.Y == ((yIndex + 1) * 75) && pic.Name.Substring(5, 3) == "Kni")
                                {

                                    return true;

                                }

                            }

                        }

                    }

                }

            }

            if (xIndex + 2 < 8)
            {

                if (yIndex - 1 >= 0)
                {

                    if (spaceTaken[xIndex + 2, yIndex - 1] == true)
                    {

                        if (isWhite)
                        {

                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex + 2) * 75) && pic.Location.Y == ((yIndex - 1) * 75) && pic.Name.Substring(5, 3) == "Kni")
                                {

                                    return true;

                                }

                            }

                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex + 2) * 75) && pic.Location.Y == ((yIndex - 1) * 75) && pic.Name.Substring(5, 3) == "Kni")
                                {

                                    return true;

                                }

                            }

                        }

                    }

                }

                if (yIndex + 1 < 8)
                {

                    if (spaceTaken[xIndex + 2, yIndex + 1] == true)
                    {

                        if (isWhite)
                        {

                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex + 2) * 75) && pic.Location.Y == ((yIndex + 1) * 75) && pic.Name.Substring(5, 3) == "Kni")
                                {

                                    return true;

                                }

                            }

                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex + 2) * 75) && pic.Location.Y == ((yIndex + 1) * 75) && pic.Name.Substring(5, 3) == "Kni")
                                {

                                    return true;

                                }

                            }

                        }

                    }

                }

            }

            if (yIndex - 2 >= 0)
            {

                if (xIndex - 1 >= 0)
                {

                    if (spaceTaken[xIndex - 1, yIndex - 2] == true)
                    {

                        if (isWhite)
                        {

                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex - 1) * 75) && pic.Location.Y == ((yIndex - 2) * 75) && pic.Name.Substring(5, 3) == "Kni")
                                {

                                    return true;

                                }

                            }

                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex - 1) * 75) && pic.Location.Y == ((yIndex - 2) * 75) && pic.Name.Substring(5, 3) == "Kni")
                                {

                                    return true;

                                }

                            }

                        }

                    }

                }

                if (xIndex + 1 < 8)
                {

                    if (spaceTaken[xIndex + 1, yIndex - 2] == true)
                    {

                        if (isWhite)
                        {

                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex + 1) * 75) && pic.Location.Y == ((yIndex - 2) * 75) && pic.Name.Substring(5, 3) == "Kni")
                                {

                                    return true;

                                }

                            }

                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex + 1) * 75) && pic.Location.Y == ((yIndex - 2) * 75) && pic.Name.Substring(5, 3) == "Kni")
                                {

                                    return true;

                                }

                            }

                        }

                    }

                }

            }

            if (yIndex + 2 < 8)
            {

                if (xIndex - 1 >= 0)
                {

                    if (spaceTaken[xIndex - 1, yIndex + 2] == true)
                    {

                        if (isWhite)
                        {

                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex - 1) * 75) && pic.Location.Y == ((yIndex + 2) * 75) && pic.Name.Substring(5, 3) == "Kni")
                                {

                                    return true;

                                }

                            }

                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex - 1) * 75) && pic.Location.Y == ((yIndex + 2) * 75) && pic.Name.Substring(5, 3) == "Kni")
                                {

                                    return true;

                                }

                            }

                        }

                    }

                }

                if (xIndex + 1 < 8)
                {

                    if (spaceTaken[xIndex + 1, yIndex + 2] == true)
                    {

                        if (isWhite)
                        {

                            foreach (PictureBox pic in blackPieces)
                            {

                                if (pic.Location.X == ((xIndex + 1) * 75) && pic.Location.Y == ((yIndex + 2) * 75) && pic.Name.Substring(5, 3) == "Kni")
                                {

                                    return true;

                                }

                            }

                        }
                        else
                        {

                            foreach (PictureBox pic in whitePieces)
                            {

                                if (pic.Location.X == ((xIndex + 1) * 75) && pic.Location.Y == ((yIndex + 2) * 75) && pic.Name.Substring(5, 3) == "Kni")
                                {

                                    return true;

                                }

                            }

                        }

                    }

                }

            }

            //Check for rooks and queens
            //Check right.
            for(int i = xIndex + 1; i < 8; i++)
            {

                if (spaceTaken[i, yIndex] == true)
                {

                    if (isWhite)
                    {
                        foreach (PictureBox pic in blackPieces)
                        {

                            if (pic.Location.X == ((i) * 75) && pic.Location.Y == ((yIndex) * 75))
                            {

                                if (pic.Name.Substring(5, 3) == "Roo" || pic.Name.Substring(5, 3) == "Que")
                                    return true;
                                else
                                    break;

                            }

                        }

                        break;

                    }
                    else
                    {

                        foreach (PictureBox pic in whitePieces)
                        {

                            if (pic.Location.X == ((i) * 75) && pic.Location.Y == ((yIndex) * 75))
                            {

                                if (pic.Name.Substring(5, 3) == "Roo" || pic.Name.Substring(5, 3) == "Que")
                                    return true;
                                else
                                    break;

                            }

                        }

                        break;

                    }

                }

            }

            //Check left
            for (int i = xIndex - 1; i >= 0; i--)
            {

                Console.WriteLine("i = " + i);

                if (spaceTaken[i, yIndex] == true)
                {

                    if (isWhite)
                    {
                        foreach (PictureBox pic in blackPieces)
                        {

                            if (pic.Location.X == ((i) * 75) && pic.Location.Y == ((yIndex) * 75))
                            {

                                if (pic.Name.Substring(5, 3) == "Roo" || pic.Name.Substring(5, 3) == "Que")
                                    return true;
                                else
                                    break;

                            }

                        }

                        break;

                    }
                    else
                    {

                        foreach (PictureBox pic in whitePieces)
                        {

                            if (pic.Location.X == ((i) * 75) && pic.Location.Y == ((yIndex) * 75))
                            {

                                if (pic.Name.Substring(5, 3) == "Roo" || pic.Name.Substring(5, 3) == "Que")
                                    return true;
                                else
                                    break;

                            }

                        }

                        break;

                    }

                }

            }

            //Check down.
            for (int i = yIndex + 1; i < 8; i++)
            {

                if (spaceTaken[xIndex, i] == true)
                {

                    if (isWhite)
                    {
                        foreach (PictureBox pic in blackPieces)
                        {

                            if (pic.Location.X == ((xIndex) * 75) && pic.Location.Y == ((i) * 75))
                            {

                                if (pic.Name.Substring(5, 3) == "Roo" || pic.Name.Substring(5, 3) == "Que")
                                    return true;
                                else
                                    break;

                            }

                        }

                        break;

                    }
                    else
                    {

                        foreach (PictureBox pic in whitePieces)
                        {

                            if (pic.Location.X == ((xIndex) * 75) && pic.Location.Y == ((i) * 75))
                            {

                                if (pic.Name.Substring(5, 3) == "Roo" || pic.Name.Substring(5, 3) == "Que")
                                    return true;
                                else
                                    break;

                            }

                        }

                        break;

                    }

                }

            }

            //Check up
            for (int i = yIndex - 1; i >= 0; i--)
            {

                if (spaceTaken[xIndex, i] == true)
                {

                    if (isWhite)
                    {
                        foreach (PictureBox pic in blackPieces)
                        {

                            if (pic.Location.X == ((xIndex) * 75) && pic.Location.Y == ((i) * 75))
                            {

                                if (pic.Name.Substring(5, 3) == "Roo" || pic.Name.Substring(5, 3) == "Que")
                                    return true;
                                else
                                    break;

                            }

                        }

                        break;

                    }
                    else
                    {

                        foreach (PictureBox pic in whitePieces)
                        {

                            if (pic.Location.X == ((xIndex) * 75) && pic.Location.Y == ((i) * 75))
                            {

                                if (pic.Name.Substring(5, 3) == "Roo" || pic.Name.Substring(5, 3) == "Que")
                                    return true;
                                else
                                    break;

                            }

                        }

                        break;

                    }

                }

            }

            return false;

        }

        /*********************************************************************************
         Method     : forfeit_Button_click
         Purpose    : If a player gives up or is out of moves, this button can be used to
                      end the game and reset the board.
         Parameters : 1. Sender  - Object calling the event.
                      2. e       - Arguments for the click
         Returns    : N/A
        *********************************************************************************/
        private void forfeit_Button_Click(object sender, EventArgs e)
        {

            //If the player accepts that they forfeit, count the pieces lost and display statistics before
            //resetting the board.
            if(MessageBox.Show("Are you sure you would like to forfeit?", "Forfeiting game", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                string message = "";
                int whitePiecesLost = 16 - (whitePiecesRemaining + 1);
                int blackPiecesLost = 16 - (blackPiecesRemaining + 1);


                if (moveCount % 2 == 0)
                    message += "Black has won the game!\n\n";
                else
                    message += "White has won the game!\n\n";

                message += ("Turns taken: " + moveCount + "\nWhite pieces lost: " + whitePiecesLost + "\nBlack pieces lost: " + blackPiecesLost);

                MessageBox.Show(message, "GAME OVER");

                moveCount = 0;

                //Reset the move and taken tables to their initial values.
                for (int i = 0; i < 8; i++)
                {

                    for (int k = 0; k < 8; k++)
                    {

                        possibleMove[i, k] = false;

                        if (i == 0 || i == 1 || i == 6 || i == 7)
                        {
                            spaceTaken[k, i] = true;
                        }
                        else
                        {
                            spaceTaken[k, i] = false;
                        }
                    }
                }

                //Reset board data.
                for (int i = 0; i < 8; i++)
                {
                    for (int k = 0; k < 8; k++)
                    {

                        boardData[k, i] = "";
                    }
                }

                //Reset the pieces to their initial positions.
                resetBoard();

            }

        }

        /*********************************************************************************
         Method     : stalemate
         Purpose    : To be used if there is no clear victor.
         Parameters : 1. Sender  - Object calling the event.
                      2. e       - Arguments for the click
         Returns    : N/A
        *********************************************************************************/
        private void stalemate_Button_Click(object sender, EventArgs e)
        {

            //If the stalemate is confirmed, show statistics and reset the board.
            if (MessageBox.Show("Are you sure there is a stalemate?", "Stalemate", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int whitePiecesLost = 16 - (whitePiecesRemaining + 1);
                int blackPiecesLost = 16 - (blackPiecesRemaining + 1);

                MessageBox.Show("The game ended in a stalemate!\n\nTurns taken: " + moveCount + "\nWhite pieces lost: " + whitePiecesLost + "\nBlack pieces lost: " + blackPiecesLost, "GAME OVER");

                moveCount = 0;

                for (int i = 0; i < 8; i++)
                {

                    for (int k = 0; k < 8; k++)
                    {

                        possibleMove[i, k] = false;

                        if (i == 0 || i == 1 || i == 6 || i == 7)
                        {
                            spaceTaken[k, i] = true;
                        }
                        else
                        {
                            spaceTaken[k, i] = false;
                        }
                    }
                }

                for (int i = 0; i < 8; i++)
                {
                    for (int k = 0; k < 8; k++)
                    {

                        boardData[k, i] = "";
                    }
                }

                resetBoard();

            }

        }

        /*********************************************************************************
         Method     : resetBoard
         Purpose    : Similar to createPieces(). Make all pieces visible again and place
                      them back where they should be. Also, update board data table.
         Parameters : None
         Returns    : N/A
        *********************************************************************************/
        private void resetBoard()
        {

            foreach (PictureBox pic in whitePieces)
                pic.Show();

            foreach (PictureBox pic in blackPieces)
                pic.Show();

            whitePawn0.Location = new Point(0, 75);
            pieceLoc = whitePawn0.Location;
            whitePawn0.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whitePawn0.Location = tempLoc;
            boardData[0, 1] = whitePawn0.Name;

            whitePawn1.Location = new Point(75, 75);
            pieceLoc = whitePawn1.Location;
            whitePawn1.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whitePawn1.Location = tempLoc;
            boardData[1, 1] = whitePawn1.Name;

            whitePawn2.Location = new Point(150, 75);
            pieceLoc = whitePawn2.Location;
            whitePawn2.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whitePawn2.Location = tempLoc;
            boardData[2, 1] = whitePawn2.Name;

            whitePawn3.Location = new Point(225, 75);
            pieceLoc = whitePawn3.Location;
            whitePawn3.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whitePawn3.Location = tempLoc;
            boardData[3, 1] = whitePawn3.Name;

            whitePawn4.Location = new Point(300, 75);
            pieceLoc = whitePawn4.Location;
            whitePawn4.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whitePawn4.Location = tempLoc;
            boardData[4, 1] = whitePawn4.Name;

            whitePawn5.Location = new Point(375, 75);
            pieceLoc = whitePawn5.Location;
            whitePawn5.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whitePawn5.Location = tempLoc;
            boardData[5, 1] = whitePawn5.Name;

            whitePawn6.Location = new Point(450, 75);
            pieceLoc = whitePawn6.Location;
            whitePawn6.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whitePawn6.Location = tempLoc;
            boardData[6, 1] = whitePawn6.Name;

            whitePawn7.Location = new Point(525, 75);
            pieceLoc = whitePawn7.Location;
            whitePawn7.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whitePawn7.Location = tempLoc;
            boardData[7, 1] = whitePawn0.Name;

            whiteRook0.Location = new Point(0, 0);
            pieceLoc = whiteRook0.Location;
            whiteRook0.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whiteRook0.Location = tempLoc;
            boardData[0, 0] = whiteRook0.Name;

            whiteRook1.Location = new Point(525, 0);
            pieceLoc = whiteRook1.Location;
            whiteRook1.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whiteRook1.Location = tempLoc;
            boardData[7, 0] = whiteRook1.Name;

            whiteKnight0.Location = new Point(75, 0);
            pieceLoc = whiteKnight0.Location;
            whiteKnight0.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whiteKnight0.Location = tempLoc;
            boardData[1, 0] = whiteKnight0.Name;

            whiteKnight1.Location = new Point(450, 0);
            pieceLoc = whiteKnight1.Location;
            whiteKnight1.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whiteKnight1.Location = tempLoc;
            boardData[6, 0] = whiteKnight1.Name;

            whiteBishop0.Location = new Point(150, 0);
            pieceLoc = whiteBishop0.Location;
            whiteBishop0.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whiteBishop0.Location = tempLoc;
            boardData[2, 0] = whiteBishop0.Name;

            whiteBishop1.Location = new Point(375, 0);
            pieceLoc = whiteBishop1.Location;
            whiteBishop1.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whiteBishop1.Location = tempLoc;
            boardData[5, 0] = whiteBishop1.Name;

            whiteKing.Location = new Point(225, 0);
            pieceLoc = whiteKing.Location;
            whiteKing.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whiteKing.Location = tempLoc;
            boardData[3, 0] = whiteKing.Name;

            whiteQueen.Location = new Point(300, 0);
            pieceLoc = whiteQueen.Location;
            whiteQueen.BackColor = determineLocation(pieceLoc, ref tempLoc);
            whiteQueen.Location = tempLoc;
            boardData[4, 0] = whiteQueen.Name;

            //Black Pieces
            blackPawn0.Location = new Point(0, 450);
            pieceLoc = blackPawn0.Location;
            blackPawn0.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackPawn0.Location = tempLoc;
            boardData[0, 6] = blackPawn0.Name;

            blackPawn1.Location = new Point(75, 450);
            pieceLoc = blackPawn1.Location;
            blackPawn1.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackPawn1.Location = tempLoc;
            boardData[1, 6] = blackPawn1.Name;

            blackPawn2.Location = new Point(150, 450);
            pieceLoc = blackPawn2.Location;
            blackPawn2.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackPawn2.Location = tempLoc;
            boardData[2, 6] = blackPawn2.Name;

            blackPawn3.Location = new Point(225, 450);
            pieceLoc = blackPawn3.Location;
            blackPawn3.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackPawn3.Location = tempLoc;
            boardData[3, 6] = blackPawn3.Name;

            blackPawn4.Location = new Point(300, 450);
            pieceLoc = blackPawn4.Location;
            blackPawn4.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackPawn4.Location = tempLoc;
            boardData[4, 6] = blackPawn4.Name;

            blackPawn5.Location = new Point(375, 450);
            pieceLoc = blackPawn5.Location;
            blackPawn5.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackPawn5.Location = tempLoc;
            boardData[5, 6] = blackPawn5.Name;

            blackPawn6.Location = new Point(450, 450);
            pieceLoc = blackPawn6.Location;
            blackPawn6.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackPawn6.Location = tempLoc;
            boardData[6, 6] = blackPawn6.Name;

            blackPawn7.Location = new Point(525, 450);
            pieceLoc = blackPawn7.Location;
            blackPawn7.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackPawn7.Location = tempLoc;
            boardData[7, 6] = blackPawn7.Name;

            blackRook0.Location = new Point(0, 525);
            pieceLoc = blackRook0.Location;
            blackRook0.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackRook0.Location = tempLoc;
            boardData[0, 7] = blackRook0.Name;

            blackRook1.Location = new Point(525, 525);
            pieceLoc = blackRook1.Location;
            blackRook1.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackRook1.Location = tempLoc;
            boardData[7, 7] = blackRook1.Name;

            blackKnight0.Location = new Point(75, 525);
            pieceLoc = blackKnight0.Location;
            blackKnight0.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackKnight0.Location = tempLoc;
            boardData[1, 7] = blackKnight0.Name;

            blackKnight1.Location = new Point(450, 525);
            pieceLoc = blackKnight1.Location;
            blackKnight1.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackKnight1.Location = tempLoc;
            boardData[6, 7] = blackKnight1.Name;

            blackBishop0.Location = new Point(150, 525);
            pieceLoc = blackBishop0.Location;
            blackBishop0.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackBishop0.Location = tempLoc;
            boardData[2, 7] = blackBishop0.Name;

            blackBishop1.Location = new Point(375, 525);
            pieceLoc = blackBishop1.Location;
            blackBishop1.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackBishop1.Location = tempLoc;
            boardData[5, 7] = blackBishop1.Name;

            blackKing.Location = new Point(300, 525);
            pieceLoc = blackKing.Location;
            blackKing.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackKing.Location = tempLoc;
            boardData[4, 7] = blackKing.Name;

            blackQueen.Location = new Point(225, 525);
            pieceLoc = blackQueen.Location;
            blackQueen.BackColor = determineLocation(pieceLoc, ref tempLoc);
            blackQueen.Location = tempLoc;
            boardData[3, 7] = blackQueen.Name;

        }
    }
}
