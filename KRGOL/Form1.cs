using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KRGOL
{
	public partial class Form1 : Form
	{
		// The universe array
		bool[,] universe = new bool[5, 5];

		// Paused Bool
		bool isPaused = false;

		// Drawing colors
		Color gridColor = Color.Black;
		Color cellColor = Color.Gray;

		// The Timer class
		Timer timer = new Timer();

		// Generation count
		int generations = 0;

		public Form1()
		{
			InitializeComponent();
		}

		// Calculate the next generation of cells
		private void NextGeneration()
		{
			bool[,] newGrid = new bool[5, 5];
			for (int i = 0; i < universe.GetLength(0); i++)
			{
				for (int j = 0; j < universe.GetLength(1); j++)
				{
					int count = CountNeighborsFinite(i, j);
					if (universe[i, j])
					{
						//Rules
						if (count == 2 || count == 3)
						{
							newGrid[i, j] = true;
						}
						if (count < 2 || count > 3)
						{
							newGrid[i, j] = false;
						}
					}
					else
					{
						if (count == 3)
						{
							newGrid[i, j] = true;
						}
					}
				}
			}
			universe = newGrid;
			// Increment generation count
			generations++;

			// Update status strip generations
			toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
		}

		// The event called by the timer every Interval milliseconds.
		private void Timer_Tick(object sender, EventArgs e)
		{
			NextGeneration();
			Refresh();

		}

		private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
		{
			// Calculate the width and height of each cell in pixels
			// CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
			int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
			// CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
			int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

			// A Pen for drawing the grid lines (color, width)
			Pen gridPen = new Pen(gridColor, 1);

			// A Brush for filling living cells interiors (color)
			Brush cellBrush = new SolidBrush(cellColor);

			// Iterate through the universe in the y, top to bottom
			for (int y = 0; y < universe.GetLength(1); y++)
			{
				// Iterate through the universe in the x, left to right
				for (int x = 0; x < universe.GetLength(0); x++)
				{
					// A rectangle to represent each cell in pixels
					Rectangle cellRect = Rectangle.Empty;
					cellRect.X = x * cellWidth;
					cellRect.Y = y * cellHeight;
					cellRect.Width = cellWidth;
					cellRect.Height = cellHeight;

					// Fill the cell with a brush if alive
					if (universe[x, y] == true)
					{
						e.Graphics.FillRectangle(cellBrush, cellRect);
					}

					// Outline the cell with a pen
					e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
				}
			}

			// Cleaning up pens and brushes
			gridPen.Dispose();
			cellBrush.Dispose();
		}

		private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
		{
			// If the left mouse button was clicked
			if (e.Button == MouseButtons.Left)
			{
				// Calculate the width and height of each cell in pixels
				int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
				int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

				// Calculate the cell that was clicked in
				// CELL X = MOUSE X / CELL WIDTH
				int x = e.X / cellWidth;
				// CELL Y = MOUSE Y / CELL HEIGHT
				int y = e.Y / cellHeight;

				// Toggle the cell's state
				universe[x, y] = !universe[x, y];

				// Tell Windows you need to repaint
				graphicsPanel1.Invalidate();
			}
		}

		private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{

		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			for (int y = 0; y < universe.GetLength(1); y++)
			{
				// Iterate through the universe in the x, left to right
				for (int x = 0; x < universe.GetLength(0); x++)
				{
					universe[x, y] = false;
				}
			}

			// Tell Windows you need to repaint
			graphicsPanel1.Invalidate();
			generations = 0;
			toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();

		}
		private int CountNeighborsFinite(int x, int y)
		{
			int count = 0;
			int xLen = universe.GetLength(0);
			int yLen = universe.GetLength(1);
			for (int yOffset = -1; yOffset <= 1; yOffset++)
			{
				for (int xOffset = -1; xOffset <= 1; xOffset++)
				{
					int xCheck = x + xOffset;
					int yCheck = y + yOffset;
					// if xOffset and yOffset are both equal to 0 then continue
					if (xOffset == 0 && yOffset == 0)
					{
						continue;
					}
					// if xCheck is less than 0 then continue
					if (xCheck < 0)
					{
						continue;
					}
					// if yCheck is less than 0 then continue
					if (yCheck < 0)
					{
						continue;
					}
					// if xCheck is greater than or equal too xLen then continue
					if (xCheck >= xLen)
					{
						continue;
					}
					// if yCheck is greater than or equal too yLen then continue
					if (yCheck >= yLen)
					{
						continue;
					}

					if (universe[xCheck, yCheck] == true) count++;
				}
			}
			return count;
		}

		private void Randomize(bool isSeed)
		{
			Random rand = new Random();
			for (int y = 0; y < universe.GetLength(1); y++)
			{
				for (int x = 0; x < universe.GetLength(0); x++)
				{
					if (isSeed)
					{

					}
					else
					{
						int num = rand.Next(0, 2);
						if (num == 0)
						{
							universe[x, y] = true;

						}
						else
						{
							universe[x, y] = false;

						}
					}
				}
			}

		}

		private void Start_Click(object sender, EventArgs e)
		{
			isPaused = false;
			timer.Interval = 1000 / 8; // milliseconds
			timer.Tick += Timer_Tick;
			timer.Enabled = true; // start timer running
		}

		private void Pause_Click(object sender, EventArgs e)
		{
			isPaused = true;
			timer.Enabled = false;
		}

		private void Next_Click(object sender, EventArgs e)
		{
			if (isPaused)
			{
				generations = generations + 1;
				toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
			}


		}

		//Time Values
		private void superFastToolStripMenuItem_Click(object sender, EventArgs e)
		{
			timer.Interval = 1000 / 64;
		}

		private void verySlowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			timer.Interval = 1000 / 2;
		}

		private void slowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			timer.Interval = 1000 / 4;
		}

		private void normalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			timer.Interval = 1000 / 8;
		}

		private void fastToolStripMenuItem_Click(object sender, EventArgs e)
		{
			timer.Interval = 1000 / 16;
		}

		private void veryFastToolStripMenuItem_Click(object sender, EventArgs e)
		{
			timer.Interval = 1000 / 32;
		}

		private void fromTimeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Randomize(false);
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Filter = "All Files|*.*|Cells|*.cells";
			dlg.FilterIndex = 2; dlg.DefaultExt = "cells";


			if (DialogResult.OK == dlg.ShowDialog())
			{
				StreamWriter writer = new StreamWriter(dlg.FileName);

				// Write any comments you want to include first.
				// Prefix all comment strings with an exclamation point.
				// Use WriteLine to write the strings to the file. 
				// It appends a CRLF for you.
				writer.WriteLine("!This is my comment.");

				// Iterate through the universe one row at a time.
				for (int y = 0; y < universe.GetLength(1); y++)
				{
					// Create a string to represent the current row.
					String currentRow = string.Empty;

					// Iterate through the current row one cell at a time.
					for (int x = 0; x < universe.GetLength(0); x++)
					{
						// If the universe[x,y] is alive then append 'O' (capital O)
						// to the row string.
						if (universe[x, y] == true)
						{
							currentRow += "O";
						}
						// Else if the universe[x,y] is dead then append '.' (period)
						// to the row string.
						else if (universe[x, y] == false)
						{
							currentRow += ".";
						}
					}

					// Once the current row has been read through and the 
					// string constructed then write it to the file using WriteLine.
					writer.WriteLine(currentRow);
				}

				// After all rows and columns have been written then close the file.
				writer.Close();
			}
		}

		private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "All Files|*.*|Cells|*.cells";
			dlg.FilterIndex = 2;
			int yPos = 0;

			if (DialogResult.OK == dlg.ShowDialog())
			{
				StreamReader reader = new StreamReader(dlg.FileName);

				// Create a couple variables to calculate the width and height
				// of the data in the file.
				int maxWidth = 0;
				int maxHeight = 0;

				// Iterate through the file once to get its size.
				while (!reader.EndOfStream)
				{
					// Read one row at a time.
					string row = reader.ReadLine();

					// If the row begins with '!' then it is a comment
					// and should be ignored.
					if (row.StartsWith("!") == true)
					{
						continue;
					}

					// If the row is not a comment then it is a row of cells.
					// Increment the maxHeight variable for each row read.
					// Get the length of the current row string
					// and adjust the maxWidth variable if necessary.
					else
					{
						maxHeight++;
						maxWidth = row.Length;
					}
				}

				// Resize the current universe and scratchPad
				// to the width and height of the file calculated above.
				universe = new bool[maxWidth, maxHeight];

				// Reset the file pointer back to the beginning of the file.
				reader.BaseStream.Seek(0, SeekOrigin.Begin);

				// Iterate through the file again, this time reading in the cells.
				while (!reader.EndOfStream)
				{
					// Read one row at a time.
					string row = reader.ReadLine();

					// If the row begins with '!' then
					// it is a comment and should be ignored.
					if (row.StartsWith("!") == true)
					{
						continue;
					}
					// If the row is not a comment then 
					// it is a row of cells and needs to be iterated through.
					else
					{
						for (int xPos = 0; xPos < row.Length; xPos++)
						{
							// If row[xPos] is a 'O' (capital O) then
							// set the corresponding cell in the universe to alive.
							if (row[xPos] == 'O')
							{
								universe[xPos, yPos] = true;
							}
							if (row[xPos] == '.')
							{
								universe[xPos, yPos] = false;
							}
						}
						yPos++;

						// If row[xPos] is a '.' (period) then
						// set the corresponding cell in the universe to dead.
					}
				}


				// Close the file.
				reader.Close();
				graphicsPanel1.Invalidate();
			}
		}
	}
}
