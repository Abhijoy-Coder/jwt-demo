namespace JwtDemo.Models;

public class UserConstants
    {
        public static List<UserModel> Users = new()
        {
                new UserModel() { UserName="Alan",Password="Alan1234",Role="Admin" }
        };
    }