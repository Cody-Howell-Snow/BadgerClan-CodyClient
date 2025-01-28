using BadgerClan.Logic;
using BadgerClan.Logic.Bot;

namespace BadgerClan_CodyClient.API;

public class Strategy {
    public string strategy = "nomovement";

    public List<Move> PlanMoves(MoveRequest request) {
        switch (strategy) {
            case "RunAndGun": return RunAndGun(request);
            case "nomovement": return new List<Move>();
            case "MoveUp": return DirectMovement(request, "up");
            case "MoveLeft": return DirectMovement(request, "left");
            case "MoveDown": return DirectMovement(request, "down");
            case "MoveRight": return DirectMovement(request, "right");

            default: return new List<Move>();
        }
    }

    private List<Move> DirectMovement(MoveRequest request, string direction) {
        var moves = new List<Move>();

        foreach (var unit in request.Units) {
            switch (direction) {
                case "up": moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Toward(unit.Location + new Coordinate(0, 1)))); break;
                case "left": moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Toward(unit.Location + new Coordinate(-1, 0)))); break;
                case "down": moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Toward(unit.Location + new Coordinate(0, -1)))); break;
                case "right": moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Toward(unit.Location + new Coordinate(1, 0)))); break;
            }
        }
        return moves;
    }

    private List<Move> RunAndGun(MoveRequest request) {
        var moves = new List<Move>();
        foreach (var unit in request.Units) {
            var enemies = request.Units;
            var closest = enemies.OrderBy(u => u.Location.Distance(unit.Location)).FirstOrDefault();
            if (closest != null) {

                if (closest.Location.Distance(unit.Location) <= unit.AttackDistance) {
                    moves.Add(SharedMoves.AttackClosest(unit, closest));
                    moves.Add(SharedMoves.AttackClosest(unit, closest));
                } else if (request.Medpacs > 0 && unit.Health < unit.MaxHealth) {
                    moves.Add(new Move(MoveType.Medpac, unit.Id, unit.Location));
                } else {
                    moves.Add(SharedMoves.StepToClosest(unit, closest, request));
                }
            }
        }
        return moves;
    }
}
