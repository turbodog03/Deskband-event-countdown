using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eventCountDown
{
    public partial class CountDownControl : UserControl
    {
        //注册快捷键的API
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        //取消注册快捷键的API
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        Label completedEventsLabel = new Label();
        //快捷键的修饰键
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8
        }

        private Timer timer;
        private int remainingSeconds;            

        // 做一个模拟的完成的事件数量
        int completedEvents = new Random().Next(1, 10);

        public CountDownControl()
        {
            InitializeComponent();

            // UI
            this.VisibleChanged += new EventHandler(CountDownControl_VisibleChanged);
            // 在这里为 eventLabel 添加改变大小的事件处理器——貌似无法直接改变左右边界，是固定的
            // this.eventLabel.SizeChanged += EventLabel_SizeChanged;
            int taskBarHeight = Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.WorkingArea.Height;
            this.Height = taskBarHeight;

            this.Paint += LeftBorderPaint;

            // Timer
            this.timer = new Timer
            {
                Interval = 1000 // 设置为每一秒触发一次 Tick 事件
            };
            this.timer.Tick += new EventHandler(this.Timer_Tick); // 添加 Tick 事件的处理器

            completedEventsLabel.TextAlign = ContentAlignment.MiddleCenter;
            completedEventsLabel.Dock = DockStyle.Fill;
            completedEventsLabel.Visible = true;
            completedEventsLabel.Text = "Completed events: " + completedEvents;
            completedEventsLabel.Font = new System.Drawing.Font("苹方-简", 11F);
            completedEventsLabel.ForeColor = Color.White;
            this.Controls.Add(completedEventsLabel);

            ToggleCountdownShownStatus(false);

        }

        #region Hot Key
        private void CountDownControl_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                //注销Id号为100的热键设定
                UnregisterHotKey(this.Handle, 100);
            }
        }

        private void CountDownControl_Load(object sender, EventArgs e)
        {
            // 注册全局热键 Ctrl+Shift+D，唯一标识为100。
            RegisterHotKey(this.Handle, 100, (uint)KeyModifiers.Control | (uint)KeyModifiers.Shift, (uint)Keys.D);

        }

        /// <summary>
        /// 监听系统消息，执行热键关联的方法
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:    //按下的是Ctrl+Shift+D
                            var popupWindow = new PopupWindow();
                            if (popupWindow.ShowDialog() == DialogResult.OK)
                            {
                                ToggleCountdownShownStatus(true);
                                this.countDownLabel.Text = popupWindow.TimeInMinutes.ToString() + ":00";
                                this.eventLabel.Text = popupWindow.EventName;
                                completedEventsLabel.Visible = false;
                                this.StartCountdown(popupWindow.TimeInMinutes);
                            }

                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }
        #endregion

        #region UI
        private void EventLabel_SizeChanged(object sender, EventArgs e)
        {
            if (sender is Label eventLabel)
            {
                if (eventLabel.Parent is CountDownControl control)
                {
                    countDownLabel.Left = eventLabel.Right;
                }
            }
        }

        private void LeftBorderPaint(object sender, PaintEventArgs e)
        {
            // 绘制左边框
            int BORDER_WIDTH = 50;
            double BORDER_PADDING_PERCENTAGE = 0.20;
            int gap = (int)(this.Height * BORDER_PADDING_PERCENTAGE);
            using (Pen pen = new Pen(Color.White, BORDER_WIDTH)) // 设置边框颜色和宽度
            {
                // 左移直线，因为默认沿中心线画，这样就有一半看不见
                e.Graphics.DrawLine(pen, new Point(BORDER_WIDTH / 2, gap), new Point(BORDER_WIDTH / 2, this.Height - gap)); // 从左上角到左下角绘制边框
            }
        }
        #endregion

        #region Timer
        public void StartCountdown(int minutes)
        {
            this.remainingSeconds = minutes * 60; // 把分钟转换为秒数
            this.timer.Start(); // 启动计时器
        }

        private void HandleEventDone()
        {
            this.timer.Stop(); // 如果倒计时结束，停止计时器

            ToggleCountdownShownStatus(false);

            // 显示完成的事件数量
            this.completedEventsLabel.Visible = true;
            this.completedEventsLabel.Text = "Completed events: " + completedEvents;

            // this.Invalidate(); // 强制立即重绘控件
            System.Media.SystemSounds.Beep.Play(); // 还有很多种提示音，可以去看 System.Media.SystemSounds的文档。不过还是有空的话自定义个提示音吧

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.remainingSeconds <= 0)
            {
                HandleEventDone();
            }
            else
            {
                this.remainingSeconds--; // 递减剩余秒数

                // 计算剩余的分钟数和秒数
                int minutes = this.remainingSeconds / 60;
                int seconds = this.remainingSeconds % 60;

                // 更新 countDownLabel 的文字
                this.countDownLabel.Text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
            }
        }

        private void ToggleCountdownShownStatus(bool status)
        {
            this.countDownLabel.Visible = status;
            this.eventLabel.Visible = status;
            this.tableLayoutPanel1.Visible = status;
        }
        #endregion
    }
}