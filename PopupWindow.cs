using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eventCountDown
{
    public partial class PopupWindow : Form
    {
        public string EventName { get; private set; }
        public int TimeInMinutes { get; private set; }

        private TextBox inputTextBox;
        private Label promptLabel;

        public PopupWindow()
        {
            InitializeComponent();

            // 不显示标题栏、边框和系统菜单
            this.FormBorderStyle = FormBorderStyle.None;

            // 不在任务栏显示
            this.ShowInTaskbar = false;

            // 创建提示信息标签
            this.promptLabel = new Label()
            {
                Text = "请按照格式'事件名 计时分钟数'输入", 
                Width = 600 , // 设置输入框宽度
                Font = new Font("苹方-简", 11F),
            };
            this.Controls.Add(this.promptLabel);

            // 实例化 inputTextBox 控件
            this.inputTextBox = new TextBox()
            {
                Location = new Point(0, this.promptLabel.Height),
                Width = 600, // 设置输入框宽度
                Font = new Font("苹方-简", 14F),
        };

            inputTextBox.KeyDown += InputTextBox_KeyDown;
            this.Controls.Add(this.inputTextBox);

            // 在窗口显示后立即设定输入框为焦点
            // 必须是在shown后调用而不是在init的时候聚焦，否则焦点可能会被夺走
            this.Shown += new EventHandler(PopupWindow_Shown);

            // 调整窗口位置到屏幕中间 
            this.StartPosition = FormStartPosition.CenterScreen;

            // 保证窗口总是在最前面
            this.TopMost = true;
        }
        private void PopupWindow_Shown(object sender, EventArgs e)
        {
            this.inputTextBox.Focus();
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var inputParts = this.inputTextBox.Text.Split(new[] { ' ' }, 2);
                if (inputParts.Length == 2 && int.TryParse(inputParts[1], out int time))
                {
                    this.EventName = inputParts[0];
                    this.TimeInMinutes = time;
                    
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("输入的格式不正确，请按照'事件名 计时分钟数'的格式输入");
                }
            }
            else if(e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }


}
