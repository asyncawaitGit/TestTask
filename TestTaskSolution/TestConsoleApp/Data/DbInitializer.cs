using Microsoft.Extensions.Configuration;
using Npgsql;
using Sms.Test;

namespace TestConsoleApp;

public sealed class DbInitializer
{
    private readonly string _cs;

    public DbInitializer(IConfiguration cfg)
    {
        _cs = cfg.GetConnectionString("Postgres")
              ?? throw new InvalidOperationException("Connection string Postgres not found");
    }

    public async Task InitAsync()
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync();

        var sql = """
        CREATE TABLE IF NOT EXISTS menu_items (
            id TEXT PRIMARY KEY,
            article TEXT NOT NULL,
            name TEXT NOT NULL,
            price NUMERIC
        );
        """;

        await using var cmd = new NpgsqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task InsertMenuAsync(IEnumerable<MenuItem> items)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync();

        await using var tx = await conn.BeginTransactionAsync();

        var sql = """
        INSERT INTO menu_items (id, article, name, price)
        VALUES (@id, @article, @name, @price)
        ON CONFLICT (id) DO UPDATE SET
            article = EXCLUDED.article,
            name    = EXCLUDED.name,
            price   = EXCLUDED.price;
        """;

        foreach (var item in items)
        {
            await using var cmd = new NpgsqlCommand(sql, conn, tx);

            cmd.Parameters.AddWithValue("id", item.Id);
            cmd.Parameters.AddWithValue("article", item.Article);
            cmd.Parameters.AddWithValue("name", item.Name);
            cmd.Parameters.AddWithValue(
                "price",
                (object?)item.Price ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        await tx.CommitAsync();
    }
}