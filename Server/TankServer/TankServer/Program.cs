using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class ClientState
{
    public Socket socket;
    public ByteArray readBuff = new ByteArray();
    public long lastPingTime = 0;
    public Player player;
}

internal class Program
{
    public static void Main(string[] args)
    {
        if (!DbManager.Connect("game", "127.0.0.1", 3306, "root", ""))
        {
            Console.WriteLine("数据库连接失败！！！！");
            return;
        }
        //if (DbManager.Register("Test", "1234"))
        //{
        //    Console.WriteLine("注册成功！");
        //}
        //if (DbManager.CreatePlayer("Megumin"))
        //{
        //    Console.WriteLine("创建成功！");
        //}
        //PlayerData playerData = new PlayerData();
        //playerData.coin = 10000;
        //playerData.text = "update text";
        //DbManager.UpdatePlayerData("Megumin", playerData);
        
        NetManager.StartLoop(8888);
    }
}
