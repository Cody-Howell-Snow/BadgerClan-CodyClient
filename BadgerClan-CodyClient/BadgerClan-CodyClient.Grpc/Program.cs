using ProtoBuf.Grpc.Server;
using BadgerClan_CodyClient.Library;
using BadgerClan.Logic;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<Strategy>();
builder.Services.AddCodeFirstGrpc();
var app = builder.Build();

app.MapGrpcService<StrategyChanger>();

app.MapGet("/", () => "At Cody's GRPC API.");
app.MapPost("/", (MoveRequest request, Strategy bot) => {
    MoveResponse m = new MoveResponse(bot.PlanMoves(request));
    return m;
});

app.Run();


public class StrategyChanger(Strategy s, ILogger<StrategyChanger> logger) : IStrategyChanger {
    public Task<StringResponse> StrategyChange(StringRequest request) {
        s.strategy = request.NewStrat;
        logger.LogInformation("Changed strat to " + request.NewStrat);

        return Task.FromResult(new StringResponse {
            NewStrat = request.NewStrat,
            stratChanged = true
        });
    }
}