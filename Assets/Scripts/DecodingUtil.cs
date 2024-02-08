using System;
using System.Collections.Generic;
using UnityEngine;

public static class DecodingUtil
{
    //Return the notation of given coordinates (SEEMS TO BE OK)
    public static string XYToNotation(int x, int y)
    {
        char forX = (char)('A' + (x));
        return $"{forX}{y + 1}";
    }
    //Return the chess notation in the A1 format
    public static Tuple<int, int> NotationToXY(string notation)
    {
        int x = (int)notation[0] - (int)'A';
        int y = Convert.ToInt32(notation[1].ToString()) - 1;
        return Tuple.Create(x, y);
    }
    //Split the input string into a list where every element contains two chars
    public static List<string> splitIntoPieces(string input)
    {
        int length = input.Length;
        int chunkSize = 2;
        int numOfChunks = (int)Math.Ceiling((double)length / chunkSize);
        List<string> result = new List<string>();
        for (int i = 0; i < numOfChunks; i++)
        {
            int startIndex = i * chunkSize;
            int endIndex = Math.Min(startIndex + chunkSize, length);
            result.Add(input.Substring(startIndex, endIndex - startIndex));
        }
        //Debugging
        //string r = string.Join(", ", result);
        //UnityEngine.Debug.LogError(r);
        //END
        return result;
    }
}
