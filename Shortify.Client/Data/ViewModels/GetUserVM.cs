﻿using Microsoft.AspNetCore.Identity;

namespace Shortify.Client.Data.ViewModels
{
    public class GetUserVM
    {
        public string? Id { get; set; }
        public string FullName { get; set; }
    }
}
