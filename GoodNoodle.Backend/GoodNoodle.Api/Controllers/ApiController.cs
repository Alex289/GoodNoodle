using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Notifications;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace GoodNoodle.Api.Controllers;

public class ApiController : ControllerBase
{
    private readonly DomainNotificationHandler _notifications;

    public ApiController(INotificationHandler<DomainNotification> notifications)
    {
        _notifications = (DomainNotificationHandler)notifications;
    }

    protected bool IsValidOperation()
    {
        return (!_notifications.HasNotifications());
    }

    protected IActionResult CreateResponse<T>(T result)
    {
        if (IsValidOperation())
        {
            return Ok(
                new ApiResponse<T>
                {
                    Success = true,
                    Data = result,
                    Errors = Array.Empty<DomainError>()
                });
        }

        var isNotFoundError = CheckForNotFoundError();

        if (isNotFoundError)
        {
            return NotFound(
                new ApiResponse<T>
                {
                    Success = false,
                    Errors = _notifications.GetNotifications().Select(n => n.Error).ToArray(),
                });
        }

        var isUnauthorized = CheckForUnauthorized();

        if (isUnauthorized)
        {
            return new ObjectResult(new ApiResponse<T>
            {
                Success = false,
                Errors = _notifications.GetNotifications().Select(n => n.Error).ToArray()
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }

        return BadRequest(
            new ApiResponse<T>
            {
                Success = false,
                Errors = _notifications.GetNotifications().Select(n => n.Error).ToArray(),
            });
    }

    protected IActionResult CreateResponse<T>(string path, T result)
    {
        if (IsValidOperation())
        {
            return Created(path,
                new ApiResponse<T>()
                {
                    Success = true,
                    Data = result,
                    Errors = Array.Empty<DomainError>()
                });
        }

        var isUnauthorized = CheckForUnauthorized();

        if (isUnauthorized)
        {
            return new ObjectResult(new ApiResponse<T>
            {
                Success = false,
                Errors = _notifications.GetNotifications().Select(n => n.Error).ToArray()
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }

        return BadRequest(
            new ApiResponse<T>()
            {
                Success = false,
                Errors = _notifications.GetNotifications().Select(n => n.Error).ToArray(),
            });
    }

    protected IActionResult CreateResponse()
    {
        if (IsValidOperation())
        {
            return Ok(
                new ApiResponse<bool>()
                {
                    Success = true,
                    Errors = Array.Empty<DomainError>()
                });
        }

        var isNotFoundError = CheckForNotFoundError();

        if (isNotFoundError)
        {
            return NotFound(
                new ApiResponse<string>
                {
                    Success = false,
                    Errors = _notifications.GetNotifications().Select(n => n.Error).ToArray(),
                });
        }

        var isUnauthorized = CheckForUnauthorized();

        if (isUnauthorized)
        {
            return new ObjectResult(new ApiResponse<string>
            {
                Success = false,
                Errors = _notifications.GetNotifications().Select(n => n.Error).ToArray()
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }

        return BadRequest(
            new
            {
                Success = false,
                Errors = _notifications.GetNotifications().Select(n => n.Error).ToArray(),
            });
    }

    private bool CheckForNotFoundError()
    {
        return _notifications
                .GetNotifications()
                .FirstOrDefault()
                .Error.Key == DomainErrorCodes.NotFound; ;
    }

    private bool CheckForUnauthorized()
    {
        return _notifications
                .GetNotifications()
                .FirstOrDefault()
                .Error.Key == DomainErrorCodes.UserIsNotAuthorized;
    }
}
