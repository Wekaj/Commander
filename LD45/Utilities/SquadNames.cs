using System;
using System.Collections.Generic;

namespace LD45.Utilities {
    public static class SquadNames {
        private static readonly List<string> _firstParts = new List<string>();
        private static readonly Dictionary<char, List<string>> _secondParts = new Dictionary<char, List<string>>();

        static SquadNames() {
            AddFirstParts(new string[] {
                "Assemblage", "Assembly",
                "Band", "Bunch", "Bundle", "Board",
                "Collective", "Company", "Crowd", "Club",
                "Dozen",
                "Empire",
                "Flock", "Faculty", "Frenzy",
                "Group", "Gathering", "Gang", 
                "Herd", "House",
                "Jumble", "Jury",
                "Lark", "League",
                "Mass", "Mix", "Murder",
                "Nation", "Nest",
                "Party", "Posse", "Populace", "Proletariat",
                "Squad", "Society", "Suite", "Selection",
                "Troop", "Tribe"
            });

            AddSecondParts(new string[] {
                "Aces", "Adams",
                "Bros", "Blokes", "Bodies", "Bozos", "Bruisers", "Biscuits", "Bangers", "Babes",
                "Creeps", "Customers", "Creatures", "Carls", "Cuties",
                "Davids", "Dumbos", "Dummies", "Darlings", "Dudes",
                "Egos", "Eggs",
                "Freds", "Fruitcakes", "Fools",
                "Goons", "Grunions", "Gals", "Goobers",
                "Hooligans", "Helpers",
                "Jerks", "Jostlers", "Jerries",
                "Lackies", "Laughers",
                "Mortals", "Muffins", "Misters", "Monsters",
                "Nerds", "Ninnies", "Nincompoops", 
                "Peeps", "Plebs", "Plinkers",
                "Sirs", "Specimens", "Sisters", "Saps", "Sexies",
                "Terrors", "Thugs", "Toms"
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
