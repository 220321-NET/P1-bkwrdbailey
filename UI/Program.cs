﻿using UI;
using DL;
using BL;

// Connection string for DB Microsoft Azure
string connectionString = File.ReadAllText("./connectionString.txt");

IRepository repo = new DBRepository(connectionString);
IStoreBL bl = new StoreBL(repo);

new LoginMenu(bl).Menu();