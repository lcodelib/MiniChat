using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)//窗口初始化函数
        {
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);//注册窗口X函数事件
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ipaddr.Text.Equals("")&&!name.Text.Equals(""))//判断ip地址和用户名是否为空
            {
                Form2 f2 = new Form2(this,ipaddr.Text,name.Text);//实例化一个新窗口
                f2.Show();//打开新窗口
                this.Hide();//隐藏当前窗口
            }
            else
            {
                MessageBox.Show("请输入您的IP或用户名！", "聊天室", MessageBoxButtons.OK, MessageBoxIcon.Question);//如果其中一个为空，则弹出
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)//点击窗口x调用该函数
        {
            if (DialogResult.No == MessageBox.Show("您确定要退出登录吗?", "聊天室", MessageBoxButtons.YesNo, MessageBoxIcon.Question))//弹出提示
            {
                e.Cancel = true;
            }
        }
 
    }
}
