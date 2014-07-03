using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Xml.Serialization;
using Hearthstone_Deck_Tracker.Stats;

namespace Hearthstone_Deck_Tracker.Hearthstone
{
    public class Deck : ICloneable
    {
        public string Name;
        public string Class;
        public string Note;
        public string Url;

        [XmlArray(ElementName = "Tags")]
        [XmlArrayItem(ElementName = "Tag")]
        public List<string> Tags;

       // public int DeckStatsId;

        [XmlIgnore] 
        public DeckStats Stats;

        [XmlIgnore]
        public string GetClass
        {
            get { return string.IsNullOrEmpty(Class) ? "(No Class Selected)" : "(" + Class + ")"; }
        }

        [XmlIgnore]
        public string GetName
        {
            get { return Name == "" ? "No Name Set" : Name; }
        }

        [XmlIgnore]
        public string TagList
        {
            get { return Tags.Count > 0 ? "[" + Tags.Aggregate((t, n) => t + ", " + n) + "]" : ""; }
        }

        [XmlIgnore]
        public SolidColorBrush ClassColorBrush
        {
            get
            {
                return new SolidColorBrush(ClassColor);
            }
        }

        [XmlIgnore]
        public Color ClassColor
        {
            get
            {
                switch (Class)
                {
                    case "Druid":
                        return (Color)ColorConverter.ConvertFromString("#FF7D0A");
                    case "Death Knight":
                        return (Color)ColorConverter.ConvertFromString("#C41F3B");
                    case "Hunter":
                        return (Color)ColorConverter.ConvertFromString("#ABD473");
                    case "Mage":
                        return (Color)ColorConverter.ConvertFromString("#69CCF0");
                    case "Monk":
                        return (Color)ColorConverter.ConvertFromString("#00FF96");
                    case "Paladin":
                        return (Color)ColorConverter.ConvertFromString("#F58CBA");
                    case "Priest":
                        return (Color)ColorConverter.ConvertFromString("#FFFFFF");
                    case "Rogue":
                        return (Color)ColorConverter.ConvertFromString("#FFF569");
                    case "Shaman":
                        return (Color)ColorConverter.ConvertFromString("#0070DE");
                    case "Warlock":
                        return (Color)ColorConverter.ConvertFromString("#9482C9");
                    case "Warrior":
                        return (Color)ColorConverter.ConvertFromString("#C79C6E");
                    default:
                        return Colors.Gray;
                }
            }
        }

        [XmlArray(ElementName = "Cards")]
        [XmlArrayItem(ElementName = "Card")]
        public ObservableCollection<Card> Cards;

        public Deck()
        {
            Cards = new ObservableCollection<Card>();
            Tags = new List<string>();
            Stats = new DeckStats();
        }

        public Deck(string name, string className, IEnumerable<Card> cards, IEnumerable<string> tags, string note, DeckStats stats, string url)
        {
            Name = name;
            Class = className;
            Cards = new ObservableCollection<Card>();
            Stats = new DeckStats();
            foreach (var card in cards)
            {
                Cards.Add((Card)card.Clone());
            }
            Tags = new List<string>(tags);
            Note = note;
            Url = url;
            Stats = stats;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Class);
        }

        public object Clone()
        {
            return new Deck(Name, Class, Cards, Tags, Note, Url, Stats);
        }

        public override bool Equals(object obj)
        {
            var deck = obj as Deck;
            if (deck == null) return false;
            return Name == deck.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
