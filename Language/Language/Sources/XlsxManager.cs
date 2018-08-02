using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class XlsxManager
{
    public Dictionary<string, TableReader> tables = new Dictionary<string, TableReader>();
    public Dictionary<string, DataStruct> dataStructs = new Dictionary<string, DataStruct>();

    public TableReader ignaoreTable = new TableReader();
    public List<string> ignaoreTableList = new List<string>();
    public TableReader structTable = new TableReader();


    public DataStruct GetDataStruct(string name)
    {
        if(dataStructs.ContainsKey(name))
        {
            return dataStructs[name];
        }
        return null;
    }

    public void LoadIgnore(string path, string sheetName)
    {
        ignaoreTable.path = path;
        ignaoreTable.sheetName = sheetName;
        ignaoreTable.Load();

        foreach (Dictionary<string, string> rowData in ignaoreTable.dataList)
        {
            string tableName = rowData["tableName"].ToLower().Replace("\\", "/");
            if (!tableName.EndsWith(".xlsx"))
            {
                tableName += ".xlsx";
            }

            ignaoreTableList.Add(tableName);
        }
    }

    public bool IsIgnore(string path)
    {
        path = path.Replace("\\", "/").ToLower();
        bool result = false;
        foreach(string name in ignaoreTableList)
        {
            if(path.EndsWith(name))
            {
                result = true;
                break;
            }
        }
        return result;
    }


    public void LoadAllTable(string xlsxDirs, List<string> enableReadLangFieldPrefixs = null, List<string> langFieldPrefixs = null)
    {
        List<string> fileList = new List<string>();
        string[] dirs = xlsxDirs.Split(";");

        for(int i = 0; i < dirs.Length; i ++)
        {
            string dir = dirs[i].Trim();
            if(string.IsNullOrEmpty(dir))
            {
                continue;
            }

            string[] files = Directory.GetFiles(dir, "*.xlsx", SearchOption.AllDirectories);
            foreach(string file in files)
            {
                if (Path.GetFileName(file).StartsWith("~$"))
                    continue;

                if(!fileList.Contains(file))
                {
                    fileList.Add(file);
                }
            }
        }

        for(int i = 0; i < fileList.Count; i ++)
        {
            string path = fileList[i].Trim();
            if (IsIgnore(path))
                continue;

            TableReader tableReader = new TableReader();
            tableReader.path = path;
            tableReader.Load(enableReadLangFieldPrefixs, langFieldPrefixs);
            tables.Add(tableReader.dataStruct.name, tableReader);
            dataStructs.Add(tableReader.dataStruct.name, tableReader.dataStruct);
        }
    }





    public void ExportTsAll()
    {
        ExportTsClient();
    }

    public void ExportTsClient()
    {
        List<ExportClientTS> list = new List<ExportClientTS>();
        foreach(var kvp in dataStructs)
        {
            ExportClientTS item = new ExportClientTS();
            item.dataStruct = kvp.Value;
            item.Export();

            list.Add(item);
        }

        ExportClientTS.ExportConfigIncludes(list);
        ExportClientTS.ExportConfigManagerList(list);
    }


    public void ExportCsvs(string csvRoot)
    {
        foreach(var kvp in tables)
        {
            ExportCsv.Export(kvp.Value, csvRoot);
        }
    }


    public void ExportTSLangConfigLoaderList()
    {
        List<ExportClientTS> list = new List<ExportClientTS>();
        foreach (var kvp in dataStructs)
        {
            ExportClientTS item = new ExportClientTS();
            item.dataStruct = kvp.Value;
            item.Export();

            list.Add(item);
        }

        ExportClientTS.ExportConfigIncludes(list);
        ExportClientTS.ExportConfigManagerList(list);
    }

}