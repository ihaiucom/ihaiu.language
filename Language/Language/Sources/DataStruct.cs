using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using BeardedManStudios.Templating;

public class DataStruct
{
    public string name;
    public string cn;
    public List<DataField> fields = new List<DataField>();
    public bool isExtend = false;



    public void ExportServer()
    {

    }
}