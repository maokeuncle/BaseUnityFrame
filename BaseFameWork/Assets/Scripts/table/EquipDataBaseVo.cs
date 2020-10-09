public class EquipDataBaseVo : IByteVo
{
    public int ID; // 编号
    public string Name; // 设备名称
    public int Eqpid; // 设备编号
    public string DataType; // 数据类型
    public string Unit; // 展示单位
    public string Tag; // 备注
    public int MinVal; // 正常范围(最小值)
    public int MaxVal; // 正常范围(最大值)
    public string PosVect; // 摄像机坐标
    public string RoatVect; // 摄像机旋转角

    public void fromData(ByteArr byteArr)
    {
        ID = byteArr.readInt();
        Name = byteArr.readString();
        Eqpid = byteArr.readInt();
        DataType = byteArr.readString();
        Unit = byteArr.readString();
        Tag = byteArr.readString();
        MinVal = byteArr.readInt();
        MaxVal = byteArr.readInt();
        PosVect = byteArr.readString();
        RoatVect = byteArr.readString();
    }
}
