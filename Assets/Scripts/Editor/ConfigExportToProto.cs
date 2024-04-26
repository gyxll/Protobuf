using Google.Protobuf;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ConfigExportToProto
{
    /// <summary>
    /// 就是导出cs的命名空间，可以自定义
    /// </summary>
    public const string PackageName = "Com.Dxll.Proto";
    /// <summary>
    /// 生成的CS文件保存路径
    /// </summary>
    public const string scriptPath = "Scripts/Config/";
    /// <summary>
    /// 生成的proto文件保存目录
    /// </summary>
    public const string protoPath = "Config/";
    /// <summary>
    /// excel目录
    /// </summary>
    public const string excelPath = "Config/";

    /// <summary>
    /// 脚本数据
    /// </summary>
    public class ScriptConfigInfo
    {
        /// <summary>
        /// excel名
        /// </summary>
        public string sheetName;
        /// <summary>
        /// 类型名
        /// </summary>
        public string TypeName;
        /// <summary>
        /// 变量名
        /// </summary>
        public string VariableName;
    }
    /// <summary>
    /// 导出proto文件并生成相应的cs文件
    /// </summary>
    [MenuItem("Tools/Config2Proto", priority = 1)]
    public static void ExportProto()
    {
        string excelDir = Path.Combine(Environment.CurrentDirectory, excelPath);

        DirectoryInfo excelDirInfo = new(excelDir);
        System.IO.FileInfo[] excelFiles = excelDirInfo.GetFiles("*.xlsx", SearchOption.TopDirectoryOnly);

        //可导配置列表
        List<string> sheetNames = new();
        //可导配置相关信息（用于创建初始化代码）
        List<ScriptConfigInfo> scriptInfoArr = new();

        #region 遍历所有配置导出单个proto文件
        foreach (System.IO.FileInfo excelFile in excelFiles)
        {
            if (excelFile.Name.StartsWith("~"))
                continue;            

            FileStream stream = File.Open(excelFile.FullName, FileMode.Open, FileAccess.Read);
            try
            {
                ExcelPackage excelReader = new(stream);
                ExcelWorkbook workbook = excelReader.Workbook;
                ;
                var sheetCount = workbook.Worksheets.Count;
                var enumator = excelReader.Workbook.Worksheets.GetEnumerator();
                for (int i = 0; i < sheetCount; i++)
                {
                    enumator.MoveNext();
                    ExcelWorksheet workSheet = enumator.Current;
                    int columns = workSheet.Dimension.End.Column;
                    int rows = workSheet.Dimension.End.Row;
                    //单个配置相关数据
                    ScriptConfigInfo scriptData = new();
                    scriptInfoArr.Add(scriptData);
                    scriptData.sheetName = workSheet.Name;
                    sheetNames.Add(workSheet.Name);

                    if (rows < 3)
                    {
                        Debug.LogError($"{excelFile.Name}中{workSheet.Name}行数小于4");
                        return;
                    }
                    string SaveValue = "syntax = \"proto3\";\npackage " + PackageName + ";\n\n";
                    SaveValue += "message " + workSheet.Name + "Config{\n";

                    for (int j = 0; j <= columns; j++)
                    {
                        if (j == 0) { continue; }
                        if (string.IsNullOrEmpty(workSheet.GetValue<string>(1, j)))
                        {
                            continue;
                        }
                        string valueName = workSheet.GetValue<string>(3, j);
                        if (string.IsNullOrEmpty(valueName))
                        {
                            Debug.LogError($"{excelFile.Name}第中{workSheet.Name}第{j + 1}列变量名为空");
                            return;
                        }
                        valueName = valueName.TrimEnd(' ');
                        string explain = workSheet.GetValue<string>(1, j);
                        string typeName = workSheet.GetValue<string>(2, j);
                        //保存第一个字段的类型以及变量名
                        if (j == 1)
                        {
                            scriptData.VariableName = valueName.Substring(0,1).ToUpper() + valueName[1..];
                            scriptData.TypeName = typeName;
                        }
                        if (typeName == "int")
                            typeName = "int32";
                        else if (typeName == "long")
                            typeName = "int64";
                        SaveValue += $"\t//{explain}\n\t{typeName} {valueName} = {j};\n";
                    }

                    SaveValue += "}\n\n";

                    SaveValue += "message " + workSheet.Name + "ConfigData{\n";
                    SaveValue += "\trepeated " + workSheet.Name + "Config Config = 1;\n";
                    SaveValue += "}\n";

                    System.IO.FileStream fStream = System.IO.File.Create(Path.Combine(Environment.CurrentDirectory, protoPath + workSheet.Name + ".proto"));
                    char[] data = SaveValue.ToCharArray();
                    System.IO.BinaryWriter bw = new(fStream);
                    bw.Write(data);
                    bw.Close();
                    fStream.Close();
                }
            }
            catch (Exception)
            {
                stream.Close();
                return;
                throw;
            }
            
        }
        #endregion

        #region 创建一个总表proto文件
        string protoNewValue = "syntax = \"proto3\";\npackage " + PackageName + ";\n\n";
        foreach (string str in sheetNames)
        {
            protoNewValue += "import \"" + str + ".proto\";\n";
        }
        protoNewValue += "\r\nmessage ConfigDatabase {\n";
        for (int i = 0; i < sheetNames.Count; i++)
        {
            string paramName = sheetNames[i].Substring(0, 1).ToUpper() + sheetNames[i][1..];
            protoNewValue += "\t" + sheetNames[i] + "ConfigData " + paramName + "Data = " + (i + 1) + ";\n";
        }
        protoNewValue += "}\n";

        System.IO.FileStream fullStream = System.IO.File.Create(Path.Combine(Environment.CurrentDirectory ,protoPath + "ConfigDatabaseFile.proto"));
        char[] data2 = protoNewValue.ToCharArray();
        System.IO.BinaryWriter bw2 = new(fullStream);
        bw2.Write(data2);
        bw2.Close();
        fullStream.Close();
        #endregion

        AllProto2CS.ParseCS(scriptPath,protoPath);
        #region 自动补齐初始化代码
        CreateConfigScrpt(scriptInfoArr);
        #endregion
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 生成配置查询的代码
    /// </summary>
    /// <param name="scriptDataList"></param>
    public static void CreateConfigScrpt(List<ScriptConfigInfo> scriptDataList)
    {
        string temp = "";
        temp += "using " + PackageName + ";\n";
        temp += "using System.Collections.Generic;\nusing Google.Protobuf.Collections;\n";
        temp += "public class GetConfig{ \n";
        temp += "\tprivate ConfigDatabase AllConfig;\n";
        temp += "\tpublic void InitConfig(ConfigDatabase data){\n";
        temp += "\t\tAllConfig = data;\n";
        temp += "\t}\n";
        for (int i = 0; i < scriptDataList.Count; i++)
        {
            ScriptConfigInfo tempData = scriptDataList[i];
            string dicname = tempData.sheetName + "Dic";
            string dataListName = "AllConfig." + tempData.sheetName.Substring(0, 1).ToUpper() + tempData.sheetName[1..] + "Data.Config";
            string camelcaseStr = tempData.sheetName;
            string configName = tempData.sheetName + "Config";
            temp += "\n";
            temp += string.Format("\tprivate Dictionary<{0}, {1}Config> {2};\n", tempData.TypeName, tempData.sheetName, dicname);
            temp += string.Format("\tpublic {0}Config Get{1}({2} id)\n", tempData.sheetName, camelcaseStr, tempData.TypeName);
            temp += "{\n";
            temp += string.Format("\t\tif(null == {0})\n", dicname);
            temp += "\t\t{\n";
            temp += string.Format("\t\t\t{0} = new Dictionary<{1}, {2}Config>({3}.Count);\n", dicname, tempData.TypeName, tempData.sheetName, dataListName);
            temp += string.Format("\t\t\tforeach({0} oneConfig in {1})\n\t\t\t\t{2}[oneConfig.{3}] = oneConfig;\n", configName, dataListName, dicname, tempData.VariableName);
            temp += "\t\t}\n";
            temp += string.Format("\t\tif({0}.ContainsKey(id))\n\t\t\treturn {1}[id];\n\t\treturn null;", dicname, dicname);
            temp += "\n\t}\n";
            temp += string.Format("\tpublic RepeatedField<{0}> Get{1}List()", configName, camelcaseStr);
            temp += "{";
            temp += string.Format("return {0};", dataListName);
            temp += "}\n";
        }
        temp += "}";
        string filePath = Path.Combine(Environment.CurrentDirectory, "Assets", scriptPath + "GetConfig.cs");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        FileStream fs = new(filePath, FileMode.OpenOrCreate);
        //获得字节数组
        byte[] wiritedata = System.Text.Encoding.Default.GetBytes(temp);
        //开始写入
        fs.Write(wiritedata, 0, wiritedata.Length);
        //清空缓冲区、关闭流
        fs.Flush();
        fs.Close();
    }

    public static Type GetTypeByName(string str)
    {
        Type typ = Type.GetType(str + ",Assembly-CSharp-firstpass");
        if (typ == null)
        {
            typ = Type.GetType(str + ",Assembly-CSharp");
            if (typ == null)
            {
                Debug.LogError("not find!!! ");
                return null;
            }
        }
        return typ;
    }

    /// <summary>
    /// 根据excel路径和对应的类名来实例化数据
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    static Dictionary<string,System.Object> CreateData(string filePath)
    {
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
        ExcelPackage excelReader = new(stream);
        ExcelWorkbook result = excelReader.Workbook;
        var enumator = result.Worksheets.GetEnumerator();
        var sheetCount = result.Worksheets.Count;
        Dictionary<string, System.Object> paramDic = new();
        try
        {
            for (int i = 0; i < sheetCount; i++)
            {
                enumator.MoveNext();
                ExcelWorksheet workSheet = enumator.Current;
                paramDic.Add(workSheet.Name, InitSingleSheetData(workSheet));
            }
        }
        catch (Exception)
        {
            throw;
        }
        

        #region 校验数据
        stream.Close();
        #endregion
        return paramDic;
    }

    private static System.Object InitSingleSheetData(ExcelWorksheet workSheet)
    {
        //单个数据
        Type dataType = GetTypeByName(PackageName + "." + workSheet.Name + "Config");
        if (dataType == null)
        {
            Debug.LogError("type====" + workSheet.Name + "===is not find");
            return null;
        }

        //列表数据
        Type configType = GetTypeByName(PackageName + "." + workSheet.Name + "ConfigData");
        if (configType == null)
        {
            Debug.LogError("type=====" + workSheet.Name + "Config=======is not find");
            return null;
        }

        //获取列表变量
        System.Reflection.FieldInfo field = configType.GetField("config_",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.FlattenHierarchy |
            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.GetField);


        if (field == null)
        {
            Debug.LogError("field not find !!! ======" + configType.Name + "._config");
            return null;
        }

        #region 遍历整个excel表读取每一行数据（可以扩展列表，枚举，其他表数据，这里只列出基本数据类型）
        int columns = workSheet.Dimension.End.Column;
        int rows = workSheet.Dimension.End.Row;
        System.Reflection.PropertyInfo[] tmpFileds = dataType.GetProperties(System.Reflection.BindingFlags.SetField | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        System.Object configObj = Activator.CreateInstance(configType);

        IList m_DataList = field.GetValue(configObj) as IList;
        for (int i = 0; i < rows; i++)
        {
            if (i > 2)
            {
                System.Object target = Activator.CreateInstance(dataType);
                if (columns > 0)
                {
                    string dd = workSheet.GetValue<string>(i + 1, 1);
                    if (string.IsNullOrEmpty(dd))
                    {
                        break;
                    }
                }

                for (int j = 0, FiledsIndex = 0; j < columns; j++)
                {
                    string kk = workSheet.GetValue<string>(i + 1, j + 1);

                    if (FiledsIndex >= tmpFileds.Length)
                    {
                        continue;
                    }

                    TypeCode tpy = Type.GetTypeCode(tmpFileds[FiledsIndex].PropertyType);

                    string value = workSheet.GetValue<string>(i + 1, j + 1);
                    if (string.IsNullOrEmpty(value))
                    {
                        value = "";
                    }
                    value = value.TrimEnd(' ');
                    if (!tmpFileds[FiledsIndex].CanWrite)
                    {
                        continue;
                    }
                    switch (tpy)
                    {
                        case TypeCode.Int32:
                            if (kk != null)
                            {
                                if (string.IsNullOrEmpty(value))
                                {
                                    value = "0";
                                }
                                try
                                {
                                    tmpFileds[FiledsIndex].SetValue(target, Int32.Parse(value), null);
                                }
                                catch (System.Exception ex)
                                {
                                    Debug.LogError(ex.ToString());
                                    Debug.LogError(string.Format("Data error: {0} : {2}:[{1}] is not int", workSheet.Name, workSheet.GetValue<string>(i + 1, j + 1), tmpFileds[j].Name));

                                    string key = workSheet.GetValue<string>(i + 1, 1);
                                    int keyValue;
                                    if (int.TryParse(key, out keyValue))
                                    {
                                        Debug.LogError("上条错误对应的ID：" + keyValue);
                                    }
                                }
                            }
                            else
                            {
                                tmpFileds[FiledsIndex].SetValue(target, 0, null);
                            }
                            break;
                        case TypeCode.String:
                            if (kk != null)
                            {
                                tmpFileds[FiledsIndex].SetValue(target, workSheet.GetValue<string>(i + 1, j + 1), null);
                            }
                            else
                            {
                                tmpFileds[FiledsIndex].SetValue(target, "", null);
                            }
                            break;
                        case TypeCode.Single:
                            if (kk != null)
                            {
                                try
                                {
                                    if (string.IsNullOrEmpty(value))
                                    {
                                        value = "0";
                                    }
                                    tmpFileds[FiledsIndex].SetValue(target, float.Parse(value), null);
                                }
                                catch (System.Exception ex)
                                {
                                    Debug.LogError(ex.ToString());
                                    Debug.LogError(string.Format("Data error: {0} : {2}:[{1}] is not float", workSheet.Name, workSheet.GetValue<string>(i + 1, j + 1), tmpFileds[j].Name));
                                }
                            }
                            else
                            {
                                tmpFileds[FiledsIndex].SetValue(target, 0, null);
                            }
                            break;
                        case TypeCode.Boolean:
                            tmpFileds[FiledsIndex].SetValue(target, workSheet.GetValue<string>(i + 1, j + 1), null);
                            break;
                        default:
                            break;
                    }

                    FiledsIndex++;
                }
                m_DataList.Add(target);
            }
        }
        #endregion
        return configObj;
    }

    [MenuItem("Tools/ExportConfigBytes",priority =0)]
    public static void ExportExcelConfigAll()
    {
        string excelDirPath = Path.Combine(Environment.CurrentDirectory, excelPath);
        DirectoryInfo excelDirInfo = new(excelDirPath);
        System.IO.FileInfo[] excelFiles = excelDirInfo.GetFiles("*.xlsx", SearchOption.TopDirectoryOnly);

        Type AllConfigType = GetTypeByName(PackageName + ".ConfigDatabase");
        System.Object AllConfigData = Activator.CreateInstance(AllConfigType);
        PropertyInfo[] fields = AllConfigType.GetProperties();
        Dictionary<string, PropertyInfo> propertyInfoDic = new Dictionary<string, PropertyInfo>(fields.Length);
        for (int i = 0; i < fields.Length; i++)
        {
            propertyInfoDic.Add(fields[i].Name, fields[i]);
        }

        foreach (System.IO.FileInfo fileInfo in excelFiles)
        {
            var paramDic = CreateData(fileInfo.FullName);
            if (null == paramDic) { continue; }
            foreach (var item in paramDic)
            {
                string paramName = item.Key.Substring(0, 1).ToUpper() + item.Key[1..];
                propertyInfoDic[paramName].SetValue(AllConfigData, item.Value, null);
            }
        }

        byte[] dataConfig = MessageExtensions.ToByteArray(AllConfigData as IMessage);
        Debug.Log(AllConfigData);
        var pathConfig = string.Format(Path.Combine(Application.dataPath, "Res/Config/ConfigDatabase.bytes"));
        System.IO.FileStream FstreamConfig = System.IO.File.Create(pathConfig);
        System.IO.BinaryWriter bwConfig = new(FstreamConfig);
        bwConfig.Write(dataConfig);
        FstreamConfig.Close();
        bwConfig.Close();
        AssetDatabase.Refresh();
    }
}
