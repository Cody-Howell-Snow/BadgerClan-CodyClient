using BadgerClan.Logic;
using BadgerClan.Logic.Bot;

namespace BadgerClan_CodyClient.API;

public class Strategy {
    public string strategy = "Consolidate";

    public List<Move> PlanMoves(MoveRequest request) {
        switch (strategy) {
            case "Attack": return RunAndGun(request);
            case "Consolidate": return Consolidate(request);
            case "MoveUpLeft": return DirectMovement(request, "ul");
            case "MoveUpRight": return DirectMovement(request, "ur");
            case "MoveLeft": return DirectMovement(request, "l");
            case "MoveDownRight": return DirectMovement(request, "dr");
            case "MoveDownLeft": return DirectMovement(request, "dl");
            case "MoveRight": return DirectMovement(request, "r");

            default: throw new ArgumentException($"Strategy was {strategy}, and was invalid.");
        }
    }

    private List<Move> DirectMovement(MoveRequest request, string direction) {
        var moves = new List<Move>();

        foreach (var unit in request.Units) {
            switch (direction) {
                case "ul": moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Toward(unit.Location + new Coordinate(0, -1)))); break;
                case "ur": moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Toward(unit.Location + new Coordinate(1, -1)))); break;
                case "l": moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Toward(unit.Location + new Coordinate(-1, 0)))); break;
                case "dr": moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Toward(unit.Location + new Coordinate(0, 1)))); break;
                case "dl": moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Toward(unit.Location + new Coordinate(-1, 1)))); break;
                case "r": moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Toward(unit.Location + new Coordinate(1, 0)))); break;
            }
        }
        return moves;
    }

    private List<Move> Consolidate(MoveRequest request) {
        var moves = new List<Move>();

        return moves;
    }

    private List<Move> RunAndGun(MoveRequest request) {
        var moves = new List<Move>();

        var enemies = request.Units.Where(x => x.Team != request.YourTeamId);
        foreach (var unit in request.Units.Where(x => x.Team == request.YourTeamId)) {
            var closest = enemies.OrderBy(u => u.Location.Distance(unit.Location)).FirstOrDefault();
            if (closest != null) {
                if (request.Medpacs > 0 && unit.Health < unit.MaxHealth) {
                    moves.Add(new Move(MoveType.Medpac, unit.Id, unit.Location));
                } else if (closest.Location.Distance(unit.Location) <= unit.AttackDistance) {
                    moves.Add(SharedMoves.AttackClosest(unit, closest));
                    moves.Add(SharedMoves.AttackClosest(unit, closest));
                } else {
                    moves.Add(SharedMoves.StepToClosest(unit, closest, request));
                }
            }
        }
        return moves;
    }
}
