namespace MyBudgetApp
{
    partial class Component3
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_add = new System.Windows.Forms.Button();
            this.cmb_category = new System.Windows.Forms.ComboBox();
            this.DatePicker = new System.Windows.Forms.DateTimePicker();
            this.DataGrid = new System.Windows.Forms.DataGridView();
            this.txt_item = new System.Windows.Forms.TextBox();
            this.txt_amount = new System.Windows.Forms.TextBox();
            this.cmb_type = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).BeginInit();
            // 
            // btn_add
            // 
            this.btn_add.Location = new System.Drawing.Point(0, 0);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(75, 23);
            this.btn_add.TabIndex = 0;
            this.btn_add.Text = "新增紀錄";
            this.btn_add.UseVisualStyleBackColor = true;
            // 
            // cmb_category
            // 
            this.cmb_category.FormattingEnabled = true;
            this.cmb_category.Location = new System.Drawing.Point(0, 0);
            this.cmb_category.Name = "cmb_category";
            this.cmb_category.Size = new System.Drawing.Size(121, 23);
            this.cmb_category.TabIndex = 0;
            this.cmb_category.Text = "選擇種類";
            this.cmb_category.SelectedIndexChanged += new System.EventHandler(this.cmb_category_SelectedIndexChanged);
            // 
            // DatePicker
            // 
            this.DatePicker.Location = new System.Drawing.Point(0, 0);
            this.DatePicker.Name = "DatePicker";
            this.DatePicker.Size = new System.Drawing.Size(200, 25);
            this.DatePicker.TabIndex = 0;
            // 
            // DataGrid
            // 
            this.DataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGrid.Location = new System.Drawing.Point(0, 0);
            this.DataGrid.Name = "DataGrid";
            this.DataGrid.RowHeadersWidth = 51;
            this.DataGrid.RowTemplate.Height = 27;
            this.DataGrid.Size = new System.Drawing.Size(240, 150);
            this.DataGrid.TabIndex = 0;
            this.DataGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGrid_CellContentClick);
            // 
            // txt_item
            // 
            this.txt_item.Location = new System.Drawing.Point(0, 0);
            this.txt_item.Name = "txt_item";
            this.txt_item.Size = new System.Drawing.Size(100, 25);
            this.txt_item.TabIndex = 0;
            // 
            // txt_amount
            // 
            this.txt_amount.Location = new System.Drawing.Point(0, 0);
            this.txt_amount.Name = "txt_amount";
            this.txt_amount.Size = new System.Drawing.Size(100, 25);
            this.txt_amount.TabIndex = 0;
            // 
            // cmb_type
            // 
            this.cmb_type.FormattingEnabled = true;
            this.cmb_type.Location = new System.Drawing.Point(0, 0);
            this.cmb_type.Name = "cmb_type";
            this.cmb_type.Size = new System.Drawing.Size(121, 23);
            this.cmb_type.TabIndex = 0;
            this.cmb_type.Text = "cmb_type";
            this.cmb_type.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).EndInit();

        }

        #endregion

        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.ComboBox cmb_category;
        private System.Windows.Forms.DateTimePicker DatePicker;
        private System.Windows.Forms.DataGridView DataGrid;
        private System.Windows.Forms.TextBox txt_item;
        private System.Windows.Forms.TextBox txt_amount;
        private System.Windows.Forms.ComboBox cmb_type;
    }
}
