using System;
using System.Windows;
using System.Windows.Input;

namespace eventCountDown
{
    public partial class PopupWindowWPF : Window
    {
        public string EventName { get; private set; }
        public int TimeInMinutes { get; private set; }

        public PopupWindowWPF()
        {
            InitializeComponent();
        }

        void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var inputParts = inputTextBox.Text.Split(new[] { ' ' }, 2);
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

    }
}
