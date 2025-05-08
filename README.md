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

## 系統畫面
-  **會員資料列表**
![會員資料列表](https://private-user-images.githubusercontent.com/57717770/441742713-d490de18-dbfd-49b7-9f75-6345f931903f.jpg?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3NDY3MTQ1MTgsIm5iZiI6MTc0NjcxNDIxOCwicGF0aCI6Ii81NzcxNzc3MC80NDE3NDI3MTMtZDQ5MGRlMTgtZGJmZC00OWI3LTlmNzUtNjM0NWY5MzE5MDNmLmpwZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNTA1MDglMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjUwNTA4VDE0MjMzOFomWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPTk4NGYzZmU0NzcwODE2OWY4Y2MzMzc1MjdjM2M0ODNiMDgyMTBiZmEyMWY0ZDUyYzg1YjAxM2M3OTczM2ZmODUmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0In0.Kk-9-fAfFGS7vECFvR327c53PS-wMCyZh5-9t3tZbUs)

-  **使用練團卡時數**
![使用練團卡時數](https://private-user-images.githubusercontent.com/57717770/441750114-b2b55380-1c2c-4aac-96fd-fcf0077f4e41.jpg?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3NDY3MTQ4ODUsIm5iZiI6MTc0NjcxNDU4NSwicGF0aCI6Ii81NzcxNzc3MC80NDE3NTAxMTQtYjJiNTUzODAtMWMyYy00YWFjLTk2ZmQtZmNmMDA3N2Y0ZTQxLmpwZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNTA1MDglMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjUwNTA4VDE0Mjk0NVomWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPWU4MWNhOWIyZDJjNGJlMDYzMzkwZGFhYWFiNzFiMzE0Nzc3ODNhNDlmMTQ2ZjA5NzA0NjJmZWExMmY4MTY2YzkmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0In0.64K9UPHnOl9i94LW2Uz2K_hhELZL67BJqnIqS36N_k4)

-  **購買練團卡時數**
![購買練團卡時數](https://private-user-images.githubusercontent.com/57717770/441754101-04a8ba54-fd8d-4b86-bed6-2cf7dc21ae2e.jpg?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3NDY3MTUyNjgsIm5iZiI6MTc0NjcxNDk2OCwicGF0aCI6Ii81NzcxNzc3MC80NDE3NTQxMDEtMDRhOGJhNTQtZmQ4ZC00Yjg2LWJlZDYtMmNmN2RjMjFhZTJlLmpwZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNTA1MDglMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjUwNTA4VDE0MzYwOFomWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPTlmZjNiNDgwOGIxYjg0MTgxNWE2NmQwYzEwZTUyNDgxMDg4YmMxODk1NzMxMWE0NmY0MWUyMTZlYjJlM2NhOTUmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0In0.1bOuBuTwZsm6PbGBEQn-DPlnF2qXecz2OZB4nZw5Xbo)

-  **練團卡時數交易明細**
![練團卡時數交易明細](https://private-user-images.githubusercontent.com/57717770/441754395-a7ebeac9-1dce-4d08-8d10-a3cbe00a94c2.jpg?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3NDY3MTUzMDcsIm5iZiI6MTc0NjcxNTAwNywicGF0aCI6Ii81NzcxNzc3MC80NDE3NTQzOTUtYTdlYmVhYzktMWRjZS00ZDA4LThkMTAtYTNjYmUwMGE5NGMyLmpwZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNTA1MDglMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjUwNTA4VDE0MzY0N1omWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPWRlZGFjNzRmYTcxYzUyYzJkYjA4MTQ1YjE3MmRmODIyY2RlNTY0NjdiMjRjZDAxMjE0MzI4MDM5NDRmMTQ4MmMmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0In0.s574hVRwnXyniLptjnahoUJoj-itmfBj5Pq68VPN1Us)

-  **修改會員資料**
![修改會員資料](https://private-user-images.githubusercontent.com/57717770/441754596-1addf73c-7372-4f82-9c81-b0b171b37866.jpg?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3NDY3MTUzMzcsIm5iZiI6MTc0NjcxNTAzNywicGF0aCI6Ii81NzcxNzc3MC80NDE3NTQ1OTYtMWFkZGY3M2MtNzM3Mi00ZjgyLTljODEtYjBiMTcxYjM3ODY2LmpwZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNTA1MDglMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjUwNTA4VDE0MzcxN1omWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPTg3YWYxMmQ0ODUzMjg2YTcwMmQ1ZTRiNmI1ZDQ1MzVlOTBkNmU0NGJlMWM1MjZjNTMwMjhiOGQwYzQ0OWU5ZmQmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0In0.rnaHC_LJusoQSjaZER4b3x6IaioMrDQPXSS6kdNMciY)

-  **新增會員資料**
![新增會員資料](https://private-user-images.githubusercontent.com/57717770/441754868-8acb2598-52ab-497e-ace3-98483f91b1b5.jpg?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3NDY3MTUzODYsIm5iZiI6MTc0NjcxNTA4NiwicGF0aCI6Ii81NzcxNzc3MC80NDE3NTQ4NjgtOGFjYjI1OTgtNTJhYi00OTdlLWFjZTMtOTg0ODNmOTFiMWI1LmpwZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNTA1MDglMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjUwNTA4VDE0MzgwNlomWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPTVmNTI0NzA2NzhlMWY5MTAyMDIyMDQ3MzU0OThmNzU0MzY3ZWUzZGM3NmI5YzI0MjMzOWYwYzAzMjgwMTNiYWEmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0In0.3ErvnW9gTqvJQZHPV16GJeiOLcsi9SXW8lPTLF5E_gU)

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
