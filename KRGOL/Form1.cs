﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

		Random randomGenerator = new Random();

		// Paused Bool
		bool isPaused  = false;

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
			for(int i = 0;i < universe.GetLength(0); i++)
			{
				for(int j = 0; j < universe.GetLength(1);j++)
				{
					int count = CountNeighborsFinite(i, j);
					if(universe[i,j])
					{
						if(count == 2 || count == 3)
						{
							newGrid[i, j] = true;
						}
						if(count < 2 || count > 3)
						{
							newGrid[i, j] = false;
						}
					}
					else
					{
						if(count == 3)
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
					if(xCheck < 0)
					{
						continue;
					}
					// if yCheck is less than 0 then continue
					if (yCheck < 0)
					{
						continue;
					}
					// if xCheck is greater than or equal too xLen then continue
					if(xCheck >= xLen)
					{
						continue;
					}
					// if yCheck is greater than or equal too yLen then continue
					if(yCheck >= yLen)
					{
						continue;
					}

					if (universe[xCheck, yCheck] == true) count++;
				}
			}
			return count;
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
			timer.Enabled=false;
		}

		private void Next_Click(object sender, EventArgs e)
		{
			if(isPaused)
			{
				generations = generations + 1;
				toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
			}
			

		}
	}
}
