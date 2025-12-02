const int startPosition = 50;
const int maxPosition = 100;

var inputData = File.ReadLines("../../../input.txt");

//Part One
Console.WriteLine(inputData.Select(line => (direction: line[0], rotations: Convert.ToInt32(line[1..]))).
    Aggregate((position: startPosition, zeroes: 0), (x, next) => next.direction == 'R' ?
        ((x.position + next.rotations) % maxPosition, x.zeroes + (x.position == 0 ? 1 : 0)) :
        ((x.position - next.rotations % maxPosition + maxPosition) % maxPosition, x.zeroes + (x.position == 0 ? 1 : 0))).zeroes);

//Part Two
Console.WriteLine(inputData.Select(line => (direction: line[0], rotations: Convert.ToInt32(line[1..]))).
    Aggregate((position: startPosition, zeroes: 0), (x, next) => next.direction == 'R' ?
        ((x.position + next.rotations) % maxPosition, x.zeroes + ((x.position + next.rotations) / maxPosition)) :
        (((x.position - next.rotations) % maxPosition + maxPosition) % maxPosition, x.zeroes + (x.position == 0 ? (next.rotations / maxPosition) : (next.rotations >= x.position ? (1 + (next.rotations - x.position) / maxPosition) : 0)))).zeroes);