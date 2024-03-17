using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
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

        private string CurrentEvent = "";
        private DateTime StartTime = DateTime.Now;
        readonly LogHelper LogHelper = new LogHelper(path);
        readonly DBHelper dbHelper = new DBHelper(Path.Combine(path, "SQLiteDB.db"));
        static readonly string path = Global.APP_PATH;

        DateTime nowDate = DateTime.Now;
        public CountDownControl()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            try
            {
                LogHelper.LogInfo("Getting EventNum");
                int completedEvents = 0;
                try
                {
                    completedEvents = dbHelper.GetEventNumOfADay(nowDate);
                }
                catch (Exception e)
                {
                    LogHelper.LogInfo($"Failed to get eventNum of a Day , e:{e}");
                }
                LogHelper.LogInfo($"Event num: {completedEvents}");

                // UI
                this.VisibleChanged += new EventHandler(CountDownControl_VisibleChanged);
                // 在这里为 eventLabel 添加改变大小的事件处理器——貌似无法直接改变左右边界，是固定的
                int taskBarHeight = Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.WorkingArea.Height;
                this.Height = taskBarHeight;
                this.Paint += LeftBorderPaint;
                // Timer
                this.timer = new Timer
                {
                    Interval = 1000 // 设置为每一秒触发一次 Tick 事件
                };
                this.timer.Tick += new EventHandler(this.Timer_Tick);
                completedEventsLabel.TextAlign = ContentAlignment.MiddleCenter;
                completedEventsLabel.Dock = DockStyle.Fill;
                completedEventsLabel.Visible = true;
                completedEventsLabel.Text = "Completed events: " + completedEvents;
                completedEventsLabel.ForeColor = Color.White;
                // TODO：using custom font, add font existance detect
                completedEventsLabel.Font = new Font(Global.COMPELETED_LABEL_FONT, Global.COMPELETED_LABEL_FONT_SIZE);
                this.Controls.Add(completedEventsLabel);

                ToggleCountdownShownStatus(false);

                LogHelper.LogInfo("Init finished");
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
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
                            //var popupWindow = new PopupWindow();

                            var popupWindow = new PopupWindowWPF();
                            popupWindow.Activate();
                            // if (popupWindow.ShowDialog() == DialogResult.OK)

                            // DialogResult in WPF is bool, unlike winform (OK/Cancel...)
                            if ((bool)popupWindow.ShowDialog())
                            {
                                ToggleCountdownShownStatus(true);
                                HandleEventStart(popupWindow);
                            }
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }
        #endregion

        #region event
        private void HandleEventStart(PopupWindowWPF popupWindow)
        {
            DateTime nowTime = DateTime.Now;
            // a current event is running
            if (this.timer.Enabled || this.remainingSeconds > 0)
            {
                TimeSpan interval = nowTime.Subtract(StartTime);
                // current event is running for at least Min_COUNT_MIN min, record it
                if (interval.TotalMinutes > Global.MIN_COUNT_MIN)
                {
                    // write current event to database
                    dbHelper.AddOneRecordToDB(CurrentEvent, StartTime, nowTime);
                }
            }
            CurrentEvent = popupWindow.EventName;
            StartTime = nowTime;

            // in case of last event isn't finish, and reset isn't called, so call it here
            ResetFontSize(this.eventLabel);
            this.countDownLabel.Text = popupWindow.TimeInMinutes.ToString() + ":00";
            this.eventLabel.Text = CurrentEvent;
            AdjustFontSize(this.eventLabel);

            completedEventsLabel.Visible = false;
            StartCountdown(popupWindow.TimeInMinutes);
        }


        private void HandleEventDone()
        {
            this.timer.Stop(); // stop timer when count down is done

            ToggleCountdownShownStatus(false);
            dbHelper.AddOneRecordToDB(CurrentEvent, StartTime, DateTime.Now);

            int completedEvents = dbHelper.GetEventNumOfADay(nowDate);
            // 显示完成的事件数量
            this.completedEventsLabel.Visible = true;
            this.completedEventsLabel.Text = "Completed events: " + completedEvents;
            LogHelper.LogInfo($"{CurrentEvent} Finished");
            // this.Invalidate(); // 强制立即重绘控件
            PlaySound();
        }

        private void PlaySound()
        {
            string SoundPath = Path.Combine(path, "Sound.wav");
            // custom sound exist
            if (File.Exists(SoundPath))
            {
                SoundPlayer player = new SoundPlayer();
                player.SoundLocation = SoundPath;
                player.Play();
                LogHelper.LogInfo("Play Sound.wav ");
            }
            // play default
            else
            {
                SystemSounds.Beep.Play(); // 还有很多种提示音，可以去看 System.Media.SystemSounds的文档。不过还是有空的话自定义个提示音吧
                // https://learn.microsoft.com/en-us/dotnet/api/system.media.systemsounds?view=dotnet-plat-ext-8.0
                LogHelper.LogInfo("Play SystemSound");
            }

        }
        #endregion

        #region UI

        // 貌似无法实时调整大小，暂时放弃
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

        private void AdjustFontSize(Label lbl)
        {
            LogHelper.LogInfo("Start adjust fontsize");
            using (Graphics g = lbl.CreateGraphics())
            {
                SizeF textSize;
                float fontSize = lbl.Font.Size;
                Font tempFont = lbl.Font;

                // 获取利用特定的字体来绘制串时所需的宽度和高度，以实现动态绘图
                textSize = g.MeasureString(lbl.Text, tempFont);
                while (textSize.Width > lbl.Width || textSize.Height > lbl.Height)
                {
                    fontSize -= 0.5f;
                    LogHelper.LogInfo($"fontsize --, now fontsize : {fontSize}");
                    tempFont = new Font(lbl.Font.FontFamily, fontSize);
                    textSize = g.MeasureString(lbl.Text, tempFont);
                }

                lbl.Font = new Font(lbl.Font.FontFamily, fontSize);
            }
            LogHelper.LogInfo("Finish adjust fontsize");
        }

        private void ResetFontSize(Label lbl)
        {
            LogHelper.LogInfo("fontSize reset");
            lbl.Font = new Font(lbl.Font.FontFamily, Global.EVENT_LABEL_FONT_SIZE, lbl.Font.Style);
        }

        #endregion

        #region Timer
        public void StartCountdown(int minutes)
        {
            this.remainingSeconds = minutes * 60; // 把分钟转换为秒数
            this.timer.Start(); // 启动计时器
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