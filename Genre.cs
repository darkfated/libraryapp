using System;
using System.Collections.Generic;

namespace libraryapp
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
