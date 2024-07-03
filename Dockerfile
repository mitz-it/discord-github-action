FROM mcr.microsoft.com/dotnet/sdk:8.0 as build-env

WORKDIR /app
COPY . ./
RUN dotnet publish ./src/MitzIT.Actions.DiscordMessage/MitzIT.Actions.DiscordMessage.csproj -c Release -o out --no-self-contained

LABEL maintainer="MitzIT <luiz.motta@mitzit.com.br>"
LABEL repository="https://github.com/mitz-it/discord-message-action"
LABEL homepage="https://github.com/mitz-it/discord-message-action"
LABEL com.github.actions.name="MitzIT Discord Message"
LABEL com.github.actions.description="A GitHub action that sends a message to a Discord text channel"
LABEL com.github.actions.icon="message-circle"
LABEL com.github.actions.color="gray-dark"

FROM mcr.microsoft.com/dotnet/sdk:8.0
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "/MitzIT.Actions.DiscordMessage.dll"]