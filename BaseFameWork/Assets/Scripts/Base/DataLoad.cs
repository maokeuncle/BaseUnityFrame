using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Xml;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DataLoad
{
    private static  ByteArr allDataBytes;
    private static Dictionary<string,int> FilePosMap = new Dictionary<string, int>();
    
    public static IEnumerator Load()
    {
        string path = $"{Application.streamingAssetsPath}/data/data.bytes";
        UnityWebRequest webRequest = UnityWebRequest.Get(path);
        webRequest.SendWebRequest();
        
        while (!webRequest.isDone)
        {
            
            yield return null;
            if (webRequest.error != null)
            {
                webRequest = UnityWebRequest.Get(path);
                webRequest.SendWebRequest();
            }
        }

        allDataBytes = new ByteArr(webRequest.downloadHandler.data);

        if (allDataBytes.stream.Length <= 0)
        {
            Debug.LogError("data is empty");
            yield break;
        }
        
        while (allDataBytes.stream.Length - allDataBytes.stream.Position > 64)
        {
            int pos = (int)allDataBytes.stream.Position;
            int fileSize = allDataBytes.readInt();
        
            string fileName = allDataBytes.readString();
            allDataBytes.stream.Position =	pos + 64;
            FilePosMap[fileName] = pos + 60;
            allDataBytes.stream.Position += fileSize;
        }
        yield return null;
    }
    
    public static IEnumerator ReadMap<Tkey, Tvalue>(Type type)
    {
        string voName = typeof(Tvalue).Name;
        yield return new WaitForEndOfFrame();
        allDataBytes.stream.Position = FilePosMap[voName];
        type.GetField(voName.Replace("Vo", "Map")).SetValue(null, allDataBytes.readMap<Tkey, Tvalue>());
        Debug.Log($"loaded data : {voName}");
    }

    public static IEnumerator ReadMapList<Tkey, Tvalue>(Type type) where Tvalue : new()
    {
        string voName = typeof(Tvalue).Name;
        yield return new WaitForEndOfFrame();
        allDataBytes.stream.Position = FilePosMap[voName]; 
        List<Tvalue> arr = allDataBytes.readArray<Tvalue>();
        Dictionary<Tkey, IList<Tvalue>> GroupMap = new Dictionary<Tkey, IList<Tvalue>>();
        FieldInfo f = typeof(Tvalue).GetField("ID");
        foreach (Tvalue item in arr)
        {
            Tkey key = (Tkey)f.GetValue(item);
            if (GroupMap.ContainsKey(key) == false)
                GroupMap[key] = new List<Tvalue>();
            GroupMap[key].Add(item);
        }

        type.GetField(voName.Replace("Vo", "Map")).SetValue(null, GroupMap);
        
        Debug.Log($"loaded data : {voName}");
    }
    
}