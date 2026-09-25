# Deadzone Tech Editor

A small Windows save editor for **Deadzone: Rogue** that allows you to change your Tech currency.

Built in C# using WinForms.

## Features

- Select a Deadzone: Rogue `.sav` file
- Displays the current Tech amount
- Enter a new Tech value
- Automatically creates a `.bak` backup before editing
- Recalculates the save file's CRC32 checksum
- Verifies the modified save after writing

## Usage

1. Close Deadzone: Rogue.
2. Run `DeadzoneTechEditor.exe`.
3. Click **Select Save**.
4. Choose your save file.
5. Enter the desired Tech amount.
6. Click **Change Your Tech!**
7. Launch the game and verify the new value.

Deadzone: Rogue save files are normally located at:
%LOCALAPPDATA%\Valhalla\Saved\SaveGames

Backup
The editor automatically creates a backup alongside the selected save:
YourSave.sav.bak

If anything goes wrong, restore the backup by removing .bak from the filename.
Compatibility
Currently tested against the Deadzone: Rogue save format available in September 2026.
The editor currently relies on known offsets within the save format, so future game updates may require an update to this tool.
Technical Notes
The editor:
- reads the Tech value as a little-endian 32-bit integer
- modifies the Tech value directly in the save data
- recalculates the CRC32 checksum covering the save payload
- writes the corrected checksum back into the save
- reloads the written file and verifies the checksum before reporting success

Disclaimer
This is an unofficial fan-made utility and is not affiliated with the developers or publishers of Deadzone: Rogue.
Always keep backups of your save files
