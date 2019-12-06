using System.Collections.Generic;

namespace PriestsAndDevils
{
    public class AI
    {
        public static AIState startState = new AIState(0, 0, 3, 3, false, null);
        public static AIState endState = new AIState(3, 3, 0, 0, true, null);
        private AIState currentState;

        public AI()
        {
            currentState = startState;
        }

        public void Reset()
        {
            currentState = startState;
        }

        public void Update(int leftPriests, int leftDevils, int rightPriests, int rightDevils, bool location)
        {
            currentState = new AIState(leftPriests, leftDevils, rightPriests, rightDevils, location, null);
        }

        public AIState Hint()
        {
            Queue<AIState> found = new Queue<AIState>();
            Queue<AIState> visited = new Queue<AIState>();
            AIState temp = new AIState(currentState.leftPriests, currentState.leftDevils, currentState.rightPriests, currentState.rightDevils, currentState.location, null);
            found.Enqueue(temp);

            while (found.Count > 0)
            {
                temp = found.Peek();

                if (temp == endState)
                {
                    //Debug.Log("solution path:\n");
                    while (temp.parent != currentState)
                    {
                        temp = temp.parent;
                    }
                    return temp;
                }

                found.Dequeue();
                visited.Enqueue(temp);

                // next node
                if (temp.location)
                {
                    // one move to right
                    if (temp.leftPriests > 0)
                    {
                        AIState next = new AIState(temp);
                        next.parent = new AIState(temp);
                        next.location = false;
                        next.leftPriests--;
                        next.rightPriests++;
                        if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                        {
                            found.Enqueue(next);
                        }
                    }
                    if (temp.leftDevils > 0)
                    {
                        AIState next = new AIState(temp);
                        next.parent = new AIState(temp);
                        next.location = false;
                        next.leftDevils--;
                        next.rightDevils++;
                        if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                        {
                            found.Enqueue(next);
                        }
                    }
                    // two moves to right
                    if (temp.leftDevils > 0 && temp.leftDevils > 0)
                    {
                        AIState next = new AIState(temp);
                        next.parent = new AIState(temp);
                        next.location = false;
                        next.leftPriests--;
                        next.leftDevils--;
                        next.rightPriests++;
                        next.rightDevils++;
                        if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                        {
                            found.Enqueue(next);
                        }
                    }
                    if (temp.leftDevils > 1)
                    {
                        AIState next = new AIState(temp);
                        next.parent = new AIState(temp);
                        next.location = false;
                        next.leftDevils -= 2;
                        next.rightDevils += 2;
                        if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                        {
                            found.Enqueue(next);
                        }
                    }
                    if (temp.leftPriests > 1)
                    {
                        AIState next = new AIState(temp);
                        next.parent = new AIState(temp);
                        next.location = false;
                        next.leftPriests -= 2;
                        next.rightPriests += 2;
                        next.parent = new AIState(temp);
                        if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                        {
                            found.Enqueue(next);
                        }
                    }
                }
                else
                {
                    //one move to left
                    if (temp.rightPriests > 0)
                    {
                        AIState next = new AIState(temp);
                        next.parent = new AIState(temp);
                        next.location = true;
                        next.rightPriests--;
                        next.leftPriests++;
                        if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                        {
                            found.Enqueue(next);
                        }
                    }
                    if (temp.rightDevils > 0)
                    {
                        AIState next = new AIState(temp);
                        next.parent = new AIState(temp);
                        next.location = true;
                        next.rightDevils--;
                        next.leftDevils++;
                        if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                        {
                            found.Enqueue(next);
                        }
                    }
                    //two moves to left
                    if (temp.rightDevils > 0 && temp.rightDevils > 0)
                    {
                        AIState next = new AIState(temp);
                        next.parent = new AIState(temp);
                        next.location = true;
                        next.rightPriests--;
                        next.rightDevils--;
                        next.leftPriests++;
                        next.leftDevils++;
                        if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                        {
                            found.Enqueue(next);
                        }
                    }
                    if (temp.rightDevils > 1)
                    {
                        AIState next = new AIState(temp);
                        next.parent = new AIState(temp);
                        next.location = true;
                        next.rightDevils -= 2;
                        next.leftDevils += 2;
                        if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                        {
                            found.Enqueue(next);
                        }
                    }
                    if (temp.rightPriests > 1)
                    {
                        AIState next = new AIState(temp);
                        next.parent = new AIState(temp);
                        next.location = true;
                        next.rightPriests -= 2;
                        next.leftPriests += 2;
                        if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                        {
                            found.Enqueue(next);
                        }
                    }
                }
            }
            return null;
        }
    }

    public class AIState
    {
        public int leftPriests;
        public int leftDevils;
        public int rightPriests;
        public int rightDevils;
        public bool location;
        public AIState parent;

        public AIState()
        {
            leftDevils = 0;
            leftPriests = 0;
            rightDevils = 0;
            rightPriests = 0;
            parent = null;
        }

        public AIState(int leftPriests, int leftDevils, int rightPriests, int rightDevils, bool boat, AIState parent)
        {
            this.leftPriests = leftPriests;
            this.leftDevils = leftDevils;
            this.rightPriests = rightPriests;
            this.rightDevils = rightDevils;
            this.location = boat;
            this.parent = parent;
        }

        public AIState(AIState another)
        {
            leftPriests = another.leftPriests;
            leftDevils = another.leftDevils;
            rightPriests = another.rightPriests;
            rightDevils = another.rightDevils;
            location = another.location;
            parent = another.parent;
        }

        public static bool operator ==(AIState lhs, AIState rhs)
        {
            return (lhs.leftPriests == rhs.leftPriests && lhs.leftDevils == rhs.leftDevils &&
                lhs.rightPriests == rhs.rightPriests && lhs.rightDevils == rhs.rightDevils &&
                lhs.location == rhs.location);
        }

        public static bool operator !=(AIState lhs, AIState rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType().Equals(this.GetType()) == false)
            {
                return false;
            }
            AIState temp = (AIState)obj;
            return leftPriests.Equals(temp.leftPriests) &&
                leftDevils.Equals(temp.leftDevils) &&
                rightDevils.Equals(temp.rightDevils) &&
                rightPriests.Equals(temp.rightPriests) &&
                location.Equals(temp.location);
        }

        public override int GetHashCode()
        {
            return leftDevils.GetHashCode() + leftPriests.GetHashCode() +
                rightDevils.GetHashCode() + rightPriests.GetHashCode() +
                location.GetHashCode();
        }

        public bool Valid()
        {
            return ((leftPriests == 0 || leftPriests >= leftDevils) &&
                (rightPriests == 0 || rightPriests >= rightDevils));
        }
    }
}