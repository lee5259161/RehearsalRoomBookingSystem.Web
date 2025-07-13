# 練團室時數卡管理系統 (Rehearsal Room Booking System)

這是一個用於管理練團室時數卡的系統，專門為音樂工作室、排練室等場所設計。系統提供會員管理、練團卡時數控管、交易紀錄查詢等功能。

## 功能特點

- 👥 會員管理
  - 會員資料的新增、查詢、修改
  - 支援依電話號碼搜尋會員
  - 分頁顯示會員列表

- 💳 練團卡管理
  - 會員練團卡時數購買(以10小時為單位)
  - 練團卡時數使用紀錄
  - 剩餘時數查詢
  - 交易記錄回復功能

- 👤 管理員功能
  - 管理員登入驗證
  - 系統操作紀錄
 
## 系統畫面

- 會員資料列表
![會員資料列表圖片](https://github.com/user-attachments/assets/bde94b05-3ce3-436e-aa64-137af4297ae0)

- 練團卡時數交易明細
![練團卡時數交易明細圖片](https://github.com/user-attachments/assets/feff3fc8-5f20-4bb8-aac1-25f04724d8cd)

- 購買練團卡時數
![購買練團卡時數圖片](https://github.com/user-attachments/assets/2bff8a13-69c2-4d09-9778-1995d8f8dbfc)

- 使用練團卡時數
![使用練團卡時數圖片](https://github.com/user-attachments/assets/56c2d99b-d39a-4e7c-8066-5967684c495c)

- 新增會員資料
![新增會員資料圖片](https://github.com/user-attachments/assets/1ec075e1-2182-47e8-8fe0-5e84ca29515b)

- 修改會員資料
![修改會員資料圖片](https://github.com/user-attachments/assets/20089a64-797b-4748-a81d-4aba92b9623d)


## 技術架構

- **後端框架**：ASP.NET Core 6.0
- **資料庫**：SQLite

## 系統需求

- .NET 6.0 SDK
- Visual Studio 2022 或更新版本（建議）
- SQLite

## 安裝說明

1. 複製專案
```bash
git clone [repository-url]
cd RehearsalRoomBookingSystem.Web
```

2. 還原相依套件
```bash
dotnet restore
```

3. 執行專案
```bash
dotnet run
```

系統會自動建立 SQLite 資料庫檔案並進行初始化。

## 主要特色

- 🔐 安全性
  - Cookie 驗證機制
  - 密碼加密儲存
  - 存取權限控管

- 📊 效能與可靠性
  - Repository 模式確保資料存取效率
  - 交易紀錄完整追蹤
  - Serilog 日誌記錄

- 🎯 使用者體驗
  - 直覺的操作介面
  - 即時的資料驗證
  - 清晰的錯誤提示

## 系統架構

系統採用多層式架構設計：

- **表現層** (Web)
  - Controllers
  - Views
  - Models

- **服務層** (Service)
  - DTOs
  - 業務邏輯實作
  - 資料轉換

- **資料層** (Repository)
  - 資料存取
  - 實體定義
  - 查詢實作

- **共用層** (Common)
  - 輔助工具
  - 共用介面
  - 例外處理

## 授權協議

MIT License

Copyright (c) 2025 Ray Lee

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
