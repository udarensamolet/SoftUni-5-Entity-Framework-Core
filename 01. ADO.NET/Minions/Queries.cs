namespace Minions
{
    public static class Queries
    { 
        public const string VillainsWithMoreThanThreeMinions = 
            @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                FROM Villains AS v 
                JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
            GROUP BY v.Id, v.Name 
              HAVING COUNT(mv.VillainId) > 3 
            ORDER BY COUNT(mv.VillainId)";

        public const string VillainNameById =
            @"SELECT Name 
                FROM Villains 
               WHERE Id = @Id";

        public const string VillainMinionsInfoById =
            @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                     m.Name, 
                     m.Age
                FROM MinionsVillains AS mv
                JOIN Minions As m ON mv.MinionId = m.Id
               WHERE mv.VillainId = @Id
               ORDER BY m.Name";

        public const string GetTownId =
            @"SELECT [Id] 
                FROM [Towns] 
               WHERE [Name] = @townName";

        public const string AddTownQuery =
            @"INSERT INTO Towns (Name) VALUES (@townName)";

        public const string GetVillainId =
            @"SELECT Id 
                FROM Villains 
               WHERE Name = @Name";

        public const string AddVillainQuery =
            @"INSERT INTO Villains (Name, EvilnessFactorId) VALUES (@villainName, @evilnessFactorId)";

        public const string GetEvilnessFactor =
            @"SELECT [Id]
                FROM [EvilnessFactors]
               WHERE [Name] = 'Evil'";

        public const string AddMinionQuery =
            @"INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";

        public const string GetMinion =
            @"SELECT Id FROM Minions 
               WHERE Name = @Name
                 AND Age = @Age
                 AND TownId = @townId";

        public const string AddMinionToVillain =
            @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)";

        public const string UpdateTownNames =
            @"UPDATE Towns
                 SET Name = UPPER(Name)
               WHERE CountryCode = (SELECT c.Id FROM Countries AS c 
                                     WHERE c.Name = @countryName)";

        public const string GetChangedTowns =
            @"SELECT [t].[Name] 
                FROM [Towns] as [t]
                JOIN [Countries] AS [c] 
                  ON [c].[Id] = [t].[CountryCode]
               WHERE [c].[Name] = @countryName";

        public const string GetVillainName =
            @"SELECT Name 
                FROM Villains 
               WHERE Id = @villainId";

        public const string ReleaseMinionsOfVillain =
            @"DELETE FROM MinionsVillains 
               WHERE VillainId = @villainId";

        public const string DeleteVillain =
            @"DELETE FROM Villains
               WHERE Id = @villainId";

        public const string GetAllMinionNames =
            @"SELECT Name FROM Minions";

        public const string GetMinionIdsAndIncrementTheirAge =
            @" UPDATE Minions
                  SET Name = LOWER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                WHERE Id = @Id";

        public const string GetMinionsAndAge =
            @"SELECT [Name], [Age] FROM [Minions]";

        public const string IncreaseAgeQuery =
            @"EXEC [dbo].[usp_GetOlder] @minionId";

        public const string GetMinionInfo =
            @"SELECT Name, Age FROM Minions WHERE Id = @Id";
    }
}
