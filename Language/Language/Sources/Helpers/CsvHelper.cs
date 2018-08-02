using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public static class CsvHelper
{
    public static string[] toStringArray(this string txt , string separator = @"[;,:,；,：]")
    {
        return Regex.Split(txt, separator);
    }

    public static int[] toIntArray(this string txt, string separator = @"[;,:,；,：]")
    {
        List<int> list = new List<int>();
        string[] arr = txt.toStringArray(separator);
        for(int i = 0; i<arr.Length; i ++)
        {
            list.Add(arr[i].ToInt32());
        }
        return list.ToArray();
    }



    public static float[] toFloatArray(this string txt, string separator = @"[;,:,；,：]")
    {
        List<float> list = new List<float>();
        string[] arr = txt.toStringArray(separator);
        for (int i = 0; i < arr.Length; i++)
        {
            list.Add(arr[i].ToSingle());
        }
        return list.ToArray();
    }




    public static bool[] toBooleanArray(this string txt, string separator = @"[;,:,；,：]")
    {
        List<bool> list = new List<bool>();
        string[] arr = txt.toStringArray(separator);
        for (int i = 0; i < arr.Length; i++)
        {
            list.Add(arr[i].ToBoolean());
        }
        return list.ToArray();
    }



    public static int csvGetInt(this string[]  csv, int i)
    {
        return csv[i].ToInt32();
    }


    public static float csvGetFloat(this string[] csv, int i)
    {
        return csv[i].ToSingle();
    }



    public static bool csvGetBoolean(this string[] csv, int i)
    {
        return csv[i].ToBoolean();
    }

    public static string csvGetString(this string[] csv, int i)
    {
        return csv[i];
    }
}