namespace BackpackProblem;

internal class DataModel
{
    public static int[] Weights = {
        350, 350, 350, 350, 350, 350, 350, 350, 350, 350, 350, 350, 350, 350, 350, 350, 350, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123,
        123, 123, 123, 123, 123, 123, 123, 123, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87, 87
    };
    public int TotalWeight { get; }
    public int NumItems { get; }
    public int NumBins { get; private set; }
    public Bin[] BinCapacities { get; private set; } = Array.Empty<Bin>();
    public int LostWeight { get; private set; } = 0;
    public DataModel()
    {
        NumItems = Weights.Length;
        NumBins = 8;
        TotalWeight = Weights.Sum();
    }

    public void LastBinCapacities(Bin[] capacities, int lostWeight)
    {
        BinCapacities = capacities;
        NumBins = capacities.Length;
        LostWeight = lostWeight;
    }

    public void Display()
    {
        for (var i = 0; i < BinCapacities.Length; i++)
        {
            var binWeight = 0;
            Console.WriteLine("Bin " + i);
            foreach (var (key, value) in BinCapacities[i].Weights)
            {
                Console.WriteLine($"Item {key} weight: {value}");
                binWeight += value;
            }
            Console.WriteLine("Packed bin weight: " + binWeight);
        }
    }
    
}