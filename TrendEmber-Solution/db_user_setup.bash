#!/bin/bash

# Check if correct number of arguments is passed
if [ "$#" -ne 3 ]; then
    echo "Usage: $0 <db_name> <db_user> <db_password>"
    exit 1
fi

# Set database variables from command line arguments
DB_NAME=$1
DB_USER=$2
DB_PASSWORD=$3

# Check if PostgreSQL is running, and if not, start it
if ! pg_isready -h localhost -p 5432; then
    echo "PostgreSQL is not running. Starting it..."
    # You can start PostgreSQL here if necessary, depending on the OS
    # Example for macOS (if installed via Homebrew):
    brew services start postgresql
fi

# Connect to PostgreSQL as the superuser (postgres) and execute commands
echo "Setting up database and user..."

# Create database and user if they don't exist
psql -U postgres <<EOF
DO \$\$ BEGIN
    CREATE DATABASE $DB_NAME;
    EXCEPTION WHEN duplicate_table THEN RAISE NOTICE 'Database $DB_NAME already exists.';
END \$\$;

DO \$\$ BEGIN
    CREATE USER $DB_USER WITH PASSWORD '$DB_PASSWORD';
    EXCEPTION WHEN duplicate_object THEN RAISE NOTICE 'User $DB_USER already exists.';
END \$\$;

GRANT ALL PRIVILEGES ON DATABASE $DB_NAME TO $DB_USER;
EOF

echo "Database $DB_NAME and user $DB_USER created successfully."
