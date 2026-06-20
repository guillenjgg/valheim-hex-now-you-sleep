# NowYouSleep

Similar to Enshrouded's sleep mechanic, NowYouSleep allows night to pass when at least one player is sleeping in Valheim multiplayer.

In vanilla multiplayer, every player must sleep before the night can be skipped.  
NowYouSleep changes that behavior so a single sleeping player can trigger Valheim's normal sleep-to-morning transition.

## Features

- Allows night to pass when at least one player is in bed
- Works in single player, peer-hosted multiplayer, and dedicated servers
- Uses Valheim's built-in sleep and morning skip system
- Server-authoritative multiplayer behavior
- Only the host/server needs the mod installed
- Configurable enable/disable setting
- Optional debug logging

---

## Multiplayer

NowYouSleep is designed to be server-authoritative.

This means the sleep check only runs on the world host or dedicated server.  
Installing the mod on a regular client does nothing by itself.

### Installation Requirements

| Environment | Install Required |
|---|---|
| Single Player | Player |
| Peer-Hosted Multiplayer | Host |
| Dedicated Server | Dedicated Server |
| Regular Clients | Not Required |

### Compatibility Examples

| Scenario | Result |
|---|---|
| Host has mod, clients do not | Works |
| Dedicated server has mod, clients do not | Works |
| Client has mod, host/server does not | Does not work |

### In-Game Behavior

When the host or dedicated server has NowYouSleep installed:

- Any connected player can enter a bed
- The server detects at least one sleeping player
- All connected players enter the normal sleep transition
- Night passes normally using Valheim's built-in morning skip system

This mod does not create a custom sleep system or force custom RPC behavior.  
It simply changes the vanilla sleep requirement from:

```text
Everyone must be sleeping
```

to:

```text
At least one player must be sleeping
```

---

## Requirements

- denikson-BepInExPack_Valheim-5.4.2333

---

## Configuration

Config file location:

```text
BepInEx/config/hex.nowyousleep.cfg
```

Example configuration:

```ini
[General]

## Enable or disable the mod
# Setting type: Boolean
# Default value: true
Enabled = true

## Enable debug logging
# Setting type: Boolean
# Default value: false
DebugMode = false
```

## Support and Feedback
Report bugs, request features, or provide feedback:

- Discord: https://discord.gg/wU2FXD94v4

## Github
- Source code: https://github.com/guillenjgg/valheim-hex-now-you-sleep