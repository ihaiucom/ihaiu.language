using System;
using System.Collections.Generic;
using System.Text;


public class GenerateConfigManager
{
    public XlsxManager xlsxManager;

    public GenerateConfigManager(XlsxManager xlsxManager)
    {
        this.xlsxManager = xlsxManager;
    }

    public void Generate()
    {
        foreach (var kvp in xlsxManager.tables)
        {
            TableReader table = kvp.Value;

            if (!table.hasLangField)
                continue;


            string langPath = Setting.Options.langDir + "/editor/" + table.tableName + ".xlsx";
            LangTableData langTableData = new LangTableData();
            langTableData.langPath = langPath;
            langTableData.ReaderLangTable();
            langTableData.SetFields(table.dataStruct.fields);
            langTableData.SetDataList(table.dataList);
            langTableData.Sort("id");
            langTableData.Save();

        }

    }
}