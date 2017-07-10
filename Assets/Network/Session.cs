using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.IO;

public class Session {

    private Socket Socket_ = null;
    private IPEndPoint End_point_ = null;

    private Int16 Header_;
    private byte[] HeaderBuffer_;
    private byte[] BodyBuffer_;

    public static int MaxPacketSize = 8000;
    public static int MaxBufferSize = 8096;

    public delegate void onConnectCallback();
    public delegate void onDisconnectCallback(SocketError ErrorCode);
    public delegate void onRecvCallback(MemoryStream Buffer);

    public onConnectCallback onConnectCallback_;
    public onDisconnectCallback onDisconnectCallback_;
    public onRecvCallback onRecvCallback_;

    public Queue<MemoryStream> SendQueue_;

    ~Session()
    {
        Disconnect();
    }

    public void Init()
    {
        HeaderBuffer_ = new byte[sizeof(Int16)];
        BodyBuffer_ = new byte[MaxBufferSize];
        
        SendQueue_ = new Queue<MemoryStream>();
    }

    public void Connect(String Host, int Port, onConnectCallback ConnectCallback, onDisconnectCallback DisconnectCallback, onRecvCallback RecvCallback, bool NoDelay = true)
    {
        Init();

        onConnectCallback_    = ConnectCallback;
        onDisconnectCallback_ = DisconnectCallback;
        onRecvCallback_       = RecvCallback;

        this.End_point_ = new IPEndPoint(IPAddress.Parse(Host), Port);
        this.Socket_ = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        this.Socket_.NoDelay = NoDelay;

        SocketAsyncEventArgs EventArgs = new SocketAsyncEventArgs();
        EventArgs.Completed += onConnect;
        EventArgs.RemoteEndPoint = this.End_point_;

        bool pending = Socket_.ConnectAsync(EventArgs);
        if (!pending)
        {
            onConnect(null, EventArgs);
            return;
        }
    }

    public void Disconnect()
    {
        if (Socket_ == null) return;

        Debug.Log("Disconnect called");
        Socket_.Disconnect(false);
        Socket_.Close();
        Socket_ = null;
        //Socket_.Shutdown();
    }

    private void onConnect(object sender, SocketAsyncEventArgs e)
    {
        if (e.SocketError == SocketError.Success)
        {
            onConnectCallback_();
            DoReadHeader();
        }
        else
        {
            onDisconnectCallback_(e.SocketError);
        }
    }

    private void DoReadHeader()
    {
        try
        {
            Socket_.BeginReceive(HeaderBuffer_, 0, sizeof(Int16), SocketFlags.None,
                new AsyncCallback(OnReadHeader), Socket_);
        }
        catch (Exception e)
        {

            onDisconnectCallback_(SocketError.SocketError);
        }
    }

    private void OnReadHeader(IAsyncResult ar)
    {
        try
        {
            SocketError SE;
            int RecvLength = Socket_.EndReceive(ar, out SE);
            
            if (SE == SocketError.Success && RecvLength > 0 && RecvLength <= sizeof(Int16))
            {
                Header_ = BitConverter.ToInt16(HeaderBuffer_, 0);
                DoReadBody();
            }
            else
            {
                onDisconnectCallback_(SE);
            }

        }
        catch (Exception e)
        {
            onDisconnectCallback_(SocketError.SocketError);
        }
    }

    private void DoReadBody()
    {
        try
        {
            Socket_.BeginReceive(BodyBuffer_, 0, Header_, SocketFlags.None,
                new AsyncCallback(OnReadBody), Socket_);
        }
        catch (Exception e)
        {

            onDisconnectCallback_(SocketError.SocketError);
        }
    }

    private void OnReadBody(IAsyncResult ar)
    {
        try
        {
            SocketError SE;
            int RecvLength = Socket_.EndReceive(ar, out SE);
            if (SE == SocketError.Success && RecvLength > 0 && RecvLength < MaxBufferSize)
            {
                //Debug.Log("몸통 받은 사이즈: " + RecvLength);
                var Buf = new MemoryStream();
                Buf.Write(BodyBuffer_, 0, RecvLength);

                onRecvCallback_(Buf); 

                DoReadHeader();
            }
            else
            {
                onDisconnectCallback_(SE);
            }

        }
        catch (Exception e)
        {
            onDisconnectCallback_(SocketError.SocketError);
        }
    }

    public void Send(MemoryStream Buffer)
    {
        bool WriteInProgress = false;

        if (SendQueue_.Count > 0)
        {
            WriteInProgress = true;
        }

        SendQueue_.Enqueue(Buffer);

        if (!WriteInProgress)
        {
            DoSend();
        }
    }

    public void DoSend()
    {
        MemoryStream Buffer = SendQueue_.Dequeue();

        Socket_.BeginSend(Buffer.GetBuffer(), 0, Convert.ToInt32(Buffer.Length), 0,
      new AsyncCallback(onSend), Socket_);
    }

    private void onSend(IAsyncResult ar)
    {
        try
        {
            Socket Sock = (Socket)ar.AsyncState;

            int SendByte = Sock.EndSend(ar);
            if (SendQueue_.Count > 0)
            {
                DoSend();
            }
            
        }
        catch (Exception e)
        {
            SendQueue_.Clear();
            onDisconnectCallback_(SocketError.SocketError);
        }
    }

}
