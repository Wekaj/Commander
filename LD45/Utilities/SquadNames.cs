using System;
using System.Collections.Generic;

namespace LD45.Utilities {
    public static class SquadNames {
        private static readonly List<string> _firstParts = new List<string>();
        private static readonly Dictionary<char, List<string>> _secondParts = new Dictionary<char, List<string>>();

        static SquadNames() {
            AddFirstParts(new string[] {
                "Assemblage", "Assembly",
                "Band", "Bunch", "Bundle",
                "Collective", "Company", "Crowd", "Club",
                "Destruction", "Doom",
                "Empire", "Exile",
                "Flock", "Failure", "Field",
                "Group", "Gathering", "Gang", 
                "Herd", "House",
                "Jumble",
                "Kollective", "Kompany", "Krowd", "Klub",
                "Loss", "Lack",
                "Mass", "Mutiny", "Mix", "Masters", "Murder",
                "Nation", "Nest",
                "Party", "Posse", "Populace", "Proletariat",
                "Squad", "Society", "Suite", "Selection",
                "Troop", "Tray"
            });

            AddSecondParts(new string[] {
                "Aces", "Adams",
                "Bros", "Blokes", "Bodies", "Bozos", "Bruisers", "Biscuits", "Bangers", "Babes",
                "Creeps", "Customers", "Creatures", "Carls", "Cuties",
                "Davids", "Dumbos", "Dummies", "Darlings", "Dudes", "Doods",
                "Egos", "Eggs",
                "Freds", "Fruitcakes",
                "Goons", "Grunions", "Gals", "Goobers",
                "Hooligans", "Helpers",
                "Jerks", "Jostlers", "Jerries",
                "Kreeps", "Kustomers", "Kreatures", "Karls", "Kuties", "Koalas", "Kakas",
                "Lackies", "Lamps", "Laughers",
                "Mortals", "Muffins", "Misters", "Monsters",
                "Nerds", "Ninnies", "Nincompoops", 
                "Peeps", "Plebs", "Plinkers",
                "Sirs", "Specimens", "Sisters", "Saps", "Sexies",
                "Terrors", "Thugs"
            });
        }

        public static string Generate(Random random) {
            string firstPart = _firstParts[random.Next(_firstParts.Count)];

            string secondPart = "People";
            if (_secondParts.TryGetValue(firstPart[0], out List<string> list)) {
                secondPart = list[random.Next(list.Count)];
            }

            return "The " + firstPart + " of " + secondPart;
        }

        private static void AddFirstParts(params string[] firstParts) {
            _firstParts.AddRange(firstParts);
        }

        private static void AddSecondParts(params string[] secondsParts) {
            for (int i = 0; i < secondsParts.Length; i++) {
                if (_secondParts.TryGetValue(secondsParts[i][0], out List<string> list)) {
                    list.Add(secondsParts[i]);
                }
                else {
                    _secondParts.Add(secondsParts[i][0], new List<string> { secondsParts[i] });
                }
            }
        }
    }
}
