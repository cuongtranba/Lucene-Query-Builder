using System.Collections.Generic;

namespace LuceneQueryBuilder
{
    public static class StackExtention
    {
        public static bool IsEmpty<T>(this Stack<T> stack)
        {
            if (stack.Count == 0)
            {
                return true;
            }
            return false;
        }
    }
}
