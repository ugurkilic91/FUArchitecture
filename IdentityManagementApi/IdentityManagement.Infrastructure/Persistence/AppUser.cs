using System;
using Microsoft.AspNetCore.Identity;

namespace IdentityManagement.Infrastructure.Persistence
{
	public class AppUser : IdentityUser<int>
	{
		public AppUser()
		{
		}
		public string Name { get; set; }
	}
}

