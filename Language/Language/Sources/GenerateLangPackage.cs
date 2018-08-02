using System;
using System.Collections.Generic;
using System.Text;


public class GenerateLangPackage
{
    public XlsxManager xlsxManager;

    public GenerateLangPackage(XlsxManager xlsxManager)
    {
        this.xlsxManager = xlsxManager;
    }

    public void Generate()
    {
        foreach (var kvp in xlsxManager.tables)
        {
            TableReader table = kvp.Value;

            Dictionary<string, DataField> fieldDict = new Dictionary<string, DataField>();

            foreach (DataField field in table.dataStruct.fields)
            {
                bool isLangField = false;
                string fieldName = field.field;
                foreach (string langFieldPrefix in Setting.Options.langFieldPrefixs)
                {
                    if (fieldName.StartsWith(langFieldPrefix))
                    {
                        fieldName = fieldName.Replace(langFieldPrefix, string.Empty);
                        isLangField = true;
                        break;
                    }
                }


                if (!fieldDict.ContainsKey(fieldName))
                {
                    DataField item = field.Clone();
                    item.field = fieldName;
                    item.isLangField = isLangField;
                    fieldDict.Add(fieldName, item);
                }
            }


            List<DataField> fieldList = new List<DataField>(fieldDict.Values);
            foreach (string lang in Setting.Options.langs)
            {
                string langFieldPrefix = lang.Replace("-", "_");
                List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();
                foreach (Dictionary<string, string> line in table.dataList)
                {
                    Dictionary<string, string> langLine = new Dictionary<string, string>();
                    foreach (DataField field in fieldList)
                    {
                        string value = string.Empty;
                        string key = field.field;

                        if (!line.ContainsKey(key))
                        {
                            key = langFieldPrefix + "_" + field.field;
                            if (!line.ContainsKey(key))
                            {
                                key = "cn_" + field.field;
                            }
                        }

                        if (line.ContainsKey(key))
                        {
                            value = line[key];
                        }

                        langLine.Add(field.field, value);
                    }
                    dataList.Add(langLine);
                }


                string langPath = Setting.Options.langDir + "/" + lang + "/" + table.tableName + ".xlsx";
                LangTableData langTableData = new LangTableData();
                langTableData.langPath = langPath;
                langTableData.SetFields(fieldList);
                langTableData.SetDataList(dataList);
                langTableData.Save();
            }






        }

    }

}