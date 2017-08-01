using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    /* 
     * Tic Tac Toe on the console
     * July, 2017
     * James A. Stewart
     * 
     * The player should enter the x and y coordinates for each move (1-3, 1-3).
     * The map is redrawn before each of the player's turns.  a 'c' represents a square the computer has played,
     * and a 'p' stands for the player's squares.
     * 
     * The player always goes first, so the player can have 5 of the nine moves
     * 
     * 
     * 
     */
    class Program
    {
        static char[,] map = new char[3, 3];
        static char player = 'p';
        static char computer = 'c';

        static void Main(string[] args)
        {
            
            //Initialize the playing area
            bool player_won = false;
            bool computer_won = false;
            char winner;
            
            int x, y;
            setupMap();
            int turn = 0;   //there can only be 9 turns as there are only 9 spaces.

            /*
            //debug use:  call funtions individually to see if they work
            get_player_move(out x, out y);
            Console.WriteLine("{0},{1}", x, y);
            move(x, y, player);
            */
            
            while (turn<9)
            {
                if (turn != -1)     //was comparing to 0 to make computer go first to facillitate testing draw case
                {                   //set to -1 to always return true so that the player goes first again.
                    bool valid = false;
                    //increment turn
                    turn++;
                    //draw the map
                    draw_map();
                    //get users move
                    int tries = 0;
                    do
                    {
                        tries++;
                        if (tries > 1)
                        {
                            Console.WriteLine("Please pick an empty square.");
                        }
                        get_player_move(out x, out y);
                        valid = move(x, y, player);             //this updates the map
                    } while (!valid);
                    //check to see if player won
                    if (checkForWin(out winner))
                    {
                        if (winner == player)
                        {
                            display_win_message();
                            break;
                        }
                        else
                        {
                            display_loss_message();
                            break;
                        }
                    }
                }
                //get computer's move
                //increment turn
                turn++;
                if (turn <= 9)  //skip if the map is full
                {
                    generate_computer_move(out x, out y);   //assumed to only return valid moves
                    move(x, y, computer);
                    //check to see if computer won
                    if (checkForWin(out winner))
                    {
                        if (winner == player)
                        {
                            display_win_message();
                            break;
                        }
                        else
                        {
                            display_loss_message();
                            break;
                        }
                    }
                }

            }
            if (turn >= 9)  //map is full.
            {
                Console.WriteLine("\n\n\n\nIt's a draw!");
            }
            draw_map();
            wait();
        }


        static void setupMap()
        {
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    map[i, j] = ' ';
                }
            }
        }
        static void get_player_move(out int x, out int y)
        {
            x = 1;y = 1;
            do
            {
                bool goodx = false;
                bool goody = false;
                Console.Write("Which square do you want (x,y)?");
                string ans = Console.ReadLine();
                if (ans.Contains(','))
                {
                    string ans1 = ans.Substring(0, ans.IndexOf(','));
                    // creates an error.  I don't know why:  string ans2 = ans.Substring(ans.IndexOf(',') + 1, ans.Length-1);
                    string ans2 = "";
                    for (int q = ans.IndexOf(',') + 1; q < ans.Length; q++)
                    {
                        ans2 += ans[q];
                    }
                    //Console.WriteLine("{0},{1}", ans1, ans2);
                    if (!int.TryParse(ans1, out x))
                    {
                        Console.WriteLine("Please try again using the digits 1, 2, or 3, separated by a comma.");
                        continue;
                    } else
                    {
                        if (x > 0 && x < 4)
                        {
                            goodx = true;
                        }
                    }
                    if (!int.TryParse(ans2, out y))
                    {
                        Console.WriteLine("Please try again using the digits 1, 2, or 3, separated by a comma.");
                        continue;
                    }
                    else
                    {
                        if (y > 0 && y < 4)
                        {
                            goody = true;
                        }
                    }
                    if (goodx && goody)
                    {
                        break;
                    }
                }
            } while (true);
        }
        static void generate_computer_move(out int x, out int y)
        {            
            Random r = new Random();
            double d;            
            do
            {
                d = r.NextDouble();
                x = (int)(d * 3);
                d = r.NextDouble();
                y = (int)(d * 3);
            } while (map[x, y] != ' ');  //repeatuntil you get a blank space
            //translate to 1-based coords
            x++;
            y++;
        }
        static bool  move(int x, int y, char player)    //This just returns false if the input is not empty.
        {                                               //It returns true and updates the map if the space is empty.
            bool valid_move = false;
            if (map[x - 1, y - 1] == ' ')
            {
                map[x - 1, y - 1] = player;  //-1 because indeces are zero based.  Our coords are 1 based.
                valid_move = true;
            }
            return valid_move;
        }
                    
        static bool checkForWin(out char winner)
        {
            winner = (char)0;  //t will return a value, so it should be set.  But nobody has won yet.
            bool found = false;
            //check verticals
            for (int i = 0; i < 3; i++)
            {
                if ((map[i,0]!=' ')&&(map[i, 0]== map[i, 1] && map[i, 0] == map[i, 2])) //must not include a row of empty spaces
                {
                    winner = map[i, 0];
                    found = true;
                    return found;
                }
            }
            //check horizontals
            for (int i = 0; i < 3; i++)
            {
                if ((map[0, i] != ' ') && (map[0,i] == map[1, i] && map[0, i] == map[2, i]))
                {
                    winner = map[0, i];
                    found = true;
                    return found;
                }
            }
            //check diagonals
            if (map[1, 1] != ' ')   //the conditional looked so bad I wanted to make the check for non-empty separate
            {
                if (((map[0, 0] == map[1, 1]) && (map[1, 1] == map[2, 2])) || ((map[0, 2] == map[1, 1]) && (map[1, 1] == map[2, 0])))
                {          //  top-left to bottom-right                    ||          bottom-left to top-right
                    winner = map[1, 1];
                    found = true;
                    return found;
                }
            }
            return found;
        }
        static void draw_map()
        {
            Console.Write("  -------\n");
            for (int i = 0; i < 3; i++)
            {
                Console.Write("{0} |", i+1);
                for(int j = 0; j < 3; j++)
                {
                    Console.Write(map[i, j] + "|");
                }
                Console.Write("\n  -------\n");
            }
            Console.WriteLine("   1 2 3");
        } 

        static void display_win_message()
        {
            Console.WriteLine("\n\n\n\nYou won!");
        }
        static void display_loss_message()
        {
            Console.WriteLine("\n\n\n\nYou lost!");
        }
        static void wait()
        {
            Console.ReadKey();
        }
    }
}
