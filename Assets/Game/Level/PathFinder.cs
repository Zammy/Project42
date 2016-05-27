using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class PathFinder
{
    private class Step : IEquatable<Step>
    {
        public Point Pos;
        public int Heuristic;
        public int FromStart;

        public Step Parent = null;

        public Step()
        {
        }

        public Step(Point pos, int Heuristic, int FromStart)
        {
            this.Pos = pos;
            this.Heuristic = Heuristic;
            this.FromStart = FromStart;
        }

        public int Score { get { return Heuristic + FromStart; } }

        #region IEquatable implementation

        public bool Equals(Step other)
        {
            if (other == null)
                return false;

            return other.Pos == this.Pos;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("[Step ({0}) : Heuristic={1} FromStart={2} Score={3}]", this.Pos, this.Heuristic, this.FromStart, this.Score);
        }
    }

    static List<Step> closedList = new List<Step>();
    static List<Step> openList = new List<Step>();

    public static Point[] PathFromAtoB(Point start, Point goal)
    {
        //        Debug.LogFormat("======= {0} to {1} =======", start, goal);

        closedList.Clear();
        openList.Clear();

        openList.Add(new Step(start, (start - goal).Length, 0));

        Step stepGoal = null;
        while (true)
        {
            var lowestScoreStep = GetLowestScoreFromList(openList, goal);
            //            Debug.LogFormat("lowestScoreStep {0}", lowestScoreStep);
            if (lowestScoreStep == null)
            {
                throw new UnityException("Open list should never be empty!");
            }
            if (lowestScoreStep.Pos == goal)
            {
                //                Debug.Log("Found goal!");
                stepGoal = lowestScoreStep;
                break;
            }

            openList.Remove(lowestScoreStep);
            closedList.Add(lowestScoreStep);

            Step[] stepsAround = GetStepsAroundStep(lowestScoreStep, goal);
            foreach (var step in stepsAround)
            {
                if (step == null)
                {
                    continue;
                }

                if (closedList.Contains(step))
                {
                    continue;
                }

                var sameStep = FindStepInPos(openList, step.Pos);

                if (sameStep != null)
                {
                    if (sameStep.Score <= step.Score)
                    {
                        continue;
                    }

                    openList.Remove(sameStep);
                }

                //                Debug.Log("Adding to open list " + step.ToString());

                openList.Add(step);
            }
        }

        return ExtractPath(stepGoal);
    }

    static Step GetLowestScoreFromList(List<Step> steps, Point goal)
    {
        int minScore = int.MaxValue;
        Step bestStep = null;
        foreach (var step in steps)
        {
            if (step.Pos == goal)
                return step;

            // >= so that if we have steps with equal score we will add the most recent addition
            if (minScore >= step.Score)
            {
                minScore = step.Score;
                bestStep = step;
            }
        }
        return bestStep;
    }

    static Step[] GetStepsAroundStep(Step around, Point goal)
    {
        Level level = Level.Instance;

        Step[] steps = new Step[4];

        System.Func<Point, Step> addStep = (Point pos) =>
        {
            if (pos == goal)
            {
                return new Step()
                {
                    Pos = pos,
                    Heuristic = 0,
                    FromStart = around.FromStart + 1,
                    Parent = around
                };
            }

            if (!level.IsPassable(pos))
            {
                return null;
            }

            return new Step()
            {
                Pos = pos,
                Heuristic = (pos - goal).Length,
                FromStart = around.FromStart + 1,
                Parent = around
            };
        };

        steps[0] = addStep(new Point(around.Pos.X + 1, around.Pos.Y));
        steps[1] = addStep(new Point(around.Pos.X - 1, around.Pos.Y));
        steps[2] = addStep(new Point(around.Pos.X, around.Pos.Y + 1));
        steps[3] = addStep(new Point(around.Pos.X, around.Pos.Y - 1));

        return steps;
    }

    static Step FindStepInPos(List<Step> steps, Point pos)
    {
        foreach (var step in steps)
        {
            if (step.Pos == pos)
            {
                return step;
            }
        }

        return null;
    }

    static Point[] ExtractPath(Step step)
    {
        List<Point> path = new List<Point>();

        while (step != null && step.Parent != null)
        {
            path.Add(step.Pos);

            if (step.Parent != null)
            {
                step = step.Parent;
            }
        }

        path.Reverse();

        return path.ToArray();
    }
}

