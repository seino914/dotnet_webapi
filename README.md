# TodoApi - ASP.NET Core Web API

このプロジェクトは、ASP.NET Core 8.0 を使用した Todo リスト管理 API です。JWT 認証を実装し、ユーザーごとの Todo 管理を可能にします。

## 機能

- ユーザー認証（JWT）
  - ユーザー登録
  - ログイン
- Todo リスト管理
  - Todo 一覧の取得
  - 個別 Todo の取得
  - Todo の作成
  - Todo の更新
  - Todo の削除

## 必要条件

- .NET 8.0 SDK
- SQL Server（LocalDB も可）
- Entity Framework Core Tools
  ```bash
  dotnet tool install --global dotnet-ef
  ```

## セットアップ

1. リポジトリのクローン

```bash
git clone <repository-url>
cd backend
```

2. 開発環境の設定

```bash
# appsettings.Development.jsonの作成
cp appsettings.Development.template.json appsettings.Development.json
```

必要に応じて`appsettings.Development.json`の設定を編集してください。

3. 依存関係のインストール

```bash
dotnet restore
```

4. データベースの更新

```bash
# マイグレーションの作成（初回のみ）
dotnet ef migrations add InitialCreate

# データベースの更新
dotnet ef database update
```

5. アプリケーションの実行

```bash
dotnet run
```

## API エンドポイント

### 認証

- POST /api/auth/register

  - 新規ユーザー登録

  ```json
  {
    "username": "string",
    "password": "string"
  }
  ```

- POST /api/auth/login
  - ログイン
  ```json
  {
    "username": "string",
    "password": "string"
  }
  ```

### Todo 管理（要認証）

- GET /api/todo

  - すべての Todo を取得

- GET /api/todo/{id}

  - 特定の Todo を取得

- POST /api/todo

  - 新しい Todo を作成

  ```json
  {
    "title": "string",
    "description": "string",
    "isCompleted": false
  }
  ```

- PUT /api/todo/{id}

  - Todo を更新

  ```json
  {
    "title": "string",
    "description": "string",
    "isCompleted": boolean
  }
  ```

- DELETE /api/todo/{id}
  - Todo を削除

## 認証

API は JWT Bearer トークン認証を使用します。`/api/auth/login`エンドポイントで取得したトークンを、以降のリクエストの Authorization ヘッダーに含める必要があります：

```
Authorization: Bearer <your-token>
```

## 開発環境

- Visual Studio Code または Visual Studio 2022
- Swagger UI: http://localhost:5065/swagger

## トラブルシューティング

### データベース接続エラー

- SQL Server が実行されていることを確認してください
- `appsettings.Development.json`の接続文字列が正しいことを確認してください

### マイグレーションエラー

- Entity Framework Core Tools がインストールされていることを確認してください
- 以下のコマンドでマイグレーションをリセットできます：
  ```bash
  dotnet ef database drop
  dotnet ef migrations remove
  dotnet ef migrations add InitialCreate
  dotnet ef database update
  ```
