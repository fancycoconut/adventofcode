// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

Part1("sample.txt");
Part1("input.txt");

void Part1(string filename)
{
    var camelHands = File.ReadLines(filename)
        .Select(x => new CamelHand(x))
        .ToArray();
    
    Array.Sort(camelHands);
    
    //DumpHands($"{Directory.GetCurrentDirectory()}/input-dump.txt", camelHands);
    
    var totalWinnings = GetTotalWinnings(camelHands);
    
    Console.WriteLine($"Part 1 - the total winnings are {totalWinnings}");

}

void DumpHands(string filename, CamelHand[] camelHands)
{
    using var writer = new StreamWriter(filename);
    foreach (var camelHand in camelHands)
    {
        writer.WriteLine(camelHand.Dump());
    }
}

int GetTotalWinnings(CamelHand[] camelHands)
{
    var sum = 0;
    for (var i = 1; i <= camelHands.Length; i++)
    {
        var camelHand = camelHands[i - 1];
        var winnings = i * camelHand.GetBid();
        sum += winnings;
    }

    return sum;
}

public class CamelHand : IComparable<CamelHand>
{
    private readonly string _hand;
    private readonly int _bid;
    private readonly Dictionary<char, int> _map;
    private readonly Lazy<HandType> _handType;

    public CamelHand(string raw)
    {
        var parts = raw.Split(" ");
        _hand = parts[0];
        _bid = int.Parse(parts[1]);
        _map = GetCardsCount(_hand.AsSpan());
        _handType = new Lazy<HandType>(InitializeHandType());
    }

    public int CompareTo(CamelHand? other)
    {
        if (other is null) return 1;

        var handType = (int)GetHandType();
        var otherHandType = (int)other.GetHandType();
        if (handType < otherHandType) return -1;
        if (handType == otherHandType)
        {
            var hand = GetHand().AsSpan();
            var otherHand = other.GetHand().AsSpan();
            
            for (var i = 0; i < 5; i++)
            {
                var cardValue = GetCardValue(hand[i]);
                var otherCardValue = GetCardValue(otherHand[i]);
                
                if (cardValue == otherCardValue) continue;
                if (cardValue < otherCardValue) return -1;
                return 1;
            }
            
            return 0;
        }
        
        return 1;
    }

    public int GetBid() => _bid;

    public string GetHand() => _hand;

    public string Dump() => $"{_hand} {_handType.Value}";

    private static int GetCardValue(char card)
    {
        return card switch
        {
            '2' => 2,
            '3' => 3,
            '4' => 4,
            '5' => 5,
            '6' => 6,
            '7' => 7,
            '8' => 8,
            '9' => 9,
            'T' => 10,
            'J' => 11,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
            _ => 0
        };
    }

    private HandType GetHandType() => _handType.Value;

    private HandType InitializeHandType()
    {
        if (IsOfAKind(5)) return HandType.FiveOfAKind;
        if (IsOfAKind(4)) return HandType.FourOfAKind;
        if (IsFullHouse()) return HandType.FullHouse;
        if (IsOfAKind(3)) return HandType.ThreeOfAKind;
        if (IsTwoPair()) return HandType.TwoPair;
        if (IsOnePair()) return HandType.OnePair;

        return HandType.HighCard;
    }

    private bool IsOfAKind(int kind)
    {
        var topCount = 0;
        switch (kind)
        {
            case 5:
                return _map.Values.Count == 1;
            case 4:
                topCount = _map.OrderByDescending(x => x.Value)
                    .Select(x => x.Value)
                    .First();
                return topCount == 4;
            case 3:
                topCount = _map.OrderByDescending(x => x.Value)
                    .Select(x => x.Value)
                    .First();
                return topCount == 3 && _map.Values.Count == 3;
            default:
                throw new InvalidOperationException($"{kind} or a kind is unsupported!");
        }
    }

    private bool IsFullHouse()
    {
        if (_map.Values.Count != 2) return false;

        var sortedCards = _map.OrderByDescending(x => x.Value)
            .Select(x => x.Value)
            .ToArray();

        return sortedCards[0] == 3 && sortedCards[1] == 2;
    }

    private bool IsTwoPair()
    {
        if (_map.Values.Count != 3) return false;
        
        var sortedCards = _map.OrderByDescending(x => x.Value)
            .Select(x => x.Value)
            .ToArray();

        return sortedCards[0] == 2 && sortedCards[1] == 2 && sortedCards[2] == 1;
    }
    
    private bool IsOnePair()
    {
        if (_map.Values.Count != 4) return false;

        var sortedCards = _map.OrderByDescending(x => x.Value)
            .Select(x => x.Value);

        return sortedCards.First() == 2;
    }

    private static Dictionary<char, int> GetCardsCount(ReadOnlySpan<char> hand)
    {
        var map = new Dictionary<char, int>();
        foreach (var card in hand)
        {
            if (map.ContainsKey(card))
                map[card] += 1;
            else
                map[card] = 1;
        }

        return map;
    }
}

public enum HandType
{
    Unknown = 0,
    HighCard = 1,
    OnePair = 2,
    TwoPair = 3,
    ThreeOfAKind = 4,
    FullHouse = 5,
    FourOfAKind = 6,
    FiveOfAKind = 7
}