using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.SQLite;
using System.IO;
using System.Drawing.Text;

namespace MyBudgetApp
{
    public partial class Form1 : Form
    {

        // 存儲所有支出和收入的列表
        private List<Expense> expenses = new List<Expense>();

        // UI元件宣告
        private ComboBox cmb_category;
        private ComboBox cmb_type;
        private DataGridView dataGrid;
        private TextBox txt_item;
        private TextBox txt_amount;
        private DateTimePicker datePicker;
        private Label lbl_Income;
        private Label lbl_Expense;
        private Label lbl_balance;
        private Chart chartPie;
        private DataGridView dataBudget;
        private Label lbl_BudgetAlert;
        private Button btn_add;
        private Button btn_update;
        private Button btn_SaveBudget;
        private MonthCalendar calendarExpense;
        private DataGridView dataSearchResult;
        private TabControl tabControl;
        private TabPage tabTransactions;
        private TabPage tabChart;
        private TabPage tabBudget;
        private TabPage tabSearch;
        private Panel leftPanel;
        private Button btn_clear;

        // 收支項目類別
        public class Expense
        {
            public DateTime Date { get; set; }
            public string Item { get; set; }
            public string Category { get; set; }
            public decimal Amount { get; set; }
            public string Type { get; set; }
        }

        // 用字典儲存每個分類的預算
        private Dictionary<string, decimal> budgetLimits = new Dictionary<string, decimal>();

        // 表單建構函式
        public Form1()
        {
            InitializeComponent();
            Initialize_component(); // 初始化元件
            SetupUI();

            // 註冊表單載入事件
            this.Load += new EventHandler(Form1_Load);
        }

        // 初始化元件
        private void Initialize_component()
        {
            // 基本表單設定
            this.SuspendLayout();

            // 設定表單基本屬性
            this.Text = "我的記帳本";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.ResumeLayout(false);
        }
      
        // 設置UI元件及其初始值
        private void SetupUI()
        {
            // 左側面板：輸入區域 
            leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 300,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(leftPanel);         

            // 日期選擇器
            Label lblDate = new Label { Text = "日期:", Location = new Point(20, 20), AutoSize = true };
            leftPanel.Controls.Add(lblDate);

            datePicker = new DateTimePicker
            {
                Location = new Point(20, 45),
                Width = 250,
                Format = DateTimePickerFormat.Short
            };
            leftPanel.Controls.Add(datePicker);

            // 項目名稱輸入
            Label lblItem = new Label { Text = "項目名稱:", Location = new Point(20, 80), AutoSize = true };
            leftPanel.Controls.Add(lblItem);

            txt_item = new TextBox
            {
                Location = new Point(20, 105),
                Width = 250
            };
            leftPanel.Controls.Add(txt_item);

            // 收入/支出類型
            Label lblType = new Label { Text = "類型:", Location = new Point(20, 140), AutoSize = true };
            leftPanel.Controls.Add(lblType);

            cmb_type = new ComboBox
            {
                Location = new Point(20, 165),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmb_type.Items.AddRange(new string[] { "收入", "支出" });
            cmb_type.SelectedIndex = 0;
            leftPanel.Controls.Add(cmb_type);

            // 分類
            Label lblCategory = new Label { Text = "分類:", Location = new Point(20, 200), AutoSize = true };
            leftPanel.Controls.Add(lblCategory);

            cmb_category = new ComboBox
            {
                Location = new Point(20, 225),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmb_category.Items.AddRange(new string[] { "飲食", "交通", "娛樂", "教育", "醫療", "服飾", "美妝", "運動", "其他" });
            cmb_category.SelectedIndex = 0;
            leftPanel.Controls.Add(cmb_category);

            // 金額
            Label lblAmount = new Label { Text = "金額:", Location = new Point(20, 260), AutoSize = true };
            leftPanel.Controls.Add(lblAmount);

            txt_amount = new TextBox
            {
                Location = new Point(20, 285),
                Width = 250
            };
            leftPanel.Controls.Add(txt_amount);

            // 新增按鈕
            btn_add = new Button
            {
                Text = "新增",
                Location = new Point(20, 330),
                Width = 120,
                Height = 40
            };
            btn_add.Click += new EventHandler(btn_add_Click);
            leftPanel.Controls.Add(btn_add);

            // 更新統計按鈕
            btn_update = new Button
            {
                Text = "更新統計",
                Location = new Point(150, 330),
                Width = 120,
                Height = 40
            };
            btn_update.Click += new EventHandler(btn_update_Click);
            leftPanel.Controls.Add(btn_update);

            // 清除按鈕
            btn_clear = new Button
            {
                Text = "清除",
                Location = new Point(20, 380),
                Width = 120,
                Height = 40
            };
            btn_clear.Click += new EventHandler(btn_clear_Click);
            leftPanel.Controls.Add(btn_clear);

            // 統計數據顯示
            lbl_Income = new Label
            {
                Text = "總收入: $0",
                Location = new Point(20, 430),
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            leftPanel.Controls.Add(lbl_Income);

            lbl_Expense = new Label
            {
                Text = "總支出: $0",
                Location = new Point(20, 480),
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            leftPanel.Controls.Add(lbl_Expense);

            lbl_balance = new Label
            {
                Text = "餘額: $0",
                Location = new Point(20, 530),
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            leftPanel.Controls.Add(lbl_balance);

            // 預算警告
            lbl_BudgetAlert = new Label
            {
                Text = "",
                Location = new Point(20, 580),
                Width = 250,
                Height = 100,
                ForeColor = Color.Red,
                Font = new Font("Arial", 10)
            };
            leftPanel.Controls.Add(lbl_BudgetAlert);

            // 右側面板：資料顯示區域
            tabControl = new TabControl
            {
                Location = new Point(320, 0),
                Size = new Size(580, 700),
                Padding = new Point(20, 10),
                Margin = new Padding(20,0,0,0)
            };
            this.Controls.Add(tabControl);

            // 交易記錄頁
            tabTransactions = new TabPage("交易記錄");
            tabControl.TabPages.Add(tabTransactions);

            dataGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            tabTransactions.Controls.Add(dataGrid);

            // 設定DataGridView欄位
            dataGrid.ColumnCount = 5;
            dataGrid.Columns[0].Name = "日期";
            dataGrid.Columns[1].Name = "項目名稱";
            dataGrid.Columns[2].Name = "分類";
            dataGrid.Columns[3].Name = "金額";
            dataGrid.Columns[4].Name = "類型";

            //圓餅圖頁
            tabChart = new TabPage("支出分析");
            tabControl.TabPages.Add(tabChart);

            chartPie = new Chart
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
            };
            chartPie.Series.Clear();
            chartPie.ChartAreas.Add(new ChartArea("PieArea"));
            chartPie.Legends.Add(new Legend("Legend"));
            tabChart.Controls.Add(chartPie);

            // 預算設定頁
            tabBudget = new TabPage("預算設定");
            tabControl.TabPages.Add(tabBudget);

            dataBudget = new DataGridView
            {
                Dock = DockStyle.Top,  // 改成 Dock.Top
                Height = 300,
                AllowUserToAddRows = false
            };
            tabBudget.Controls.Add(dataBudget);


            // 設定預算DataGridView欄位
            dataBudget.ColumnCount = 3;
            dataBudget.Columns[0].Name = "Category"; 
            dataBudget.Columns[0].HeaderText = "分類";
            dataBudget.Columns[1].Name = "Limit";     
            dataBudget.Columns[1].HeaderText = "預算金額";
            dataBudget.Columns[2].Name = "Status";    
            dataBudget.Columns[2].HeaderText = "狀態";

            // 初始化預算資料
            foreach (string category in cmb_category.Items)
            {
                dataBudget.Rows.Add(category, 0, "未設定");
            }

            // 儲存預算按鈕
            btn_SaveBudget = new Button
            {
                Text = "儲存預算",
                Anchor = AnchorStyles.Top | AnchorStyles.Left,  // 使用 Anchor 而非 Location
                Location = new Point(10, 320),
                Width = 150,
                Height = 40
            };

            btn_SaveBudget.Click += new EventHandler(btn_SaveBudget_Click);
            tabBudget.Controls.Add(btn_SaveBudget);

            // 記錄查詢頁
            tabSearch = new TabPage("記錄查詢");
            tabControl.TabPages.Add(tabSearch);

            // 月曆控制項
            calendarExpense = new MonthCalendar
            {
                Dock = DockStyle.Top,  // 改為 Dock.Top
                MaxSelectionCount = 1
            };
            calendarExpense.DateChanged += new DateRangeEventHandler(calendarExpense_DateChanged);
            tabSearch.Controls.Add(calendarExpense);

            // 搜尋結果DataGridView
            dataSearchResult = new DataGridView
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(10, 190),
                Size = new Size(540, 300),
                AllowUserToAddRows = false,
                ReadOnly = true
            };
            tabSearch.Controls.Add(dataSearchResult);

            // 設定搜尋結果DataGridView欄位
            dataSearchResult.ColumnCount = 5;
            dataSearchResult.Columns[0].Name = "日期";
            dataSearchResult.Columns[1].Name = "項目名稱";
            dataSearchResult.Columns[2].Name = "分類";
            dataSearchResult.Columns[3].Name = "金額";
            dataSearchResult.Columns[4].Name = "類型";
        }

        // 新增記帳項目按鈕點擊事件
        private void btn_add_Click(object sender, EventArgs e)
        {
            // 驗證項目名稱
            if (string.IsNullOrEmpty(txt_item.Text))
            {
                MessageBox.Show("請輸入項目名稱");
                return;
            }

            // 驗證金額
            if (string.IsNullOrEmpty(txt_amount.Text))
            {
                MessageBox.Show("請輸入金額");
                return;
            }

            if (!decimal.TryParse(txt_amount.Text, out decimal amount))
            {
                MessageBox.Show("請輸入有效的金額");
                return;
            }

            // 根據類型處理金額正負號
            string type = cmb_type.SelectedItem.ToString();
            if (type == "收入")
            {
                amount = Math.Abs(amount);
            }
            else if (type == "支出")
            {
                amount = -Math.Abs(amount);
            }
            else
            {
                MessageBox.Show("請選擇收入或支出");
                return;
            }

            // 創建新的Expense物件
            Expense newExpense = new Expense
            {
                Date = datePicker.Value.Date,
                Item = txt_item.Text,
                Category = cmb_category.SelectedItem.ToString(),
                Amount = amount,
                Type = type
            };

            try
            {
                // 將新項目添加到DataGridView
                dataGrid.Rows.Add(
                    newExpense.Date.ToString("yyyy/MM/dd"),
                    newExpense.Item,
                    newExpense.Category,
                    Math.Abs(newExpense.Amount).ToString("0"),
                    newExpense.Type
                );

                // 將項目添加到expenses列表
                expenses.Add(newExpense);

                // 存入資料庫
                DatabaseHelper.InsertExpense(newExpense);

                // 清空輸入欄位
                txt_item.Clear();
                txt_amount.Clear();
                cmb_category.SelectedIndex = 0;
                cmb_type.SelectedIndex = 0;

                // 更新統計和預算狀態
                UpdateStatistics();
                UpdateBudgetStatus();

                MessageBox.Show("記錄已成功添加！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加記錄時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            //確認是否清空數據
            var result = MessageBox.Show("確定要清空所有數據嗎？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    // 清空DataGridView
                    dataGrid.Rows.Clear();
                    dataBudget.Rows.Clear();
                    dataSearchResult.Rows.Clear();
                    // 清空預算字典
                    budgetLimits.Clear();
                    // 清空支出列表
                    expenses.Clear();
                    // 清空圓餅圖
                    chartPie.Series.Clear();
                    // 清空輸入欄位
                    txt_item.Clear();
                    txt_amount.Clear();
                    cmb_category.SelectedIndex = 0;
                    cmb_type.SelectedIndex = 0;
                    lbl_Income.Text = "總收入: $0";
                    lbl_Expense.Text = "總支出: $0";
                    lbl_balance.Text = "餘額: $0";
                    //重製預算表
                    foreach (string category in cmb_category.Items)
                    {
                        dataBudget.Rows.Add(category, 0, "未設定");
                    }
                    MessageBox.Show("所有數據已清空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"清空數據時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // 清空輸入欄位
            txt_item.Clear();
            txt_amount.Clear();
            cmb_category.SelectedIndex = 0;
            cmb_type.SelectedIndex = 0;

            // 更新統計和預算狀態
            UpdateStatistics();
            UpdateBudgetStatus();

        }
        

        // 更新統計按鈕點擊事件
        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateStatistics();
                UpdateBudgetStatus();
                MessageBox.Show("統計數據已更新", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新統計時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 更新統計數據和圓餅圖
        private void UpdateStatistics()
        {
            // 計算總額
            decimal totalIncome = expenses
                .Where(x => x.Type == "收入")
                .Sum(x => Math.Abs(x.Amount));

            decimal totalExpense = expenses
                .Where(x => x.Type == "支出")
                .Sum(x => Math.Abs(x.Amount));

            decimal balance = totalIncome - totalExpense;

            // 更新標籤
            lbl_Income.Text = $"總收入: ${totalIncome:0}";
            lbl_Expense.Text = $"總支出: ${totalExpense:0}";
            lbl_balance.Text = $"餘額: ${balance:0}";

            // 清除現有圓餅圖
            chartPie.Series.Clear();

            // 建立新的數據系列
            Series series = new Series
            {
                Name = "支出",
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                LabelFormat = "{0}元 ({1:P1})"
            };

            // 計算每個分類的總支出
            var categoryGroups = expenses
                .Where(x => x.Type == "支出")
                .GroupBy(x => x.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Total = g.Sum(x => Math.Abs(x.Amount))
                });

            // 將數據添加到系列中
            foreach (var group in categoryGroups)
            {
                series.Points.AddXY(group.Category, group.Total);
            }

            // 添加系列到圖表
            chartPie.Series.Add(series);

            // 確保圖例存在後再加入
            if (chartPie.Legends.Count == 0)
            {
                chartPie.Legends.Add(new Legend("Legend"));
                chartPie.ChartAreas.Add(new ChartArea("PieArea"));
            }
            
        }

        // 儲存預算按鈕點擊事件
        private void btn_SaveBudget_Click(object sender, EventArgs e)
        {
            try
            {
                budgetLimits.Clear();
                foreach (DataGridViewRow row in dataBudget.Rows)
                {
                    string category = row.Cells["Category"].Value?.ToString();
                    if (string.IsNullOrEmpty(category))
                        continue;

                    decimal limit = 0;

                    // 檢查儲存格值是否為 null 或空字串
                    string limitStr = row.Cells["Limit"].Value?.ToString();
                    if (!string.IsNullOrEmpty(limitStr) && decimal.TryParse(limitStr, out limit))
                    {
                        budgetLimits[category] = limit;
                    }
                    else
                    {
                        budgetLimits[category] = 0; // 默認為0
                    }
                }

                MessageBox.Show("預算已成功儲存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateBudgetStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"儲存預算時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 更新預算狀態與提醒
        private void UpdateBudgetStatus()
        {
            if (dataBudget == null || dataBudget.Rows.Count == 0 || lbl_BudgetAlert == null)
            {
                return; // 防止例外狀況
            }

            lbl_BudgetAlert.Text = "";
            bool hasWarning = false;

            foreach (DataGridViewRow row in dataBudget.Rows)
            {
                if (row.Cells["Category"].Value == null)
                    continue;

                string category = row.Cells["Category"].Value.ToString();
                decimal limit = budgetLimits.ContainsKey(category) ? budgetLimits[category] : 0;
                decimal spent = expenses
                    .Where(x => x.Type == "支出" && x.Category == category)
                    .Sum(x => Math.Abs(x.Amount));
                decimal remaining = limit - spent;

                if (limit == 0)
                {
                    row.Cells["Status"].Value = "未設定";
                }
                else if (spent >= limit)
                {
                    row.Cells["Status"].Value = "超支";
                    lbl_BudgetAlert.Text += $"[警告] 分類 {category} 已超出預算！\n";
                    hasWarning = true;
                }
                else
                {
                    row.Cells["Status"].Value = $"正常 (剩餘: {remaining:0})";
                }

                row.Cells["Limit"].Value = limit.ToString("0");
            }

            // 如果有超出預算的類別，彈出警告視窗
            if (hasWarning)
            {
                MessageBox.Show(lbl_BudgetAlert.Text, "預算警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 月曆日期變更事件
        private void calendarExpense_DateChanged(object sender, DateRangeEventArgs e)
        {
            try
            {
                if (calendarExpense == null || dataSearchResult == null)
                    return;

                DateTime selectedDate = calendarExpense.SelectionStart.Date;

                var filtered = expenses
                    .Where(x => x.Date.Date == selectedDate)
                    .ToList();

                DisplaySearchResults(filtered);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查詢記錄時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 顯示搜尋結果
        private void DisplaySearchResults(List<Expense> filtered)
        {
            if (dataSearchResult == null)
                return; // 防止空參考例外狀況

            dataSearchResult.Rows.Clear();

            foreach (var exp in filtered)
            {
                dataSearchResult.Rows.Add(
                    exp.Date.ToShortDateString(),
                    exp.Item,
                    exp.Category,
                    Math.Abs(exp.Amount).ToString("0"),
                    exp.Type
                );
            }
        }

        // 表單載入事件
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // 初始化資料庫
                DatabaseHelper.InitializeDatabase();

                // 載入資料
                expenses = DatabaseHelper.LoadExpenses();

                // 確保 dataGrid 已初始化
                if (dataGrid != null)
                {
                    // 填充DataGridView
                    dataGrid.Rows.Clear();
                    foreach (var exp in expenses)
                    {
                        dataGrid.Rows.Add(
                            exp.Date.ToShortDateString(),
                            exp.Item,
                            exp.Category,
                            Math.Abs(exp.Amount).ToString("0"),
                            exp.Type
                        );
                    }
                }

                // 更新統計和預算狀態
                UpdateStatistics();
                UpdateBudgetStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"載入資料時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // 資料庫操作輔助類別
    public static class DatabaseHelper
    {
        public static string dbPath = "expenses.db";

        // 初始化資料庫
        public static void InitializeDatabase()
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
                using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    string sql = @"CREATE TABLE Expenses (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Date TEXT,
                                    Item TEXT,
                                    Category TEXT,
                                    Amount REAL,
                                    Type TEXT
                                  )";
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 插入支出/收入項目到資料庫
        public static void InsertExpense(Form1.Expense exp)
        {
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string sql = "INSERT INTO Expenses (Date, Item, Category, Amount, Type) VALUES (@Date, @Item, @Category, @Amount, @Type)";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Date", exp.Date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Item", exp.Item);
                cmd.Parameters.AddWithValue("@Category", exp.Category);
                cmd.Parameters.AddWithValue("@Amount", exp.Amount);
                cmd.Parameters.AddWithValue("@Type", exp.Type);
                cmd.ExecuteNonQuery();
            }
        }

        // 從資料庫載入所有支出/收入項目
        public static List<Form1.Expense> LoadExpenses()
        {
            List<Form1.Expense> list = new List<Form1.Expense>();

            // 檢查資料庫文件是否存在
            if (!File.Exists(dbPath))
            {
                InitializeDatabase();
                return list;
            }

            try
            {
                using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    string sql = "SELECT * FROM Expenses";
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime date;
                            if (!DateTime.TryParse(reader["Date"].ToString(), out date))
                            {
                                // 如果日期解析失敗，使用當前日期
                                date = DateTime.Now;
                            }

                            decimal amount;
                            if (!decimal.TryParse(reader["Amount"].ToString(), out amount))
                            {
                                amount = 0;
                            }

                            list.Add(new Form1.Expense
                            {
                                Date = date,
                                Item = reader["Item"]?.ToString() ?? string.Empty,
                                Category = reader["Category"]?.ToString() ?? string.Empty,
                                Amount = amount,
                                Type = reader["Type"]?.ToString() ?? string.Empty
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 記錄錯誤或顯示錯誤訊息
                Console.WriteLine($"清空數據時發生錯誤: {ex.Message}");
                // 可以考慮在這裡添加更多的錯誤處理邏輯
            }
            return list;
        }

        //清空資料庫紀錄
        public static void ClearDatabase()
        {
            try
            {
                using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    string sql = "DELETE FROM Expenses";
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // 記錄錯誤或顯示錯誤訊息
                Console.WriteLine($"清空數據時發生錯誤: {ex.Message}");
                // 可以考慮在這裡添加更多的錯誤處理邏輯
            }
        }
    }
}