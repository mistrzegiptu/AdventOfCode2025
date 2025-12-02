class Program
{
    public static void Main()
    {
        const int startPosition = 50;
        const int maxPosition = 100;

        var inputData = File.ReadLines("../../../input.txt");

        int currentPosition = startPosition;
        int counter = 0;
        foreach(var line in inputData)
        {
            char direction = line[0];
            int rotations = Convert.ToInt32(line[1..]);

            if (direction == 'R')
            {
                counter += (currentPosition + rotations) / 100;
                currentPosition = (currentPosition + rotations) % maxPosition;
            }
            
            if(direction == 'L')
            {
                if(currentPosition == 0)
                {
                    counter += rotations / 100;
                }
                else if(rotations >= currentPosition)
                {
                    counter += 1 + (rotations - currentPosition) / maxPosition;
                }
                currentPosition = ((currentPosition - rotations) % maxPosition + maxPosition) % maxPosition; 
            }
        }

        Console.WriteLine(counter);
    }
}