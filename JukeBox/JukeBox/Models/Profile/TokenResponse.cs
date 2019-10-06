namespace JukeBox.Models.Profile
{
    using System;
    using Newtonsoft.Json;
    using SQLite;

    public class TokenResponse
    {
        #region Properties
        [PrimaryKey, AutoIncrement]
        public int TokenResponseId { get; set; }

  
        public string AccessToken { get; set; }


        public string TokenType { get; set; }


        public int ExpiresIn { get; set; }

        
        public string UserName { get; set; }


        public DateTime Issued { get; set; }

        public DateTime Expires { get; set; }

        public string ErrorDescription { get; set; }
		#endregion

		#region Methods
		public override int GetHashCode()
		{
            return TokenResponseId;
		}
		#endregion
	}
}
