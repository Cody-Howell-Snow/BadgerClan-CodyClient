using BadgerClan.Logic;
using BadgerClan_CodyClient.API;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<Strategy>();
var app = builder.Build();

app.MapGet("/", () => "Arrived at Cody's API.");

app.MapPost("/", (MoveRequest request, Strategy bot) => {
    app.Logger.LogInformation("Received move request for game {gameId} turn {turnNumber}", request.GameId, request.TurnNumber);
    MoveResponse m = new MoveResponse(bot.PlanMoves(request));
    return m;
});

app.MapGet("/change/{value}", (string value, Strategy bot) => {
    app.Logger.LogInformation("Adjusted strategy to: " + value);
    bot.strategy = value;
});

app.Run();

