using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApplication2
{
    public partial class Form2 : Form
    {
        Form f1;//用于保存form1传过来的对象
        String ip, name;//传过来的ip和name
        Thread thread;//子线程对象
        Socket newclient;//Socket网络对象
        int flag ;//网络标志
        public Form2(Form f1,String ip,String name)
        {
            this.f1 = f1;//接收登录窗口form1对象
            this.ip = ip;//接收ip字符
            this.name = name;//接收用户名字符
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;//关闭子线程刷新ui限制
           
           
        }

        private void Form2_Load(object sender, EventArgs e)//窗口初始化函数
        {
            flag = 0;//默认网络标志位0，表示接通网络
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);//注册窗口X事件

            this.listView1.View = View.List;//设置Viewlist显示模式为列表模式

            this.thread = new Thread(new ThreadStart(this.recv));//实例化子线程
            this.thread.Start();//开启子线程
            
        }
        private void recv()//子线程
        { 
            byte[] data = new byte[1024];//byte数据类型，用于保存接受的socket数据
            newclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//实例化socket对象
            IPEndPoint ie = new IPEndPoint(IPAddress.Parse(ip), 8899);//设置ip地址与端口号
            try
            {
                newclient.Connect(ie);//开始连接
            }
            catch (SocketException err)
            {
                UpdateList("与服务器无法建立连接！");//抛出异常
                UpdateList(err.ToString());
                flag = 1;//设置网络标志位1，也就是无法连接到网络
                return;
            }
            int recv = newclient.Receive(data);//接收服务器上线数据
            string stringdata = Encoding.Default.GetString(data, 0, recv);//将byte数据转化为字符类型
            UpdateList(stringdata);//传入ui数据并刷新
            while (true)
            {
                data = new byte[1024];//byte数据类型，用于保存接受的socket数据
                recv = newclient.Receive(data);//接收消息数据
                stringdata = Encoding.Default.GetString(data, 0, recv);//将byte数据转化为字符类型
                UpdateList(stringdata);//传入ui数据并刷新
            }

        }

        private void UpdateList(String messg)//ui刷新函数，传入值为消息
        {
            this.listView1.BeginUpdate();//开始刷新
            ListViewItem lvi = new ListViewItem();//实例化一个item对象
            lvi.Text = messg;//设置item对象文本值
            this.listView1.Items.Add(lvi);//添加item对象
            this.listView1.EndUpdate();//停止刷新

        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)//点击窗口x调用该函数
        {
            if (flag==0) { //判断网络标志是否为0，也就是是否成功连接到网络。
            newclient.Send(Encoding.UTF8.GetBytes("exitthis"));//设置字符编码为UTF-8并发送退出消息
            newclient.Shutdown(SocketShutdown.Both);//停止socket连接
            newclient.Close();//关闭socket连接
            }
            thread.Abort();//关闭子线程
            f1.Show();//显示登录窗口form1
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)//点击发送按钮调用该函数
        {
            if (!textBox1.Text.Equals(""))//判断输入的消息是否为空
            {
                if (flag == 0)//判断网络连接是否成功
                {
                    newclient.Send(Encoding.UTF8.GetBytes(name + ":" + textBox1.Text + "\n"));//发送socket消息，并编码UTF-8
                }
            }
        }
    }
}
