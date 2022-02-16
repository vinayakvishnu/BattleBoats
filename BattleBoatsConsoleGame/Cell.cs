using System;

public class Cell
{
	private int col;
	private int row;		
	private char status;
	// '-' not guessed, no boat
	// 'B' not guessed, boat
	// 'H' guessed, boat
	// 'M' guessed, no boat

	public Cell(int col, int row, char status)
	{
		this.col = col;
		this.row = row;				
		this.status = status;
	}
	public int getCol()
	{
		return col;
	}

	public void setCol(int c)
	{
		this.row = c;
	}

	public int getRow()
    {
		return row;
    }

	public void setRow(int r)
	{
		this.row = r;
	}

	public char getStatus()
    {
		return status;
    }

	public void setStatus(char s)
    {
		this.status = s;
    }
}
