using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;

namespace Minions
{
    public class StartUp
    {
        public static void Main()
        {
            SqlConnection sqlConnection = new SqlConnection(Configuration.ConnectionString);

            sqlConnection.Open();

            using (sqlConnection)
            {
                // Problem 02: Villain Names
                PrintVilliansWithMoreThan3Minions(sqlConnection);

                // Problem 03: Minion Names
                int villainId = int.Parse(Console.ReadLine());
                PrintAllMinionsByWithNamesAndAgesForGivenVillian(sqlConnection, villainId);

                // Problem 04: Add Minion
                string[] minionInfo = Console.ReadLine()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();
                string[] villainName = Console.ReadLine()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();
                AddMinion(sqlConnection, minionInfo, villainName);

                // Problem 05: Change Town Names Casing
                string countryName = Console.ReadLine();
                ChangeTownNamesCasing(sqlConnection, countryName);

                // Problem 06: Remove Villain
                int villainId2 = int.Parse(Console.ReadLine());
                RemoveVillain(sqlConnection, villainId2);

                // Problem 07: Print All Minion Names
                PrintAllMinionNames(sqlConnection);

                // Problem 08: Increase Minion Age
                int[] minionIds = Console.ReadLine()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();
                IncreaseMinionAge(sqlConnection, minionIds);

                //Problem 09: Ibncrease Age Stored Procedure
                int minionId = int.Parse(Console.ReadLine());
                IncreaseMinionAge(sqlConnection, minionId);
            }
        }


        // Problem 02: Villain Names
        private static void PrintVilliansWithMoreThan3Minions(SqlConnection sqlConnection)
        {
            SqlCommand sqlCommand = new SqlCommand(Queries.VillainsWithMoreThanThreeMinions, sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            using (sqlDataReader)
            {
                while (sqlDataReader.Read())
                {
                    string villainName = sqlDataReader.GetString(0);
                    int minionsCount = sqlDataReader.GetInt32(1);
                    Console.WriteLine($"{villainName} - {minionsCount}");
                }
            }
        }

        // Problem 03: Minion Names
        private static void PrintAllMinionsByWithNamesAndAgesForGivenVillian(SqlConnection sqlConnection, int villainId)
        {
            SqlCommand getVillainNameCommand = new SqlCommand(Queries.VillainNameById, sqlConnection);

            SqlParameter idParameteer1 = new SqlParameter(@"Id", SqlDbType.Int);
            idParameteer1.Value = villainId;
            getVillainNameCommand.Parameters.Add(idParameteer1);

            object villainNameObject = getVillainNameCommand.ExecuteScalar();

            if (villainNameObject == null)
            {
                Console.WriteLine($"No villain with ID {villainId} exists in the database.");
            }
            else
            {
                string villainName = (string)villainNameObject;
                SqlCommand villainMinionsInfoCommand = new SqlCommand(Queries.VillainMinionsInfoById, sqlConnection);

                SqlParameter idParameteer2 = new SqlParameter(@"Id", SqlDbType.Int);
                idParameteer2.Value = villainId;
                villainMinionsInfoCommand.Parameters.Add(idParameteer2);

                SqlDataReader sqlDataReader = villainMinionsInfoCommand.ExecuteReader();

                using (sqlDataReader)
                {
                    Console.WriteLine($"Villain: {villainName}");
                    if (!sqlDataReader.HasRows)
                    {
                        Console.WriteLine("(no minions)");
                    }
                    else
                    {
                        while (sqlDataReader.Read())
                        {
                            long rowNumber = sqlDataReader.GetInt64(0);
                            string minionName = sqlDataReader.GetString(1);
                            int minionAge = sqlDataReader.GetInt32(2);
                            Console.WriteLine($"{rowNumber}. {minionName} {minionAge}");
                        }
                    }
                }
            }
        }

        // Problem 04: Add Minion
        private static void AddMinion(SqlConnection sqlConnection, string[] minionInfo, string[] villainName)
        {
            string minionName = minionInfo[1];
            int minionAge = int.Parse(minionInfo[2]);
            string townName = minionInfo[3];

            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
            try
            {
                int townId = GetTownId(sqlConnection, sqlTransaction, townName);
                int villainId = GetVillainId(sqlConnection, sqlTransaction, villainName[1]);
                int minionId = AddMinionAndGetId(sqlConnection, sqlTransaction, minionName, minionAge, townId);

                SqlCommand addMinionToVillainCommand = new SqlCommand(Queries.AddMinionToVillain, sqlConnection, sqlTransaction);
                addMinionToVillainCommand.Parameters.AddWithValue("@villainId", villainId);
                addMinionToVillainCommand.Parameters.AddWithValue("@minionId", minionId);

                addMinionToVillainCommand.ExecuteNonQuery();
                Console.WriteLine($"Successfully added {minionName} to be minion of {villainName[1]}.");

                sqlTransaction.Commit();
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                Console.WriteLine(ex.Message.ToString());
            }
        }

        // Problem 04 HelperMethod 01
        private static int GetTownId(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string townName)
        {
            SqlCommand townIdCommand = new SqlCommand(Queries.GetTownId, sqlConnection, sqlTransaction);
            townIdCommand.Parameters.AddWithValue("@townName", townName);

            object townIdObj = townIdCommand.ExecuteScalar();
            if (townIdObj == null)
            {
                SqlCommand addTownCommand = new SqlCommand(Queries.AddTownQuery, sqlConnection, sqlTransaction);
                addTownCommand.Parameters.AddWithValue("@townName", townName);
                addTownCommand.ExecuteNonQuery();

                Console.WriteLine($"Town {townName} was added to the database.");
                townIdObj = townIdCommand.ExecuteScalar();
            }
            return (int)townIdObj;
        }

        // Problem 04 HelperMethod 02
        private static int GetVillainId(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string villainName)
        {
            SqlCommand villainIdCommand = new SqlCommand(Queries.GetVillainId, sqlConnection, sqlTransaction);
            villainIdCommand.Parameters.AddWithValue("@Name", villainName);

            object villainIdObj = villainIdCommand.ExecuteScalar();
            if (villainIdObj == null)
            {
                SqlCommand evilnessFactorCommand = new SqlCommand(Queries.GetEvilnessFactor, sqlConnection, sqlTransaction);
                int evilnessFactorId = (int)evilnessFactorCommand.ExecuteScalar();

                SqlCommand addVillainCommand = new SqlCommand(Queries.AddVillainQuery, sqlConnection, sqlTransaction);
                addVillainCommand.Parameters.AddWithValue("@villainName", villainName);
                addVillainCommand.Parameters.AddWithValue("@evilnessFactorId", evilnessFactorId);
                addVillainCommand.ExecuteNonQuery();

                Console.WriteLine($"Villain {villainName} was added to the database.");
                villainIdObj = villainIdCommand.ExecuteScalar();
            }

            return (int)villainIdObj;
        }

        // Problem 04 HelperMethod 03
        private static int AddMinionAndGetId(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string minionName, int minionAge, int townId)
        {
            SqlCommand addMinionCommand = new SqlCommand(Queries.AddMinionQuery, sqlConnection, sqlTransaction);
            addMinionCommand.Parameters.AddWithValue("@name", minionName);
            addMinionCommand.Parameters.AddWithValue("@age", minionAge);
            addMinionCommand.Parameters.AddWithValue("@townId", townId);

            addMinionCommand.ExecuteNonQuery();

            SqlCommand getMinionIdCommand = new SqlCommand(Queries.GetMinion, sqlConnection, sqlTransaction);
            getMinionIdCommand.Parameters.AddWithValue("@name", minionName);
            getMinionIdCommand.Parameters.AddWithValue("@age", minionAge);
            getMinionIdCommand.Parameters.AddWithValue("@townId", townId);

            int minionId = (int)getMinionIdCommand.ExecuteScalar();

            return minionId;
        }

        // Problem 05: Change Town Names Casing
        private static void ChangeTownNamesCasing(SqlConnection sqlConnection, string countryName)
        {
            SqlCommand updateTownsNamesCmd = new SqlCommand(Queries.UpdateTownNames, sqlConnection);
            updateTownsNamesCmd.Parameters.AddWithValue("@countryName", countryName);

            int changedRows = updateTownsNamesCmd.ExecuteNonQuery();
            if (changedRows > 0)
            {
                Console.WriteLine($"{changedRows} town names were affected.");
                SqlCommand getChangedTownsCmd = new SqlCommand(Queries.GetChangedTowns, sqlConnection);
                getChangedTownsCmd.Parameters.AddWithValue("@countryName", countryName);
                string[] changedTowns = new string[changedRows];
                int i = 0;
                SqlDataReader dataReader = getChangedTownsCmd.ExecuteReader();
                using (dataReader)
                {
                    while (dataReader.Read())
                    {
                        changedTowns[i++] = (string)dataReader["Name"];
                    }
                }
                Console.WriteLine($"[{string.Join(", ", changedTowns)}]");
            }
            else
            {
                Console.WriteLine($"No town names were affected.");
            }
        }

        // Problem 06: Remove Villain
        private static void RemoveVillain(SqlConnection sqlConnection, int villainId)
        {
            SqlCommand getVillainNameCmd = new SqlCommand(Queries.GetVillainName, sqlConnection);
            getVillainNameCmd.Parameters.AddWithValue("@villainId", villainId);
            string villainName = (string)getVillainNameCmd.ExecuteScalar();

            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
            try
            {
                SqlCommand releaseMinionsFromVillainCmd = new SqlCommand(Queries.ReleaseMinionsOfVillain, sqlConnection, sqlTransaction);
                releaseMinionsFromVillainCmd.Parameters.AddWithValue("@villainId", villainId);
                int minionsReleased = releaseMinionsFromVillainCmd.ExecuteNonQuery();

                SqlCommand deleteVillainsCmd = new SqlCommand(Queries.DeleteVillain, sqlConnection, sqlTransaction);
                deleteVillainsCmd.Parameters.AddWithValue("@villainId", villainId);
                int villainDeleted = deleteVillainsCmd.ExecuteNonQuery();

                if (villainDeleted != 1)
                {
                    sqlTransaction.Rollback();
                    Console.WriteLine($"No such villain was found.");
                }
                else
                {
                    Console.WriteLine($"{villainName} was deleted.");
                    Console.WriteLine($"{minionsReleased} minions were released.");
                }
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                Console.WriteLine(ex.Message.ToString());
            }
        }

        // Problem 07: Print All Minion Names
        private static void PrintAllMinionNames(SqlConnection sqlConnection)
        {
            SqlCommand getMinionNamesCmd = new SqlCommand(Queries.GetAllMinionNames, sqlConnection);
            SqlDataReader dataReader = getMinionNamesCmd.ExecuteReader();
            List<string> minions = new List<string>();

            using (dataReader)
            {
                while (dataReader.Read())
                {
                    minions.Add(dataReader.GetString(0));
                }
            }

            for (int i = 0; i < minions.Count / 2; i++)
            {
                Console.WriteLine(minions[i]);
                Console.WriteLine(minions[minions.Count - i - 1]);
            }
            if (minions.Count % 2 != 0)
            {
                Console.WriteLine(minions[minions.Count / 2]);
            }
        }

        // Problem 08: Increase Minion Age
        private static void IncreaseMinionAge(SqlConnection sqlConnection, int[] minionIds)
        {
            foreach (var id in minionIds)
            {
                SqlCommand getMinionsCmd = new SqlCommand(Queries.GetMinionIdsAndIncrementTheirAge, sqlConnection);
                getMinionsCmd.Parameters.AddWithValue("@Id", id);
                getMinionsCmd.ExecuteNonQuery();
            }

            SqlCommand printMinionsCmd = new SqlCommand(Queries.GetMinionsAndAge, sqlConnection);
            SqlDataReader dataReader = printMinionsCmd.ExecuteReader();

            using (dataReader)
            {
                while (dataReader.Read())
                {
                    string minionName = dataReader.GetString(0);
                    int minionAge = dataReader.GetInt32(1);
                    Console.WriteLine($"{minionName} {minionAge}");
                }
            }
        }

        // Problem 09: Increase Age Stored Procedure
        private static void IncreaseMinionAge(SqlConnection sqlConnection, int minionId)
        {
            SqlCommand increaseAgeCmd = new SqlCommand(Queries.IncreaseAgeQuery, sqlConnection);
            increaseAgeCmd.Parameters.AddWithValue("@minionId", minionId);
            increaseAgeCmd.ExecuteNonQuery();

            SqlCommand getMinionInfoCmd = new SqlCommand(Queries.GetMinionInfo, sqlConnection);
            getMinionInfoCmd.Parameters.AddWithValue("@Id", minionId);

            SqlDataReader dataReader = getMinionInfoCmd.ExecuteReader();

            using (dataReader)
            {
                while (dataReader.Read())
                {
                    string minionName = dataReader.GetString(0);
                    int minionAge = dataReader.GetInt32(1);
                    Console.WriteLine($"{minionName} - {minionAge} years old");
                }
            }
        }
    }
}