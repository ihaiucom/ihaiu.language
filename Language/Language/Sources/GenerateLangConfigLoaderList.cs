using BeardedManStudios.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


public class GenerateLangConfigLoaderList
{
    public XlsxManager xlsxManager;

    public GenerateLangConfigLoaderList(XlsxManager xlsxManager)
    {
        this.xlsxManager = xlsxManager;
    }

    public void Generate()
    {
        List<object[]> lines = new List<object[]>();
        foreach (var kvp in xlsxManager.tables)
        {
            TableReader table = kvp.Value;


            lines.Add(new object[] { table.tableName.FirstLower(), table.tableName });
        }

        TemplateSystem template = new TemplateSystem(File.ReadAllText(TemplatingFiles.Client.LangConfigLoaderList));
        template.AddVariable("tables", lines.ToArray());
        string content = template.Parse();
        string path = OutPaths.Client.LangConfigLoaderList;


        PathHelper.CheckPath(path);
        File.WriteAllText(path, content);
    }

}