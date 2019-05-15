using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfaces;
using TestProject.Core.Services.Interfaces;

namespace TestProject.Core.Services
{
    public class UserService
        : IUserService
    {
        private readonly ILoginRepository _loginRepository;

        private readonly IUserHelper _userHelper;

        public UserService(ILoginRepository loginRepository, IUserHelper userHelper)
        {
            _loginRepository = loginRepository;
            _userHelper = userHelper;
        }

        public int LoginInSocialAccount(User user)
        {

            int? getUserId = _loginRepository.GetSocialAccountUserId(user);

            int createUserId;

            if (getUserId == null)
            {
                createUserId = _loginRepository.Save(user);

                _userHelper.UserId = createUserId;

                return createUserId;
            }

            createUserId = getUserId.GetValueOrDefault();

            _userHelper.UserId = createUserId;

            return createUserId;
        }

        public void ChangeImage(int userId, string imagePath)
        {
            User user = _loginRepository.Get(userId);

            user.ImagePath = imagePath;

            _loginRepository.Update(user);
        }

        public void ChangePassword(int userId, string password)
        {
            User user = _loginRepository.Get(userId);

            user.Password = password;

            _loginRepository.Update(user);

            _userHelper.UserPassword = password;
        }

        public bool IsValidLogin(string login)
        {

            string result = _loginRepository.GetUserLogin(login);

            return result == null;

        }

        public void Logout()
        {
            _userHelper.DeleteUserStatus();
            _userHelper.DeleteUserAccessToken();
        }
    }
}
