using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 多语言--行数据
/// </summary>
public class LangRowData
{
    public LangTableData langTableData;

    // ID
    public string id;
    // 状态
    public LangStateType state;

    // 字段查找单元格数据
    public Dictionary<string, LangCellData> cellsByField = new Dictionary<string, LangCellData>();

    public LangRowData()
    {

    }

    public void AddCell(LangCellData cell)
    {
        cell.langRowData = this;
        cellsByField.Add(cell.field, cell);
    }

    public LangCellData SetCell(string field, string value, bool idKeyIsId = true)
    {
        if(cellsByField.ContainsKey(field))
        {
            if(field == "id" && !idKeyIsId)
            {
                cellsByField[field].value = value;
            }
            else
            {
                if (cellsByField[field].value != value)
                {
                    cellsByField[field].comment = value;
                    cellsByField[field].state = LangStateType.Modify;
                }
            }
        }
        else
        {
            LangCellData cell = new LangCellData();
            cell.field = field;
            cell.value = value;
            cell.state = LangStateType.Add;
            cell.langRowData = this;
            cellsByField.Add(cell.field, cell);
        }

        return cellsByField[field];
    }

    public LangCellData GetCell(string field)
    {
        if(cellsByField.ContainsKey(field))
        {
            return cellsByField[field];
        }
        return null;
    }





}