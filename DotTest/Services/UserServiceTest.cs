using DotNest.DataAccess.Entities;
using DotNest.Models;
using DotNest.Services;

namespace DotTest;

public class UserServiceTest
{
    private readonly UserService _userService;
    private readonly List<User> _userData;

    public UserServiceTest()
    {
        var mockData = new MockData();
        _userService = new UserService(mockData.UserRepository);
        _userData = mockData.UserData;
    }

    [Fact]
    public void GetIdFromUsername_ValidUsername()
    {
        string username = "user1";

        int expectedUserId = _userData[0].Id;
        int actualUserId = _userService.GetIdFromUsername(username);

        Assert.Equal(expectedUserId, actualUserId);
    }

    [Fact]
    public void GetIdFromUsername_InvalidUsername()
    {
        string username = "user";

        int expectedUserId = -1;
        int actualUserId = _userService.GetIdFromUsername(username);

        Assert.Equal(expectedUserId, actualUserId);
    }

    [Fact]
    public void GetIdFromUsername_NullUsername()
    {
        int expectedUserId = -1;
        int actualUserId = _userService.GetIdFromUsername(null);

        Assert.Equal(expectedUserId, actualUserId);
    }

    [Fact]
    public void ConfirmLoginValues_InvalidUsername()
    {
        LoginModel loginModel = new()
        {
            Username = "user",
            Password = "user-password",
        };

        bool actualResult = _userService.ConfirmLoginValues(loginModel);

        Assert.False(actualResult);
    }

    [Fact]
    public void ConfirmLoginValues_InvalidPassword()
    {
        User user = _userData[0];

        LoginModel loginModel = new()
        {
            Username = user.Username,
            Password = "invalid-password",
        };

        bool actualResult = _userService.ConfirmLoginValues(loginModel);

        Assert.False(actualResult);
    }

    [Fact]
    public void ConfirmLoginValues_ValidLoginModel()
    {
        User user = _userData[0];

        LoginModel loginModel = new()
        {
            Username = user.Username,
            Password = String.Concat(user.Username, "-password"),
        };

        bool actualResult = _userService.ConfirmLoginValues(loginModel);

        Assert.True(actualResult);
    }

    [Fact]
    public void RegisterUser_DuplicateUsername()
    {
        User user = _userData[0];
        string password = "user-password";

        RegisterModel registerModel = new()
        {
            Username = user.Username,
            Email = "user@gmail.com",
            Password = password,
            ConfirmPassword = password,
        };

        Exception thrownException = Assert.Throws<InvalidOperationException>(() => _userService.RegisterUser(registerModel));

        Assert.Equal("The username already exists", thrownException.Message);
    }

    [Fact]
    public void RegisterUser_DuplicateEmail()
    {
        User user = _userData[0];
        string password = "user-password";

        RegisterModel registerModel = new()
        {
            Username = "user",
            Email = user.Email,
            Password = password,
            ConfirmPassword = password,
        };

        Exception thrownException = Assert.Throws<InvalidOperationException>(() => _userService.RegisterUser(registerModel));

        Assert.Equal("The email already exists", thrownException.Message);
    }

    [Fact]
    public void RegisterUser_ValidRegisterModel()
    {
        string password = "user-password";

        RegisterModel registerModel = new()
        {
            Username = "user",
            Email = "user@gmail.com",
            Password = password,
            ConfirmPassword = password,
        };

        try
        {
            _userService.RegisterUser(registerModel);
        }
        catch (Exception ex)
        {

            Assert.Fail("Expected no exception, but got: " + ex.Message);
        }
    }
}
