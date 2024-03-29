﻿using System.Windows;
using System.Windows.Input;

namespace eventCountDown
{
    public partial class PopupWindowWPF : Window
    {
        public string EventName { get; private set; }
        public int TimeInMinutes { get; private set; }

        public string LastEvent;

        LogHelper LogHelper = new LogHelper(Global.APP_PATH, Global.GLOBAL_LOG_FILE_NAME, "PopupWindow");

        public PopupWindowWPF(string LastEvent)
        {
            this.LastEvent = LastEvent;
            InitializeComponent();
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LogHelper.LogInfo("popupWindow: enter pressed");
                string[] inputParts = inputTextBox.Text.Split(new[] { ' ' }, 2);
                if (inputParts.Length == 2 && int.TryParse(inputParts[1], out int time))
                {
                    EventName = inputParts[0];
                    TimeInMinutes = time;

                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("输入的格式不正确，请按照'事件名 计时分钟数'的格式输入");
                }
            }
            else if (e.Key == Key.Escape)
            {
                DialogResult = false;
                Close();
            }
        }

        // arrow key may be consumed by default controls, like move in the textbox. using previewKeyDown to capture it, which happens before KeyDown
        private void InputTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // in WPF, arrow key, f key, alt, ctrl, shift key are system keys
            if (e.Key == Key.Up)
            {
                inputTextBox.Text = this.LastEvent + " ";
                inputTextBox.CaretIndex = inputTextBox.Text.Length;  // Place the cursor at the end of the text
                LogHelper.LogInfo($"popupWindow: up arrow pressed, lastEvent is {LastEvent}");
            }
        }

    }
}
