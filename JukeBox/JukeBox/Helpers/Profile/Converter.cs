namespace JukeBox.Helpers
{
	using System;
	using Domain;
    using JukeBox.Models.Profile;
    using Models;

	public static class Converter
    {
        public static UserLocal ToUserLocal(User user , int userId)
        {
            return new UserLocal
            {
                Email = user.Email,
                FirstName = user.FirstName,
                ImagePath = user.ImagePath,
                LastName = user.LastName,
                Telephone = user.Telephone,
                UserId = userId,
                UserTypeId = user.UserId,
                ImageArray = user.ImageArray,
                BalanceAvailable = user.BalanceAvailable,
                BalanceAvailableFormat  = user.BalanceAvailable == 0 ? "R 0.00" : string.Format("R {0}", System.Math.Round(user.BalanceAvailable ?? 0, 2))


        };
        }

		public static User ToUserDomain(UserLocal user)
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
                ImageArray = user.ImageArray,
                BalanceAvailable = user.BalanceAvailable,
                BalanceAvailableFormat = user.BalanceAvailableFormat
           
               
            };
		}
	}
}
