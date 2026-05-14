#include <iostream>
#include <fstream>
#include <string>
#include <windows.h>
#include <conio.h> // ✅ REQUIRED for _getch()

using namespace std;

// ================= DESIGN FUNCTIONS =================

void setColor(int color)
{
    SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), color);
}

void loadingScreen()
{
    cout << "\nInitializing System ";

    for (int i = 0; i < 5; i++)
    {
        cout << ".";
        Sleep(400);
    }

    cout << "\n\n";
}

void banner()
{
    setColor(11);

    cout << R"(

============================================================
        ██████╗ ███████╗██████╗  █████╗ ██╗██████╗
        ██╔══██╗██╔════╝██╔══██╗██╔══██╗██║██╔══██╗
        ██████╔╝█████╗  ██████╔╝███████║██║██████╔╝
        ██╔══██╗██╔══╝  ██╔═══╝ ██╔══██║██║██╔══██╗
        ██║  ██║███████╗██║     ██║  ██║██║██║  ██║
        ╚═╝  ╚═╝╚══════╝╚═╝     ╚═╝  ╚═╝╚═╝╚═╝  ╚═╝

                REPAIR TRACKER SYSTEM
============================================================

)";

    setColor(7);
}

// ================= LOGIN =================

bool validateLogin(string username, string password)
{
    ifstream file("users.txt");

    if (!file.is_open()) // ✅ safety check
    {
        cout << "Error: users.txt not found!\n";
        return false;
    }

    string line;

    while (getline(file, line))
    {
        int comma = line.find(',');

        string fileUser = line.substr(0, comma);
        string filePass = line.substr(comma + 1);

        if (fileUser == username && filePass == password)
            return true;
    }

    return false;
}

// ================= PASSWORD INPUT =================

string getHiddenPassword()
{
    string password = "";
    char ch;

    while (true)
    {
        ch = _getch(); // read key without showing it

        if (ch == 13) // ENTER
            break;
        else if (ch == 8) // BACKSPACE
        {
            if (!password.empty())
            {
                password.pop_back();
                cout << "\b \b";
            }
        }
        else
        {
            password += ch;
            cout << "*";
        }   
    }

    cout << endl;
    return password;
}

// ================= MAIN =================

int main()
{
    SetConsoleOutputCP(CP_UTF8);
    SetConsoleCP(CP_UTF8);

    system("title Repair Tracker System");

    while (true)
    {
        system("cls");
            
        banner();
        loadingScreen();

        string username, password;

        setColor(14);
        cout << " LOGIN PORTAL\n";
        cout << "----------------------------------\n";
        setColor(7);

        cout << " Username : ";
        cin >> username;

        cout << " Password : ";
        password = getHiddenPassword(); // ✅ FIXED (removed 'r')

        cout << "\n";

        if (validateLogin(username, password))
        {
            setColor(10);
            cout << " ✔ Access Granted!\n";
            loadingScreen();

            system("start RepairTrackerSystem.exe");            
            break;
        }
        else
        {
            setColor(12);
            cout << " ✖ Invalid credentials! Try again...\n";
            Sleep(2000);
        }
    }

    setColor(7);
    return 0;
}