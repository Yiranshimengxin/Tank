using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class MsgHandler
{
    public static void MsgMove(ClientState c, MsgBase msg)
    {
        MsgMove msgMove = (MsgMove)msg;
        Console.WriteLine(msgMove.x);
        msgMove.x++;
        NetManager.Send(c, msgMove);
    }
}

