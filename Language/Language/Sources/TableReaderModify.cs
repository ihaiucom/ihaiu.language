using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;

public class TableReaderModify
{
    // 另存为路径
    public string savePath;

    // xlsx文件
    public string path;

    // sheet名称
    public string sheetName;

    // 表面
    public string tableName;

    // 数据结构
    public DataStruct dataStruct = new DataStruct();

    public Dictionary<int, DataField> fieldDictByIndex = new Dictionary<int, DataField>();
    public Dictionary<string, DataField> fieldDict = new Dictionary<string, DataField>();

    // 数据列表
    public List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();
    // 注释列表
    public List<Dictionary<string, string>> commentList = new List<Dictionary<string, string>>();
    // ID to 行号
    public Dictionary<string, int> id2RowDict = new Dictionary<string, int>();
    // ID to 行数据
    public Dictionary<string, Dictionary<string, string>> id2RowDataDict = new Dictionary<string, Dictionary<string, string>>();



    // 是否有语言字段
    public bool hasLangField;


    public void Load(List<string> enableReadLangFieldPrefixs = null, List<string> langFieldPrefixs = null, bool readComment = false)
    {

        if (string.IsNullOrEmpty(tableName))
        {
            tableName = Path.GetFileNameWithoutExtension(path).FirstUpper();
        }
        Log.Info($"读取配置表 {tableName}");
        dataStruct.name = tableName;

        var xlsx = new FileInfo(path);
        using (var package = new ExcelPackage(xlsx))
        {

            ExcelWorksheet ws = null;
            if (package.Workbook.Worksheets.Count > 0)
            {
                IEnumerator enumerator = package.Workbook.Worksheets.GetEnumerator();
                while (enumerator.MoveNext() && ws == null)
                {
                    if(string.IsNullOrEmpty(sheetName))
                    {
                        ws = (ExcelWorksheet)enumerator.Current;
                    }
                    else
                    {
                        if (((ExcelWorksheet)enumerator.Current).Name == sheetName)
                        {
                            ws = (ExcelWorksheet)enumerator.Current;
                        }
                    }
                }
            }

            if(ws == null)
            {
                Log.Error($"没有找到sheet path:{path}, sheetName:{sheetName}");
                return;
            }

            if(ws.Cells.Rows < 3)
            {
                Log.Error($" path:{path}, sheetName:{sheetName}， rows:{ws.Cells.Rows}, 行数小于3行， 必须要有(type, cn, field)");
                return;
            }

            sheetName = ws.Name;



           
            int columnNum = 0;
            for(int i = 1; i < ws.Cells.Columns; i ++ )
            {

                if (ws.Cells[1, i].Value == null)
                    break;

                if (ws.GetValue(2, i) == null)
                {
                    Log.Error($" path:{path}, sheetName:{sheetName}， 是空单元格 2行{i}列  ");
                    continue;
                }

                if (ws.GetValue(3, i) == null)
                {
                    Log.Error($" path:{path}, sheetName:{sheetName}， 是空单元格 3行{i}列  ");
                    continue;
                }


                string type = ws.GetValue(Setting.Options.xlsxHeadTypeLine, i).ToString().Trim();
                string cn = ws.GetValue(Setting.Options.xlsxHeadCnLine, i).ToString().Trim();
                string en = ws.GetValue(Setting.Options.xlsxHeadFieldLine, i).ToString().Trim();

                if (string.IsNullOrEmpty(type))
                {
                    Log.Error($" path:{path}, sheetName:{sheetName}， 是空单元格 type行{i}列 {type} {cn} {en} ");
                    continue;
                }

                if (string.IsNullOrEmpty(en))
                {
                    Log.Error($" path:{path}, sheetName:{sheetName}， 是空单元格 field{i}列 {type} {cn} {en} ");
                    continue;
                }

                // 检测可以读取的字段
                if(enableReadLangFieldPrefixs != null && enableReadLangFieldPrefixs.Count > 0)
                {
                    bool enable = false;
                    foreach(string fieldPrefix in enableReadLangFieldPrefixs)
                    {
                        if(en.StartsWith(fieldPrefix))
                        {
                            enable = true;
                        }
                    }

                    if(!enable)
                    {
                        continue;
                    }
                }



                DataField field = new DataField();
                field.typeName = type;
                field.cn = cn;
                field.field = en;
                field.index = i;
                dataStruct.fields.Add(field);
                fieldDictByIndex.Add(i, field);
                fieldDict.Add(field.field, field);
                columnNum = i;
            }

            // 检测是否有语言字段
            if(langFieldPrefixs != null && langFieldPrefixs.Count > 0)
            {
                this.hasLangField = false;
                foreach (DataField field in dataStruct.fields)
                {
                    foreach (string fieldPrefix in langFieldPrefixs)
                    {
                        if (field.field.StartsWith(fieldPrefix))
                        {
                            this.hasLangField = true;
                        }
                    }
                }

                if(!this.hasLangField)
                {
                    return;
                }
            }



            for (int r = 4; r < ws.Cells.Rows; r ++)
            {

                if (ws.Cells[r, 1].Value == null)
                    break;

                Dictionary<string, string> rowData = new Dictionary<string, string>();
                Dictionary<string, string> rowCommentData = new Dictionary<string, string>();
                foreach (DataField field in dataStruct.fields)
                {
                    int c = field.index;

                    string value = string.Empty;
                    string comment = string.Empty;


                    if(readComment)
                    {
                        if (ws.Cells[r, c].Comment != null)
                        {
                            comment = ws.Cells[r, c].Comment.Text;
                        }
                    }


                    if (ws.Cells[r, c].Value != null)
                    {
                        value = ws.GetValue(r, c).ToString().Trim();
                    }

                    if (fieldDictByIndex.ContainsKey(c))
                    {
                        if(rowData.ContainsKey(fieldDictByIndex[c].field))
                        {
                            Log.Error($" path:{path}, sheetName:{sheetName}， 存在相同的 field={fieldDictByIndex[c].field} {c}列");
                        }
                        else
                        {
                            rowData.Add(fieldDictByIndex[c].field, value);
                        }
                    }


                    if (readComment)
                    {
                        rowCommentData.Add(fieldDictByIndex[c].field, comment);
                    }
                }

                if (rowData.Count > 0)
                {
                    dataList.Add(rowData);
                    commentList.Add(rowCommentData);
                    if(rowData.ContainsKey("id"))
                    {
                        id2RowDict.Add(rowData["id"], r);
                        id2RowDataDict.Add(rowData["id"], rowData);
                    }
                }
            }

            package.Dispose();
        }
    }
    

    public void CheckModify(TableReader langTable)
    {

        List<DataField> fields = new List<DataField>();

        foreach (DataField field in langTable.dataStruct.fields)
        {
            string fieldName = field.field;
            foreach (string langFieldPrefix in Setting.Options.langFieldPrefixs)
            {
                if (fieldName.StartsWith(langFieldPrefix))
                {
                    field.isLangField = true;
                    fields.Add(field);
                    break;
                }
            }
        }

        bool hasModify = false;

        var xlsx = new FileInfo(path);
        using (var package = new ExcelPackage(xlsx))
        {

            ExcelWorksheet ws = null;
            if (package.Workbook.Worksheets.Count > 0)
            {
                IEnumerator enumerator = package.Workbook.Worksheets.GetEnumerator();
                while (enumerator.MoveNext() && ws == null)
                {
                    if (string.IsNullOrEmpty(sheetName))
                    {
                        ws = (ExcelWorksheet)enumerator.Current;
                    }
                    else
                    {
                        if (((ExcelWorksheet)enumerator.Current).Name == sheetName)
                        {
                            ws = (ExcelWorksheet)enumerator.Current;
                        }
                    }
                }
            }

            if (ws == null)
            {
                Log.Error($"没有找到sheet path:{path}, sheetName:{sheetName}");
                return;
            }

            if (ws.Cells.Rows < 3)
            {
                Log.Error($" path:{path}, sheetName:{sheetName}， rows:{ws.Cells.Rows}, 行数小于3行， 必须要有(type, cn, field)");
                return;
            }

            foreach (Dictionary<string, string> langRowData in langTable.dataList)
            {
                if (!langRowData.ContainsKey("id"))
                    return;

                string id = langRowData["id"];

                if (!id2RowDataDict.ContainsKey(id))
                    continue;

                Dictionary<string, string> sourceRowData = id2RowDataDict[id];
                foreach (DataField field in fields)
                {
                    if (sourceRowData.ContainsKey(field.field))
                    {
                        if (langRowData[field.field] != sourceRowData[field.field])
                        {
                            ExcelRange excelCell = ws.Cells[id2RowDict[id], fieldDict[field.field].index];

                            excelCell.Value = langRowData[field.field];
                            hasModify = true;
                            ws.Comments.Add(excelCell, sourceRowData[field.field], "zf");

                            excelCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            excelCell.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                        }
                    }
                }
            }

            if(hasModify)
            {

                Process.Start("C:/Windows/System32/attrib.exe", " -R");
                Process p = Process.Start("C:/Windows/System32/attrib.exe", $" -R {path}");
                p.WaitForExit();
                p.Close();


                PathHelper.CheckPath(savePath);
                package.SaveAs(new FileInfo(savePath));

                CopyCommand.Copy(savePath, path, true);
            }

            package.Dispose();
        }

    }
}
