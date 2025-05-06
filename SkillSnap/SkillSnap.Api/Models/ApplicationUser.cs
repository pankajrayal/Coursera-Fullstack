﻿using Microsoft.AspNetCore.Identity;

namespace SkillSnap.Api.Models {
  public class ApplicationUser: IdentityUser {
    public string FullName { get; set; }
  }
}