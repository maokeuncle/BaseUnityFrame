using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
public class CustomEditor : EditorWindow
{
    private string text;

    private string _templateViewContent;
    private string _templateViewPath;
    private string _templateProxyContent;
    private string _templateProxyPath;
    private string _generalProxyContent;
    private string _generalProxyPath;
    
    [MenuItem("Unity研习社/Init")]
    private static void AddInitWindow()
    {
        CustomEditor initWindow = (CustomEditor)GetWindow(typeof(CustomEditor), false, "InitWindow", true);
        initWindow.Show();
    }

    private void OnGUI()
    {
        //文本输入框控件
        text = EditorGUILayout.TextField("请输入View名称:", text);
        if (GUILayout.Button("创建", GUILayout.ExpandWidth(true)))
        {
            //Proxy代码模板路径
            _templateProxyPath = "Assets/Scripts/ScriptDemo/ProxyDemo.cs";
            //View代码模板路径
            _templateViewPath = "Assets/Scripts/ScriptDemo/ViewDemo.cs";
            //Proxy代码路径
            _generalProxyPath = "Assets/Scripts/Base/Proxy.cs";
            
            string viewPath = "Assets/Scripts/" + text;//生成路径
            if (!Directory.Exists(viewPath))
            {
                if (!Directory.Exists("Assets/UIPrefab"))
                {
                    Directory.CreateDirectory("Assets/UIPrefab");
                    //创建对应的prefab
                    SuperTool.CreatPrefab(text,"Assets/UIPrefab");
                }
                else
                {
                    //创建对应的prefab
                    SuperTool.CreatPrefab(text,"Assets/UIPrefab");
                }
                //创建存放view-proxy的文件夹
                Directory.CreateDirectory(viewPath);
                //创建View脚本
                _templateViewContent = File.ReadAllText(_templateViewPath);
                _templateViewContent = _templateViewContent.Replace("ViewDemo",text+"View");
                SuperTool.WriteFile(viewPath+"/"+text,"View",_templateViewContent);
                //创建Proxy脚本
                _templateProxyContent = File.ReadAllText(_templateProxyPath);
                _templateProxyContent = _templateProxyContent.Replace("ProxyDemo", text + "Proxy");
                _templateProxyContent = _templateProxyContent.Replace("ViewDemo", text + "View");
                SuperTool.WriteFile(viewPath+"/"+text,"Proxy",_templateProxyContent);
                //初始化Proxy变量
                _generalProxyContent = File.ReadAllText(_generalProxyPath);
                _generalProxyContent = _generalProxyContent.Insert(
                    _generalProxyContent.IndexOf('{') + 1,
                    "   public static " + text + "Proxy " + text + "Proxy;"+"\r\n");
                //初始化Proxy方法
                string val = text + "Proxy" + " = " + "new " + text + "Proxy" + "().InitSet() as " + text + "Proxy;  "+"\r\n";
                _generalProxyContent = _generalProxyContent.Insert(_generalProxyContent.IndexOf('}', 1)-1, val);
                SuperTool.WriteFile("Assets/Scripts/Base/","Proxy",_generalProxyContent);
                //刷新资源
                AssetDatabase.Refresh();
                DestroyImmediate(GameObject.Find(text));
                ShowNotification(new GUIContent(text+":已经初始化完毕！"));
            }
            else
            {
                //通知栏
                ShowNotification(new GUIContent(text+":已经创建！"));
            }
        }
    }

}
