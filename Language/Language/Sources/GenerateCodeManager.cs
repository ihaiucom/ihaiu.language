using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

public class GenerateCodeManager
{

    public void Run()
    {

        List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();
        
        string content = File.ReadAllText(Setting.Options.TextCodeTS);
        string pattern = "static[ \t]*(.*)[ \t]*=[ \t]*\"(.*)\"[ \t]*;";
        int id = 1;
        foreach (Match match in Regex.Matches(content, pattern))
        {
            string key = match.Groups[1].ToString().Trim();
            string value = match.Groups[2].ToString().Trim();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("id", id.ToString());
            dict.Add("key", key);
            dict.Add("zh_cn_value", value);
            dataList.Add(dict);
            id++;
        }

        List<DataField> fields = new List<DataField>();
        DataField field = new DataField();
        field.typeName = "int";
        field.cn = "ID";
        field.field = "id";
        fields.Add(field);

        field = new DataField();
        field.typeName = "string";
        field.cn = "Key";
        field.field = "key";
        fields.Add(field);

        field = new DataField();
        field.typeName = "string";
        field.cn = "值";
        field.field = "zh_cn_value";
        fields.Add(field);


        LangTableData langTableData = new LangTableData();
        langTableData.langPath = Setting.Options.TextCodeXlsx;
        langTableData.idKey = "key";
        langTableData.ReaderLangTable();
        langTableData.SetFields(fields);
        langTableData.SetDataList(dataList);
        langTableData.Sort("id");
        langTableData.ResetGenerateID();
        langTableData.Save();

    }
}