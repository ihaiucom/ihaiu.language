using System;
using System.Collections.Generic;
using System.Text;

public class OutPaths
{
    public class Client
    {
        public static string ConfigStructTeamplate
        {
            get
            {
                return Setting.Options.outDir + "/Client/Config/ConfigStructs/{0}.ts";
            }
        }

        public static string ConfigTemplate
        {
            get
            {
                return Setting.Options.outDir + "/Client/Config/ConfigExtends/{0}.ts";
            }
        }

        public static string ConfigReaderStructTemplate
        {
            get
            {
                return Setting.Options.outDir + "/Client/Config/ReaderStructs/{0}.ts";
            }
        }

        public static string ConfigReaderTemplate
        {
            get
            {
                return Setting.Options.outDir + "/Client/Config/ReaderExtends/{0}.ts";
            }
        }

        public static string ConfigManagerListTemplate
        {
            get
            {
                return Setting.Options.outDir + "/Client/Config/ConfigManagerList.ts";
            }
        }

        public static string ConfigIncludesTemplate
        {
            get
            {
                return Setting.Options.outDir + "/Client/Config/ConfigIncludes.ts";
            }
        }


        public static string LangConfigLoaderList
        {
            get
            {
                return Setting.Options.outDir + "/Client/LangConfigLoaderList.ts";
            }
        }
    }

    public class Server
    {
        public static string ConfigTemplate
        {
            get
            {
                return Setting.Options.outDir + "/Server/Config/{0}.ts";
            }
        }


        public static string DTTemplate
        {
            get
            {
                return Setting.Options.outDir + "/Server/Config/{0}.ts";
            }
        }
    }
}