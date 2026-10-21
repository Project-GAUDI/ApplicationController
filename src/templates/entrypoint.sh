#! /bin/sh

cat application.info

exec dotnet ApplicationController.dll "$@"
