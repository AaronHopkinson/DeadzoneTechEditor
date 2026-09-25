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
