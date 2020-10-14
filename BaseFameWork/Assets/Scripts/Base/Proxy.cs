/// <summary>
/// 该类为所有ui面板proxy的总调用入口，由它来访问所有的proxy
/// </summary>
public class Proxy
{   public static LoginProxy LoginProxy;
   
   

    public static void InitAsset()
    {
 
LoginProxy = new LoginProxy().InitSet() as LoginProxy;  
 }
}
