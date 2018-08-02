using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CommandLine;


public class Options
{
    // 运行完，是否自动关闭cmd
    [Option("autoEnd", Required = false, Default = false)]
    public bool autoEnd { get; set; }

    // 命令
    [Option("cmd", Required = false, Default = "generateconfig")]
    public string cmd { get; set; }

    // 启动参数设置 配置路径
    [Option("optionSetting", Required = false, Default = "./LanguageSetting.json")]
    public string optionSetting { get; set; }

    // xlsx目录(可以用分号';'分割填写多个路径)
    [Option("xlsxDir", Required = false, Default = "E:/zengfeng/GamePF/gamepf_doc/Config/Common")]
    public string xlsxDir { get; set; }

    // xlsx 需要的语言
    [Option("langs", Required = false, Default = new string[] { "zh_cn", "en"})]
    public string[] langs { get; set; }

    // xlsx 语言字段前缀
    [Option("langFieldPrefixs", Required = false, Default = new string[] { "zh_cn_" , "cn_", "en_"})]
    public string[] langFieldPrefixs { get; set; }

    // 语言 xlsx目录
    [Option("langDir", Required = false, Default = "E:/zengfeng/GamePF/gamepf_doc/Config/Lang")]
    public string langDir { get; set; }

    // 语言 里的文本是否可以删除 原始表中没有的
    [Option("langEnableRemove", Required = false, Default = false)]
    public bool langEnableRemove { get; set; }

    // 语言 强制不能移除的表名
    [Option("langDisableRemoveTables", Required = false, Default = new string[] { })]
    public string[] langDisableRemoveTables { get; set; }


    // 配置输出目录
    [Option("outDir", Required = false, Default = "E:/zengfeng/GamePF/gamepf_doc/Config/LangOut")]
    public string outDir { get; set; }

    // xlsx配置文件 (可以配置扩展数据结构)
    [Option("exportSettingXlsx", Required = false, Default = "E:/zengfeng/GamePF/gamepf_doc/Config/ExportSetting.xlsx")]
    public string exportSettingXlsx { get; set; }

    // xlsx配置文件数据结构配置在哪个表单 (exportSettingXlsx文件中的StructSheet表单)
    [Option("settingStructSheet", Required = false, Default = "StructSheet")]
    public string settingStructSheet { get; set; }

    // xlsx配置文件忽略配置在哪个表单 (exportSettingXlsx文件中的IgnoreSheet表单)
    [Option("settingIgnoreSheet", Required = false, Default = "IgnoreSheet")]
    public string settingIgnoreSheet { get; set; }

    // 导出TypeScript文件的模板
    [Option("templateDir", Required = false, Default = "./Template")]
    public string templateDir { get; set; }

    // csv分隔符
    [Option("csvSeparator", Required = false, Default = "\t")]
    public string csvSeparator { get; set; }

    // xlsx文件内容如果有用到csv分隔符时，用该配置替换
    [Option("csvSeparatorReplace", Required = false, Default = "")]
    public string csvSeparatorReplace { get; set; }

    // xlsx文件内容如果有用到换行符时，用该配置替换
    [Option("csvLineSeparatorReplace", Required = false, Default = "|n|")]
    public string csvLineSeparatorReplace { get; set; }

    // 表头--Type 所在行
    [Option("xlsxHeadTypeLine", Required = false, Default = 1)]
    public int xlsxHeadTypeLine { get; set; }

    // 表头--中文 所在行
    [Option("xlsxHeadCnLine", Required = false, Default = 2)]
    public int xlsxHeadCnLine { get; set; }

    // 表头--字段 所在行
    [Option("xlsxHeadFieldLine", Required = false, Default = 3)]
    public int xlsxHeadFieldLine { get; set; }

    // 文本--代码
    [Option("TextCodeTS", Required = false, Default = "E:/zengfeng/GamePF/Gidea-PF-Client/GamePF/src/Config/keys/TEXT.ts")]
    public string TextCodeTS { get; set; }

    // 文本--代码
    [Option("TextCodeXlsx", Required = false, Default = "E:/zengfeng/GamePF/gamepf_doc/Config/Lang/editor/TextCode.xlsx")]
    public string TextCodeXlsx { get; set; }


    // 文本--Fgui
    [Option("TextCodeTS", Required = false, Default = "E:/zengfeng/GamePF/gamepf_art/FairyGUI/exports/lang/zh_cn.xml")]
    public string TextFguiXml { get; set; }

    // 文本--Fgui
    [Option("TextFguiXlsx", Required = false, Default = "E:/zengfeng/GamePF/gamepf_doc/Config/Lang/editor/TextFgui.xlsx")]
    public string TextFguiXlsx { get; set; }


    public void Save(string path = null)
    {
        if (string.IsNullOrEmpty(path))
            path = "./LanguageSetting.json";

        string json = JsonHelper.ToJsonType(this);
        File.WriteAllText(path, json);
    }

    public static Options Load(string path = null)
    {
        if (string.IsNullOrEmpty(path))
            path = "./LanguageSetting.json";

        string json = File.ReadAllText(path);
        Options options = JsonHelper.FromJson<Options>(json);
        return options;
    }

    public List<string> enableReadLangFieldPrefixs()
    {
        List<string> dict = new List<string>();
        foreach (string field in langFieldPrefixs)
        {
            dict.Add(field);
        }
        dict.Add("id");
        return dict;
    }


    public List<string> getLangFieldPrefixs()
    {
        return new List<string>(langFieldPrefixs);
    }


}