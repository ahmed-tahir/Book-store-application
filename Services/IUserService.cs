﻿namespace BookStoreApplication.Services
{
    public interface IUserService
    {
        string GetUserId();
        bool IsAuthenticated();
    }
}