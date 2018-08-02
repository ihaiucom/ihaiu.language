using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 多语言--单元格数据
/// </summary>
public class LangCellData
{
    public LangRowData langRowData;

    // ID
    public int id;
    // 字段
    public string field;
    // 值
    public string value;
    // 原理的注释
    public string beforeComment;
    // 注释
    public string comment;

    // 状态
    public LangStateType state;

    // 显示状态
    public LangStateType showState
    {
        get
        {
            switch(langRowData.langTableData.state)
            {
                case LangStateType.Add:
                    return LangStateType.None;
            }


            switch (langRowData.state)
            {
                case LangStateType.Add:
                    return LangStateType.Add;
                case LangStateType.Remove:
                    return LangStateType.Remove;
            }

            return state;
        }
    }

}