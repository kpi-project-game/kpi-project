using Microsoft.Data.Sqlite;
using KPI_PROJECT.Models;

namespace KPI_PROJECT.Database;

public class DatabaseManager
{
    private string _connectionString = "Data Source=game.db";

    public void InitDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        
        command.CommandText = @"
            PRAGMA foreign_keys = ON;
            CREATE TABLE IF NOT EXISTS Users (
                TelegramId INTEGER PRIMARY KEY,
                Nickname TEXT,
                RegistrationDate DATETIME DEFAULT CURRENT_TIMESTAMP
            );

            CREATE TABLE IF NOT EXISTS Characters (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                TelegramId INTEGER,
                Class TEXT,
                Level INTEGER DEFAULT 1,
                MaxHp INTEGER,
                HP INTEGER,
                HandDmg INTEGER,
                PhisDefense INTEGER,
                BasePhisDefense INTEGER,
                MagicPower INTEGER,
                IsAlive INTEGER DEFAULT 1,
                FOREIGN KEY(TelegramId) REFERENCES Users(TelegramId)
            );";
        command.ExecuteNonQuery();
    }

    public void EnsureUserExists(long tgId, string name)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "INSERT OR IGNORE INTO Users (TelegramId, Nickname) VALUES ($id, $name)";
        command.Parameters.AddWithValue("$id", tgId);
        command.Parameters.AddWithValue("$name", name);
        command.ExecuteNonQuery();
    }

    public void SaveCharacter(Character p)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Characters (TelegramId, Class, MaxHp, HP, HandDmg, PhisDefense, BasePhisDefense, MagicPower) 
            VALUES ($tgId, $class, $maxHp, $hp, $dmg, $def, $bDef, $mp)";
        
        command.Parameters.AddWithValue("$tgId", p.TelegramId);
        command.Parameters.AddWithValue("$class", p.Class);
        command.Parameters.AddWithValue("$maxHp", p.MaxHp);
        command.Parameters.AddWithValue("$hp", p.Hp);
        command.Parameters.AddWithValue("$dmg", p.HandDmg);
        command.Parameters.AddWithValue("$def", p.PhisDefense);
        command.Parameters.AddWithValue("$bDef", p.BasePhisDefense);
        command.Parameters.AddWithValue("$mp", p.MagicPower);
        
        command.ExecuteNonQuery();
    }
    
    public Character? GetActiveCharacter(long telegramId)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var command = connection.CreateCommand();
    
        command.CommandText = "SELECT * FROM Characters WHERE TelegramId = $tgId AND IsAlive = 1 LIMIT 1";
        command.Parameters.AddWithValue("$tgId", telegramId);
        

        using var reader = command.ExecuteReader();
    
        if (reader.Read())
        {
            return new Character
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                TelegramId = reader.GetInt64(reader.GetOrdinal("TelegramId")),
                Class = reader.GetString(reader.GetOrdinal("Class")),
                Level = reader.GetInt32(reader.GetOrdinal("Level")),
                Hp = reader.GetInt32(reader.GetOrdinal("HP")),
                MaxHp = reader.GetInt32(reader.GetOrdinal("MaxHp")),
                HandDmg = reader.GetInt32(reader.GetOrdinal("HandDmg")),
                PhisDefense = reader.GetInt32(reader.GetOrdinal("PhisDefense")),
                BasePhisDefense = reader.GetInt32(reader.GetOrdinal("BasePhisDefense")),
                MagicPower = reader.GetInt32(reader.GetOrdinal("MagicPower"))
            };
        }
    
        return null;
    }
}