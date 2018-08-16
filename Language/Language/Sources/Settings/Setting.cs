using CommandLine;
using System;
using System.IO;


public class CmdType
{
    // 将 config 里带有 zh_cn表分离出来
    public const string generateconfig = "generateconfig";
    // 将 fgui 里的语言xml转换成 xlsx
    public const string generatefgui = "generatefgui";
    // 将 代码 里的文本转换成 xlsx
    public const string generatecode = "generatecode";
    // 将 Lang/editor 里的xlsx 分给各自语言包 Lang/zh-cn, Lang/en
    public const string langpackage = "langpackage";
    // 生成 TS 代码读取器 和对应的 csvn
    public const string langreader = "langreader";
    // 将Editor里的文本替换原文本
    public const string editor2source = "editor2source";
}


public class Setting
{
    public static Options Options { get; set; }
    public static string cmd = CmdType.generateconfig;

    public static void Init(string[] args)
    {
        bool useSetting = args.Length == 0;
        foreach (string op in args)
        {
            if (op.StartsWith("--optionSetting"))
            {
                useSetting = true;
                break;
            }
        }

        Parse(args);

        if (!File.Exists(Options.optionSetting))
        {
            Options.Save(Options.optionSetting);
        }

        cmd = Options.cmd;
        if (string.IsNullOrEmpty(cmd))
        {
            cmd = CmdType.generateconfig;
        }

        if (useSetting)
        {
            Options = Options.Load(Options.optionSetting);
        }
    }


    public static void Parse(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithNotParsed(error => throw new Exception($"命令行格式错误!"))
            .WithParsed(options =>
            {
                Options = options;
            });
    }


    public static string CsvRoot
    {
        get
        {
            return Options.outDir + "/lang";
        }
    }

    public static string JsonRoot
    {
        get
        {
            return Options.outDir + "/json";
        }
    }

}