using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ExportCsv
{
    public static void Export(TableReader table, string csvRoot)
    {
        string csvSeparator = Setting.Options.csvSeparator;


        StringWriter sw = new StringWriter();

        List<string> headTypes = new List<string>();
        List<string> headCns = new List<string>();
        List<string> headFields = new List<string>();
        foreach (var kvp in table.fieldDictByIndex)
        {
            headTypes.Add(kvp.Value.typeName);
            headCns.Add(ReplaceSpearator(kvp.Value.cn));
            headFields.Add(kvp.Value.field);
        }

        sw.WriteLine(string.Join(csvSeparator, headTypes));
        sw.WriteLine(string.Join(csvSeparator, headCns));
        sw.WriteLine(string.Join(csvSeparator, headFields));


        foreach (Dictionary<string, string> line in table.dataList)
        {
            List<string> strList = new List<string>();
            foreach (var kvp in table.fieldDictByIndex)
            {
                if (line.ContainsKey(kvp.Value.field))
                {
                    strList.Add(ReplaceSpearator(line[kvp.Value.field]));
                }
                else
                {
                    strList.Add("-");
                }
            }
            sw.WriteLine(string.Join(csvSeparator, strList));
        }




        string path = csvRoot + "/" + table.tableName + ".csv";
        PathHelper.CheckPath(path);
        File.WriteAllText(path, sw.ToString(), Encoding.UTF8);

    }

    public static string ReplaceSpearator(string txt)
    {
        txt = txt.Replace("\\r\\n", "\n");
        txt = txt.Replace("\r\n", "\n");
        txt = txt.Replace("\\n", "\n");
        txt = txt.Replace("\n", Setting.Options.csvLineSeparatorReplace);

        if (!string.IsNullOrEmpty(Setting.Options.csvSeparatorReplace))
        {
            txt = txt.Replace(Setting.Options.csvSeparator, Setting.Options.csvSeparatorReplace);
        }

        return txt;
    }
}