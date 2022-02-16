using System;

public class Boat
{
	private int size;
	private Boolean orientation;
	private Cell[] hitbox;

	public Boat(int size, Boolean orientation, Cell[] box)
	{
		this.size = size;
		this.orientation = orientation;
		this.hitbox = box;
	}

	public int getSize()
    {
		return size;
    }

	public void setSize(int size)
    {
		this.size = size;
    }

	public Boolean getOrientation()
    {
		return orientation;
    }

	public void setOrientation(Boolean orientation)
	{
		this.orientation = orientation;
	}

	public Cell[] getBox()
    {
		return hitbox;
    }

	public void setBox(Cell[] box)
	{
		this.hitbox = box;
	}

	public Boolean isSunk()
    {
		for (int i = 0; i < hitbox.Length; i++)
        {
			if (hitbox[i].getStatus() != 'H')
            {
				return false;
            }
        }
		return true;
    }
}
