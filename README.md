# DumpStrings
Simple program which is dumping all strings referenced in methods from .NET file using dnlib

## Usage
Either drag & drop .NET binary or execute it like that:
>DumpStrings.exe [Path to .NET executable file]

## Example output
>[Program.Main] Failed on searching for method that returns what we want!
>[Program.Main] Patching method...
>[Program.Main] Saving to disk...
>[Program.Main] Saved @ {0}
>[Program.Main] Exception: {0}

## Used libraries
[dnlib](https://github.com/0xd4d/dnlib)