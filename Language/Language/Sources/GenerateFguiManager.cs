using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

public class GenerateFguiManager
{
    public void Run()
    {

        List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();


        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(Setting.Options.TextFguiXml);


        XmlNode resources = xmlDocument.SelectSingleNode(@"resources");

        XmlNodeList xmlNodeList = resources.ChildNodes;

        int id = 1;
        string comment = string.Empty;
        foreach (XmlNode node in xmlNodeList)
        {
            switch(node.NodeType)
            {
                case XmlNodeType.Comment:
                    comment = node.InnerText.Trim();
                    Console.WriteLine(comment);
                    break;

                case XmlNodeType.Element:
                    string name = node.Attributes.GetNamedItem("name").InnerText;
                    string mz = node.Attributes.GetNamedItem("mz").InnerText;
                    string value = node.InnerText.Trim();
                    Console.WriteLine($"name={name}, mz={mz}, value={value}");

                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("id", id.ToString());
                    dict.Add("comment", comment);
                    dict.Add("name", name);
                    dict.Add("mz", mz);
                    dict.Add("zh_cn_value", value);
                    dataList.Add(dict);
                    id++;
                    break;
            }
        }


        List<DataField> fields = new List<DataField>();
        DataField field = new DataField();
        field.typeName = "int";
        field.cn = "ID";
        field.field = "id";
        fields.Add(field);

        field = new DataField();
        field.typeName = "string";
        field.cn = "备注";
        field.field = "comment";
        fields.Add(field);

        field = new DataField();
        field.typeName = "string";
        field.cn = "UI编号";
        field.field = "name";
        fields.Add(field);

        field = new DataField();
        field.typeName = "string";
        field.cn = "UI元件名称";
        field.field = "mz";
        fields.Add(field);

        field = new DataField();
        field.typeName = "string";
        field.cn = "值";
        field.field = "zh_cn_value";
        fields.Add(field);


        LangTableData langTableData = new LangTableData();
        langTableData.langPath = Setting.Options.TextFguiXlsx;
        langTableData.idKey = "name";
        langTableData.ReaderLangTable();
        langTableData.SetFields(fields);
        langTableData.SetDataList(dataList);
        langTableData.Sort("id");
        langTableData.ResetGenerateID();
        langTableData.Save();
    }
}