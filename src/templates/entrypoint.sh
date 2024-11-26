#! /bin/bash

cat application.info

exec dotnet ApplicationController.dll "$@"
