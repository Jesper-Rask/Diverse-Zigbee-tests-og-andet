namespace BlazorApp1.Authentication
{
    public class UserAccountService
    {
        private List<UserAccount> _users;

        public UserAccountService()
        {
            _users = new()
            {
                new UserAccount { UserName = "jra", Password = "ask", Role = "Administrator" },
                new UserAccount { UserName = "y", Password = "y", Role = "User" },
                new UserAccount { UserName = "z", Password = "z", Role = "ReadOnly" },

            };
        }
        public UserAccount? GetByUserName(string userName)
        {
            return _users.FirstOrDefault(x => x.UserName == userName);
        }
    }
}
