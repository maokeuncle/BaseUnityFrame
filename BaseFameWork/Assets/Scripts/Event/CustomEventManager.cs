using System.Collections.Generic;
using UnityEngine;

public static class CustomEventManager
{

    public delegate void eventHandler(CusEventArgs e);


    private static Dictionary<string, HashSet<eventHandler>> eventDic = new Dictionary<string, HashSet<eventHandler>>();
    

    public static void AddListener(string type,eventHandler listener)
    {
        if (!eventDic.ContainsKey(type))
        {
            HashSet<eventHandler> eventCollection = new HashSet<eventHandler>();
            eventDic.Add(type,eventCollection);
            eventDic[type].Add(listener);
        }
        else
        {
            eventDic[type].Add(listener);
        }
    }


    public static void DispatchEvent(CusEventArgs e)
    {
        foreach (var item in eventDic[e.type])
        {
            item?.Invoke(e);
        }
    }

    public static void RemoveListener(string type, eventHandler listener)
    {
        if(!eventDic.ContainsKey(type))
            Debug.LogError(type+":不存在！");
        else
        {
            eventDic[type].Remove(listener);
        }
    }
    public static void RemoveAllListeners(string type)
    {
        if(!eventDic.ContainsKey(type))
            Debug.LogError(type+":不存在！");
        else
        {
            eventDic[type].Clear();
        }
    }
    
}

public class CusEventArgs
{
    public readonly string type;
    public readonly object message;
    public readonly object[] messageArr;
    
    public CusEventArgs(string type)
    {
        this.type = type;
    }

    public CusEventArgs(string type, params object[] messageArr)
    {
        this.type = type;
        this.messageArr = messageArr;
    }
    public CusEventArgs(string type, object message)
    {
        this.type = type;
        this.message = message;
    }
}