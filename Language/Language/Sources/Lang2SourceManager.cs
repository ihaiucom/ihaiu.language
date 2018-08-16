using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


public class Lang2SourceManager
{
    public XlsxManager xlsxManager;

    public Lang2SourceManager(XlsxManager xlsxManager)
    {
        this.xlsxManager = xlsxManager;
    }



    public void Run()
    {
        if(Directory.Exists(Setting.Options.xlsxSavaAsDir))
        {
            Directory.Delete(Setting.Options.xlsxSavaAsDir, true);
        }

        foreach (var kvp in xlsxManager.tables)
        {
            TableReader table = kvp.Value;



            string sourcePath = Setting.Options.xlsxDir + "/" + table.tableName + ".xlsx";
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"【Warm】 不存在该文件 {sourcePath}");
                continue;
            }

            TableReaderModify tableReaderModify = new TableReaderModify();
            tableReaderModify.path = sourcePath;
            tableReaderModify.savePath = Setting.Options.xlsxSavaAsDir + "/" + table.tableName + ".xlsx";
            tableReaderModify.Load();
            tableReaderModify.CheckModify(table);

        }
    }

}
