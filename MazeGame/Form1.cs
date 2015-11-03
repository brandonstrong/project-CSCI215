using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MazeGame
{
    public partial class Form1 : Form
    {
        // Set the global positions for movement
        private int y = 0;
        private int x = 0;

        // Set start x and y in case user wants to re-play
        private int startx = 0;
        private int starty = 0;

        // Set maze bounds
        private int top = -1;
        private int left = -1;
        private int right = 0;
        private int bottom = 0;
        
        // Initialize the picture paths
        private Image path;
        private Image brick;
        private Image manfront;
        private Image manback;
        private Image manright;
        private Image manleft;
        private Image flag;
        // Set array for storing picture boxes
        List<List<PictureBox>> piclist = new List<List<PictureBox>>();
        List<List<char>> map = new List<List<char>>();

        public Form1()
        {
            // Initialize the form
            InitializeComponent();

            // Set the images from their file locations
            path = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "images/path.png");
            brick = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "images/brick.png");
            manfront = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "images/front.png");
            manback = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "images/back.png");
            manright = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "images/right.png");
            manleft = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "images/left.png");
            flag = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "images/flag.png");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Get the map from a text file reader
            using (StreamReader read = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "maps/map.txt")){
                
                // Get the first line in the text file
                string line = read.ReadLine();

                // Set the right bound from the text file size
                right = line.Count();

                // Get the rest of the lines in the text file and add them all into the map
                while (line != null)
                {
                    List<char> addToMap = line.ToList();
                    map.Add(addToMap);

                    // Increment the bottom for every line we add
                    bottom++;

                    // Get new line
                    line = read.ReadLine();
                }
            }
            
            for (int i = 0; i < bottom; i++)
            {
                List<PictureBox> addToList = new List<PictureBox>();
                for (int j = 0; j < right; j++)
                {
                    // Create a new picture box to add to form
                    PictureBox p = new PictureBox();

                    // Edit picture box size, location, and image
                    p.Width = path.Width;
                    p.Height = path.Height;
                    p.Location = new Point(j * brick.Width, i * brick.Height);

                    // Set image depending on what the map is
                    if (map[i][j] == 'x')
                    {
                        p.Image = brick;
                    }
                    else if(map[i][j] == ' ')
                    {
                        p.Image = path;
                    }
                    else if (map[i][j] == 's')
                    {
                        p.Image = manfront;
                        y = i;
                        x = j;
                        starty = i;
                        startx = j;
                    }
                    else if (map[i][j] == 'f')
                    {
                        p.Image = flag;
                    }

                    // Add new picture box to form and array
                    Controls.Add(p);
                    addToList.Add(p);
                }
                piclist.Add(addToList);
            }
            piclist[0][0].Image = brick;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up && (y - 1) > top && map[y-1][x] != 'x')
            {
                // Move up
                piclist[y--][x].Image = path;
                piclist[y][x].Image = manback;
            }
            else if (e.KeyCode == Keys.Down && (y + 1) < bottom && map[y + 1][x] != 'x')
            {
                // Move down
                piclist[y++][x].Image = path;
                piclist[y][x].Image = manfront;
            }
            else if (e.KeyCode == Keys.Left && (x - 1) > left && map[y][x - 1] != 'x')
            {
                // Move left
                piclist[y][x--].Image = path;
                piclist[y][x].Image = manleft;
            }
            else if (e.KeyCode == Keys.Right && (x + 1) < right && map[y][x + 1] != 'x')
            {
                // Move right
                piclist[y][x++].Image = path;
                piclist[y][x].Image = manright;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

            // If player wins, congratulate them and let them play again or quit.
            if (map[y][x] == 'f')
            {
                DialogResult r = MessageBox.Show("Congratulations, You WON!!!\nWant to play again?", "Winner!", MessageBoxButtons.YesNo);
                if (r == DialogResult.Yes)
                {
                    // Reset values to original positions
                    piclist[y][x].Image = flag;
                    piclist[starty][startx].Image = manfront;
                    x = startx;
                    y = starty;
                }
                else
                {
                    this.Close();
                }
            }
        }        
    }
}
