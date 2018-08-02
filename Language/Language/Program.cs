using System;
using System.IO;
using System.Text;


class Program
{
    static void Main(string[] args)
    {
        //注册EncodeProvider
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


        Setting.Init(args);

        switch (Setting.cmd)
        {
            // 将 config 里带有 zh_cn表分离出来
            case CmdType.generateconfig:
                generateconfig();
                break;
            // 将 fgui 里的语言xml转换成 xlsx
            case CmdType.generatefgui:
                generatefgui();
                break;
            // 将 代码 里的文本转换成 xlsx
            case CmdType.generatecode:
                generatecode();
                break;
            // 将 Lang/editor 里的xlsx 分给各自语言包 Lang/zh-cn, Lang/en
            case CmdType.langpackage:
                langpackage();
                break;
            // 生成 TS 代码读取器 和对应的 csv
            case CmdType.langreader:
                langreader();
                break;
        }


        Console.WriteLine("完成!");

        if (!Setting.Options.autoEnd)
            Console.Read();
    }

    // 将 config 里带有 zh_cn表分离出来
    static void generateconfig()
    {
        XlsxManager xlsxManager = new XlsxManager();
        xlsxManager.LoadIgnore(Setting.Options.exportSettingXlsx, Setting.Options.settingIgnoreSheet);
        xlsxManager.LoadAllTable(Setting.Options.xlsxDir, Setting.Options.enableReadLangFieldPrefixs(), Setting.Options.getLangFieldPrefixs());

        GenerateConfigManager generateConfigManager = new GenerateConfigManager(xlsxManager);
        generateConfigManager.Generate();

    }

    // 将 代码 里的文本转换成 xlsx
    static void generatecode()
    {
        new GenerateCodeManager().Run();
    }


    // 将 fgui 里的语言xml转换成 xlsx
    static void generatefgui()
    {
        new GenerateFguiManager().Run();
    }


    // 将 Lang/editor 里的xlsx 分给各自语言包 Lang/zh-cn, Lang/en
    static void langpackage()
    {
        XlsxManager xlsxManager = new XlsxManager();
        xlsxManager.LoadAllTable(Setting.Options.langDir + "/editor");

        GenerateLangPackage generateLangPackage = new GenerateLangPackage(xlsxManager);
        generateLangPackage.Generate();
    }


    // 生成 TS 代码读取器 和对应的 csv
    static void langreader()
    {
        string[] dirs = Directory.GetDirectories(Setting.Options.langDir);
        XlsxManager xlsxManager = null;
        foreach (string dir in dirs)
        {
            string dirname = Path.GetFileName(dir);
            if (dirname == "editor")
                continue;

            string csvRoot = Setting.CsvRoot + "/" + dirname;

            xlsxManager = new XlsxManager();
            xlsxManager.LoadAllTable(dir);
            xlsxManager.ExportCsvs(csvRoot);
        }

        if(xlsxManager != null)
        {
            GenerateLangConfigLoaderList generate = new GenerateLangConfigLoaderList(xlsxManager);
            generate.Generate();
        }




    }
}
