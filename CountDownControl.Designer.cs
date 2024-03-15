
namespace eventCountDown
{
    partial class CountDownControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.countDownLabel = new System.Windows.Forms.Label();
            this.eventLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // countDownLabel
            // 
            this.countDownLabel.BackColor = System.Drawing.Color.Transparent;
            this.countDownLabel.CausesValidation = false;
            this.countDownLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.countDownLabel.Font = new System.Drawing.Font("苹方-简", 11F);
            this.countDownLabel.ForeColor = System.Drawing.Color.White;
            this.countDownLabel.Location = new System.Drawing.Point(153, 0);
            this.countDownLabel.Margin = new System.Windows.Forms.Padding(0);
            this.countDownLabel.Name = "countDownLabel";
            this.countDownLabel.Padding = new System.Windows.Forms.Padding(5);
            this.countDownLabel.Size = new System.Drawing.Size(103, 41);
            this.countDownLabel.TabIndex = 1;
            this.countDownLabel.Text = "10:00";
            this.countDownLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // eventLabel
            // 
            this.eventLabel.BackColor = System.Drawing.Color.Transparent;
            this.eventLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventLabel.Font = new System.Drawing.Font("苹方-简", 11F);
            this.eventLabel.ForeColor = System.Drawing.Color.White;
            this.eventLabel.Location = new System.Drawing.Point(0, 0);
            this.eventLabel.Margin = new System.Windows.Forms.Padding(0);
            this.eventLabel.Name = "eventLabel";
            this.eventLabel.Padding = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.eventLabel.Size = new System.Drawing.Size(153, 41);
            this.eventLabel.TabIndex = 2;
            this.eventLabel.Text = "test event";
            this.eventLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.Controls.Add(this.eventLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.countDownLabel, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(256, 41);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // CountDownControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CountDownControl";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(260, 45);
            this.Load += new System.EventHandler(this.CountDownControl_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label countDownLabel;
        private System.Windows.Forms.Label eventLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
