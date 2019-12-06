using System.Collections.Generic;

namespace PriestsAndDevils
{
    public class AI
    {
        // 指明游戏初始的状态。
        public static AIState startState = new AIState(0, 0, 3, 3, false, null);
        // 指明游戏终止的状态。
        public static AIState endState = new AIState(3, 3, 0, 0, true, null);
        // 指明当前状态。
        private AIState currentState;

        // 默认构造函数。
        public AI()
        {
            currentState = startState;
        }

        // 重置 AI 。
        public void Reset()
        {
            currentState = startState;
        }

        // 更新状态。
        public void Update(int leftPriests, int leftDevils, int rightPriests, int rightDevils, bool location)
        {
            currentState = new AIState(leftPriests, leftDevils, rightPriests, rightDevils, location, null);
        }

        // 计算下一步。
        public AIState Hint()
        {
            // 使用 BFS 广度搜索寻找可行解。
            Queue<AIState> found = new Queue<AIState>();
            Queue<AIState> visited = new Queue<AIState>();
            AIState temp = new AIState(currentState.leftPriests, currentState.leftDevils, currentState.rightPriests, currentState.rightDevils, currentState.location, null);
            found.Enqueue(temp);

            while (found.Count > 0)
            {
                temp = found.Peek();

                if (temp == endState)
                {
                    while (temp != null && temp.parent != currentState)
                    {
                        temp = temp.parent;
                    }
                    return temp;
                }

                found.Dequeue();
                visited.Enqueue(temp);

                // 当船在左岸时。
                if (temp.location)
                {
                    // 尝试移动左岸的 1 个牧师至右岸。
                    if (temp.leftPriests > 0)
                    {
                        AIState next = new AIState(temp);
                        next.parent = new AIState(temp);
                        next.location = false;
                        next.leftPriests--;
                        next.rightPriests++;
                        // 尝试移动。
                        if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                        {
                            found.Enqueue(next);
                        }
                    }
                    // 尝试移动左岸的 1 个魔鬼至右岸。
                    if (temp.leftDevils > 0)
                    {
                        AIState next = new AIState(temp);
                        next.parent = new AIState(temp);
                        next.location = false;
                        next.leftDevils--;
                        next.rightDevils++;
                        // 尝试移动。
                        if (next.Valid() && !visited.Contains(next) && !found.Contains(next))
                        {
                            found.Enqueue(next);
                        }
                    }
                    // 尝试移动左岸的 2 个牧师至右岸。
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
                    // 尝试移动左岸的 2 个魔鬼至右岸。
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
                    // 尝试移动左岸的 1 个牧师和 1 个魔鬼至右岸。
                    if (temp.leftPriests > 0 && temp.leftDevils > 0)
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
                }
                else
                {
                    // 尝试移动右岸的 1 个牧师至左岸。
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
                    // 尝试移动左岸的 1 个魔鬼至右岸。
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
                    // 尝试移动右岸的 2 个牧师至左岸。
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
                    // 尝试移动右岸的 2 个牧师至左岸。
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
                    // 尝试移动右岸的 1 个牧师和 1 个魔鬼至左岸。
                    if (temp.rightPriests > 0 && temp.rightDevils > 0)
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
                }
            }
            return endState;
        }
    }

    public class AIState
    {
        // 指明左岸的牧师数量。
        public int leftPriests;
        // 指明左岸的魔鬼数量。
        public int leftDevils;
        // 指明右岸的牧师数量。
        public int rightPriests;
        // 指明右岸的魔鬼数量。
        public int rightDevils;
        // 指明船的位置：true 表示在左岸；false 表示在右岸。
        public bool location;
        public AIState parent;

        // 默认构造函数。
        public AIState()
        {
            leftDevils = 0;
            leftPriests = 0;
            rightDevils = 0;
            rightPriests = 0;
            // 设置父状态。
            parent = null;
        }

        // 自定义构造函数。
        public AIState(int leftPriests, int leftDevils, int rightPriests, int rightDevils, bool boat, AIState parent)
        {
            this.leftPriests = leftPriests;
            this.leftDevils = leftDevils;
            this.rightPriests = rightPriests;
            this.rightDevils = rightDevils;
            this.location = boat;
            this.parent = parent;
        }

        // 拷贝构造函数。
        public AIState(AIState another)
        {
            leftPriests = another.leftPriests;
            leftDevils = another.leftDevils;
            rightPriests = another.rightPriests;
            rightDevils = another.rightDevils;
            location = another.location;
            parent = another.parent;
        }

        // 判断当前状态是否可行。
        public bool Valid()
        {
            return ((leftPriests == 0 || leftPriests >= leftDevils) &&
                (rightPriests == 0 || rightPriests >= rightDevils));
        }

        // 重写 Equals 函数。
        public override bool Equals(object obj)
        {
            // 判断 obj 是否为空类型。
            if (obj == null)
            {
                return false;
            }
            // 判断类型是否相同。
            if (obj.GetType().Equals(this.GetType()) == false)
            {
                return false;
            }
            // 判断成员是否相等。
            AIState temp = (AIState)obj;
            return leftPriests.Equals(temp.leftPriests) &&
                leftDevils.Equals(temp.leftDevils) &&
                rightDevils.Equals(temp.rightDevils) &&
                rightPriests.Equals(temp.rightPriests) &&
                location.Equals(temp.location);
        }

        // 配合 Equals 函数，重写 GetHashCode 函数。
        public override int GetHashCode()
        {
            // 我们使用各成员的 HashCode 的累加和作为 AIState 的 HashCode 。
            return leftDevils.GetHashCode() + leftPriests.GetHashCode() +
                rightDevils.GetHashCode() + rightPriests.GetHashCode() +
                location.GetHashCode();
        }

        // 重载 == 操作符。
        public static bool operator ==(AIState lhs, AIState rhs)
        {
            if (AIState.ReferenceEquals(lhs, rhs))
            {
                return true;
            }
            if (AIState.ReferenceEquals(lhs, null))
            {
                return false;
            }
            return lhs.Equals(rhs);
        }

        // 重载 != 操作符。
        public static bool operator !=(AIState lhs, AIState rhs)
        {
            return !(lhs == rhs);
        }
    }
}