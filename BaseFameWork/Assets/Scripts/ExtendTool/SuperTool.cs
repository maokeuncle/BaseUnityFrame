using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static partial class SuperTool
{
    #region WriteFile
    public static void WriteFile(string path, string fileName, string datas)
    {
        // if (!Directory.Exists(path))
        //     Directory.CreateDirectory(path);
        FileStream fs = new FileStream($"{path}{fileName}.cs", FileMode.Create);
        //获得字节数组
        byte[] data = System.Text.Encoding.Default.GetBytes(datas); 
        //开始写入
        fs.Write(data, 0, data.Length);
        //清空缓冲区、关闭流
        fs.Flush();
        fs.Close();
    }
    #endregion
    
    #region CreatePrafab
    public static void CreatPrefab(string name, string prefabPath)
    {
        //创建对应的prefab
        GameObject empty = new GameObject {name = name};
        empty.AddComponent<Image>();
        empty.GetComponent<Image>().enabled = false;
        string path = prefabPath + "/" + name + "View.prefab";
        PrefabUtility.SaveAsPrefabAsset(empty, path);
    }
    #endregion

}
