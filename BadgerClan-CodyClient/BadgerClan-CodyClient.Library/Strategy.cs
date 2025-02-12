using BadgerClan.Logic;
using BadgerClan.Logic.Bot;

namespace BadgerClan_CodyClient.Library;

public class Strategy {
    public string strategy = "Consolidate";
    public int speed = 1;

    public List<Move> PlanMoves(MoveRequest request) {
        switch (strategy) {
            case "Attack": return RunAndGun(request);
            case "Consolidate": return Consolidate(request);
            case "Defend": return Defend(request);
            case "Swarm": return Swarm(request);
            case "Flee": return Flee(request);
            case "MoveUpLeft": return DirectMovement(request, "ul");
            case "MoveUpRight": return DirectMovement(request, "ur");
            case "MoveLeft": return DirectMovement(request, "l");
            case "MoveDownRight": return DirectMovement(request, "dr");
            case "MoveDownLeft": return DirectMovement(request, "dl");
            case "MoveRight": return DirectMovement(request, "r");

            default: throw new ArgumentException($"Strategy was {strategy}, and was invalid.");
        }
    }

    private List<Move> Flee(MoveRequest request) {
        var moves = new List<Move>();

        var enemies = request.Units.Where(x => x.Team != request.YourTeamId);
        foreach (var unit in request.Units.Where(x => x.Team == request.YourTeamId)) {
            var closest = enemies.OrderBy(u => u.Location.Distance(unit.Location)).FirstOrDefault();
            if (closest != null) {
                moves.Add(SharedMoves.StepAwayFromClosest(unit, closest, request));
            }
        }
        return moves;
    }

    private List<Move> DirectMovement(MoveRequest request, string direction) {
        var moves = new List<Move>();

        foreach (var unit in request.Units) {
            switch (direction) {
                case "ul": moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.MoveNorthWest(speed))); break;
                case "ur": moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.MoveNorthEast(speed))); break;
                case "l": moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.MoveWest(speed))); break;
                case "dr": moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.MoveSouthEast(speed))); break;
                case "dl": moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.MoveSouthWest(speed))); break;
                case "r": moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.MoveEast(speed))); break;
            }
        }
        return moves;
    }

    private List<Move> Consolidate(MoveRequest request) {
        var moves = new List<Move>();
        var units = request.Units.Where(x => x.Team == request.YourTeamId);
        var first = units.FirstOrDefault();

        foreach (var unit in request.Units.Where(x => x.Team == request.YourTeamId)) {
            if (unit.Equals(first)) continue;
            if (unit.Location.Distance(first.Location) <= 1) continue;

            moves.Add(SharedMoves.StepToClosest(unit, first, request));
        }
        return moves;
    }

    private List<Move> Defend(MoveRequest request) {
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
                }
            }
        }

        return moves;
    }

    /// <summary>
    /// Includes healing
    /// </summary>
    private List<Move> RunAndGun(MoveRequest request) {
        var moves = new List<Move>();

        var enemies = request.Units.Where(x => x.Team != request.YourTeamId);
        foreach (var unit in request.Units.Where(x => x.Team == request.YourTeamId)) {
            var closestandweak = enemies.OrderBy(u => u.Health).ThenBy(u => u.Location.Distance(unit.Location));
            var target = closestandweak.ToList()[unit.Id % 3];
            if (target != null) {
                if (target.Location.Distance(unit.Location) <= unit.AttackDistance) {
                    moves.Add(SharedMoves.AttackClosest(unit, target));
                    moves.Add(SharedMoves.AttackClosest(unit, target));
                } else if (request.Medpacs > 0 && unit.Health < unit.MaxHealth) {
                    moves.Add(new Move(MoveType.Medpac, unit.Id, unit.Location));
                } else {
                    moves.Add(SharedMoves.StepToClosest(unit, target, request));
                }
            }
        }
        return moves;
    }

    /// <summary>
    /// Most aggresive attacks on a single enemy
    /// </summary>
    private List<Move> Swarm(MoveRequest request) {
        var moves = new List<Move>();

        var enemies = request.Units.Where(x => x.Team != request.YourTeamId);
        var first = request.Units.Where(x => x.Team == request.YourTeamId).FirstOrDefault();
        var closest = enemies.OrderBy(u => u.Location.Distance(first.Location)).FirstOrDefault();
        foreach (var unit in request.Units.Where(x => x.Team == request.YourTeamId)) {
            if (closest != null) {
                if (closest.Location.Distance(unit.Location) <= unit.AttackDistance) {
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
