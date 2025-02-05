using System;

public class Fraction
{
    private int top;
    private int bottom;

    public Fraction()
    {
        top = 1;
        bottom = 1;
    }

    public Fraction(int number)
    {
        top = number;
        bottom = 1;

    }

    public Fraction(int top_Alt, int bottom_Alt)
    {
        top = top_Alt;
        bottom = bottom_Alt;
    }

    public string GetFractionString()
    {

        string text = $"{top}/{bottom}";
        return text;
    }

    public double GetDecimalValue()
    {
        return(double)top / (double)bottom;
    }
}