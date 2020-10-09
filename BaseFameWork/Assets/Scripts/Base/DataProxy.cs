using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 数据访问类
/// </summary>
public class DataProxy : MonoBehaviour
{
    /*
     * ServerBaseVo：由打表工具中表：Server.xlsx生成
     */
    
    public static Dictionary<int, ServerBaseVo> ServerBaseMap;
 
    /*
     * IList<EquipDataBaseVo> 若相同id的数据有多个则使用list进行存储
     */
    
    public static Dictionary<int, IList<EquipDataBaseVo>> EquipDataBaseMap;
    
    
    public IEnumerator Init()
    {
        
        //加载所有数据（byte字节流）
        yield return DataLoad.Load();
        
        //分别读取每个表中的数据
        //id不重复
        yield return StartCoroutine(DataLoad.ReadMap<int, ServerBaseVo>(typeof(DataProxy)));
        //id重复
        yield return StartCoroutine(DataLoad.ReadMapList<int, EquipDataBaseVo>(typeof(DataProxy)));
        
    }

    private void Awake()
    {
        StartCoroutine(Init());

    }
}
