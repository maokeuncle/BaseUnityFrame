using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum COMPARE_ENUM
{
    COMPARE_TRUE = -1,
    COMPARE_EQUAL = 0,
    COMPARE_EQUAL_OR_BIG = 1,
    COMPARE_BIG = 2,
    COMPARE_EQUAL_OR_LESS = 3,
    COMPARE_LESS = 4
}

public class Utils
{
    public static float scale = 5f;
    
    public static Dictionary<String,COMPARE_ENUM> COMPARE_MAP=new Dictionary<String,COMPARE_ENUM>()
    {
        {"=",COMPARE_ENUM.COMPARE_EQUAL},
        {">=",COMPARE_ENUM.COMPARE_EQUAL_OR_BIG},
        {">",COMPARE_ENUM.COMPARE_BIG},
        {"<=",COMPARE_ENUM.COMPARE_EQUAL_OR_LESS},
        {"<",COMPARE_ENUM.COMPARE_LESS}
    };

    public static bool compare(int a, int b, int mode) {
        return  compare(  a,   b, (COMPARE_ENUM) mode);
    }

    public static bool compare(int a, int b, COMPARE_ENUM mode) {
        switch (mode) {
        case COMPARE_ENUM.COMPARE_TRUE:
            return true;
        case COMPARE_ENUM.COMPARE_EQUAL:
            return a == b;
        case COMPARE_ENUM.COMPARE_EQUAL_OR_BIG:
            return a >= b;
        case COMPARE_ENUM.COMPARE_EQUAL_OR_LESS:
            return a <= b;
        case COMPARE_ENUM.COMPARE_BIG:
            return a > b;
        case COMPARE_ENUM.COMPARE_LESS:
            return a < b;
        }
        return false;
    }

    public static void addUIItemComponent(MonoBehaviour cantainer, string[] itemNames, string path = "")
    {
        if (path != "")
            path += "/";
        foreach (string item in itemNames)
        {
            FieldInfo fi = cantainer.GetType().GetField(item);
            Transform t = cantainer.transform.Find(path + item);
            if (t == null)
                continue;

            Component component = t.GetComponent(fi.FieldType);
            if (component == null)
            {
                component = t.gameObject.AddComponent(fi.FieldType);
            }

            fi.SetValue(cantainer, component);
        }
    }

    public static void setUiItemWithSameName(MonoBehaviour cantainer, string[] itemNames, string path = "")
    {
        if (path != "")
            path += "/";
        foreach (string item in itemNames)
        {
            FieldInfo fi = cantainer.GetType().GetField(item);
            Transform t = cantainer.transform.Find(path + item);
            if (t == null)
                continue;
            if (fi.FieldType == typeof(GameObject))
            {
                fi.SetValue(cantainer, t.gameObject);
            }
            else if (fi.FieldType == typeof(Transform))
            {
                fi.SetValue(cantainer, t);
            }
            else
            {
                fi.SetValue(cantainer, t.GetComponent(fi.FieldType));
            }
        }
    }

    public static void setMonoValue(object[] monos, object[] datas)
    {
        if (monos == null || datas == null)
            return;
        
        object mono;
        object data;
        for (int i = 0; i < monos.Length;i++)
        {
            if (i > datas.Length - 1)
                break;

            mono = monos[i];
            data = datas[i];
            // if (mono is UISprite)
            // {
            //     (mono as UISprite).spriteName = (string) data;
            // }
            // else if (mono is UILabel)
            // {
            //     (mono as UILabel).text = (string) data;
            // }
            // else if (mono is GameObject)
            // {
            //     (mono as GameObject).SetActive((bool) data);
            // }
            // else if (mono is UISlider)
            // {
            //     (mono as UISlider).value = (float)data;
            // }
            // else if (mono is TweenLoopFill)
            // {
            //     (mono as TweenLoopFill).setEndValue((float) data);
            // }
        }
    }

    // public static GameObject getClonedChild(GameObject goParent)
    // {
    //     GameObject goCloned = null;
    //     for (int i = 0; i < goParent.transform.childCount; i++)
    //     {
    //         GameObject goChild = goParent.transform.GetChild(i).gameObject;
    //         if (!goChild.activeSelf)
    //         {
    //             goCloned = goChild;
    //             break;
    //         }
    //     }
    //     if (goCloned == null)
    //         goCloned = NGUITools.AddChild(goParent.gameObject, goParent.transform.GetChild(0).gameObject);
    //
    //     goCloned.SetActive(true);
    //
    //     return goCloned;
    // }
    
    public static GameObject AddChild (Transform parent, GameObject prefab)
    {
        var go = UnityEngine.Object.Instantiate(prefab, parent.transform);
        var t = go.transform;
        t.parent = parent;
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = Vector3.one;
        go.SetActive(true);
        return go;
    }
    
    public static void resetShaders(Transform item)
    {
        if (Application.isEditor == false&&Application.platform!=RuntimePlatform.OSXPlayer)
            return;

        if (item == null)
            return;
        Renderer[] rds = item.GetComponentsInChildren<Renderer> (true);
        foreach (Renderer r in rds)
        {
            foreach (Material m in r.sharedMaterials)
            {
                if (m!=null&&m.shader != null) 
                {
                    m.shader = Shader.Find (m.shader.name);
                }
            }
        }

        Projector[] projectors =item.GetComponentsInChildren<Projector> (true);
        foreach (var p in projectors)
        {
            if (p.material==null||p.material.shader == null)
                continue;
            p.material.shader = Shader.Find(p.material.shader.name);
        }
    }

    public static bool isSimpleType(Type type)
    {
        return type.IsPrimitive || type.Equals(typeof(string));
    }

    public static void setParent(GameObject go, GameObject goParent)
    {
        setParent(go, goParent.transform);
    }

    public static void setParent(GameObject go, Transform parent)
    {
        go.transform.SetParent(parent);
        go.transform.localPosition = Vector3.zero;
        go.transform.localEulerAngles = Vector3.zero;
        go.transform.localScale = Vector3.one;
    }

    public static Color hexToColor(string hex)
    {
        hex = hex.Replace ("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace ("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length > 6)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r,g,b,a);
    }

    public static Color rgbToColor(int r, int g, int b)
    {
        return new Color(r / 255, g / 255, b / 255);
    }
    

    // 利用二进制序列化和反序列实现
    public static T DeepCopyWithBinarySerialize<T>(T obj)
    {
        object retval;
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            // 序列化成流
            bf.Serialize(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);
            // 反序列化成对象
            retval = bf.Deserialize(ms);
            ms.Close();
        }

        return (T)retval;
    }
 
    public static void logPath(Transform tChild)
    {
        string path = tChild.name;
        Transform tParent = tChild.parent;
        while (true)
        {
            if (tParent == null)
                break;
            
            path = string.Format("{0}/{1}", tParent.name, path);
            tParent = tParent.parent;
        }
        
        Debug.LogError(path);
    }
    
    public delegate void LoadCallback<T>(T arg0);
    public static void loadAsset<T>(string strAsset, LoadCallback<T> callback)
    {
        Addressables.LoadAssetAsync<T>(strAsset).Completed +=
            (op) =>
            {
                if (op.Result == null)
                {
                    Debug.LogError(string.Format("asset : {0} is not exist", strAsset));
                    return;
                }
                
                if (callback != null)
                {
                    callback(op.Result);
                    callback = null;
                }
            };
    }
    
    public static void addTriggersListener(GameObject obj, EventTriggerType eventID, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = obj.AddComponent<EventTrigger>();
        }

        if (trigger.triggers.Count == 0)
        {
            trigger.triggers = new List<EventTrigger.Entry>();
        }

        UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(action);
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventID;
        entry.callback.AddListener(callback);
        trigger.triggers.Clear();
        trigger.triggers.Add(entry);
    }
    
    public static string getFullPath(string fileName)
    {
        string path = string.Format("file:///{0}/../res", Application.dataPath) ;
      

        path = string.Format("{0}/{1}", path, fileName);
        return path;
    }

    public static void setTransform(Transform t, int[] param)
    {
        if (param == null)
            return;

        t.localPosition = new Vector3(param[0], param[1], param[2]);
        t.localRotation = Quaternion.Euler(0f, param[3], 0f);

        float s = param[4];
        t.localScale = new Vector3(s, s, s);
    }

    public static void setChildrenLayer(Transform t, int layer)
    {
        if (t == null)
            return;
        t.gameObject.layer = layer;
        foreach (Transform item in t)
        {
            setChildrenLayer(item, layer);
        }
    }
    
    public static void logBytes(string logInfo, byte[] bytes)
    {
        string strLog = "";
        foreach (byte b in bytes)
        {
            strLog += b + " ";
        }
        
        Debug.LogError(string.Format("{0} : length {1}, {2}", logInfo, bytes.Length, strLog));
    }

}

