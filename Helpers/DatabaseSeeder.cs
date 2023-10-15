using Commons.Categories;
using Repositories.Managers;
using Commons.Models;
using Commons.Types;
using Microsoft.EntityFrameworkCore;
using Services.Authentication;

namespace Helpers;

public class DatabaseSeeder
{
    private readonly UwcDbContext _uwcDbContext;

    private readonly List<Account> _allAccounts = new();
    private readonly List<VehicleData> _allVehicles = new();

    public DatabaseSeeder(UwcDbContext uwcDbContext)
    {
        _uwcDbContext = uwcDbContext;
        _uwcDbContext.Database.EnsureCreated();
    }

    public void ResetDatabase()
    {
        _uwcDbContext.Accounts.ExecuteDelete();
        _uwcDbContext.McpDatas.ExecuteDelete();
        _uwcDbContext.VehicleDatas.ExecuteDelete();
        _uwcDbContext.Messages.ExecuteDelete();
        _uwcDbContext.TaskDatas.ExecuteDelete();
    }

    public void SeedDatabase()
    {
        SeedSupervisorProfiles();
        SeedDriverProfiles();
        SeedCleanerProfiles();
        SeedMcpData();
        SeedVehicles();
        SeedMessages();
        FinishSeeding();
    }

    public void SeedSupervisorProfiles()
    {
        var maleSupervisorFirstNames = DatabaseSeederHelper.MaleSupervisorFirstNames;
        var femaleSupervisorFirstNames = DatabaseSeederHelper.FemaleSupervisorFirstNames;
        var supervisorLastNames = DatabaseSeederHelper.SupervisorLastNames;

        for (var i = 0; i < 10; i++)
        {
            var firstName = i < 5 ? maleSupervisorFirstNames[i] : femaleSupervisorFirstNames[i - 5];
            var lastName = supervisorLastNames[i];
            var gender = i < 5 ? Gender.Male : Gender.Female;
            var dateOfBirth = DatabaseSeederHelper.GenerateRandomDate(1960, 2000);

            var account = new Account()
            {
                Username = firstName.ToLower() + "_" + lastName.ToLower(),
                PasswordHash = "password",
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                DateOfBirth = dateOfBirth,
                UserRole = UserRole.Supervisor,
                Address = "",
            };
            account.GenerateSaltAndHash();

            _uwcDbContext.Accounts.Add(account);
            _allAccounts.Add(account);
        }

        _uwcDbContext.SaveChanges();
    }

    public void SeedDriverProfiles()
    {
        var maleDriverFirstNames = DatabaseSeederHelper.MaleDriverFirstNames;
        var femaleDriverFirstNames = DatabaseSeederHelper.FemaleDriverFirstNames;
        var driverLastNames = DatabaseSeederHelper.DriverLastNames;

        for (var i = 0; i < 20; i++)
        {
            var firstName = i < 10 ? maleDriverFirstNames[i] : femaleDriverFirstNames[i - 10];
            var lastName = driverLastNames[i];
            var gender = i < 10 ? Gender.Male : Gender.Female;
            var dateOfBirth = DatabaseSeederHelper.GenerateRandomDate(1960, 2000);

            var account = new Account()
            {
                Username = firstName.ToLower() + "_" + lastName.ToLower(),
                PasswordHash = "password",
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                DateOfBirth = dateOfBirth,
                UserRole = UserRole.Driver,
                Address = "",
            };
            account.GenerateSaltAndHash();

            _uwcDbContext.Accounts.Add(account);
            _allAccounts.Add(account);
        }

        _uwcDbContext.SaveChanges();
    }

    public void SeedCleanerProfiles()
    {
        var maleCleanerFirstNames = DatabaseSeederHelper.MaleCleanerFirstNames;
        var femaleCleanerFirstNames = DatabaseSeederHelper.FemaleCleanerFirstNames;
        var cleanerLastNames = DatabaseSeederHelper.CleanerLastNames;

        for (var i = 0; i < 20; i++)
        {
            var firstName = i < 10 ? maleCleanerFirstNames[i] : femaleCleanerFirstNames[i - 10];
            var lastName = cleanerLastNames[i];
            var gender = i < 10 ? Gender.Male : Gender.Female;
            var dateOfBirth = DatabaseSeederHelper.GenerateRandomDate(1960, 2000);

            var account = new Account()
            {
                Username = firstName.ToLower() + "_" + lastName.ToLower(),
                PasswordHash = "password",
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                DateOfBirth = dateOfBirth,
                UserRole = UserRole.Cleaner,
                Address = "",
            };
            account.GenerateSaltAndHash();

            _uwcDbContext.Accounts.Add(account);
            _allAccounts.Add(account);
        }

        _uwcDbContext.SaveChanges();
    }

    public void SeedMcpData()
    {
        var mcpDataList = new List<McpData>
        {
            new() { Capacity = 127, Coordinate = new Coordinate(10.7647293589828, 106.663266154958), Address = "580 Bà Hạt, P.6, Q.10" },
            new() { Capacity = 82, Coordinate = new Coordinate(10.7675037535589, 106.667284093661), Address = "500 Nguyễn Tri Phương, P.9, Q.10" },
            new() { Capacity = 142, Coordinate = new Coordinate(10.7629567124463, 106.656977523526), Address = "968 3 Tháng 2, P.15, Q.11" },
            new() { Capacity = 98, Coordinate = new Coordinate(10.7815403101810, 106.655190599178), Address = "389 Lý Thường Kiệt, P.8, Q.Tân Bình" },
            new() { Capacity = 162, Coordinate = new Coordinate(10.7771799685277, 106.660531841364), Address = "334 Tô Hiến Thành, P.14, Q.10" },
            new() { Capacity = 128, Coordinate = new Coordinate(10.7712254318066, 106.665784313171), Address = "54 Thành Thái, P.10, Q.10" },
            new() { Capacity = 89, Coordinate = new Coordinate(10.7668398628915, 106.659288627832), Address = "300 Lý Thường Kiệt, P.14, Q.10" },
            new() { Capacity = 120, Coordinate = new Coordinate(10.752208718089143, 106.64971397274442), Address = "96 Phạm Đình Hổ, P.2, Q.6" },
            new() { Capacity = 200, Coordinate = new Coordinate(10.786135, 106.651209), Address = "1150 Lạc Long Quân, P.8, Q.Tân Bình" },
            new() { Capacity = 201, Coordinate = new Coordinate(10.782575, 106.660679), Address = "153 Bắc Hải, P.15, Q.10" },
            new() { Capacity = 195, Coordinate = new Coordinate(10.775672, 106.667233), Address = "533 Sư Vạn Hạnh, P.12, Q.10" },
            new() { Capacity = 141, Coordinate = new Coordinate(10.776559, 106.663600), Address = "218 Thành Thái, P.15, Q.10" },
            new() { Capacity = 137, Coordinate = new Coordinate(10.771117, 106.652352), Address = "84 Nguyễn Thị Nhỏ, P.9, Q.Tân Bình" },
            new() { Capacity = 183, Coordinate = new Coordinate(10.7807, 106.676), Address = "276 Cách Mạng Tháng Tám, P.15, Q.3" },
            new() { Capacity = 140, Coordinate = new Coordinate(10.7612, 106.661), Address = "93 Lý Thường Kiệt, P.7, Q.10" }
        };

        foreach (var mcpData in mcpDataList)
        {
            _uwcDbContext.McpDatas.Add(mcpData);
        }

        _uwcDbContext.SaveChanges();
    }

    public void SeedVehicles()
    {
        var licensePlates = DatabaseSeederHelper.GenerateLicensePlates(30);
        string[] models = { "Model 1", "Model 2", "Model 3", "Model 4", "Model 5" };
        VehicleType[] vehicleTypes = { VehicleType.FrontLoader, VehicleType.SideLoader, VehicleType.RearLoader };

        var random = new Random();

        for (var i = 0; i < 30; i++)
        {
            var licensePlate = licensePlates[i];
            var model = models[random.Next(models.Length)];
            var vehicleType = vehicleTypes[random.Next(vehicleTypes.Length)];
            var currentLoad = random.NextDouble() * 100;
            var capacity = random.NextDouble() * 200;
            var averageSpeed = random.NextDouble() * 80;

            var vehicle = new VehicleData
            {
                LicensePlate = licensePlate,
                Model = model,
                VehicleType = vehicleType,
            };

            _allVehicles.Add(vehicle);
            _uwcDbContext.VehicleDatas.Add(vehicle);
        }

        _uwcDbContext.SaveChanges();
    }

    public void SeedMessages()
    {
        // Between ID 1 and ID 11

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[0],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(7).AddMinutes(30).AddSeconds(5),
            Content = "Hey, just finished the morning pickups. The smell today is something else..."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[0],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(8).AddMinutes(35).AddSeconds(14),
            Content = "Ran into some traffic too. It's like the whole city decided to hit the roads at once."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[10],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(8).AddMinutes(54).AddSeconds(2),
            Content =
                "Oh, and remember that alley near Green Street? Still blocked, had to do some crazy maneuvering to turn around."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[10],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(9).AddMinutes(2).AddSeconds(1),
            Content = "Thanks for the update. Sorry about the traffic and the alley. You're a pro at handling those tight spots!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[10],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(9).AddMinutes(2).AddSeconds(27),
            Content = "By the way, we got a call from Mrs. Henderson on Maple Avenue. She's concerned her bin was missed."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[0],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(9).AddMinutes(2).AddSeconds(59),
            Content = "I remember that one, her bin was half buried in snow. I'll swing by after lunch to sort it out."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[10],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(9).AddMinutes(3).AddSeconds(35),
            Content =
                "Thanks, appreciate you taking care of it. And don't forget to log any unexpected delays or issues in the system."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[0],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(11).AddMinutes(57).AddSeconds(5),
            Content = "Sure thing, I'll log everything once I'm back in the truck. Lunch break now, I'm starving!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[10],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(11).AddMinutes(59).AddSeconds(1),
            Content =
                "Enjoy your lunch! Also, we've been getting compliments on your friendly waves to the neighbors. Keep spreading that positivity."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[0],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(11).AddMinutes(59).AddSeconds(59),
            Content = "Thanks, boss! I figure a smile and a wave make trash days better for everyone. :)"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[10],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(14).AddMinutes(5).AddSeconds(14),
            Content =
                "You're absolutely right. Oh, and I'm looking into getting some AC repairs for the trucks. Hang in there with the heat!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[0],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(14).AddMinutes(35).AddSeconds(7),
            Content = "That would be amazing, it feels like a sauna in here sometimes. Fingers crossed for cooler days soon."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[10],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(14).AddMinutes(36).AddSeconds(55),
            Content = "I hear you! And speaking of repairs, the rearview camera should be fixed by Friday. Safety first!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[0],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(15).AddMinutes(5).AddSeconds(5),
            Content = "That's a relief. Those alley pickups won't be as nerve-wracking with a working camera. Can't wait!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[10],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(15).AddMinutes(10).AddSeconds(8),
            Content = "You got this. Keep up the great work, and remember, you're our waste warrior out there."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[0],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(15).AddMinutes(11).AddSeconds(9),
            Content = "Thanks, boss. I wear the title with pride. Off to tackle the afternoon route now!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[10],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(15).AddMinutes(24).AddSeconds(57),
            Content = "Go show that trash who's boss! If you ever find a way to make it smell like roses, let me know."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[0],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(15).AddMinutes(25).AddSeconds(18),
            Content = "Haha, will do! Catch you later, boss. Gotta conquer this rubbish realm!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[10],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(16).AddMinutes(36).AddSeconds(27),
            Content = "Well done, you have a productive day!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[0],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(16).AddMinutes(36).AddSeconds(45),
            Content = "Thank you, I will drive back to our company now."
        });


        // Between ID 1 and 20 others

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[11],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(7).AddMinutes(1).AddSeconds(1),
            Content = "I have a new task, please accept if you can."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[12],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(9).AddMinutes(25).AddSeconds(45),
            Content = "Your current task is no longer needed, do the next one please."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[13],
            ReceiverAccount = _allAccounts[0],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(12).AddMinutes(5).AddSeconds(51),
            Content =
                "I have done transporting the waste from the assigned MCP, I think my vehicle's current load is not enough for the next task."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[14],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(14).AddMinutes(7).AddSeconds(6),
            Content = "Hello, are you there. I need your help for this task, you are near the MCP I want to clear."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[15],
            ReceiverAccount = _allAccounts[0],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(16).AddMinutes(6).AddSeconds(9),
            Content = "Sorry, I have a headache. May I leave now."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[31],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(9).AddMinutes(9).AddSeconds(9),
            Content = "Sorry to keep you waiting, I was busy this morning and cannot reply your message."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[33],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(13).AddMinutes(45).AddSeconds(52),
            Content = "Hmm, that's a good idea. You could help the other cleaners."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[35],
            ReceiverAccount = _allAccounts[0],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(15).AddMinutes(4).AddSeconds(41),
            Content = "Help me, I get lost on my way!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[37],
            ReceiverAccount = _allAccounts[0],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(16).AddMinutes(26).AddSeconds(26),
            Content = "I'm done. Goodbye sir."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[39],
            ReceiverAccount = _allAccounts[0],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(16).AddMinutes(45).AddSeconds(10),
            Content = "See you tomorrow, sir."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[16],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(8).AddMinutes(4).AddSeconds(15),
            Content =
                "Are you there, please focus on the task of emptying the MCP at BK university. The students need fresh air to study. Be quick, please. Thank you."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[17],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(8).AddMinutes(5).AddSeconds(17),
            Content = "Have you cleaned the MCP on Ly Thuong Kiet street, I see that you are traveling on the wrong route."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[18],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(9).AddMinutes(48).AddSeconds(58),
            Content = "Why are you late today. We have a strict schedule today, be quick and go to work."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[19],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(11).AddMinutes(6).AddSeconds(19),
            Content = "Haha, have a good lunch, maybe I will compose task for you during the your lunch break time."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[20],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(11).AddMinutes(7).AddSeconds(25),
            Content = "Good job, Steve. You are done for today."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[32],
            ReceiverAccount = _allAccounts[0],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(13).AddMinutes(6).AddSeconds(5),
            Content = "The MCP is now cleaned. Be not worried."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[34],
            ReceiverAccount = _allAccounts[0],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(8).AddMinutes(15).AddSeconds(15),
            Content = "Sir, I have sent a request for a day off. Can you sign it for me?"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[0],
            ReceiverAccount = _allAccounts[36],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(7).AddMinutes(9).AddSeconds(48),
            Content = "Good morning, sir."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[38],
            ReceiverAccount = _allAccounts[0],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(14).AddMinutes(22).AddSeconds(11),
            Content =
                "Wait, maybe there has been a misunderstanding here. You want me to clean the MCP on the To Hien Thanh street first, not the one on the Su Van Hanh?"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[40],
            ReceiverAccount = _allAccounts[0],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(15).AddMinutes(11).AddSeconds(8),
            Content = "Oh, I see. I need to go to the toilet. I'll be back."
        });


        // Between ID 11 and 20 others

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[7],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(7).AddMinutes(5).AddSeconds(15),
            Content = "Thank you, I will drive back to our company now."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[11],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(14).AddMinutes(22).AddSeconds(17),
            Content = "Hello."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[4],
            ReceiverAccount = _allAccounts[10],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(8).AddMinutes(12).AddSeconds(34),
            Content = "Just finished the early morning route. The streets are looking much cleaner now!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[18],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(10).AddMinutes(25).AddSeconds(17),
            Content = "Received a report of a malfunctioning trash compactor. I'll check it out and get it fixed."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[22],
            ReceiverAccount = _allAccounts[10],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(9).AddMinutes(8).AddSeconds(56),
            Content = "Just completed a successful trash pickup on Main Street. The team's efficiency is improving!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[44],
            ReceiverAccount = _allAccounts[10],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(11).AddMinutes(45).AddSeconds(23),
            Content = "Received a request for extra bins in the downtown area. How should we proceed?"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[46],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(12).AddMinutes(30).AddSeconds(59),
            Content = "Completed the trash collection for today. Ready to head back and wrap up for the day!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[35],
            ReceiverAccount = _allAccounts[10],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(14).AddMinutes(15).AddSeconds(42),
            Content = "Inspecting the truck before starting the afternoon routes. Safety first!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[22],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(7).AddMinutes(40).AddSeconds(5),
            Content = "Just cleared a blocked alleyway for better access during our trash pickups. Smooth sailing ahead!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[19],
            ReceiverAccount = _allAccounts[10],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(13).AddMinutes(52).AddSeconds(19),
            Content = "Sorting and separating recyclables to ensure proper disposal. Every bit counts!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[17],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(15).AddMinutes(18).AddSeconds(7),
            Content = "Planning the route for tomorrow's pickups. Time to optimize our efficiency!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[26],
            ReceiverAccount = _allAccounts[10],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(10).AddMinutes(59).AddSeconds(30),
            Content = "Received a call about a lost trash bin. I'll investigate and see if we can locate it."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[16],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(9).AddMinutes(30).AddSeconds(15),
            Content = "Preparing for a special waste collection event this weekend. Let's make it a success!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[5],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(14).AddMinutes(2).AddSeconds(48),
            Content = "Sorting and segregating hazardous waste for proper disposal. Safety is our priority!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[29],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(11).AddMinutes(23).AddSeconds(11),
            Content = "Inspecting the collection bins for damages and maintenance needs. Keeping our equipment in top shape!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[48],
            ReceiverAccount = _allAccounts[10],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(8).AddMinutes(54).AddSeconds(26),
            Content = "Received a request for a special recycling pickup. Coordinating with the team to ensure a smooth operation."
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[33],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(15).AddMinutes(37).AddSeconds(53),
            Content = "Educating residents about proper recycling practices. Let's reduce contamination in our bins!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[39],
            ReceiverAccount = _allAccounts[10],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(13).AddMinutes(10).AddSeconds(39),
            Content = "Completed a community cleanup event. The neighborhood is looking much cleaner and more vibrant!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[2],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(7).AddMinutes(20).AddSeconds(12),
            Content = "Coordinating with local schools for recycling education programs. Cultivating eco-conscious citizens!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[47],
            ReceiverAccount = _allAccounts[10],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(12).AddMinutes(5).AddSeconds(28),
            Content =
                "Received positive feedback from a resident about our improved trash collection efficiency. Our hard work is paying off!"
        });

        _uwcDbContext.Messages.Add(new Message
        {
            SenderAccount = _allAccounts[10],
            ReceiverAccount = _allAccounts[37],
            Timestamp = DateTime.UtcNow.AddDays(-1).Date.AddHours(17).AddMinutes(5).AddSeconds(45),
            Content = "Heading out for a nighttime pickup. Let's keep the city clean even during the quiet hours!"
        });

        _uwcDbContext.SaveChanges();
    }

    public void FinishSeeding()
    {
        _uwcDbContext.SaveChanges();
    }
}