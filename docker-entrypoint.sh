#!/bin/ash
set -e

echo "-- Waiting for database ..."
while ! pg_isready -U ${POSTGRES_USER:-kamino} -d postgres://${POSTGRES_HOST:-pgsqldb}:${POSTGRES_PORT:-5432}/${POSTGRES_DB:-kamino} -t 1; do
    sleep 1s
done

echo "-- Starting!"
./Kamino.Public
