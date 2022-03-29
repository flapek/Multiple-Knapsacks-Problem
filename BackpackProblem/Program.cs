using BackpackProblem;
using Google.OrTools.LinearSolver;

DataModel data = new();

var capacities = Enumerable.Range(0, 8).Select(_ => 2000).ToArray();
var totalWeight = 0;
var lostWeight = 2000;
while (Math.Abs(totalWeight - data.TotalWeight) > 0.001 || lostWeight > 1000)
{
    lostWeight = 0;
    totalWeight = 0;
    var allItems = Enumerable.Range(0, data.NumItems).ToArray();
    var allBins = Enumerable.Range(0, data.NumBins).ToArray();
    var solver = Solver.CreateSolver("SCIP");

    var x = new Variable[data.NumItems, data.NumBins];
    foreach (var i in allItems)
    foreach (var b in allBins)
        x[i, b] = solver.MakeBoolVar($"x_{i}_{b}");

    foreach (var i in allItems)
    {
        var constraint = solver.MakeConstraint(0, 1, "");
        foreach (var b in allBins)
            constraint.SetCoefficient(x[i, b], 1);
    }

    foreach (var b in allBins)
    {
        var constraint = solver.MakeConstraint(0, capacities[b], "");
        foreach (var i in allItems)
            constraint.SetCoefficient(x[i, b], DataModel.Weights[i]);
    }

    var objective = solver.Objective();
    foreach (var i in allItems)
    foreach (var b in allBins)
        objective.SetCoefficient(x[i, b], 1);

    objective.SetMaximization();

    var resultStatus = solver.Solve();

    if (resultStatus != Solver.ResultStatus.OPTIMAL)
        Console.WriteLine("The problem does not have an optimal solution!");

    List<Bin> bins = new();
    foreach (var b in allBins)
    {
        var binWeight = 0;
        Dictionary<int, int> a = new();
        foreach (var i in allItems)
        {
            if (Math.Abs(x[i, b].SolutionValue() - 1) < 0.05)
            {
                binWeight += DataModel.Weights[i];
                a.Add(i, DataModel.Weights[i]);
            }
        }

        bins.Add(new Bin(capacities[b], a));
        lostWeight += capacities[b] - binWeight;
        totalWeight += binWeight;
    }

    if (data.BinCapacities.Length != 0 && lostWeight >= data.LostWeight)
    {
        capacities = ReloadCapacities(capacities, data.TotalWeight);
        continue;
    }
    
    data.LastBinCapacities(bins.ToArray(), lostWeight);
}

data.Display();
Console.WriteLine("Total packed weight: {0}", totalWeight);
Console.WriteLine("Total weight: {0}", data.TotalWeight);
Console.WriteLine("Lost weight: {0}", lostWeight);
Console.WriteLine("Bins: {0}", string.Join(", ", capacities));


int[] ReloadCapacities(int[] capacities, int totalWeight)
{
    var vehicleSizes = new[] {1000, 1500, 2000};
    do
    {
        capacities[Random.Shared.Next(0, capacities.Length - 1)] =
            vehicleSizes[Random.Shared.Next(0, vehicleSizes.Length - 1)];

        while (totalWeight >= capacities.Sum())
        {
            var min = capacities.Min();
            switch (min)
            {
                case 1000:
                    capacities[capacities.ToList().FindIndex(x => x == min)] = 1500;
                    break;
                case 1500:
                    capacities[capacities.ToList().FindIndex(x => x == min)] = 2000;
                    break;
            }
        }
    } while (totalWeight >= capacities.Sum());

    return capacities;
}