using Priority_Queue;
using System;
using System.Linq;

namespace SlidingPuzzleIDAStar
{
    public static class IDAStar
    {
        public static State CreateState()
        {
            string input = Console.ReadLine();

            Int32.TryParse(input, out int N);

            int[,] board = new int[N, N];

            for (int i = 0; i < N; i++)
            {
                string inputValue = Console.ReadLine();

                for (int j = 0; j < N; j++)
                {
                    string element = inputValue.Split(' ')[j];
                    Int32.TryParse(element, out int value);
                    board[i, j] = value;
                }
            }

            Console.WriteLine();

            return new State(N, board);
        }

        public static void GetChildStates(State state, SimplePriorityQueue<State> priorityQueue)
        {
            Tuple<int, int> emptyTile = state.FindEmptyTile();

            if (emptyTile.Item1 + 1 < state.BoardSize)
            {
                int[,] child = new int[state.BoardSize, state.BoardSize];

                for (int i = 0; i < state.BoardSize; i++)
                {
                    for (int j = 0; j < state.BoardSize; j++)
                    {
                        if (i == emptyTile.Item1 && j == emptyTile.Item2)
                        {
                            child[i, j] = state.Board[i + 1, j];
                            child[i + 1, j] = state.Board[i, j];
                        }

                        else
                        {
                            if (i == emptyTile.Item1 + 1 && j == emptyTile.Item2)
                            {
                                continue;
                            }
                            else
                            {
                                child[i, j] = state.Board[i, j];
                            }
                        }
                    }
                }

                State childState = new State(state.BoardSize, child)
                {
                    Distance = state.Distance + 1,
                    PathDirections = state.PathDirections.Select(move => new Move()).ToList()
                };

                childState.PathDirections.Add(Move.Up);

                priorityQueue.Enqueue(childState, childState.Weight);
            }

            if (emptyTile.Item1 - 1 >= 0)
            {
                int[,] child = new int[state.BoardSize, state.BoardSize];

                for (int i = 0; i < state.BoardSize; i++)
                {
                    for (int j = 0; j < state.BoardSize; j++)
                    {
                        if (i == emptyTile.Item1 && j == emptyTile.Item2)
                        {
                            child[i, j] = state.Board[i - 1, j];
                            child[i - 1, j] = state.Board[i, j];
                        }
                        else
                        {
                            child[i, j] = state.Board[i, j];
                        }
                    }
                }

                State childState = new State(state.BoardSize, child)
                {
                    Distance = state.Distance + 1,
                    PathDirections = state.PathDirections.Select(move => new Move()).ToList()
                };

                childState.PathDirections.Add(Move.Down);

                priorityQueue.Enqueue(childState, childState.Weight);
            }

            if (emptyTile.Item2 + 1 < state.BoardSize)
            {
                int[,] child = new int[state.BoardSize, state.BoardSize];

                for (int i = 0; i < state.BoardSize; i++)
                {
                    for (int j = 0; j < state.BoardSize; j++)
                    {
                        if (i == emptyTile.Item1 && j == emptyTile.Item2)
                        {
                            child[i, j] = state.Board[i, j + 1];
                            child[i, j + 1] = state.Board[i, j];
                        }
                        else
                        {
                            if (i == emptyTile.Item1 && j == emptyTile.Item2 + 1)
                            {
                                continue;
                            }
                            else
                            {
                                child[i, j] = state.Board[i, j];
                            }
                        }
                    }
                }

                State childState = new State(state.BoardSize, child)
                {
                    Distance = state.Distance + 1,
                    PathDirections = state.PathDirections.Select(move => new Move()).ToList()
                };

                childState.PathDirections.Add(Move.Left);

                priorityQueue.Enqueue(childState, childState.Weight);
            }

            if (emptyTile.Item2 - 1 >= 0)
            {
                int[,] child = new int[state.BoardSize, state.BoardSize];

                for (int i = 0; i < state.BoardSize; i++)
                {
                    for (int j = 0; j < state.BoardSize; j++)
                    {
                        if (i == emptyTile.Item1 && j == emptyTile.Item2)
                        {
                            child[i, j] = state.Board[i, j - 1];
                            child[i, j - 1] = state.Board[i, j];
                        }

                        else
                        {
                            child[i, j] = state.Board[i, j];
                        }
                    }
                }

                State childState = new State(state.BoardSize, child)
                {
                    Distance = state.Distance + 1,
                    PathDirections = state.PathDirections.Select(move => new Move()).ToList()
                };

                childState.PathDirections.Add(Move.Right);

                priorityQueue.Enqueue(childState, childState.Weight);
            }
        }

        public static void IDAS(State initialState, int size)
        {
            SimplePriorityQueue<State> priorityQueue = new SimplePriorityQueue<State>();

            priorityQueue.Enqueue(initialState, initialState.Weight);

            while (priorityQueue.Count != 0)
            {
                priorityQueue.TryDequeue(out State currentState);

                int h = currentState.CalculateHeuristic();

                if (h == 0)
                {
                    currentState.PrintState();

                    Console.WriteLine($"\nDistance from the goal state -> {currentState.Distance}");

                    foreach (var movement in currentState.PathDirections)
                    {
                        Console.WriteLine(movement.ToString());
                    }

                    Console.WriteLine($"Steps count -> {currentState.PathDirections.Count}");

                    break;
                }
                else
                {
                    GetChildStates(currentState, priorityQueue);
                }
            }
        }
    }
}
