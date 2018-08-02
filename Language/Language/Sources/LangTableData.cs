using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

/// <summary>
/// 多语言--表数据
/// </summary>
public class LangTableData
{
    public string langPath;
    public string idKey = "id";

    public string sheetName = "Sheet1";
    List<DataField> fields = new List<DataField>();
    Dictionary<string, LangRowData> rowDatasById = new Dictionary<string, LangRowData>();


    // 表--多语言配置
    TableReader langTable;
    // 状态
    public LangStateType state;

    bool? _enableRemove = null;
    bool enableRemove
    {
        get
        {
            if(!_enableRemove.HasValue)
            {
                _enableRemove = Setting.Options.langEnableRemove;
                if(!Setting.Options.langEnableRemove)
                {
                    foreach (string item in Setting.Options.langDisableRemoveTables)
                    {
                        if (Path.GetFileNameWithoutExtension(item) == Path.GetFileNameWithoutExtension(langPath))
                        {
                            _enableRemove = false;
                            break;
                        }
                    }
                }
            }
            return _enableRemove.Value;
        }
    }


    // 读取语言表
    public void ReaderLangTable()
    {
        if (File.Exists(langPath))
        {
            state = LangStateType.None;
            TableReader tableReader = new TableReader();
            tableReader.path = langPath;
            tableReader.Load(null, null, true);
            langTable = tableReader;
            fields = tableReader.dataStruct.fields;
            sheetName = tableReader.sheetName;

            for (int i = 0; i < tableReader.dataList.Count; i ++)
            {
                Dictionary<string, string> data = tableReader.dataList[i];
                Dictionary<string, string> common = tableReader.commentList[i];
                LangRowData langRowData = new LangRowData();
                langRowData.langTableData = this;
                langRowData.id = data[idKey];
                foreach(DataField field in fields)
                {
                    LangCellData langCellData = new LangCellData();
                    langCellData.field = field.field;
                    langCellData.value = data[field.field];
                    langCellData.beforeComment = common[langCellData.field];

                    langRowData.AddCell(langCellData);
                }
                rowDatasById.Add(langRowData.id, langRowData);
            }

        }
        else
        {
            state = LangStateType.Add;
        }
    }

    // 设置新字段
    public void SetFields(List<DataField> fieldList)
    {
        foreach (DataField field in fieldList)
        {
            bool hasCon = false;
            foreach (DataField item in this.fields)
            {
                if (item.field == field.field)
                    hasCon = true;
            }

            if (!hasCon)
            {
                this.fields.Add(field);
                field.state = LangStateType.Add;
            }
        }


        Dictionary<string, DataField> fieldDict= new Dictionary<string, DataField>();
        Dictionary<string, DataField> langFields = new Dictionary<string, DataField>();

        foreach (DataField item in this.fields)
        {
            bool hasCon = false;
            foreach (DataField field in fieldList)
            {
                if (item.field == field.field)
                    hasCon = true;
            }


            if (!hasCon)
            {
                item.state = LangStateType.Remove;
            }

            if(item.field.StartsWith("zh_cn_"))
            {
                string fieldname = item.field.Replace("zh_cn_", "");
                langFields.Add(fieldname, item);
            }
            else if(item.field.StartsWith("cn_"))
            {
                string fieldname = item.field.Replace("cn_", "");
                langFields.Add(fieldname, item);
            }

            fieldDict.Add(item.field, item);
        }


        foreach(var kvp in langFields)
        {
            foreach(string langName in Setting.Options.langs)
            {
                if (langName == "zh_cn" || langName == "cn")
                    continue;

                string fieldName = langName + "_" + kvp.Key;
                if (!fieldDict.ContainsKey(fieldName))
                {
                    int index = GetLangFieldIndex(kvp.Key) + 1;
                    DataField item = new DataField();
                    item.typeName = kvp.Value.typeName;
                    item.cn = kvp.Value.cn;
                    item.field = fieldName;
                    item.state = LangStateType.Add;
                    item.isOtherLang = true;
                    this.fields.Insert(index, item);
                }
                else
                {
                    fieldDict[fieldName].isOtherLang = true;
                }
            }
        }

    }

    private int GetLangFieldIndex(string fieldName)
    {
        int index = -1;
        for (int i = 0; i < fields.Count; i++)
        {
            foreach (string langName in Setting.Options.langs)
            {
                string langFieldName = langName + "_" + fieldName;
                if(fields[i].field == langFieldName)
                {
                    index = Math.Max(i, index);
                }
            }
        }

        if(index == -1)
        {
            index = fields.Count - 1;
        }
        return index;
    }

    // 设置新数据列表
    public void SetDataList(List<Dictionary<string, string>> dataList)
    {
        Dictionary<string, bool> dataDict = new Dictionary<string, bool>();
        foreach(Dictionary<string, string> data in dataList)
        {
            string id = data[idKey];
            if(dataDict.ContainsKey(id))
            {
                Log.Warning(Path.GetFileName(langPath) + $" 存在多个id={id}的行");
            }
            else
            {
                dataDict.Add(id, true);
            }

            if (rowDatasById.ContainsKey(id))
            {
                LangRowData langRowData = rowDatasById[id];
                foreach (DataField field in fields)
                {
                    if(data.ContainsKey(field.field))
                    {
                        langRowData.SetCell(field.field, data[field.field], idKey == "id");
                    }
                }
            }
            else
            {
                LangRowData langRowData = new LangRowData();
                langRowData.langTableData = this;
                langRowData.id = id;
                langRowData.state = LangStateType.Add;

                foreach (DataField field in fields)
                {
                    LangCellData langCellData = new LangCellData();
                    langCellData.state = data.ContainsKey(field.field) ? LangStateType.Add : LangStateType.None;
                    langCellData.field = field.field;
                    langCellData.value = data.ContainsKey(field.field) ? data[field.field] : string.Empty;
                    langRowData.AddCell(langCellData);
                }
                rowDatasById.Add(langRowData.id, langRowData);
            }
        }

        List<LangRowData> removeList = new List<LangRowData>();
        foreach(var kvp in rowDatasById)
        {
            if(!dataDict.ContainsKey(kvp.Value.id))
            {
                kvp.Value.state = LangStateType.Remove;
                removeList.Add(kvp.Value);
            }
        }

        if(enableRemove)
        {
            foreach (LangRowData item  in removeList)
            {
                rowDatasById.Remove(item.id);
            }
        }
    }

    // 排序
    public void Sort(string field)
    {
        List<LangRowData> list = new List<LangRowData>(rowDatasById.Values);
        list.Sort((LangRowData a, LangRowData b) => 
        {
            LangCellData cellA =  a.GetCell(field);
            LangCellData cellB = b.GetCell(field);
            if(cellA != null && cellB != null && cellA.value != null && cellB.value != null)
            {
                if(field == "id")
                {
                    return cellA.value.ToInt32().CompareTo(cellB.value.ToInt32());
                }
                return cellA.value.CompareTo(cellB.value);
            }
            else
            {
                return 0;
            }
        });

        rowDatasById.Clear();
        foreach (LangRowData row in list)
        {
            if(!rowDatasById.ContainsKey(row.GetCell(idKey).value))
                rowDatasById.Add(row.GetCell(idKey).value, row);
        }
    }

    // 重新生成ID
    public void ResetGenerateID()
    {
        int id = 1;
        foreach (var kvp in rowDatasById)
        {
            kvp.Value.SetCell("id", id.ToString(), false);
            id++;
        }
    }

    // 保存
    public void Save()
    {

        if (File.Exists(langPath))
            File.Delete(langPath);

        PathHelper.CheckPath(langPath);


        var xlsx = new FileInfo(langPath);
        using (var package = new ExcelPackage(xlsx))
        {
            ExcelWorksheet ws = package.Workbook.Worksheets.Add(sheetName);

            Dictionary<int, int> columnWidths = new Dictionary<int, int>();

            int colCount = fields.Count;

            // 表头
            for (int i = 0; i < fields.Count; i ++)
            {
                DataField field = fields[i];
                ws.Cells[Setting.Options.xlsxHeadTypeLine, i + 1].Value = field.typeName ;
                ws.Cells[Setting.Options.xlsxHeadCnLine, i + 1].Value = field.cn;
                ws.Cells[Setting.Options.xlsxHeadFieldLine, i + 1].Value = field.field;
                if (state != LangStateType.Add)
                {
                    Color color = Color.FromArgb(70, 90, 110);

                    switch (field.state)
                    {
                        case LangStateType.Add:
                            color = Color.FromArgb(40, 80, 40);
                            break;
                        case LangStateType.Remove:
                            color = Color.FromArgb(70, 70, 70);
                            break;
                    }

                    ExcelRange excelCellHead1 = ws.Cells[Setting.Options.xlsxHeadTypeLine, i + 1];
                    ExcelRange excelCellHead2 = ws.Cells[Setting.Options.xlsxHeadCnLine, i + 1];
                    ExcelRange excelCellHead3 = ws.Cells[Setting.Options.xlsxHeadFieldLine, i + 1];

                    excelCellHead1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    excelCellHead1.Style.Fill.BackgroundColor.SetColor(color);
                    excelCellHead1.Style.Font.Color.SetColor(field.textColor);

                    excelCellHead2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    excelCellHead2.Style.Fill.BackgroundColor.SetColor(color);
                    excelCellHead2.Style.Font.Color.SetColor(field.textColor);

                    excelCellHead3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    excelCellHead3.Style.Fill.BackgroundColor.SetColor(color);
                    excelCellHead3.Style.Font.Color.SetColor(field.textColor);
                }

                columnWidths[i + 1] = Math.Clamp(field.field.Length, 10, 30);
            }


            using (var range = ws.Cells[1, 1, 3, colCount])
            {
                // 字体样式
                range.Style.Font.Bold = true;

                // 背景颜色
                //range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                //range.Style.Fill.BackgroundColor.SetColor(Color.Blue);
            }


            // 冻结前3行
            ws.View.FreezePanes(4, 1);

            int rowIndex = 4;
            foreach (var kvp in rowDatasById)
            {
                LangRowData langRowData = kvp.Value;

                for (int i = 0; i < fields.Count; i++)
                {
                    DataField field = fields[i];
                    LangCellData langCellData = langRowData.GetCell(field.field);
                    ExcelRange excelCell = ws.Cells[rowIndex, i + 1];

                    excelCell.Style.Font.Color.SetColor(field.textColor);

                    if (langCellData == null)
                    {
                        langCellData = new LangCellData();
                        langCellData.langRowData = langRowData;
                        langCellData.value = string.Empty;
                        langCellData.state = LangStateType.Add;
                    }

                    if (langCellData != null)
                    {
                        excelCell.Value = langCellData.value;
                        

                        columnWidths[i + 1] = Math.Max(langCellData.value.Length, columnWidths[i + 1]);
                        if (!string.IsNullOrEmpty(langCellData.comment))
                        {
                            ws.Comments.Add(excelCell, langCellData.comment, "zf");
                        }

                        switch(langCellData.showState)
                        {
                            case LangStateType.Add:
                                excelCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                excelCell.Style.Fill.BackgroundColor.SetColor(Color.Green);
                                break;
                            case LangStateType.Remove:
                                excelCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                excelCell.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                                break;
                            case LangStateType.Modify:
                                excelCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                excelCell.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                                break;
                        }
                    }
                }

                rowIndex++;
            }


            foreach (var kvp in columnWidths)
            {
                ws.Column(kvp.Key).Width = Math.Min(80, kvp.Value * 2);
            }


            package.Save();


        }
    }
}