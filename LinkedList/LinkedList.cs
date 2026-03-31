namespace LinkedList
{
    /// <summary>
    /// This is a basic linked list. It was developed as a proof of concept to
    /// explore how a linked list would be coded in C#.
    /// </summary>
    /// <typeparam name="Tk">Type of the keys for the list. Must be a comparable type.</typeparam>
    /// <typeparam name="Tv">Type of the values that will be stored in the list.</typeparam>
    public class LinkedList<Tk, Tv> where Tk : IComparable<Tk>
    {
        private LinkedListEntry? firstEntry;
        private LinkedListEntry? lastEntry;
        private int count;

        public void Add(Tk key, Tv value)
        {
            LinkedListEntry? entry = new LinkedListEntry()
            {
                Key = key,
                Value = value
            };

            if (firstEntry == null)
            {
                // This is the first entry, so mark it as first and last and we are done
                firstEntry = entry;
                lastEntry = entry;
                count++;
                return;
            }

            // Walk the list to find the position to insert the new entry
            LinkedListEntry? searchEntry = firstEntry;
            while (searchEntry != null)
            {
                if (searchEntry.Key.CompareTo(key) > 0)
                {
                    // We have found an entry with a key greater than the new entry, so insert the new entry before it
                    LinkedListEntry? previousEntry = searchEntry.Previous;
                    if (previousEntry == null)
                    {
                        // This is a new first entry, so mark it as such
                        firstEntry = entry;
                    }
                    else
                    {
                        // Insert the new entry after the one just before this one
                        previousEntry.Next = entry;
                        entry.Previous = previousEntry;
                    }
                    entry.Next = searchEntry;
                    searchEntry.Previous = entry;
                    count++;
                    return;
                }
                searchEntry = searchEntry.Next;
            }

            // We got through the entire list, so insert the new entry at the end of the list
            LinkedListEntry oldLastEntry = lastEntry ?? throw new InvalidOperationException("Unexpected null last entry");
            oldLastEntry.Next = entry;
            entry.Previous = oldLastEntry;
            lastEntry = entry;
            count++;
            return;
        }

        public int Count
        {
            get
            {
                return count;
            }
        }

        public Tv? Get(Tk key)
        {
            if (firstEntry == null)
            {
                // No entries, so return null
                return default(Tv);
            }
            LinkedListEntry? searchEntry = firstEntry;
            while (searchEntry != null)
            {
                if (searchEntry.Key.CompareTo(key) == 0)
                {
                    // Found the key, so return the value
                    return searchEntry.Value;
                }
                searchEntry = searchEntry.Next;
            }
            // No match was found so return null
            return default(Tv);
        }

        public void Remove(Tk key)
        {
            if (firstEntry == null)
            {
                // No entries, so key cannot be present
                throw new InvalidOperationException("Key was not found");
            }

            if (firstEntry.Key.CompareTo(key) == 0)
            {
                // The first entry is the one to remove, so remove it and we are done
                firstEntry = firstEntry.Next;
                if (firstEntry != null)
                {
                    firstEntry.Previous = null;
                }
                else
                {
                    // We just removed the only entry, so mark last entry as null as well
                    lastEntry = null;
                }
                count--;
                return;
            }

            if (lastEntry?.Key.CompareTo(key) == 0)
            {
                // The last entry is the one to remove, so remove it and we are done
                lastEntry = lastEntry.Previous;
                if (lastEntry != null)
                {
                    lastEntry.Next = null;
                }
                else
                {
                    // We just removed the only entry, so mark first entry as null as well
                    // (This should never happen because we already checked if the first entry is the one to remove,
                    // but we will do it for safety just in case there is a bug in the code)
                    firstEntry = null;
                }
                count--;
                return;
            }

            LinkedListEntry? searchEntry = firstEntry;
            while (searchEntry != null)
            {
                if (searchEntry.Key.CompareTo(key) == 0)
                {
                    // Found the key, so remove the entry from the list
                    LinkedListEntry previousEntry = searchEntry.Previous ?? throw new InvalidOperationException("Unexpected null previous entry");
                    previousEntry.Next = searchEntry.Next;
                    // Set the previous and next of the removed entry to null to avoid issues with garbage collection of its old neighbors
                    searchEntry.Previous = null;
                    searchEntry.Next = null;
                    // Trust garbage collection to clean up the removed entry now that it is no longer connected to the list
                    count--;
                    return;
                }
                searchEntry = searchEntry.Next;
            }
            // No match was found
            throw new InvalidOperationException("Key was not found");
        }

        public Tv[] ToArray()
        {
            Tv[] array = new Tv[Count];
            int index = 0;
            LinkedListEntry? searchEntry = firstEntry;
            while (searchEntry != null)
            {
                array[index++] = searchEntry.Value;
                searchEntry = searchEntry.Next;
            }
            return array;
        }

        public Dictionary<Tk, Tv> ToDictionary()
        {
            Dictionary<Tk, Tv> dictionary = new Dictionary<Tk, Tv>();
            LinkedListEntry? searchEntry = firstEntry;
            while (searchEntry != null)
            {
                dictionary.Add(searchEntry.Key, searchEntry.Value);
                searchEntry = searchEntry.Next;
            }
            return dictionary;
        }

        public List<Tv> ToList()
        {
            List<Tv> values = new List<Tv>();
            LinkedListEntry? searchEntry = firstEntry;
            while (searchEntry != null)
            {
                values.Add(searchEntry.Value);
                searchEntry = searchEntry.Next;
            }
            return values;
        }

        private class LinkedListEntry
        {
            public Tk Key { get; set; } = default(Tk)!;
            public Tv Value { get; set; } = default(Tv)!;
            public LinkedListEntry? Previous { get; set; }
            public LinkedListEntry? Next { get; set; }
        }
    }
}
