namespace JukeBox.Helpers
{
	using System;
	using Domain;
    using JukeBox.Models.Profile;
    using Models;

	public static class Converter
    {
        public static UserLocal ToUserLocal(User user)
        {
            return new UserLocal
            {
                Email = user.Email,
                FirstName = user.FirstName,
                ImagePath = user.ImagePath,
                LastName = user.LastName,
                Telephone = user.Telephone,
                UserId = user.UserId,
                UserTypeId = user.UserTypeId,
                BalanceAvailable = user.BalanceAvailable

            };
        }

		public static User ToUserDomain(UserLocal user, byte[] imageArray)
		{
            return new User
            {
                Email = user.Email,
                FirstName = user.FirstName,
                ImagePath = user.ImagePath,
                LastName = user.LastName,
                Telephone = user.Telephone,
                UserId = user.UserId,
                UserTypeId = user.UserTypeId.Value,
                ImageArray = imageArray,
                BalanceAvailable = user.BalanceAvailable
               
            };
		}
	}
}
