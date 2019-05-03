using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfaces;
using TestProject.Core.Servicies.Interfaces;

namespace TestProject.Core.Servicies
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
            int? getUserId = _loginRepository.GetSocialAccountUser(user);

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
    }
}
