#!/bin/sh

set -e

sql_files=(
    "Pictures.sql"
    "Users.sql"
    "Rentals.sql"
    "Bookings.sql"
)

/opt/mssql/bin/sqlservr &

echo "Waiting for SQL Server to start..."
sleep 20

# Check if a known table exists
initialized=$(/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -h -1 -W -Q "SET NOCOUNT ON; SELECT COUNT(*) FROM sys.tables WHERE name = 'Users'" -C)
if [ $initialized = "1" ]; then
    echo "Skipping initialization"
else
    echo "Running SQL scripts..."
    for file in "${sql_files[@]}"; do
        echo "Running $file"
        /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -i "init/$file" -C
    done
fi

echo "Listing tables in the database:"
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -Q "SELECT name FROM sys.tables;" -C

wait
