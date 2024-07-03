# MitzIT Discord Message Action

An action to send messages and reactions to a Discord text channel.

## Usage

```yaml
name: pull_request

on:
  pull_request:
    types: [ labeled ]

jobs:
  discord:
    runs-on: ubuntu-latest
    steps:
      - name: Send message to Discord
        uses: mitz-it/discord-message-action@v1
        with:
          token: ${{ secrets.DISCORD_BOT_TOKEN }}
          channel-id: ${{ secrets.DISCORD_CHANNEL_ID }}
          message-content: "Hello from GitHub"
```

## Adding Reactions

You can add an emote reaction to your text message

```yaml
name: pull_request

on:
  pull_request:
    types: [ labeled ]

env:
  EMOTE_NAME: smiley

jobs:
  discord:
    runs-on: ubuntu-latest
    steps:
      - name: Send message to Discord
        uses: mitz-it/discord-message-action@v1
        with:
          token: ${{ secrets.DISCORD_BOT_TOKEN }}
          channel-id: ${{ secrets.DISCORD_CHANNEL_ID }}
          message-content: "Hello from GitHub"
          emote-id: ${{ env.EMOTE_ID }}
          emote-name: ${{ env.EMOTE_NAME }}
          emote-animated: false
```
Or use with custom emojis from your server:

```yaml
name: pull_request

on:
  pull_request:
    types: [ labeled ]

env:
  EMOTE_ID: 1234567891011121314
  EMOTE_NAME: custom

jobs:
  discord:
    runs-on: ubuntu-latest
    steps:
      - name: Send message to Discord
        uses: mitz-it/discord-message-action@v1
        with:
          token: ${{ secrets.DISCORD_BOT_TOKEN }}
          channel-id: ${{ secrets.DISCORD_CHANNEL_ID }}
          message-content: "Hello from GitHub"
          emote-id: ${{ env.EMOTE_ID }}
          emote-name: ${{ env.EMOTE_NAME }}
          emote-animated: false
```