using Microsoft.EntityFrameworkCore;
using OotakiWebSample2.DTO;

namespace YourApp.Data
{
    /// <summary>
    /// アプリケーションのデータベースコンテキスト。
    /// Entity Framework Core を使用してデータベースへのアクセスを管理する。
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// AppDbContext のコンストラクター
        /// </summary>
        /// <param name="options">データベース接続設定を含むオプション</param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /// <summary>SQLクエリ結果をマッピングするDTO（主キーなし）</summary>
        public DbSet<ItemListDto> ItemListDto { get; set; }

        /// <summary>
        /// モデルの構成をカスタマイズする。
        /// </summary>
        /// <param name="modelBuilder">モデル構成に使用するビルダー</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ItemListDto はSQLクエリ結果のマッピング用のため主キーなしに設定
            modelBuilder.Entity<ItemListDto>().HasNoKey();
        }
    }
}