using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public class DataField
{
    public string   field;
    public string   cn;
    public string   typeName;
    public int      index;

    public DataField Clone()
    {
        DataField d = new DataField();
        d.field = field;
        d.cn = cn;
        d.typeName = typeName;
        d.index = index;
        return d;
    }

    // 状态
    public LangStateType state;
    public bool isOtherLang = false;
    public bool isLangField = false;

    public System.Drawing.Color textColor
    {
        get
        {
            if(isOtherLang)
            {
                return System.Drawing.Color.FromArgb(80, 0, 50);
            }
            else
            {
                if(field.StartsWith("zh_cn") || field.StartsWith("cn_"))
                {
                    return System.Drawing.Color.FromArgb(0, 50, 100);
                }
            }
            return System.Drawing.Color.Black;
        }
    }

    static Regex EnableRegex = new Regex("^[A-Za-z_]+[A-Za-z0-9_]*");

    public bool fieldNameIsEnable
    {
        get
        {
            return EnableRegex.IsMatch(field);
        }
    }

    public string GetTsTypeName()
    {
        string name = GetTsTypeName(typeName);
        if(name.EndsWith("[]"))
        {
            return GetTsTypeName(name.Replace("[]", "")) + "[]";
        }
        return name;
    }

    public string GetTsTypeName(string typeName)
    {
        string name = typeName.Trim().ToLower();
        switch (name)
        {
            case "string":
                return "string";
            case "int64":
            case "int":
            case "float":
            case "double":
                return "number";
            case "boolean":
            case "bool":
                return "boolean";
        }

        name = typeName.Trim().Replace(" ", "");
        return name;
    }
}
