﻿using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace BBS.Interfaces
{
    public interface IDatabase
    {
        public SqliteConnection SqLiteConnection();
    }
}
