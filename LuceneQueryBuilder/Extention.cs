using System.Collections.Generic;

namespace LuceneQueryBuilder
{
    public static class Extention
    {
        public static bool IsEmpty<T>(this Stack<T> stack)
        {
            if (stack.Count == 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsEmpty(this List<string> listSyntax )
        {
            if (listSyntax.Count==0)
            {
                return true;
            }
            return false;
        }

        public static void PopLast(this List<string> listSyntax)
        {
            listSyntax.RemoveAt(listSyntax.Count-1);
        }

        public static string PopFirst(this List<string> listSyntax)
        {
            var value = listSyntax[0];
            listSyntax.RemoveAt(0);
            return value;
        }

        public static string PeekLast(this List<string> listSyntax)
        {
            return listSyntax[listSyntax.Count - 1];
        }
    }
}
