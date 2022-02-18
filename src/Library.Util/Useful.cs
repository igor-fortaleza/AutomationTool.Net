using System.Collections.Generic;

namespace Library.Useful
{
    class Useful
    {
        public void LoadBalance(List<string> original, List<string> modification)
        {
            lock (original)
            {
                int count = original.Count;
                for (int index = 0; index < count; ++index)
                {
                    if (!modification.Contains(original[index]))
                    {
                        original.Remove(original[index]);
                        count = original.Count;
                        --index;
                    }
                }
                foreach (string str in modification)
                {
                    if (!original.Contains(str))
                        original.Add(str);
                }
            }
        }
    }
}
