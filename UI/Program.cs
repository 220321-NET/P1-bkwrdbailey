using UI;
// using DL;
// using BL;

// Connection string for DB Microsoft Azure
// string connectionString = File.ReadAllText("./connectionString.txt");

// IRepository repo = new DBRepository(connectionString);
// IStoreBL bl = new StoreBL(repo);
HttpService http = new HttpService();
new LoginMenu(http).Menu();