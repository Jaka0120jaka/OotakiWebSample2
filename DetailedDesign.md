# OotakiWebSample2 詳細設計書

## 1. システム概要

受付一覧データの表示・非表示を制御するASP.NET Core MVCアプリケーション。
ユーザーごとに項目の表示/非表示設定をデータベースに保存し、画面上でリアルタイムに切り替えることができる。

## 2. 技術スタック

| 項目 | 内容 |
|---|---|
| フレームワーク | ASP.NET Core MVC (.NET 8.0) |
| ORM | Entity Framework Core 8.0 |
| データベース | PostgreSQL |
| DBプロバイダー | Npgsql.EntityFrameworkCore.PostgreSQL 8.0.4 |
| フロントエンド | Bootstrap 5, jQuery |
| 通信方式 | Ajax (Fetch API) |

## 3. アーキテクチャ

3層構造（Controller → Service → Repository）を採用し、DIコンテナによる依存性注入で疎結合を実現する。

```
[View (Razor)] → [Controller] → [Service (IService)] → [Repository (IItemRepository)] → [DB (PostgreSQL)]
```

### 3.1 DI登録（Program.cs）

| サービス | 実装 | ライフタイム |
|---|---|---|
| IService | Service | Scoped |
| IItemRepository | ItemRepository | Scoped |
| AppDbContext | - | Scoped（AddDbContext既定） |

## 4. データベース設計

### 4.1 テーブル一覧

| テーブル名 | 用途 |
|---|---|
| hyouji_set_tbl | ユーザーごとの項目表示設定テーブル |
| item_info_mst | 項目情報マスター |
| item_list_mst | 項目値一覧マスター |

### 4.2 テーブル定義（SQLクエリから推定）

#### hyouji_set_tbl（表示設定テーブル）

| カラム名 | 用途 |
|---|---|
| scr_id | 画面ID |
| user_id | ユーザーID |
| item_id | 項目ID（item_info_mst への外部キー） |
| visible_flg | 表示フラグ（true: 表示 / false: 非表示） |

#### item_info_mst（項目情報マスター）

| カラム名 | 用途 |
|---|---|
| item_id | 項目ID（主キー） |
| item_name | 項目名 |

#### item_list_mst（項目値一覧マスター）

| カラム名 | 用途 |
|---|---|
| item_id | 項目ID（item_info_mst への外部キー） |
| value | 項目の値 |

### 4.3 テーブル関連図

```
hyouji_set_tbl.item_id ──┐
                         ├── item_info_mst.item_id
item_list_mst.item_id ───┘
```

## 5. クラス設計

### 5.1 ディレクトリ構成

```
OotakiWebSample2/
├── Controllers/
│   ├── HomeController.cs      # メインコントローラー
│   ├── IService.cs            # サービスインターフェース
│   └── Service.cs             # サービス実装
├── Data/
│   └── AppDbContext.cs        # EF Core DbContext
├── DTO/
│   └── ItemListDto.cs         # SQLクエリ結果マッピング用DTO
├── Models/
│   ├── ScreenViewModel.cs     # 画面ビューモデル / ListViewModel
│   ├── ListModalModel.cs      # モーダル用ビューモデル
│   ├── UpdateVisibleModel.cs  # 更新リクエストモデル
│   └── ErrorViewModel.cs      # エラー画面モデル
├── Repository/
│   ├── IItemRepository.cs     # リポジトリインターフェース
│   └── ItemRepository.cs      # リポジトリ実装
├── SqlQueries/
│   └── SqlQueries.cs          # SQL定数クラス
├── Views/
│   ├── Home/Index.cshtml      # メイン画面
│   └── Shared/
│       ├── _Layout.cshtml     # レイアウト
│       └── _ListModal.cshtml  # 表示設定モーダル（部分ビュー）
├── wwwroot/
│   ├── css/style.css          # カスタムスタイル
│   ├── css/site.css           # サイト共通スタイル
│   └── js/site.js             # 共通JS（未使用）
└── Program.cs                 # エントリーポイント・DI設定
```

### 5.2 クラス詳細

#### 5.2.1 HomeController

| 項目 | 内容 |
|---|---|
| 名前空間 | OotakiWebSample2.Controllers |
| 依存 | IService（コンストラクタインジェクション） |

| メソッド | HTTP | 戻り値 | 説明 |
|---|---|---|---|
| Index() | GET | Task\<IActionResult\> | 画面ID=1, ユーザーID=1 で画面データを取得し、ScreenViewModel をビューに渡す |
| UpdateVisible(UpdateVisibleRequest) | POST | Task\<IActionResult\> | JSON リクエストを受け取り、表示フラグをDB更新。成功時 200 OK を返す |
| Error() | GET | IActionResult | エラー画面を表示 |

#### 5.2.2 IService / Service

| 項目 | 内容 |
|---|---|
| 名前空間 | OotakiWebSample2.Controllers |
| 依存 | IItemRepository（コンストラクタインジェクション） |

| メソッド | 引数 | 戻り値 | 説明 |
|---|---|---|---|
| GetScreenAsync | screenId: int, userId: int | Task\<ScreenViewModel\> | リポジトリからDTO一覧を取得し、ItemName でグループ化して ScreenViewModel に変換する |
| UpdateVisibleFlagAsync | visibleFlg: bool, itemName: string, screenId: int, userId: int | Task | リポジトリの更新処理に委譲する |

##### GetScreenAsync 処理フロー

1. IItemRepository.GetScreenItemsAsync で ItemListDto のリストを取得
2. ItemName でグループ化（GroupBy）
3. 各グループを ListViewModel に変換
   - ItemName: グループキー
   - Values: グループ内の全 Value をリスト化
   - VisibleFlag: グループ内先頭レコードの VisibleFlag を使用
4. ScreenViewModel に格納して返却

#### 5.2.3 IItemRepository / ItemRepository

| 項目 | 内容 |
|---|---|
| 名前空間 | OotakiWebSample2.Repository |
| 依存 | AppDbContext（コンストラクタインジェクション） |

| メソッド | SQL定数 | EF Core メソッド | 説明 |
|---|---|---|---|
| GetScreenItemsAsync | GetScreenList | FromSqlRaw + AsNoTracking + ToListAsync | 画面ID・ユーザーIDに対応する項目一覧を読み取り専用で取得 |
| UpdateVisibleFlagAsync | UpdateVisibleFlag | ExecuteSqlRawAsync | 表示フラグを直接UPDATE文で更新 |

#### 5.2.4 AppDbContext

| 項目 | 内容 |
|---|---|
| 名前空間 | YourApp.Data |
| 基底クラス | DbContext |
| 接続先 | appsettings.json の ConnectionStrings:DefaultConnection（PostgreSQL） |

| DbSet | 型 | 主キー |
|---|---|---|
| ItemListDto | ItemListDto | なし（HasNoKey） |

#### 5.2.5 SqlQueryConstants

| 定数名 | 種別 | パラメーター | 説明 |
|---|---|---|---|
| GetScreenList | SELECT | {0}: scr_id, {1}: user_id | hyouji_set_tbl, item_info_mst, item_list_mst を結合して項目一覧を取得 |
| UpdateVisibleFlag | UPDATE | {0}: visible_flg, {1}: item_name, {2}: scr_id, {3}: user_id | hyouji_set_tbl の visible_flg を更新（item_info_mst と結合して item_name で特定） |

### 5.3 モデル / DTO

#### ItemListDto（DTO）

| プロパティ | 型 | 説明 |
|---|---|---|
| ItemName | string? | 項目名 |
| Value | string? | 項目の値 |
| VisibleFlag | bool | 表示フラグ |

#### ScreenViewModel

| プロパティ | 型 | 説明 |
|---|---|---|
| ListViewModels | List\<ListViewModel\>? | 画面に表示する項目リスト |

#### ListViewModel

| プロパティ | 型 | 説明 |
|---|---|---|
| ItemName | string? | 項目名 |
| Values | List\<string?\>? | 項目に紐づく値のリスト |
| VisibleFlag | bool | 表示フラグ |

#### ListModalModel

| プロパティ | 型 | 説明 |
|---|---|---|
| ListViewModels | List\<ListViewModel\>? | モーダル内に表示する項目リスト |
| ListModal | string? | モーダル識別子 |

#### UpdateVisibleRequest

| プロパティ | 型 | 説明 |
|---|---|---|
| VisibleFlg | bool | 更新後の表示フラグ |
| ItemName | string | 対象項目名（既定値: ""） |
| ScreenId | int | 画面ID |
| UserId | int | ユーザーID |

## 6. 画面設計

### 6.1 Index 画面（Home/Index.cshtml）

#### 画面構成

```
┌──────────────────────────────────────┐
│                          [表示設定]  │
│ ┌──────────────────────────────────┐ │
│ │ 項目A  │ 項目B  │ 項目C  │ ...  │ │  ← VisibleFlag=true の項目のみ表示
│ ├────────┼────────┼────────┼──────┤ │
│ │ 値1    │ 値1    │ 値1    │ ...  │ │
│ │ 値2    │ 値2    │ 値2    │ ...  │ │
│ └──────────────────────────────────┘ │
│         （横スクロール可能）          │
└──────────────────────────────────────┘
```

#### 表示ロジック

- VisibleFlag が true の項目のみテーブルのヘッダー・データセルとして描画
- 行数は全項目の Values の最大件数（Max）で決定
- 各セルに data-col 属性を付与し、JS での列表示切替に使用

### 6.2 表示設定モーダル（_ListModal.cshtml）

#### 画面構成

```
┌─────────────────────────────────┐
│ 表示設定                    [×] │
│                                 │
│ ☑ 項目A  ☑ 項目B  ☐ 項目C     │  ← 全項目をチェックボックスで表示
│ ☑ 項目D  ☐ 項目E  ...         │
│                                 │
│              [適用]  [閉じる]   │
└─────────────────────────────────┘
```

#### 動作仕様

- チェックボックスの初期状態は VisibleFlag に連動
- data-item 属性に ItemName を保持
- チェックボックスIDは `chk_` + ItemName（スペースは `_` に置換）

## 7. 処理フロー

### 7.1 画面初期表示

```
ブラウザ → GET /Home/Index
         → HomeController.Index()
         → Service.GetScreenAsync(1, 1)
         → ItemRepository.GetScreenItemsAsync(1, 1)
         → SQL: GetScreenList 実行
         → List<ItemListDto> 返却
         → ItemName でグループ化 → ScreenViewModel 生成
         → Index.cshtml 描画（VisibleFlag=true の項目のみテーブル表示）
         → _ListModal.cshtml 部分ビュー描画
         → HTML レスポンス返却
```

### 7.2 表示設定変更（適用ボタン押下）

```
ユーザー: モーダルで表示設定を変更し「適用」ボタンを押下

JS処理:
  1. 全チェックボックスをループ
  2. 各チェックボックスについて:
     a. data-col 属性で対応するテーブル列の display を切替（即時反映）
     b. fetch で POST /Home/UpdateVisible に Ajax リクエスト送信
        Body: { visibleFlg, itemName, screenId: 1, userId: 1 }
  3. 全リクエスト完了を Promise.all で待機
  4. モーダルを閉じる

サーバー処理:
  → HomeController.UpdateVisible(request)
  → Service.UpdateVisibleFlagAsync(...)
  → ItemRepository.UpdateVisibleFlagAsync(...)
  → SQL: UpdateVisibleFlag 実行
  → 200 OK 返却

モーダル閉じ後:
  → hidden.bs.modal イベントで location.reload() 実行
  → 画面全体をリロードして最新の表示状態を反映
```

## 8. ルーティング

| パターン | コントローラー | アクション | HTTP メソッド |
|---|---|---|---|
| / | Home | Index | GET |
| /Home/Index | Home | Index | GET |
| /Home/UpdateVisible | Home | UpdateVisible | POST |
| /Home/Error | Home | Error | GET |

既定ルート: `{controller=Home}/{action=Index}/{id?}`

## 9. CSS設計

### style.css 主要クラス

| クラス | 用途 |
|---|---|
| .page-wrapper | ページ全体の中央寄せ・余白設定（margin-top: 300px） |
| .table-wrapper | テーブルコンテナ（最大幅 1200px） |
| .btn-settings | 表示設定ボタン（テーブル右上に絶対配置） |
| .scroll-area | 横スクロール領域 |
| .custom-table | テーブル本体（固定幅レイアウト、列幅 200px） |
| .btn-primary | カスタムボタンカラー（#73a9dd ベース） |

### テーブルスタイル

- 列幅固定: 200px（min-width / max-width）
- ヘッダー背景色: #9bbee0（sticky 固定）
- 偶数行: #f2f2f2（グレー）、奇数行: #ffffff（白）
- テキスト省略: white-space: nowrap + text-overflow: ellipsis

## 10. 設定ファイル

### appsettings.json

| キー | 値 |
|---|---|
| Logging:LogLevel:Default | Information |
| Logging:LogLevel:Microsoft.AspNetCore | Warning |
| AllowedHosts | * |
| ConnectionStrings:DefaultConnection | （環境ごとに設定） |

### ミドルウェアパイプライン（Program.cs）

1. UseExceptionHandler（本番環境のみ）
2. UseHsts（本番環境のみ）
3. UseHttpsRedirection
4. UseStaticFiles
5. UseRouting
6. UseAuthorization
7. MapControllerRoute
