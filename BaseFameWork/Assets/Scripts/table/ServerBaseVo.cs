public class ServerBaseVo : IByteVo
{
    public int ID; // 编号
    public string Name; // 服务器名称
    public string Ip; // 服务器ip
    public int Port; // 服务器port

    public void fromData(ByteArr byteArr)
    {
        ID = byteArr.readInt();
        Name = byteArr.readString();
        Ip = byteArr.readString();
        Port = byteArr.readInt();
    }
}
