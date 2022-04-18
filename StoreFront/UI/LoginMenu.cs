using Models;
using BL;

namespace UI;

public class LoginMenu
{

    private readonly HttpService _httpService;
    public LoginMenu(HttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task Menu()
    {
        Console.WriteLine("=========================================\n====== Just Another Computer Store ======\n=========================================\n");

    LoginRegister:
        Console.WriteLine("[1] Login");
        Console.WriteLine("[2] Register");
        Console.WriteLine("[x] Exit");

        string answer = Console.ReadLine().Trim();
        User user = new User();

        if (answer == "1")
        {
            user = await Login();
        }
        else if (answer == "2")
        {
            user = await Register();

        }
        else if (answer.ToLower() == "x")
        {
            return;
        }
        else
        {
            Console.WriteLine("Invalid Input");
            goto LoginRegister;
        }

        if (user == null)
        {
            return;
        }
        else
        
        {
            if (user.IsEmployed == true)
            {
                Employee employee = new Employee
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Password = user.Password,
                    IsEmployed = user.IsEmployed
                };
                Console.WriteLine("Logging in...");
                await new EmployeeMenu(_httpService, employee).AdminMenu();
            }
            else
            {
                User customer = user;
                Console.WriteLine("Logging in...");
                await new CustomerMenu(_httpService, customer).StoreMenu();
            }
        }
        Console.WriteLine("Logging out...");
    }

    private async Task<User> Login()    {

    Login:
        Console.WriteLine("Enter your Username:");
        string userName = Console.ReadLine().Trim();

        List<User> users = await _httpService.GetAllUsersAsync();

        foreach (User user in users)
        {
            if (user.UserName.ToLower() == userName.ToLower())
            {
            Password:
                Console.WriteLine("Enter you Password:\nWarning: case-sensitive");
                string password = Console.ReadLine().Trim();

                if (user.Password == password)
                {
                    User signedIn = user;
                    return signedIn;
                }
                else
                {
                TryAgainAtmpt:
                    Console.WriteLine("Incorrect password...Try again? [Y/N]");
                    string innerResponse = Console.ReadLine().Trim().ToUpper();

                    if (innerResponse == "Y")
                    {
                        goto Password;
                    }
                    else if (innerResponse == "N")
                    {
                        goto outerResponseChoice;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input\n");
                        goto TryAgainAtmpt;
                    }
                }
            }
        }

        Console.WriteLine("Could not find an account with that username.");
    outerResponseChoice:
        Console.WriteLine("Would you like to try logging in again or make an account?\n[1] Try Again\n[2] Register");
        string outerResponse = Console.ReadLine().Trim();

        if (outerResponse == "1")
        {
            goto Login;
        }
        else if (outerResponse == "2")
        {
            User customer = await Register();
            return customer;
        }
        else
        {
            Console.WriteLine("Invalid Input\n");
            goto outerResponseChoice;
        }
    }

    public async Task<User> Register()
    {
    Register:
        Console.WriteLine("Enter a Username: ");
        string userName = Console.ReadLine().Trim();

        List<User> users = await _httpService.GetAllUsersAsync();

        foreach (User user in users)
        {
            if (user.UserName.ToLower() == userName.ToLower())
            {
            TryAgainResponse:
                Console.WriteLine("That username is already taken!\nTry Again?[Y/N]");
                string response = Console.ReadLine().Trim().ToUpper();

                if (response == "N")
                {
                    return null;
                }
                else if (response == "Y")
                {
                    goto Register;
                }
                else
                {
                    Console.WriteLine("Invalid input\n");
                    goto TryAgainResponse;
                }
            }
        }

        Console.WriteLine("Enter a Password\nWarning: this will be case-sensitive");
        string password = Console.ReadLine().Trim();

        User newUser = new User();

        newUser.UserName = userName;
        newUser.Password = password;

        newUser = await _httpService.AddUserAsync(newUser);

        return newUser;
    }
}