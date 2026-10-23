namespace _24_1639BasicProgrammingLanguage.Services
{
    public class InMemoryCrudStore
    {
        private readonly object _lock = new();
        private readonly Dictionary<string, List<Dictionary<string, object>>> _tables = new();
        private readonly Dictionary<string, int> _idCounters = new();

        public InMemoryCrudStore()
        {
            SeedInitialData();
        }

        private void SeedInitialData()
        {
            // 1. Mini Library Manager
            _tables["library"] = new List<Dictionary<string, object>>
            {
                new() { ["id"] = 1, ["title"] = "The Pragmatic Programmer", ["author"] = "Andrew Hunt & David Thomas", ["genre"] = "Computer Science", ["year"] = 1999 },
                new() { ["id"] = 2, ["title"] = "Clean Code", ["author"] = "Robert C. Martin", ["genre"] = "Software Engineering", ["year"] = 2008 },
                new() { ["id"] = 3, ["title"] = "Design Patterns: Elements of Reusable Object-Oriented Software", ["author"] = "Erich Gamma et al.", ["genre"] = "Architecture", ["year"] = 1994 },
                new() { ["id"] = 4, ["title"] = "Code Complete", ["author"] = "Steve McConnell", ["genre"] = "Software Engineering", ["year"] = 2004 }
            };
            _idCounters["library"] = 4;

            // 2. Personal Wishlist Manager
            _tables["wishlist"] = new List<Dictionary<string, object>>
            {
                new() { ["id"] = 1, ["name"] = "Mechanical Keyboard with Linear Switches", ["category"] = "Electronics", ["price"] = 4500.00, ["priority"] = "High", ["purchased"] = false },
                new() { ["id"] = 2, ["name"] = "Ergonomic Mesh Office Chair", ["category"] = "Furniture", ["price"] = 9200.00, ["priority"] = "Medium", ["purchased"] = true },
                new() { ["id"] = 3, ["name"] = "27-inch 4K USB-C Monitor", ["category"] = "Electronics", ["price"] = 18500.00, ["priority"] = "High", ["purchased"] = false },
                new() { ["id"] = 4, ["name"] = "Noise Cancelling Headphones", ["category"] = "Audio", ["price"] = 12000.00, ["priority"] = "Low", ["purchased"] = false }
            };
            _idCounters["wishlist"] = 4;

            // 3. Lost and Found Tracker
            _tables["lostfound"] = new List<Dictionary<string, object>>
            {
                new() { ["id"] = 1, ["name"] = "Wireless Earbuds Case", ["description"] = "White charging case found on 3rd floor library desk", ["location"] = "University Library - 3rd Floor", ["date"] = "2026-10-14", ["status"] = "Found" },
                new() { ["id"] = 2, ["name"] = "Black Leather Wallet", ["description"] = "Contains student ID card and transport card", ["location"] = "Student Cafeteria", ["date"] = "2026-10-15", ["status"] = "Lost" },
                new() { ["id"] = 3, ["name"] = "Silver Hydro Flask Tumbler", ["description"] = "32oz tumbler with developer stickers", ["location"] = "Engineering Lab 204", ["date"] = "2026-10-17", ["status"] = "Found" },
                new() { ["id"] = 4, ["name"] = "Scientific Calculator (Casio fx-991EX)", ["description"] = "Left after Advanced Mathematics examination", ["location"] = "Room 402 Main Building", ["date"] = "2026-10-18", ["status"] = "Claimed" }
            };
            _idCounters["lostfound"] = 4;

            // 4. Student Attendance Manager
            _tables["attendance"] = new List<Dictionary<string, object>>
            {
                new() { ["id"] = 1, ["name"] = "John Carlo Santos", ["studentNumber"] = "2024-01042", ["course"] = "BS Computer Science", ["section"] = "CS-2A", ["date"] = "2026-10-18", ["status"] = "Present" },
                new() { ["id"] = 2, ["name"] = "Maria Clara Dela Cruz", ["studentNumber"] = "2024-01089", ["course"] = "BS Information Technology", ["section"] = "IT-2B", ["date"] = "2026-10-18", ["status"] = "Present" },
                new() { ["id"] = 3, ["name"] = "Liam Gabriel Mendoza", ["studentNumber"] = "2024-01123", ["course"] = "BS Computer Science", ["section"] = "CS-2A", ["date"] = "2026-10-18", ["status"] = "Late" },
                new() { ["id"] = 4, ["name"] = "Sophia Nicole Reyes", ["studentNumber"] = "2024-01190", ["course"] = "BS Information Systems", ["section"] = "IS-2A", ["date"] = "2026-10-18", ["status"] = "Absent" },
                new() { ["id"] = 5, ["name"] = "Rafael Ethan Garcia", ["studentNumber"] = "2024-01205", ["course"] = "BS Computer Science", ["section"] = "CS-2A", ["date"] = "2026-10-18", ["status"] = "Present" }
            };
            _idCounters["attendance"] = 5;

            // 5. Game Collection Manager
            _tables["games"] = new List<Dictionary<string, object>>
            {
                new() { ["id"] = 1, ["title"] = "The Legend of Zelda: Tears of the Kingdom", ["genre"] = "Action-Adventure", ["platform"] = "Nintendo Switch", ["status"] = "Completed", ["rating"] = 5 },
                new() { ["id"] = 2, ["title"] = "Elden Ring", ["genre"] = "Action RPG", ["platform"] = "PC", ["status"] = "In Progress", ["rating"] = 5 },
                new() { ["id"] = 3, ["title"] = "Hollow Knight", ["genre"] = "Metroidvania", ["platform"] = "PC", ["status"] = "Completed", ["rating"] = 5 },
                new() { ["id"] = 4, ["title"] = "Cyberpunk 2077: Phantom Liberty", ["genre"] = "RPG", ["platform"] = "PlayStation 5", ["status"] = "Backlog", ["rating"] = 4 },
                new() { ["id"] = 5, ["title"] = "Stardew Valley", ["genre"] = "Farming Simulation", ["platform"] = "PC", ["status"] = "In Progress", ["rating"] = 5 }
            };
            _idCounters["games"] = 5;
        }

        public List<Dictionary<string, object>> GetAll(string table)
        {
            lock (_lock)
            {
                if (!_tables.ContainsKey(table))
                    _tables[table] = new List<Dictionary<string, object>>();

                // Return shallow copy of list to prevent concurrent enumeration issues
                return _tables[table].Select(d => new Dictionary<string, object>(d)).ToList();
            }
        }

        public Dictionary<string, object>? GetById(string table, int id)
        {
            lock (_lock)
            {
                if (!_tables.ContainsKey(table)) return null;
                var found = _tables[table].FirstOrDefault(x => Convert.ToInt32(x["id"]) == id);
                return found != null ? new Dictionary<string, object>(found) : null;
            }
        }

        public int Insert(string table, Dictionary<string, object> record)
        {
            lock (_lock)
            {
                if (!_tables.ContainsKey(table))
                {
                    _tables[table] = new List<Dictionary<string, object>>();
                    _idCounters[table] = 0;
                }

                int newId = ++_idCounters[table];
                record["id"] = newId;
                _tables[table].Add(new Dictionary<string, object>(record));
                return newId;
            }
        }

        public bool Update(string table, int id, Dictionary<string, object> updatedFields)
        {
            lock (_lock)
            {
                if (!_tables.ContainsKey(table)) return false;
                var item = _tables[table].FirstOrDefault(x => Convert.ToInt32(x["id"]) == id);
                if (item == null) return false;

                foreach (var kvp in updatedFields)
                {
                    if (kvp.Key != "id")
                    {
                        item[kvp.Key] = kvp.Value;
                    }
                }
                return true;
            }
        }

        public bool Delete(string table, int id)
        {
            lock (_lock)
            {
                if (!_tables.ContainsKey(table)) return false;
                var item = _tables[table].FirstOrDefault(x => Convert.ToInt32(x["id"]) == id);
                if (item == null) return false;
                return _tables[table].Remove(item);
            }
        }
    }
}
