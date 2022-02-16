using System;

public class Board
{
    private Cell[,] gameboard;
    private Boat[] boats;
    private int turns = 0, shots = 0, missiles = 1, drones = 1, remainingships;

    public int getRowLength()
    {
        return gameboard.GetLength(0);
    }

    public int getColumnLength()
    {
        return gameboard.GetLength(1);
    }

    public int totalNumBoats()
    {
        return boats.Length;
    }

    public int getTurns()
    {
        return turns;
    }

    public void addTurn()
    {
        this.turns++;
    }

    public int getShots()
    {
        return shots;
    }
    public void addShots()
    {
        this.shots++;
    }

    public int getMissiles()
    {
        return missiles;
    }

    public void useMissile()
    {
        this.missiles--;
    }

    public void setMissiles(int m)
    {
        this.missiles = m;
    }

    public int getDrones()
    {
        return drones;
    }

    public void useDrone()
    {
        this.drones--;
    }
    public void setDrones(int d)
    {
        this.drones = d;
    }

    public int getRemainingShips()
    {
        return remainingships;
    }

    public Board(int cols, int rows)
    {
        this.gameboard = new Cell[cols, rows];
        for (int col = 0; col < cols; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                this.gameboard[col, row] = new Cell(col, row, '-');
            }
        }

        if (rows == 3 || cols == 3)
        {
            this.remainingships = 1;
            boats = new Boat[1];
        }
        else if ((3 < rows && rows <= 4) || (3 < cols && cols <= 4))
        {
            this.remainingships = 2;
            boats = new Boat[2];
        }
        else if ((4 < rows && rows <= 6) || (4 < cols && cols <= 6))
        {
            this.remainingships = 3;
            boats = new Boat[3];
        }
        else if ((6 < rows && rows <= 8) || (6 < cols && cols <= 8))
        {
            this.remainingships = 4;
            boats = new Boat[4];
        }
        else if ((8 < rows && rows <= 10) || (8 < cols && cols <= 10))
        {
            this.remainingships = 5;
            boats = new Boat[5];
        }
    }

    public void placeBoats()
    {
        Random rand = new Random();
        int col, row, direction, length, box;
        Cell[] hitbox;
        Boolean orientation;

        int toPlace = remainingships;

        while (toPlace != 0)
        {
            col = rand.Next(0, getRowLength());
            row = rand.Next(0, getColumnLength());            

            direction = rand.Next(2);

            if (toPlace == 5) { length = 5; }
            else if (toPlace == 4) { length = 4; }
            else if ((toPlace == 3) || (toPlace == 2)) { length = 3; }
            else { length = 2; }


            if (isOpen(col, row, direction, length)) {
                hitbox = new Cell[length];
                box = length;
                if (direction == 0)
                {
                    orientation = false;
                    for (int i = 0; i < box; i++) 
                    {
                        gameboard[col, row].setStatus('B');
                        hitbox[i] = gameboard[col++, row];
                    }
                    this.boats[^toPlace] = new Boat(length, orientation, hitbox);
                }
                else if (direction == 1)
                {
                    orientation = true;
                    for (int i = 0; i < box; i++)
                    {
                        gameboard[col, row].setStatus('B');
                        hitbox[i] = gameboard[col, row++];
                    }
                    this.boats[^toPlace] = new Boat(length, orientation, hitbox);
                }
                toPlace--;
            }
        }
    }

    public Boolean isOpen(int col, int row, int direction, int length)
    {
        if (direction == 0)
        {
            while (length-- != 0)
            {
                if (((col >= getRowLength() || (row >= getColumnLength())) || (gameboard[col++,row].getStatus() != '-'))) { return false; }
            }
        }
        else if (direction == 1)
        {
            while (length-- != 0)
            {
                if (((col >= getRowLength() || (row >= getColumnLength())) || (gameboard[col, row++].getStatus() != '-'))) { return false; }
            }
        }
        return true;
    }

    public void fire(int x, int y)
    {
        addShots();
        if (x >= getRowLength() || (y >= getColumnLength()) || (x < 0) || (y < 0))
        {
            Console.WriteLine("Out of bounds.\nTurn skipped as penalty");
            addTurn();
        }
        else if ((gameboard[x, y].getStatus() == 'H') || (gameboard[x, y].getStatus() == 'M'))
        {
            Console.WriteLine("Previously selected target.\nTurn skipped as penalty.");
            addTurn();
        }
        else if (gameboard[x, y].getStatus() == 'B')
        {
            Console.WriteLine("Hit!");
            gameboard[x, y].setStatus('H');
            Boat hitboat = identifyBoat(x, y);
            isSunk(hitboat);

        }
        else if (gameboard[x, y].getStatus() == '-')
        {
            Console.WriteLine("Miss");
            gameboard[x, y].setStatus('M');
        }
        else
        {
            Console.WriteLine("Fire Error");
        }
    }

    public Boat identifyBoat(int x, int y)
    {
        //write a method to return a boat object that contains a coordinate.
        foreach (Boat boat in this.boats)
        {
            foreach (Cell coord in boat.getBox())
            {
                if ((coord.getCol() == x) && (coord.getRow() == y))
                {
                    return boat;
                }
            }
        }
        Console.WriteLine("Error");
        return null;
    }

    public void isSunk(Boat hitboat)
    {
        Boolean flag = true;
        foreach (Cell coord in hitboat.getBox())
        {
            if (coord.getStatus() != 'H')
            {
                flag = false;
                break;
            }
        }
        if (flag == true)
        {
            Console.WriteLine("Sunk!");
            this.remainingships--;
        }
    }

    public void display()
    {
        string output = "\n  ";
        for (int collabel = 0; collabel < getRowLength(); collabel++)
        {
            output += " " + collabel + " ";
        }
        output += "\n";
        for (int row = 0; row < getColumnLength(); row++)
        {
            for (int col = 0; col < getRowLength(); col++)  
            {
                if (col == 0)
                {
                    if (gameboard[col, row].getStatus() == 'H')
                    {
                        output += row + " [" + 'X' + "]";
                    }
                    else if (gameboard[col, row].getStatus() == 'M')
                    {
                        output += row + " [" + 'O' + "]";
                    }
                    else
                    {
                        output += row + " [" + "~" + "]";
                    }
                }
                else
                {
                    if (gameboard[col, row].getStatus() == 'H')
                    {
                        output += "[" + 'X' + "]";
                    }
                    else if (gameboard[col, row].getStatus() == 'M')
                    {
                        output += "[" + 'O' + "]";
                    }
                    else
                    {
                        output += "[" + "~" + "]";
                    }
                }
                
            }
            output += "\n";
        }
        Console.WriteLine(output);
    }

    public void print()
    {
        string output = "\n  ";
        for (int collabel = 0; collabel < getRowLength(); collabel++)
        {
            output += " " + collabel + " ";
        }
        output += "\n";
        for (int row = 0; row < getColumnLength(); row++)  
        {            
            for (int col = 0; col < getRowLength(); col++)
            {                
                if (col == 0)
                {
                    output += row + " [" + gameboard[col, row].getStatus() + "]";
                }
                else
                {
                    output += "[" + gameboard[col, row].getStatus() + "]";
                }
                
            }
            output += "\n";
        }
        Console.WriteLine(output);
    }

    public void missile(int x, int y)
    {
        addShots();
        useMissile();
        if ((x >= getRowLength()) || (y >= getColumnLength()) || (x < 0) || (y < 0))
        {
            Console.WriteLine("Out of bounds.\nTurn skipped as penalty.");
            addTurn();
        }
        int hits = 0;
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                hits += missileHelper(i, j);
            }
        }
        Console.WriteLine($"The missile hit {hits} target(s)!");
    }

    public int missileHelper(int x, int y)
    {
        if (x >= getRowLength() || y >= getColumnLength() || (x < 0) || (y < 0))
        {
            return 0;
        }
        else if (gameboard[x, y].getStatus() == 'B')
        {
            gameboard[x, y].setStatus('H');
            Boat hitboat = identifyBoat(x, y);
            isSunk(hitboat);
            return 1;

        }
        else if (gameboard[x, y].getStatus() == '-')
        {
            gameboard[x, y].setStatus('M');
            return 0;
        }
        else
        {
            return 0;
        }
    }

    public void drone(int direction, int index)
    {
        useDrone();
        int spots = 0;
        string dir = "Unspecified";
        if (direction == 0)
        {
            dir = "row";
            for (int i = 0; i < getRowLength(); i++)
            {
                if (gameboard[i,index].getStatus() == 'B')
                {
                    spots++;
                }
            }
        }
        else if (direction ==1)
        {
            dir = "column";
            for (int i = 0; i < getColumnLength(); i++)
            {
                if (gameboard[index, i].getStatus() == 'B')
                {
                    spots++;
                }
            }
        }
        Console.WriteLine($"Drone has scanned {spots} target(s) in {dir} {index}.");
    }
}
