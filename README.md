主程式在Form1.cs裡面

以下是 Form1.cs 的程式碼運作說明，分為主要功能模組進行解釋：

1. 表單初始化
•	建構函式 Form1()
•	呼叫 InitializeComponent() 初始化表單元件。
•	呼叫 SetupUI() 設置 UI 元件。
•	註冊 Form1_Load 事件，當表單載入時執行。
•	SetupUI()
•	設置左側面板（輸入區域）和右側面板（資料顯示區域）。
•	初始化各種 UI 元件（如 ComboBox、TextBox、DataGridView、Chart 等）。
•	設置按鈕事件處理器，例如：
•	btn_add_Click：新增記帳項目。
•	btn_update_Click：更新統計數據。
•	btn_SaveBudget_Click：儲存預算。

2. 資料管理
•	Expense 類別
•	定義記帳項目的結構，包括日期、項目名稱、分類、金額和類型（收入或支出）。
•	expenses 列表
•	用於存儲所有記帳項目。
•	budgetLimits 字典
•	儲存每個分類的預算金額。

3. 事件處理
•	btn_add_Click
•	驗證輸入資料（如項目名稱、金額）。
•	根據類型（收入或支出）處理金額正負號。
•	將新項目加入 expenses 列表和 DataGridView。
•	呼叫 DatabaseHelper.InsertExpense 將資料存入 SQLite 資料庫。
•	更新統計數據和預算狀態。
•	btn_update_Click
•	呼叫 UpdateStatistics() 和 UpdateBudgetStatus() 更新統計數據和預算狀態。
•	btn_SaveBudget_Click
•	儲存每個分類的預算金額到 budgetLimits 字典。
•	更新預算狀態。
•	calendarExpense_DateChanged
•	根據選擇的日期篩選記帳項目，並顯示在搜尋結果的 DataGridView。

4. 統計與分析
•	UpdateStatistics()
•	計算總收入、總支出和餘額。
•	更新對應的標籤（lbl_Income、lbl_Expense、lbl_balance）。
•	繪製支出類別的圓餅圖。
•	UpdateBudgetStatus()
•	根據每個分類的預算金額和實際支出，更新預算狀態。
•	如果超出預算，顯示警告訊息。

5. 資料庫操作
•	DatabaseHelper 類別
•	InitializeDatabase()
•	如果資料庫不存在，建立 SQLite 資料庫和 Expenses 資料表。
•	InsertExpense()
•	將記帳項目插入到資料庫。
•	LoadExpenses()
•	從資料庫載入所有記帳項目，並返回 Expense 列表。

6. 表單載入
•	Form1_Load
•	初始化資料庫。
•	從資料庫載入記帳項目到 expenses 列表。
•	將資料顯示在 DataGridView。
•	更新統計數據和預算狀態。

7. UI 功能模組
•	左側面板
•	包含輸入欄位（日期、項目名稱、類型、分類、金額）和按鈕（新增、更新統計）。
•	右側面板
•	使用 TabControl 分為四個頁籤：
1.	交易記錄：顯示所有記帳項目。
2.	支出分析：顯示支出類別的圓餅圖。
3.	預算設定：設定每個分類的預算金額。
4.	記錄查詢：根據日期篩選記帳項目。

總結
此程式是一個記帳應用程式，提供以下功能：
•	新增記帳項目。
•	設定分類預算。
•	分析支出類別比例。
•	查詢特定日期的記帳記錄。
•	資料持久化存儲於 SQLite 資料庫。



