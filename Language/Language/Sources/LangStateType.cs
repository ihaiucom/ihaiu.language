using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 多语言状态
/// </summary>
public enum LangStateType
{
    // 未处理
    None,

    // 新增
    Add,

    // 移除
    Remove,

    // 修改
    Modify
}