// See https://aka.ms/new-console-template for more information
using System.Linq;

const string OpponentRock = "A";
const string OpponentPaper = "B";
const string OpponentScissors = "C";
const string YouRock = "X";
const string YouPaper = "Y";
const string YouScissors = "Z";
const string YouLose = "X";
const string YouDraw = "Y";
const string YouWin = "Z";

Console.WriteLine("Hello, World!");

Func<string, int> GetOverallScoreV1 = outcome => {
  var playedActions = outcome.Split(" ");

  return (playedActions[0], playedActions[1]) switch {
    (OpponentRock, YouScissors) => 0,
    (OpponentRock, YouRock) => 3,
    (OpponentRock, YouPaper) => 6,
    (OpponentPaper, YouScissors) => 6,
    (OpponentPaper, YouRock) => 0,
    (OpponentPaper, YouPaper) => 3,
    (OpponentScissors, YouScissors) => 3,
    (OpponentScissors, YouRock) => 6,
    (OpponentScissors, YouPaper) => 0,
    _ => throw new InvalidOperationException("Invalid actions played!")
  };
};

Func<string, int> GetOverallScoreV2 = outcome => {
  var playedActions = outcome.Split(" ");

  return (playedActions[0], playedActions[1]) switch {
    (OpponentRock, YouLose) => 0,
    (OpponentRock, YouDraw) => 3,
    (OpponentRock, YouWin) => 6,
    (OpponentPaper, YouLose) => 0,
    (OpponentPaper, YouDraw) => 3,
    (OpponentPaper, YouWin) => 6,
    (OpponentScissors, YouLose) => 0,
    (OpponentScissors, YouDraw) => 3,
    (OpponentScissors, YouWin) => 6,
    _ => throw new InvalidOperationException("Invalid actions played!")
  };
};

Func<string, int> GetScoreV1 = outcome => {
  var playedActions = outcome.Split(" ");
  var overallScore = GetOverallScoreV1(outcome);

  return playedActions[1] switch {
    YouRock => 1 + overallScore,
    YouPaper => 2 + overallScore,
    YouScissors => 3 + overallScore,
    _ => throw new InvalidOperationException("You played an invalid shape - please learn the rules")
  };
};

Func<string, int> GetScoreV2 = outcome => {
  var playedActions = outcome.Split(" ");
  var overallScore = GetOverallScoreV2(outcome);

  var playedShapeScore = (playedActions[0], playedActions[1]) switch {
    (OpponentRock, YouLose) => 3,
    (OpponentRock, YouDraw) => 1,
    (OpponentRock, YouWin) => 2,
    (OpponentPaper, YouLose) => 1,
    (OpponentPaper, YouDraw) => 2,
    (OpponentPaper, YouWin) => 3,
    (OpponentScissors, YouLose) => 2,
    (OpponentScissors, YouDraw) => 3,
    (OpponentScissors, YouWin) => 1,
    _ => throw new InvalidOperationException("Invalid actions played!")
  };

  return playedShapeScore + overallScore;
};


var outcomes = File.ReadAllLines("input.txt");

var totalScore = outcomes.Select(x => GetScoreV2(x))
  .Sum();

Console.WriteLine(totalScore);
