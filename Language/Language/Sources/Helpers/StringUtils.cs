﻿using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;

public static class StringUtils
{
    /** 填充长度 */
    public static string FillLeft(this string str, int length, char c = '0')
    {
        while(str.Length < length)
        {
            str = c + str;
        }
        return str;
    }

    public static string FillRight(this string str, int length, char c = '0')
    {
        while (str.Length < length)
        {
            str = str + c;
        }
        return str;
    }

    /** 首字母小写 */
    public static string FirstLower(this string str)
	{
		return char.ToLower(str[0]) + str.Substring(1); 
	}


	/** 首字母大写 */
	public static string FirstUpper(this string str)
	{
		return char.ToUpper(str[0]) + str.Substring(1); 
	}


	public static string NumberToString(int val, int toBase)
	{
		return Convert.ToString (val, toBase);
	}

	public static string IntToString(this int val, int toBase)
	{
		return Convert.ToString (val, toBase);
	}

    public static string to0x(this int val)
    {
        return "0x" + Convert.ToString (val, 16);
    }

    public static int[] ToIntArray(this float[] src)
    {
        int[] arr = new int[src.Length];
        for(int i = 0; i < src.Length; i ++)
        {
            arr[i] = (int) src[i];
        }
        return arr;
    }

    public static string ToStr(this object[] arg)
    {
        string str = "";
        string gap = "";
        for(int i = 0; i < arg.Length; i ++)
        {
            str += gap + arg[i];
        }

        return str;
    }

    public static string ToStr<T>(this Dictionary<int, T> dict)
    {
        string str = "{";
        string gap = "";
        foreach(KeyValuePair<int, T> item in dict)
        {
            str += gap + item.Value;
            gap = ", \n";
        }

        str += "}";
        return str;
    }


    public static string ToStr<T>(this Dictionary<int, T> dict, string gapConfig = ",\n", string left = "{", string right = "}")
    {
        string str = left;
        string gap = "";
        foreach (KeyValuePair<int, T> item in dict)
        {
            str += gap + item.Value;
            gap = gapConfig;
        }

        str += right;
        return str;
    }

    public static string ToStr<T>(this List<T> list, string gapConfig=",\n", string left = "{", string right ="}")
    {
		string str = left;
        string gap = "";
        foreach(T item in list)
        {
            str += gap + item;

			gap = gapConfig;
        }

		str +=right;
        return str;
    }

	public static string ToStr<T>(this T[] list, string gapConfig = ",\n")
    {
        string str = "{";
        string gap = "";
        foreach(T item in list)
        {
            str += gap + item;

			gap = gapConfig;
        }

        str += "}";
        return str;
    }

   


    public static string ToStr(this int[] list)
    {
        string str = "{";
        string gap = "";
        foreach(int num in list)
        {
            str += gap + num.ToString();
            gap = ", ";
        }

        str += "}";
        return str;
    }


    public static string ToStrCsv(this int[] list)
    {
        string str = "";
        string gap = "";
        foreach(int num in list)
        {
            str += gap + num.ToString();
            gap = ",";
        }

        str += "";
        return str;
    }


    public static List<int> ToInt32List(this string src, char separator = ',')
    {
		//策划有时候会配；
		src = src.Replace (';', separator);

        string[] strArr = src.Split(separator);
        List<int> list = new List<int>();
        foreach(string item in strArr)
        {
            string str = item.Trim();
            if(!string.IsNullOrEmpty(str))
            {
                try
                {
                    list.Add(str.ToInt32());
                }
                catch(FormatException e)
                {
					Log.Info(string.Format("src={0}, str={1}, e={2}", src, str, e));
                }
            }
        }
        return list;
    }

    public static int[] ToInt32Array(this string src)
    {
        string[] strArr = src.Split(',');
        List<int> list = new List<int>();
        foreach(string item in strArr)
        {
            string str = item.Trim();
            if(!string.IsNullOrEmpty(str))
            {
                try
                {
                    list.Add(str.ToInt32());
                }
                catch(FormatException e)
                {
                    Log.Info(string.Format("src={0}, str={1}, e={2}", src, str, e));
                }
            }
        }

        return list.ToArray();
    }



    public static float[] ToFloatArray(this string src)
    {
        string[] strArr = src.Split(',');
        List<float> list = new List<float>();
        foreach(string item in strArr)
        {
            string str = item.Trim();
            if(!string.IsNullOrEmpty(str))
            {
                list.Add(str.ToSingle());
            }
        }

        return list.ToArray();
    }

    public static object[] ToObjectArray(this int[] src)
    {
        object[] list = new object[src.Length];
        for(int i = 0; i < src.Length; i ++)
        {
            list[i] = src[i];
        }
        return list;
    }

    public static short ToInt16(this string src)
    {
        return string.IsNullOrEmpty(src) ? (short)0 : Convert.ToInt16(src);
    }

    public static int ToInt32(this string src)
    {
        try
        {
            return string.IsNullOrEmpty(src) ? 0 : Convert.ToInt32(src);
        }
        catch(Exception e)
        {
            Log.Error($"转换int32是非法字符 {src}");
            return 0;
        }
    }

    public static long ToInt64(this string src)
    {
        return string.IsNullOrEmpty(src) ? 0 : Convert.ToInt64(src);
    }

    public static ushort ToUInt16(this string src)
    {
        return string.IsNullOrEmpty(src) ? (ushort)0 : Convert.ToUInt16(src);
    }

    public static uint ToUInt32(this string src)
    {
        return string.IsNullOrEmpty(src) ? 0 : Convert.ToUInt32(src);
    }

    public static ulong ToUInt64(this string src)
    {
        return string.IsNullOrEmpty(src) ? 0 : Convert.ToUInt64(src);
    }

    public static bool ToBoolean(this string src)
    {
        return string.IsNullOrEmpty(src) ? false : src == "0" || src == "1" ? Convert.ToBoolean(Convert.ToInt32(src))  : Convert.ToBoolean(src);
    }

    public static float ToSingle(this string src)
    {
        return string.IsNullOrEmpty(src) ? 0 : Convert.ToSingle(src);
    }

    public static double ToDouble(this string src)
    {
        return string.IsNullOrEmpty(src) ? 0 : Convert.ToDouble(src);
    }

    public static string GetString(this string[] src, int idx)
    {
        return src.Length > idx ? src[idx] : null;
    }

    public static short GetInt16(this string[] src, int idx)
    {
        return src.Length > idx ? src[idx].ToInt16() : (short)0;
    }

    public static int GetInt32(this string[] src, int idx)
    {
        return src.Length > idx ? src[idx].ToInt32() : 0;
    }

    public static long GetInt64(this string[] src, int idx)
    {
        return src.Length > idx ? src[idx].ToInt64() : 0;
    }

    public static ushort GetUInt16(this string[] src, int idx)
    {
        return src.Length > idx ? src[idx].ToUInt16() : (ushort)0;
    }

    public static uint GetUInt32(this string[] src, int idx)
    {
        return src.Length > idx ? src[idx].ToUInt32() : 0;
    }
    public static ulong GetUInt64(this string[] src, int idx)
    {
        return src.Length > idx ? src[idx].ToUInt64() : 0;
    }


    public static bool GetBoolean(this string[] src, int idx)
    {
        return src.Length > idx ? src[idx].ToBoolean() : false;
    }

    public static float GetSingle(this string[] src, int idx)
    {
        return src.Length > idx ? src[idx].ToSingle() : 0;
    }





    public static int[] GetInt32Array(this string[] src, int idx) 
    {
        if( idx < src.Length && !string.IsNullOrEmpty(src[idx]) ){
            return src[idx].ToInt32Array();
        }
        else{
            return new int[0];
        }
    }


    public static List<int> GetInt32List(this string[] src, int idx, char separator = ',') 
    {
        if( idx < src.Length && !string.IsNullOrEmpty(src[idx]) ){
            return src[idx].ToInt32List(separator);
        }
        else{
            return new List<int>();
        }
    }


    public static float[] GetFloatArray(this string[] src, int idx) 
    {
        if( idx < src.Length && !string.IsNullOrEmpty(src[idx]) ){
            return src[idx].ToFloatArray();
        }
        else{
            return new float[0];
        }
    }




    public const int ARABIC_TO_CHINESE_1 = 1;
    public const int ARABIC_TO_CHINESE_2 = 2;

    public static string ArabicToChinese(int num, int type)
    {
        string[] Chinese_Number_1 = {"零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十"};
        string[] Chinese_Number_2 = {"零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖", "拾"};
        string[] Chinese_Unit_1 = {"", "十", "百"};
        string[] Chinese_Unit_2 = {"", "拾", "佰"};

        string[] arr1 = type == ARABIC_TO_CHINESE_1 ? Chinese_Number_1 : Chinese_Number_2;
        string[] arr2 = type == ARABIC_TO_CHINESE_1 ? Chinese_Unit_1 : Chinese_Unit_2;

        List<int> tempList = new List<int>();
        int n = num;
        while (n > 0)
        {
            tempList.Add((int)(n % 10));
            n = (int)(n / 10);
        }
        tempList.Reverse();

        if (tempList.Count > arr2.Length)
        {
            return "Error";
        }

        string str = "";

        for (int i = 0; i < tempList.Count; i++)
        {
            str += arr1[tempList[i]] + arr2[tempList.Count - i - 1];
        }

        str = str.Replace("零零", "");
        str = str.Replace("零" + arr2[1], "零");
        str = str.Replace(arr2[1] + "零", arr2[1]);

        tempList.Clear();

        return str;
    }

    /** 
     * 填充字符串 
     * @param source 源字符串
     * @param length 填充到长度
     * @param fill  填充字符串
     * @param direction 方向,可选值为start和end
     */
    public static string FillStr(object obj,  int length = 2, string fill  = "0", string direction = "start") 
    {
        if (obj == null) return null;
        string source = obj.ToString();;
        if (string.IsNullOrEmpty(source)) return source;
        if (string.IsNullOrEmpty(fill)) return source;
        while (source.Length < length) {
            if (direction == "start") {
                source = fill + source;
            } else {
                source += fill;
            }
        }
        return source;
    }

    public static string Number2Text(int n) {
        char[] c = n.ToString().ToCharArray();
        StringBuilder sb = new StringBuilder();
        foreach (var it in c) {
            sb.Append(NumberText[(int)it - (int)'0']);
        }
        return sb.ToString();
    }

    private static readonly string[] NumberText = new string[]{
        "零","一","二","三","四","五","六","七","八","九","十"
    };





    public static string BytesToString(byte[] bytes)
    {
        string s = "";
        string gap = ",";
        for(int i = 0; i < bytes.Length; i ++)
        {
            s += bytes[i];
            if (i < bytes.Length - 1)
            {
                s += gap;
            }
        }

        return s;
    }

    public static void PrintBytes(string key, byte[] bytes)
    {
        Log.Info(key + "=" + BytesToString(bytes));
    }

}
