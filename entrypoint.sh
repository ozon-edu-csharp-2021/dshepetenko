#!/bin/bash

set -e
run_cmd="dotnet MerchandiseService.dll --no-build -v d"

dotnet MerchandiseService.Migrator.dll --no-build -v d -- --dryrun

dotnet MerchandiseService.Migrator.dll --no-build -v d

>&2 echo "MerchandiseService DB Migrations complete, starting app."
>&2 echo "Run MerchandiseService: $run_cmd"
exec $run_cmd