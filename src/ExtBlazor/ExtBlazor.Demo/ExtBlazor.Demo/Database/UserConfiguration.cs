using ExtBlazor.Demo.Client.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExtBlazor.Demo.Database;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        var firstNames = new List<string>()
        {
            "Erik",
            "Per Gunnar",
            "Bernst Gunnar",
            "Andreas",
            "Anders",
            "Henrik",
            "Anna",
            "Ulf",
            "Anneli",
            "Lars",
            "Lars Erik",
            "Emma",
            "Anna Karin",
            "Karin",
            "Eva",
            "Eva Lisa",
            "Lisa",
            "Kristina",
            "Christina",
            "Jessica",
            "Anette",
            "Janette",
            "Charles",
            "Karl",
            "Karl Oskar",
            "Oskar",
            "Oscar",
            "Eirik",
            "Ufo",
            "Inge",
            "Ingrid",
            "Empi",
            "Anti",
            "Elrik",
            "Alrik",
            "Gustav",
            "Gustaf",
            "George",
            "Greger",
            "Greta",
            "Joe",
            "Peter",
            "Petter",
            "Sixten",
            "Olof",
            "Olle",
            "Ole",
            "Eive",
            "Jesper",
            "Aina",
            "Gertrud",
            "Per",
            "Holger",
            "Helge",
            "Gösta",
            "Gusten",
            "Dan",
            "Halvdan",
            "Harald",
            "Sure",
            "Torsten",
            "Yngve",
            "Frej",
            "Freja",
            "Hel",
            "Ville",
            "Vilgot",
            "Ture",
            "Tove",
            "Toste",
            "Ulrik",
            "Gunnel",
            "Ida",
            "Matilda",
            "Selma",
            "Zoltan",
            "Kalle",
            "Bengt",
            "Fredrik",
            "Erika",
            "Fredrika",
            "Moa",
            "Maria",
            "Mia",
            "Agneta",
            "Birgitta",
            "Anna Greta",
            "Daniel",
            "Sten",
            "Sture",
            "Steve",
            "Lukas",
            "Linda",
            "Lina",
            "Fredrik",
            "Fin",
            "Nicole",
            "Ingvar"
        };

        var lastnames = new List<string>()
        {
            "Johansson",
            "Eriksson",
            "Janson",
            "Sjöman",
            "Sjöberg",
            "Palme",
            "Granth",
            "Lejon",
            "Von Platen",
            "Fältskog",
            "Danielsson",
            "Suresson",
            "Sture",
            "Bernadotte",
            "Schultz",
            "Wern",
            "Vennerberg",
            "Strand",
            "Banner",
            "Gyllenstjärna",
            "Bock",
            "Book",
            "Bjelke",
            "Gyllenborst",
            "Trolle",
            "Tostensson",
            "Jansson",
            "Brink",
            "Sjöholm",
            "Hallengren",
            "Gren",
            "Torsson",
            "Fröjd",
            "Tok",
            "Lindenäs",
            "Lind"
        };

        var users = new List<User>();

        foreach (var item in Enumerable.Range(1, 1007))
        {
            var firstName = firstNames[Random.Shared.Next(0, firstNames.Count() - 1)];
            var lastName = lastnames[Random.Shared.Next(0, lastnames.Count() - 1)];

            var email = firstName + "." + lastName + "@" + "testdomain.com";
            var username = "user" + item.ToString();

            var created = DateTime.Now.Subtract(TimeSpan.FromDays(Random.Shared.Next(2, 1024)));
            var changed = created.AddDays(Random.Shared.Next(1, (int)Math.Floor(DateTime.Now.Subtract(created).TotalDays)));

            var login = created.AddDays(Random.Shared.Next(1, (int)Math.Floor(DateTime.Now.Subtract(created).TotalDays)));
            var admin = Random.Shared.Next(108) == 9;
            users.Add(new User
            {
                Name = firstName + " " + lastName,
                Email = email,
                Username = username,
                Id = item,
                Phone = "+" + Random.Shared.Next(10, 99).ToString() + Random.Shared.Next(700000000, 799999999).ToString(),
                Created = created,
                Changed = changed,
                LastLogin = login,
                Admin = admin
            });
        };
        builder.HasData(users);
    }
}
